using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServerApp.Data
{
	public class DatabaseLayer
	{
		string ConnectionString = "Data Source=DESKTOP-M5V50NA;Initial Catalog=CVUTdb;Persist Security Info=True;User ID=sa;Password=root";
		private Menu GetOrderInt(XElement order)
		{
			Menu o = new Menu();
			o.IdMenu = (int)order.Attribute("IdMenu");
			o.ForDate = (DateTime)order.Attribute("ForDate");
			o.SoupName = (string)order.Attribute("SoupName");
			o.MealName = (string)order.Attribute("MealName");
			o.DesertName = (string)order.Attribute("DesertName");
			return o;
		}

		private Device GetDevice(XElement deviceXML)
		{
			Device device = new Device();
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
				Balance = (double)xNode.Attribute("Balance"),
				Orders = new List<Menu>()
			};
			return client;
		}

		public IEnumerable<MyApplication> GetSubApps()
		{
			// <root><App IsRunning=\"1\" Name=\"Inform    \"><Device IP=\"127.0.0.1\" Port=\"15001\"/></App><App IsRunning=\"0\" Name=\"Order     \"><Device IP=\"127.0.0.1\" Port=\"15002\"/></App><App IsRunning=\"0\" Name=\"Serve     \"><Device IP=\"127.0.0.1\" Port=\"15003\"/><Device IP=\"127.0.0.1\" Port=\"15004\"/></App></root>
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
							IsRunning = (bool)appXml.Attribute("IsRunning"),
							Name = (string)appXml.Attribute("Name"),
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

		public IEnumerable<Menu> GetMenu()
		{
			string xmlResult = "";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "[dbo].GetMenu";
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
			{
				return null;
			}


			Client client = null;
			XElement root = XElement.Parse(xmlResult);
			foreach (XElement ClientAndOrder in root.Elements())
			{
				// EvCislo = "217831" Titul = "" Jmeno = "Samuel" Prijmeni = "Abaffy" OCS = "u461675" RCS = "" IdDruh = "2" IdAlt = "1" KodAlt = "1" CasVydeje = "2017-01-27T14:35:04.223"
				// Ve verzi 17.1 doplnena jeste velikost porce
				// EvCislo="46140" Titul="" Jmeno="Mária" Prijmeni="Kavanová" OCS="Z10678" RCS="9056244494" VelPorce="3" IdDruh="2" IdAlt="1" KodAlt="1" CasVydeje="2017-03-08T09:36:58.653"
				// Vo verzi 18.1 doplnene PC u stornovatelných objednávek
				// EvCislo="46140" Titul="" Jmeno="Mária" Prijmeni="Kavanová" OCS="Z10678" RCS="9056244494" VelPorce="3" IdDruh="2" IdAlt="1" KodAlt="1" CasVydeje="2017-03-08T09:36:58.653" PC ="222901"
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
	}
}
