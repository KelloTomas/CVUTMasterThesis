using ServerApp.Data;
using ServerApp.SubApps;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ServerApp.Devices
{

	public class Rallo
	{
		#region private fields...
		private Queue<IMessage> _queue;
		private int _queueCapacity = 200;
		private Semaphore _semaphore;
		private Thread _signalThread;
		private bool _signalThreadJoinCalled = false;
		private TCPIPCommunicator _tcpipCommunicator;
		private QtPacketParser _packetParser;
		#endregion

		#region constructors...		
		public Rallo()
		{
			_packetParser = new QtPacketParser();
		}
		#endregion

		public void Init()
		{

		}

		#region public properties...

		private bool _terminated;
		public bool Terminated
		{
			get
			{
				return _terminated;
			}
			set
			{
				if (_terminated == value) return;
				_terminated = value;
				if (_terminated)
				{
					_terminatedEvent.Set();
				}
			}
		}
		private ManualResetEvent _terminatedEvent;
		private ISubApp subApp;

		public ManualResetEvent TerminatedEvent
		{
			get
			{
				return _terminatedEvent;
			}
		}

		public string IP { get; set; } = "127.0.0.1";

		public int Port { get; set; } = 15000;

		#endregion


		public void SendMessage(IMessage message)
		{
			lock (_queue)
			{
				_semaphore.Release();
				_queue.Enqueue(message);
			}
		}

		private void QueueMethod()
		{
			while (!_terminated)
			{
				try
				{
					switch (WaitHandle.WaitAny(new WaitHandle[] { _semaphore, _terminatedEvent }))
					{
						case 0:
							// semaphore
							IMessage message;
							lock (_queue)
							{
								message = _queue.Dequeue();
							}
							try
							{
								ProcessMessage(message);
							}
							catch (Exception)
							{
							}
							break;

						case 1:
							// terminated
							break;

						default:
							// timeout
							break;
					}
				}
				catch (Exception)
				{
					InitRestart();
				}
			}
		}

		private void ProcessMessage(IMessage message)
		{
			ConnectionStatusEnum connectionStatus = _tcpipCommunicator.ConnectionStatus;
			if (connectionStatus == ConnectionStatusEnum.ConnectedInicialized)
			{
				foreach (IAction action in message.Actions)
				{
					byte[] data = GetPacket(action).GetData();
					if (data != null)
					{
						_tcpipCommunicator.Send(data);
					}
				}
			}
		}

		protected virtual void InitRestart()
		{
			lock (_queue)
			{
				_semaphore.Close();
				_semaphore = new Semaphore(0, _queueCapacity);
				_queue.Clear();
			}
		}

		private void Dispose()
		{
			Terminated = true;
			SignalThreadJoin();
			_tcpipCommunicator.Dispose();
		}
		protected void SignalThreadJoin()
		{
			_signalThread.Join(); //nejprve se musi ukoncit signalThread, protoze tcpThread uvolnuje sdilene objekty
			_signalThreadJoinCalled = true;
		}
		private void DisposeThreadInterfaceResources()
		{
			_semaphore.Close();
			_queue.Clear();
		}

		void ReceivedMessage(string msg)
		{
			while (true)
			{
				int start = msg.IndexOf("<RSO>");
				if (start == -1)
				{
					return;
				}
				if (start > 0)
				{
					msg = msg.Remove(0, start);
				}
				int end = msg.IndexOf("</RSO>");
				if (end == -1)
				{
					return;
				}
				end += 6; // add length of </RSO> tag
				IEnumerable<IAction> actions;
				if (end == msg.Length)
				{
					// process whole message
					actions = _packetParser.CreateAction(msg);
				}
				else
				{
					// message has some suffix. Process without it.
					actions = _packetParser.CreateAction(msg.Remove(end));
				}
				foreach (IAction action in actions)
				{
					subApp?.ProcessAction(action);
				}
				// remove processed data from message
				msg = msg.Remove(0, end);
			}
		}

		public void Connect(string IP, int port)
		{
			_tcpipCommunicator = new TCPIPCommunicator(port, IP, InitTerminal, ReceivedMessage)
			{
				ReconnectInterval = 5000,
				ReadTimeOut = 5000,
				WriteTimeOut = 5000
			};
			_tcpipCommunicator.Connect();

			_semaphore = new Semaphore(0, _queueCapacity);
			_queue = new Queue<IMessage>();

			_terminated = false;
			_terminatedEvent = new ManualResetEvent(false);


			_signalThread = new Thread(QueueMethod);
			_signalThread.Name = "HWSignalThread";
			_signalThread.Start();
			_signalThreadJoinCalled = false;
		}

		#region private methods...

		public IEnumerable<IAction> InitActions
		{
			get
			{
				// nejprve vymazu vsechny layuty v terminalu
				yield return new InitAction();

				// pote az cele obrazovky, ktere jiz mohou vyuzivat ulozene fragmenty
				foreach (IStoreLayoutAction storeLayoutAction in GetStoreLayoutActions())
				{
					yield return storeLayoutAction;
				}
			}
		}

		private IEnumerable<IStoreLayoutAction> GetStoreLayoutActions()
		{
			yield return new StoreLayoutAction("ToDo", "ToDo");
		}

		private bool InitTerminal()
		{
			foreach (IAction initAction in InitActions)
			{
				// send data
				if (!_tcpipCommunicator.Send(GetPacket(initAction).GetData()))
				{
					return false;
				}
			}
			return true;
		}

		public BasePacket GetPacket(IAction action)
		{
			if (action is IInitAction)
			{
				return new InitPacket();
			}
			else if (action is IStoreLayoutAction)
			{
				return new StoreLayoutPacket((IStoreLayoutAction)action);
			}
			else if (action is IShowLayoutAction)
			{
				return new ShowLayoutPacket((IShowLayoutAction)action);
			}
			else if (action is IBeepAction)
			{
				return new BeepPacket((IBeepAction)action);
			}
			else if (action is ShowMessageBoxAction)
			{
				return new ShowMessageBoxPacket((IShowMessageBoxAction)action);
			}
			else if (action is ModalWindowShowAction)
			{
				return new ModalWindowShowPacket((IModalWindowShowAction)action);
			}
			else if (action is ModalWindowCloseAction)
			{
				return new ModalWindowClosePacket();
			}
			else
			{
				throw new ArgumentOutOfRangeException("type", action.GetType(), "Neznamy typ akce");
			}
		}
		#endregion
	}

}