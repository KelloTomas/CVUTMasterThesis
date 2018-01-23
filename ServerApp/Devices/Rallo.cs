using ServerApp.Devices.Actions;
using ServerApp.Devices.Packets;
using ServerApp.SubApps;
using ServerApp.SubApps.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public Rallo(ISubApp subApp)
		{
			this.subApp = subApp;
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
				int start = msg.IndexOf("<RLO>");
				if (start == -1)
				{
					return;
				}
				if (start > 0)
				{
					msg = msg.Remove(0, start);
				}
				int end = msg.IndexOf("</RLO>");
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


		private bool InitTerminal()
		{
			foreach (IAction initAction in subApp.InitActions)
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
			switch (action)
			{
				case IInitAction a:
					return new InitPacket();
				case IStoreLayoutAction a:
					return new StoreLayoutPacket(a);
				case IShowLayoutAction a:
					return new ShowLayoutPacket(a);
				case IBeepAction a:
					return new BeepPacket(a);
				case IShowMessageBoxAction a:
					return new ShowMessageBoxPacket(a);
				case IModalWindowShowAction a:
					return new ModalWindowShowPacket(a);
				case IModalWindowCloseAction a:
					return new ModalWindowClosePacket();
				default:
					throw new ArgumentOutOfRangeException("type", action.GetType(), "Neznamy typ akce");
			}
		}
		#endregion
	}

}