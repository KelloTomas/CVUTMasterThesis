using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DataLayer.Data;

namespace DataLayer
{
	public class DatabaseLayer
	{
		private readonly string CONNECTION_STRING;// = "Data Source=DESKTOP-M5V50NA;Initial Catalog=CVUTdb;Persist Security Info=True;User ID=sa;Password=root";

		public DatabaseLayer()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
			builder.Server = "localhost";
			builder.UserID = "root";
			builder.Password = "root";
			builder.Database = "cvutdb";
			CONNECTION_STRING = builder.ToString();
		}

		#region private methods...
		private Client GetClientInt(MySqlDataReader reader)
		{
			Client client = new Client
			{
				Id = reader.GetInt32(0),
				FirstName = reader.GetString(1),
				LastName = reader.GetString(2),
				Balance = reader.GetFloat(3),
				CardNumber = reader.GetString(4),
				Orders = new List<Order>()
			};
			return client;
		}
		#endregion

		#region public methods...
		public Menu ServeOrder(int clientId)
		{
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ServeOrder";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.AddWithValue("ClientId", clientId);
					using (MySqlDataReader reader = command.ExecuteReader())
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
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					switch (menuItem)
					{
						case Soup order:
							command.CommandText = $"SELECT IdSoup Id, Name, Description FROM Soups";
							break;
						case Meal menu:
							command.CommandText = $"SELECT IdMeal Id, Name, Description FROM Meals";
							break;
						case Desert d:
							command.CommandText = $"SELECT IdDesert Id, Name, Description FROM Deserts";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							yield return new MenuItem()
							{
								Id = reader.GetInt32(0),
								Name = reader.GetString(1),
								Description = reader.GetString(2)
							};
						}
					}
				}
			}
		}
		public void UpdateSubApp(MyApplication app)
		{
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = $"UPDATE Applications SET IsRunning = {app.IsRunning}, Applications.Name = '{app.AppName}' Where IdApplication = {app.Id}";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
						}
					}
				}
				foreach (Device d in app.Devices)
				{
					using (MySqlCommand command = connection.CreateCommand())
					{
						command.CommandText = $"UPDATE Devices SET IP = '{d.IP}', Port = {d.Port} Where IdDevice = {d.IdDevice}";
						command.CommandType = System.Data.CommandType.Text;
						using (MySqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
							}
						}
					}
				}
			}
		}
		public IEnumerable<MyApplication> GetSubApps()
		{
			List<MyApplication> apps = new List<MyApplication>();
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "SELECT IdApplication Id, IsRunning, ApplicationType.Name TypeName, Apps.Name AppName FROM Applications Apps JOIN ApplicationType ON Apps.IdApplicationType = ApplicationType.IdApplicationType";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							apps.Add(new MyApplication
							{
								Id = reader.GetInt32(0),
								IsRunning = reader.GetBoolean(1),
								TypeName = reader.GetString(2),
								AppName = reader.GetString(3),
								Devices = new List<Device>()
							});
						}
					}
				}

				foreach (var app in apps)
				{

					using (MySqlCommand command = connection.CreateCommand())
					{
						command.CommandText = $"SELECT IdDevice, IP, Port FROM Devices Device WHERE Device.IdApplication = {app.Id}";
						command.CommandType = System.Data.CommandType.Text;
						using (MySqlDataReader reader2 = command.ExecuteReader())
						{
							while (reader2.Read())
							{
								app.Devices.Add(new Device()
								{
									IdDevice = reader2.GetInt32(0),
									IP = reader2.GetString(1),
									Port = reader2.GetInt32(2),
								});
							}
						}
					}
				}
			}
			return apps;
		}

		public IEnumerable<Menu> GetMenu(DateTime? forDate = null)
		{
			if (forDate == null)
				forDate = DateTime.Now.Date;
			string xmlResult = "";
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = $"select ForDate, IdMenu, Soups.IdSoup, Soups.Name SoupName, Meals.IdMeal, Meals.Name MealName, Deserts.IdDesert, Deserts.Name DesertName from Menu JOIN Soups on menu.IdSoup = Soups.IdSoup JOIN Meals ON menu.IdMeal = Meals.IdMeal JOIN Deserts ON menu.IdDesert = Deserts.IdDesert where ForDate = '{forDate?.ToString("yyyy-MM-dd")}'";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Menu menu = new Menu
							{
								ForDate = reader.GetDateTime(0),
								IdMenu = reader.GetInt32(1)
							};
							menu.Items[0] = new MenuItem { Id = reader.GetInt32(2), Name = reader.GetString(3) };
							menu.Items[1] = new MenuItem { Id = reader.GetInt32(4), Name = reader.GetString(5) };
							menu.Items[2] = new MenuItem { Id = reader.GetInt32(6), Name = reader.GetString(7) };
							yield return menu;
						}
					}
				}
			}
		}
		public IEnumerable<Menu> GetMenu(Client forClient)
		{
			if (forClient == null)
				yield break;
			string xmlResult = "";
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = $"select Orders.ForDate, Orders.IdMenu, Soups.IdSoup, Soups.Name SoupName, Meals.IdMeal, Meals.Name MealName, Deserts.IdDesert, Deserts.Name DesertName from Orders JOIN Clients ON Orders.IdClient = Clients.IdClient JOIN Menu ON Orders.IdMenu = Menu.IdMenu AND Orders.ForDate = Menu.ForDate JOIN Soups on menu.IdSoup = Soups.IdSoup JOIN Meals ON menu.IdMeal = Meals.IdMeal JOIN Deserts ON menu.IdDesert = Deserts.IdDesert where CardNumber = {forClient.CardNumber} and isnull(Orders.Vydane)";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Menu menu = new Menu
							{
								ForDate = reader.GetDateTime(0),
								IdMenu = reader.GetInt32(1)
							};
							menu.Items[0].Name = reader.GetString(2);
							menu.Items[1].Name = reader.GetString(3);
							menu.Items[2].Name = reader.GetString(4);
							yield return menu;
						}
					}
				}
			}
		}

		public void Add(Object o)
		{
			string xmlResult = "";
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					switch (o)
					{
						case Order order:
							command.CommandText = $"INSERT INTO Orders (ForDate, IdMenu, IdClient, Vydane) VALUES('{order.ForDate.ToString("yyyy-MM-dd")}', {order.IdMenu}, {order.Client.Id}, null)";
							break;
						case Menu menu:
							command.CommandText = $"INSERT INTO Menu (ForDate, IdSoup, IdMeal, IdDesert) VALUES('{menu.ForDate.ToString("yyyy-MM-dd")}', {menu.Items[0].Id}, {menu.Items[1].Id}, {menu.Items[2].Id})";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							xmlResult += reader.GetString(0);
						}
					}
				}
			}
		}

		public void RemoveFromDatabase(object obj)
		{
			string xmlResult = "";
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
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
					using (MySqlDataReader reader = command.ExecuteReader())
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
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = $"SELECT * FROM Clients WHERE CardNumber = {cardNumber}";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							return GetClientInt(reader);
						}
					}
				}
			}
			return null;
		}

		public void UpdateClient(Client client)
		{
			string xmlResult = "";
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = $"UPDATE Clients SET FirstName = {client.FirstName}, LastName = {client.LastName}, Balance = {client.Balance}, CardNumber = {client.CardNumber} Where IdClient = {client.Id}";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
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
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "SELECT * FROM Clients";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							yield return GetClientInt(reader);
						}
					}
				}
			}
		}
		public IEnumerable<Order> GetServedOrders(int count)
		{
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "GetServedOrders";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.AddWithValue("Count", count);
					using (MySqlDataReader reader = command.ExecuteReader())
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
			Client c = GetClient(cardNumber);
			if (c == null)
				return null;
			// <root><Client IdClient=\"2\" FirstName=\"Mato      \" LastName=\"Vyskocany \" Balance=\"8.520000000000000e+002\"/><Order IdOrder=\"11\" ForDate=\"2018-01-28\" IdMenu=\"1005\" SoupName=\"Paradajková\" MealName=\"Rezen\" DesertName=\"Puding\"/><Order IdOrder=\"16\" ForDate=\"2018-01-28\" IdMenu=\"1006\" SoupName=\"Paradajková\" MealName=\"Kurca\" DesertName=\"Puding\"/><Order IdOrder=\"17\" ForDate=\"2018-01-28\" IdMenu=\"1006\" SoupName=\"Paradajková\" MealName=\"Kurca\" DesertName=\"Puding\"/><Order IdOrder=\"18\" ForDate=\"2018-01-30\" IdMenu=\"1014\" SoupName=\"Paradajková\" MealName=\"Rezen\" DesertName=\"Puding\"/></root>
			using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
			{
				connection.Open();
				using (MySqlCommand command = connection.CreateCommand())
				{
					command.CommandText = $"SELECT Orders.IdOrder, Orders.IdMenu, Orders.ForDate, Vydane Served, Soups.Name SoupName, Meals.Name MealName, Deserts.Name DesertName FROM Orders LEFT JOIN Menu ON Orders.ForDate =  Menu.ForDate AND Orders.IdMenu = Menu.IdMenu LEFT JOIN Soups ON Menu.IdSoup = Soups.IdSoup LEFT JOIN Meals ON Menu.IdMeal = Meals.IdMeal LEFT JOIN Deserts ON Menu.IdDesert = Deserts.IdDesert WHERE Orders.IdClient = {c.Id}";
					command.CommandType = System.Data.CommandType.Text;
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{

							Order o = new Order
							{
								IdOrder = reader.GetInt32(0),
								IdMenu = reader.GetInt32(1),
								ForDate = reader.GetDateTime(2),
								Served = reader.IsDBNull(3)?(DateTime?)null: reader.GetDateTime(3),
							};

							o.Items[0] = new MenuItem { Name = reader.GetString(4) };
							o.Items[1] = new MenuItem { Name = reader.GetString(5) };
							o.Items[2] = new MenuItem { Name = reader.GetString(6) };
							c.Orders.Add(o);
						}
					}
				}
			}
			return c;
		}
		#endregion
	}
}
