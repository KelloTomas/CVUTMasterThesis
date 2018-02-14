using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataLayer.Data;

namespace DataLayer
{
	public class DatabaseLayer
	{
		string ConnectionString = "Data Source=DESKTOP-M5V50NA;Initial Catalog=CVUTdb;Persist Security Info=True;User ID=sa;Password=root";

		#region private methods...
		private Order GetOrderInt(XElement order)
		{
			Order o = new Order
			{
				IdOrder = (int)order.Attribute("IdOrder"),
				IdMenu = (int)order.Attribute("IdMenu"),
				ForDate = (DateTime)order.Attribute("ForDate"),
				SoupName = (string)order.Attribute("SoupName"),
				MealName = (string)order.Attribute("MealName"),
				DesertName = (string)order.Attribute("DesertName"),
				Served = (bool)order.Attribute("Served")
			};
			return o;
		}

		private Device GetDevice(XElement deviceXML)
		{
			Device device = new Device();
			device.IdDevice = (int)deviceXML.Attribute("IdDevice");
			device.Port = (int)deviceXML.Attribute("Port");
			device.IP = (string)deviceXML.Attribute("IP");
			return device;
		}

		private Client GetClientInt(XElement xNode)
		{
			Client client = new Client
			{
				Id = (int)xNode.Attribute("IdClient"),
				FirstName = (string)xNode.Attribute("FirstName"),
				LastName = (string)xNode.Attribute("LastName"),
				Balance = (float)xNode.Attribute("Balance"),
				CardNumber = (string)xNode.Attribute("CardNumber"),
				Orders = new List<Menu>()
			};
			return client;
		}
		#endregion

		#region public methods...
		public void UpdateSubApp(MyApplication app)
		{
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].UpdateSubApp";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@appId", System.Data.SqlDbType.Int).Value = app.Id;
					command.Parameters.Add("@IsRunning", System.Data.SqlDbType.Bit).Value = app.IsRunning;
					command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = app.AppName;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							//xmlResult += reader.GetString(0);
						}
					}
				}
				foreach(Device d in app.Devices)
				{
					using (SqlCommand command = connection.CreateCommand())
					{
						command.CommandText = "[dbo].UpdateDevice";
						command.CommandType = System.Data.CommandType.StoredProcedure;
						command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = d.IdDevice;
						command.Parameters.Add("@Ip", System.Data.SqlDbType.NVarChar).Value = d.IP;
						command.Parameters.Add("@Port", System.Data.SqlDbType.Int).Value = d.Port;
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								//xmlResult += reader.GetString(0);
							}
						}
					}
				}
			}
		}
		public IEnumerable<MyApplication> GetSubApps()
		{
			// <root><App IsRunning=\"1\" Name=\"Inform\"><Device IP=\"127.0.0.1\" Port=\"15001\"/></App><App IsRunning=\"0\" Name=\"Order\"><Device IP=\"127.0.0.1\" Port=\"15002\"/></App><App IsRunning=\"0\" Name=\"Serve\"><Device IP=\"127.0.0.1\" Port=\"15003\"/><Device IP=\"127.0.0.1\" Port=\"15004\"/></App></root>
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].GetApps";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
			if (string.IsNullOrEmpty(xmlResult))
			{
				yield break;
			}

			XElement root = XElement.Parse(xmlResult);
			foreach (XElement appXml in root.Elements())
			{
				switch (appXml.Name.LocalName)
				{
					case "App":

						MyApplication app = new MyApplication
						{
							Id = (int)appXml.Attribute("Id"),
							IsRunning = (bool)appXml.Attribute("IsRunning"),
							AppName = (string)appXml.Attribute("AppName"),
							TypeName = (string)appXml.Attribute("TypeName"),
							Devices = new List<Device>()
						};
						foreach (XElement deviceXML in appXml.Elements())
						{
							app.Devices.Add(GetDevice(deviceXML));
						}
						yield return app;
						break;
					default:
						Console.WriteLine($"Error GetSubApps: received xml Element {appXml.Name.LocalName}");
						break;
				}
			}
		}

		public IEnumerable<Menu> GetMenu(DateTime? forDate = null)
		{
			if (forDate == null)
				forDate = DateTime.Now.Date;
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].GetMenu";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@ForDate", System.Data.SqlDbType.Date).Value = forDate;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
			if (string.IsNullOrEmpty(xmlResult))
			{
				yield break;
			}

			XElement root = XElement.Parse(xmlResult);
			foreach (XElement appXml in root.Elements())
			{
				switch (appXml.Name.LocalName)
				{
					case "Menu":

						Menu menu = new Menu
						{
							ForDate = (DateTime)appXml.Attribute("ForDate"),
							IdMenu = (int)appXml.Attribute("IdMenu"),
							SoupName = (string)appXml.Attribute("SoupName"),
							MealName = (string)appXml.Attribute("MealName"),
							DesertName = (string)appXml.Attribute("DesertName")
						};
						yield return menu;
						break;
					default:
						Console.WriteLine($"Error GetMenu: received xml Element {appXml.Name.LocalName}");
						break;
				}
			}
		}
		public IEnumerable<Menu> GetMenu(Client forClient)
		{
			if(forClient == null)
				yield break;
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].[GetMenuForClient]";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@CardNumber", System.Data.SqlDbType.VarChar).Value = forClient.CardNumber;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
			if (string.IsNullOrEmpty(xmlResult))
			{
				yield break;
			}

			XElement root = XElement.Parse(xmlResult);
			foreach (XElement appXml in root.Elements())
			{
				switch (appXml.Name.LocalName)
				{
					case "Menu":

						Menu menu = new Menu
						{
							ForDate = (DateTime)appXml.Attribute("ForDate"),
							IdMenu = (int)appXml.Attribute("IdMenu"),
							SoupName = (string)appXml.Attribute("SoupName"),
							MealName = (string)appXml.Attribute("MealName"),
							DesertName = (string)appXml.Attribute("DesertName")
						};
						yield return menu;
						break;
					default:
						Console.WriteLine($"Error GetMenu: received xml Element {appXml.Name.LocalName}");
						break;
				}
			}
		}

		public void AddOrder(int clientId, DateTime forDate, int menuId)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].AddOrder";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@ClientId", System.Data.SqlDbType.Int).Value = clientId;
					command.Parameters.Add("@ForDate", System.Data.SqlDbType.Date).Value = forDate;
					command.Parameters.Add("@MenuId", System.Data.SqlDbType.Int).Value = menuId;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
			Console.WriteLine($"AddOrder: {xmlResult}");
		}
		public Client GetClient(string cardNumber)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].GetClient";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@card_number", System.Data.SqlDbType.NChar).Value = cardNumber;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}

			if (string.IsNullOrEmpty(xmlResult))
			{
				return null;
			}

			XElement root = XElement.Parse(xmlResult);
			foreach (XElement client in root.Elements())
			{
				switch (client.Name.LocalName)
				{
					case "Client":
						return GetClientInt(client);
					default:
						Console.WriteLine($"Error GetClient: received xml Element {client.Name.LocalName}");
						break;
				}
			}
			return null;
		}
		
		public void UpdateClient(Client client)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].UpdateClient";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = client.Id;
					command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = client.FirstName;
					command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = client.LastName;
					command.Parameters.Add("@Balance", System.Data.SqlDbType.Float).Value = client.Balance;
					command.Parameters.Add("@CardNumber", System.Data.SqlDbType.VarChar).Value = client.CardNumber;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
		}
		public IEnumerable<Client> GetClients()
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].GetClients";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}

			if (string.IsNullOrEmpty(xmlResult))
			{
				yield break;
			}

			XElement root = XElement.Parse(xmlResult);
			foreach (XElement client in root.Elements())
			{
				switch (client.Name.LocalName)
				{
					case "Client":
						yield return GetClientInt(client);
						break;
					default:
						Console.WriteLine($"Error GetClient: received xml Element {client.Name.LocalName}");
						break;
				}
			}
			yield break;
		}
		public Client GetOrders(string cardNumber)
		{
			// <root><Client IdClient=\"2\" FirstName=\"Mato      \" LastName=\"Vyskocany \" Balance=\"8.520000000000000e+002\"/><Order IdOrder=\"11\" ForDate=\"2018-01-28\" IdMenu=\"1005\" SoupName=\"Paradajková\" MealName=\"Rezen\" DesertName=\"Puding\"/><Order IdOrder=\"16\" ForDate=\"2018-01-28\" IdMenu=\"1006\" SoupName=\"Paradajková\" MealName=\"Kurca\" DesertName=\"Puding\"/><Order IdOrder=\"17\" ForDate=\"2018-01-28\" IdMenu=\"1006\" SoupName=\"Paradajková\" MealName=\"Kurca\" DesertName=\"Puding\"/><Order IdOrder=\"18\" ForDate=\"2018-01-30\" IdMenu=\"1014\" SoupName=\"Paradajková\" MealName=\"Rezen\" DesertName=\"Puding\"/></root>
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].GetOrders";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@card_number", System.Data.SqlDbType.NChar).Value = cardNumber;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
			if (string.IsNullOrEmpty(xmlResult))
				return null;

			Client client = null;
			XElement root = XElement.Parse(xmlResult);
			foreach (XElement ClientAndOrder in root.Elements())
			{
				switch (ClientAndOrder.Name.LocalName)
				{
					case "Client":
						client = GetClientInt(ClientAndOrder);
						break;
					case "Order":
						if (client != null)
							client.Orders.Add(GetOrderInt(ClientAndOrder));
						break;
					default:
						Console.WriteLine($"Error GetOrder: received xml Element {ClientAndOrder.Name.LocalName}");
						break;
				}
				/*
				short mealKindId = (short)xNode.Attribute("IdDruh");            
                MealKind mealKind = availableMealKinds.Single(m => m.WorkplaceId == workplaceId && m.Id == mealKindId);

                Meal meal = new Meal(
                    (short)xNode.Attribute("IdAlt"),
                    string.Empty, // popis alternativy me nezajima
                    (string)xNode.Attribute("KodAlt"),
                    false,
                    mealKind);

                Client client = new Client((string)xNode.Attribute("Titul"), (string)xNode.Attribute("Jmeno"), (string)xNode.Attribute("Prijmeni"),
                    (int)xNode.Attribute("EvCislo"),
                    //null,// cislo karty v tuto chvili neznam
                    (string)xNode.Attribute("OCS"), (string)xNode.Attribute("RCS"),
                    0, string.Empty, string.Empty, string.Empty, (MealSize?)((short?)xNode.Attribute("VelPorce")) ?? MealSize.NotDefined, 0, 0, 0, 0, ClientIdentification,
                    // kod karty me u poslednich vydeju nezajima, rozhodne se nebude nikde zobrazovat
                    null, null); // zbyle udaje me nezajimaji

                DateTime servedTime = (DateTime)xNode.Attribute("CasVydeje");
                ServedMeal vydaneJidlo = new ServedMeal((int?)xNode.Attribute("PC"), meal, client, servedTime);
				*/
			}
			return client;
		}
		#endregion
	}
}
