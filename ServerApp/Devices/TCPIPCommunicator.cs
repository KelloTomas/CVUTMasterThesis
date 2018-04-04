using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Devices
{
	public delegate bool InitDelegate();
	public delegate void ReceivedDataAsync(string message);

	public class TCPIPCommunicator
	{
		#region Private fields                
		// Synchronizacni objekt pro pristup k ethernetu
		private System.Object _syncTcp = new object();

		private InitDelegate _inicializationMethod;
		private TcpClient _client;
		public NetworkStream _netStream;
		private ReceivedDataAsync ReceivedDataAsync;
		private byte[] readBuffer = new byte[4096];
		#endregion
		#region public properties...

		public int Port { get; set; }

		public string Ip { get; set; }

		public ConnectionStatusEnum ConnectionStatus { get; set; }

		public AutoResetEvent ConnectedEvent { get; set; }

		public AutoResetEvent DisconnectedEvent { get; set; }

		public event EventHandler ConnectionStatusChanged;

		public event EventHandler ConnectionErrorOccurred;

		public bool AutoReconnect { get; set; }

		public int ReconnectInterval { get; set; }

		public Thread ConnectThread { get; protected set; }

		public bool Terminated { get; set; }

		private int _readTimeOut = 5000;
		public int ReadTimeOut
		{
			get
			{
				return _readTimeOut;
			}
			set
			{
				_readTimeOut = value;
			}
		}

		private int _writeTimeOut = 5000;
		public int WriteTimeOut
		{
			get
			{
				return _writeTimeOut;
			}
			set
			{
				_writeTimeOut = value;
			}
		}
		#endregion

		#region constructors...
		public TCPIPCommunicator(int port, string ip, InitDelegate inicializationMethod, ReceivedDataAsync receivedDataAsyncMethod = null)
		{
			ReceivedDataAsync = receivedDataAsyncMethod;
			Port = port;
			Ip = ip;
			_inicializationMethod = inicializationMethod;
			ConnectionStatus = ConnectionStatusEnum.Disconnected;
			AutoReconnect = true;
			ReconnectInterval = 2000;
			ConnectedEvent = new AutoResetEvent(false);
			DisconnectedEvent = new AutoResetEvent(false);
		}
		#endregion
		#region public methods...
		public void Connect()
		{
			if (Terminated)
			{
				return;
			}
			WorkWithSync(() =>
			{
				if (ConnectionStatus == ConnectionStatusEnum.Disconnected)
				{
					// closeActiveConnection(); neni nutne volat, protoze je zajistono, ze se metoda Connect vola pouze pri odpojenem spojeni.
					ConnectionStatus = ConnectionStatusEnum.Connecting;
					ConnectThread = new Thread(ConnectMethod);
					// Kak: 03.11.2017 Zkraceno, at se nam vejde do sirky polozky v logu
					ConnectThread.Name = "TCPIPComm.Connect";
					ConnectThread.Start();
				}
			});
		}
		#endregion

		#region Protected override methods
		public void Dispose()
		{
			Terminated = true;
			StopConnectThread();
			CloseActiveConnection();
		}

		#endregion
		#region private methods...
		private bool connectToTCPServer()
		{
			try
			{
				IPAddress ipAdress;
				if (IPAddress.TryParse(Ip, out ipAdress))
				{
					_client.Connect(new IPEndPoint(ipAdress, Port));
				}
				else
				{
					_client.Connect(Ip, Port);
				}

			}
			catch (SocketException)
			{
				Console.WriteLine($"Cant connect to {Ip}, on port {Port}");
			}

			if (_client.Connected)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected void ConnectMethod()
		{
			try
			{
				CloseActiveConnection();
				_client = new TcpClient();
				while (!Terminated && !connectToTCPServer())
				{
					Thread.Sleep(ReconnectInterval);
				}
				if (!Terminated)
				{
					_netStream = _client.GetStream();
					_netStream.ReadTimeout = _readTimeOut;
					_netStream.WriteTimeout = _writeTimeOut;
					if (ReceivedDataAsync != null)
					{
						// subscribe for receiving messages
						_netStream.BeginRead(readBuffer,
									0,
									readBuffer.Length,
									EndRead,
									readBuffer);
					}
					WorkWithSync(() =>
					{
						ConnectionStatus = ConnectionStatusEnum.ConnectedNotInicialized;
					});

					if (_inicializationMethod() == true)
					{
						WorkWithSync(() =>
						{
							ConnectionStatus = ConnectionStatusEnum.ConnectedInicialized;
							ConnectedEvent.Set();
						});
					}
				}
			}
			catch (Exception ex)
			{
				// odchytam vsechny vyjimky, abych je mohl alespon zalogovat
				throw;
			}
		}

		private void StopConnectThread()
		{
			if (ConnectThread != null && ConnectThread.IsAlive)
			{
				ConnectThread.Join();
			}
		}

		private void CloseActiveConnection()
		{
			WorkWithSync(() =>
			{
				if (_netStream != null)
				{
					_netStream.Close();
					_netStream = null;
				}
				if (_client != null)
				{
					_client.Close();
					_client = null;
				}
			});
		}


		#endregion
		#region Private communication methods
		private void EndRead(IAsyncResult ar)
		{
			if (Terminated)
			{
				return;
			}
			string msg;
			try
			{
				int i = _netStream.EndRead(ar);
				msg = Encoding.ASCII.GetString(readBuffer, 0, i);
				_netStream.BeginRead(readBuffer, 0, readBuffer.Length, EndRead, readBuffer);
			}
			catch (IOException)
			{
				return;
			}
			ReceivedDataAsync.Invoke(msg);
		}
		#endregion

		#region Public communication methods

		public bool Send(byte[] data)
		{
			int length = data.Length;
			lock (_syncTcp)
			{
				if (ConnectionStatus == ConnectionStatusEnum.ConnectedInicialized || ConnectionStatus == ConnectionStatusEnum.ConnectedNotInicialized)
				{

					try
					{
						_netStream.Write(data, 0, length);
						return true;
					}
					catch (IOException e) //timeout
					{
						ConnectionStatus = ConnectionStatusEnum.Disconnected;
						DisconnectedEvent.Set();
						if (AutoReconnect)
						{
							Connect();
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Odešle řeřězec <c>data</c>. 
		/// </summary>
		/// <param name="data">Odesílaná data.</param>
		/// <returns>true, pokud odeslání proběhlo, jinak false</returns>
		public bool Send(string data)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			return Send(buffer);
		}

		/// <summary>
		/// Precte data ze socketu a vraci je jako string
		/// </summary>
		/// <param name="answer">Vyctena data</param>
		/// <returns>True, pokud byla nejaka data vyctena.</returns>
		public bool Receive(ref string answer)
		{
			byte[] readBuffer = new byte[255];
			int length = 0;
			return Receive(ref readBuffer, ref length);
		}

		/// <summary>
		/// Precte data ze socketu a vraci je jako pole byte.
		/// </summary>
		/// <param name="data">Prectena data</param>
		/// <param name="length">Pocet byte, ktere byly vycteny.</param>
		/// <returns>True, pokud byla nejaka data vyctena.</returns>
		public bool Receive(ref byte[] data, ref int length)
		{
			lock (_syncTcp)
			{
				if (ConnectionStatus == ConnectionStatusEnum.ConnectedInicialized || ConnectionStatus == ConnectionStatusEnum.ConnectedNotInicialized)
				{

					try
					{
						//_log.Debug("RECEIVING...");
						length = _netStream.Read(data, 0, data.Length);
						return true;
					}
					catch (IOException e) //timeout
					{
						ConnectionStatus = ConnectionStatusEnum.Disconnected;
						DisconnectedEvent.Set();
						if (AutoReconnect)
						{
							Connect();
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Odesla data a ihned ceka na odpoved.
		/// </summary>
		/// <param name="data">Data, ktera se odesilaji.</param>
		/// <param name="dataLength">Pocet bytu k odeslani.</param>
		/// <param name="answer">Odpoved</param>
		/// <param name="answerLength">Pocet bytu odpovedi.</param>
		/// <param name="log">Moznost definovat log</param>
		/// <returns>True, pokud odeasli a prijem probehlo.</returns>
		public bool SendReceive(byte[] data, int dataLength, ref byte[] answer, ref int answerLength, Func<byte[], int, bool> continueRead = null)
		{

			if (continueRead == null)
			{
				continueRead = (read, readLenght) =>
				{
					return false;
				};
			}

			lock (_syncTcp)
			{
				if (ConnectionStatus == ConnectionStatusEnum.ConnectedInicialized || ConnectionStatus == ConnectionStatusEnum.ConnectedNotInicialized)
				{
					{
						try
						{
							_netStream.Write(data, 0, dataLength);
							answerLength = 0;
							do
							{
								answerLength += _netStream.Read(answer, answerLength, answer.Length);
							} while (continueRead(answer, answerLength));

							return true;
						}
						catch (IOException e) //timeout
						{
							ConnectionStatus = ConnectionStatusEnum.Disconnected;
							DisconnectedEvent.Set();
							if (AutoReconnect)
							{
								Connect();
							}
						}
					}
				}
			}
			return false;
		}
		#endregion

		private void WorkWithSync(Action action)
		{
			lock (_syncTcp)
			{
				action();
			}
		}
	}
}
