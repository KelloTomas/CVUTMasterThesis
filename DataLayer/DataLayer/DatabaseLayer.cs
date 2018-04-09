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
		private const string CONNECTION_STRING = "Data Source=DESKTOP-M5V50NA;Initial Catalog=CVUTdb;Persist Security Info=True;User ID=sa;Password=root";

		#region private methods...
		private Order GetOrderInt(XElement order)
		{
			// {<Order IdOrder="30" ForDate="2018-02-02" IdMenu="1016" SoupName="Paradajkova" MealName="Rezen" DesertName="Puding" Served="0" />}
			Order o = new Order
			{
				IdOrder = (int)order.Attribute("IdOrder"),
				IdMenu = (int)order.Attribute("IdMenu"),
				ForDate = (DateTime)order.Attribute("ForDate"),
				Served = (DateTime)order.Attribute("Served")
			};

			o.Items[0] = new MenuItem { /* Id = (int)order.Attribute("IdSoup"), */ Name = (string)order.Attribute("SoupName") };
			o.Items[1] = new MenuItem { /* Id = (int)order.Attribute("Id"), */ Name = (string)order.Attribute("MealName") };
			o.Items[2] = new MenuItem { /* Id = (int)order.Attribute("Id"), */ Name = (string)order.Attribute("DesertName") };
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
				Orders = new List<Order>()
			};
			return client;
		}
		#endregion

		#region public methods...
		public Menu ServeOrder(int clientId)
		{
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.CommandText = $"[dbo].ServeOrder";
					command.Parameters.Add("@ClientId", System.Data.SqlDbType.Int).Value = clientId;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Menu o = new Menu()
							{
								IdMenu = reader.GetInt32(0),
								ForDate = reader.GetDateTime(1),
							};
							if (!reader.IsDBNull(2))
							{
								o.Items[0] = new MenuItem()
								{
									Id = reader.GetInt32(2),
									Name = reader.GetString(3),
									Description = reader.GetString(4)
								};
							}
							if (!reader.IsDBNull(5))
							{
								o.Items[1] = new MenuItem()
								{
									Id = reader.GetInt32(5),
									Name = reader.GetString(6),
									Description = reader.GetString(7)
								};
							}
							if (!reader.IsDBNull(8))
							{
								o.Items[2] = new MenuItem()
								{
									Id = reader.GetInt32(8),
									Name = reader.GetString(9),
									Description = reader.GetString(10)
								};
							}
							return o;
						}
					}
				}
			}
			return null;
		}
		public IEnumerable<MenuItem> GetTable(MenuItem menuItem)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					switch (menuItem)
					{
						case Soup order:
							command.CommandText = $"SELECT IdSoup Id, Name, Description FROM [dbo].Soups";
							break;
						case Meal menu:
							command.CommandText = $"SELECT IdMeal Id, Name, Description FROM [dbo].Meals";
							break;
						case Desert d:
							command.CommandText = $"SELECT IdDesert Id, Name, Description FROM [dbo].Deserts";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					command.CommandText += " for xml raw('Item'), root ('root')";
					command.CommandType = System.Data.CommandType.Text;
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
					case "Item":
						var a = new MenuItem();
						a.Id = (int)appXml.Attribute("Id");
						a.Name = (string)appXml.Attribute("Name");
						a.Description = (string)appXml.Attribute("Description");
						yield return a;
						break;
					default:
						Console.WriteLine($"Error GetTable: received xml Element {appXml.Name.LocalName}");
						break;
				}
			}
		}
		public void UpdateSubApp(MyApplication app)
		{
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
				foreach (Device d in app.Devices)
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
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
						//{<Menu ForDate="2018-02-02" IdMenu="1016" IdSoup="1" SoupName="Paradajkova" IdMeal="1" MealName="Rezen" IdDesert="1" DesertName="Puding" />}
						Menu menu = new Menu
						{
							ForDate = (DateTime)appXml.Attribute("ForDate"),
							IdMenu = (int)appXml.Attribute("IdMenu")
						};
						menu.Items[0] = new MenuItem { Id = (int)appXml.Attribute("IdSoup"), Name = (string)appXml.Attribute("SoupName") };
						menu.Items[1] = new MenuItem { Id = (int)appXml.Attribute("IdMeal"), Name = (string)appXml.Attribute("MealName") };
						menu.Items[2] = new MenuItem { Id = (int)appXml.Attribute("IdDesert"), Name = (string)appXml.Attribute("DesertName") };
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
			if (forClient == null)
				yield break;
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
							IdMenu = (int)appXml.Attribute("IdMenu")
						};
						menu.Items[0].Name = (string)appXml.Attribute("SoupName");
						menu.Items[1].Name = (string)appXml.Attribute("MealName");
						menu.Items[2].Name = (string)appXml.Attribute("DesertName");
						yield return menu;
						break;
					default:
						Console.WriteLine($"Error GetMenu: received xml Element {appXml.Name.LocalName}");
						break;
				}
			}
		}

		public void Add(Object o)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					switch (o)
					{
						case Order order:
							command.CommandText = $"INSERT INTO [dbo].[Orders] (ForDate, IdMenu, IdClient, Vydane) VALUES('{order.ForDate.ToString("yyyy-MM-dd")}', {order.IdMenu}, {order.Client}, null)";
							break;
						case Menu menu:
							command.CommandText = $"INSERT INTO [dbo].[Menu] (ForDate, IdSoup, IdMeal, IdDesert) VALUES('{menu.ForDate.ToString("yyyy-MM-dd")}', {menu.Items[0].Id}, {menu.Items[1].Id}, {menu.Items[2].Id})";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					command.CommandType = System.Data.CommandType.Text;
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

		public void RemoveFromDatabase(object obj)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					switch (obj)
					{
						case Order order:
							command.CommandText = $"DELETE FROM [dbo].Orders WHERE IdOrder = {order.IdOrder}";
							break;
						case Menu menu:
							command.CommandText = $"DELETE FROM [dbo].Menu WHERE IdMenu = {menu.IdMenu}";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					command.CommandType = System.Data.CommandType.Text;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
			Console.WriteLine($"Removed: {xmlResult}");
		}

		public Client GetClient(string cardNumber)
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
		public IEnumerable<Order> GetServedOrders(int count)
		{
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].[GetServerdOrders]";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add("@Count", System.Data.SqlDbType.Int).Value = count;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Order o = new Order()
							{
								IdOrder = reader.GetInt32(0),
								Served = reader.GetDateTime(1),
								Client = new Client()
								{
									Id = reader.GetInt32(2),
									LastName = reader.GetString(3),
									FirstName = reader.GetString(4),
								}
							};
							o.Items[0] = new MenuItem()
							{
								Id = reader.GetInt32(5),
								Name = reader.GetString(6),
								Description = reader.GetString(7),
							};
							o.Items[1] = new MenuItem()
							{
								Id = reader.GetInt32(8),
								Name = reader.GetString(9),
								Description = reader.GetString(10),
							};
							o.Items[2] = new MenuItem()
							{
								Id = reader.GetInt32(11),
								Name = reader.GetString(12),
								Description = reader.GetString(13),
							};
							yield return o;
						}
					}
				}
			}
		}
		public Client GetOrders(string cardNumber)
		{
			// <root><Client IdClient=\"2\" FirstName=\"Mato      \" LastName=\"Vyskocany \" Balance=\"8.520000000000000e+002\"/><Order IdOrder=\"11\" ForDate=\"2018-01-28\" IdMenu=\"1005\" SoupName=\"Paradajková\" MealName=\"Rezen\" DesertName=\"Puding\"/><Order IdOrder=\"16\" ForDate=\"2018-01-28\" IdMenu=\"1006\" SoupName=\"Paradajková\" MealName=\"Kurca\" DesertName=\"Puding\"/><Order IdOrder=\"17\" ForDate=\"2018-01-28\" IdMenu=\"1006\" SoupName=\"Paradajková\" MealName=\"Kurca\" DesertName=\"Puding\"/><Order IdOrder=\"18\" ForDate=\"2018-01-30\" IdMenu=\"1014\" SoupName=\"Paradajková\" MealName=\"Rezen\" DesertName=\"Puding\"/></root>
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
