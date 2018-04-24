using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DataLayer.Data;

namespace DataLayer
{
    public class DatabaseLayer
    {
        private const bool _useStoredProcedure = false;
#pragma warning disable IDE1006 // Naming Styles
        private readonly string CONNECTION_STRING;// = "Data Source=DESKTOP-M5V50NA;Initial Catalog=CVUTdb;Persist Security Info=True;User ID=sa;Password=root";
#pragma warning restore IDE1006 // Naming Styles

        public DatabaseLayer()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            if (true)
            {
#pragma warning disable CS0162 // Unreachable code detected
                // http://www.phpmyadmin.co/index.php
                builder.Server = "sql7.freemysqlhosting.net";
                builder.UserID = "sql7233334";
                builder.Password = "gaQBuD7bXJ";
                builder.Database = "sql7233334";
				builder.SslMode = MySqlSslMode.None;
#pragma warning restore CS0162 // Unreachable code detected
            }
            else
			{
#pragma warning disable CS0162 // Unreachable code detected
				builder.Server = "localhost";
                builder.UserID = "root";
                builder.Password = "root";
                builder.Database = "cvutdb";
				builder.SslMode = MySqlSslMode.None;
#pragma warning restore CS0162 // Unreachable code detected
			}
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
        public void ServeOrder(Order o)
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"update orders set vydane = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where idorder = {o.IdOrder}";
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
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    switch (menuItem)
                    {
                        case Soup order:
                            command.CommandText = $"select idsoup id, name, description from soups";
                            break;
                        case Meal menu:
                            command.CommandText = $"select idmeal id, name, description from meals";
                            break;
                        case Desert d:
                            command.CommandText = $"select iddesert id, name, description from deserts";
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
                    command.CommandText = $"update applications set isrunning = {app.IsRunning}, applications.name = '{app.AppName}' where idapplication = {app.Id}";
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
                        command.CommandText = $"UPDATE devices SET IP = '{d.IP}', Port = {d.Port} Where IdDevice = {d.IdDevice}";
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
                    command.CommandText = "select idapplication id, isrunning, applicationtype.name typename, apps.name appname from applications apps join applicationtype on apps.idapplicationtype = applicationtype.idapplicationtype";
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
                        command.CommandText = $"select iddevice, ip, port from devices device where device.idapplication = {app.Id}";
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
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"select fordate, idmenu, soups.idsoup, soups.name soupname, meals.idmeal, meals.name mealname, deserts.iddesert, deserts.name desertname, price from menu join soups on menu.idsoup = soups.idsoup join meals on menu.idmeal = meals.idmeal join deserts on menu.iddesert = deserts.iddesert where fordate >= '{forDate?.ToString("yyyy-MM-dd")}'";
                    command.CommandType = System.Data.CommandType.Text;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Menu menu = new Menu
                            {
                                ForDate = reader.GetDateTime(0),
                                IdMenu = reader.GetInt32(1),
                                Price = reader.GetFloat(8)
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
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"select orders.fordate, orders.idmenu, soups.idsoup, soups.name soupname, meals.idmeal, meals.name mealname, deserts.iddesert, deserts.name desertname from orders join clients on orders.idclient = clients.idclient join menu on orders.idmenu = menu.idmenu and orders.fordate = menu.fordate join soups on menu.idsoup = soups.idsoup join meals on menu.idmeal = meals.idmeal join deserts on menu.iddesert = deserts.iddesert where cardnumber = {forClient.CardNumber} and isnull(orders.vydane)";
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
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    switch (o)
                    {
                        case Order order:
                            command.CommandText = $"INSERT INTO orders (fordate, idmenu, idclient, vydane) VALUES('{order.ForDate.ToString("yyyy-MM-dd")}', {order.IdMenu}, {order.Client.Id}, null)";
                            break;
                        case Menu menu:
                            command.CommandText = $"INSERT INTO menu (fordate, idsoup, idmeal, iddesert, price) VALUES('{menu.ForDate.ToString("yyyy-MM-dd")}', {menu.Items[0].Id}, {menu.Items[1].Id}, {menu.Items[2].Id}, {menu.Price})";
                            break;
                        case Client c:
                            if (c.Id == 0)
                                command.CommandText = $"INSERT INTO clients (firstname, lastname, cardnumber, balance) VALUES('{c.FirstName}', '{c.LastName}', '{c.CardNumber}', '{c.Balance.ToString(System.Globalization.CultureInfo.InvariantCulture)}')";
                            else
                                command.CommandText = $"UPDATE clients SET firstname = '{c.FirstName}', lastname = '{c.LastName}', balance = \"{c.Balance.ToString(System.Globalization.CultureInfo.InvariantCulture)}\", cardnumber = \"{c.CardNumber}\" Where idclient = \"{c.Id}\"";

							break;
                        case Soup s:
                            if (s.Id == 0)
                                command.CommandText = $"INSERT INTO soups (name, description) VALUES('{s.Name}', '{s.Description}')";
                            else
                                command.CommandText = $"UPDATE soups SET name='{s.Name}', description = '{s.Description}' WHERE idsoup = {s.Id}";
                            break;
                        case Meal s:
                            if (s.Id == 0)
                                command.CommandText = $"INSERT INTO meals (name, description) VALUES('{s.Name}', '{s.Description}')";
                            else
                                command.CommandText = $"UPDATE meals SET name='{s.Name}', description = '{s.Description}' WHERE idmeal = {s.Id}";
                            break;
                        case Desert s:
                            if (s.Id == 0)
                                command.CommandText = $"INSERT INTO deserts (name, description) VALUES('{s.Name}', '{s.Description}')";
                            else
                                command.CommandText = $"UPDATE deserts SET name='{s.Name}', description = '{s.Description}' WHERE iddesert = {s.Id}";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    command.CommandType = System.Data.CommandType.Text;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reader.GetString(0);
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
                            command.CommandText = $"DELETE FROM Orders WHERE IdOrder = {order.IdOrder}";
                            break;
                        case Menu menu:
                            command.CommandText = $"DELETE FROM Menu WHERE IdMenu = {menu.IdMenu}";
                            break;
                        case Client client:
                            command.CommandText = $"DELETE FROM Clients WHERE IdClient = {client.Id}";
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
                    command.CommandText = $"select * from clients where cardnumber = {cardNumber}";
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
        public IEnumerable<Client> GetClients()
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select * from clients";
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
                    if (_useStoredProcedure)
                    {
#pragma warning disable CS0162 // Unreachable code detected
                        command.CommandText = "GetServedOrders";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("Count", count);
#pragma warning restore CS0162 // Unreachable code detected
                    }
                    else
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = $"select orders.idorder, orders.vydane, clients.idclient, clients.lastname, clients.firstname, soups.*, meals.*, deserts.* from orders left join clients on orders.idclient = clients.idclient left join menu on orders.idmenu = menu.idmenu left join soups on menu.idsoup = soups.idsoup left join meals on menu.idmeal = meals.idmeal left join deserts on menu.iddesert = deserts.iddesert where vydane is not null order by vydane desc limit {count}";
                    }
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
                    command.CommandText = $"select orders.idorder, orders.idmenu, orders.fordate, vydane served, soups.name soupname, meals.name mealname, deserts.name desertname from orders left join menu on orders.fordate =  menu.fordate and orders.idmenu = menu.idmenu left join soups on menu.idsoup = soups.idsoup left join meals on menu.idmeal = meals.idmeal left join deserts on menu.iddesert = deserts.iddesert where orders.idclient = {c.Id}";
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
                                Served = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
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
        public IEnumerable<MenuItem> Get(object obj)
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    switch (obj)
                    {
                        case Soup s:
                            command.CommandText = $"select * from soups";
                            break;
                        case Meal m:
                            command.CommandText = $"select * from meals";
                            break;
                        case Desert d:
                            command.CommandText = $"select * from deserts";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    command.CommandType = System.Data.CommandType.Text;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MenuItem i = (MenuItem)Activator.CreateInstance(obj.GetType());
                            i.Id = reader.GetInt32(0);
                            i.Name = reader.GetString(1);
                            i.Description = reader.GetString(2);
                            yield return i;
                        }
                    }
                }
            }
            yield break;
        }
        public (bool, string) CreateOrder(Client client, Menu menu)
        {
            if (client.Balance >= menu.Price)
            {
                var o = new Order { Client = client, ForDate = menu.ForDate, IdMenu = menu.IdMenu };
                this.Add(o);
                return (true, "Objednané");
            }
            else
            {
                return (false, "Nedostatok penazí na účte");

            }
        }
        #endregion
    }
}
