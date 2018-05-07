using DataLayer;
using DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DatabaseLayer _db;
		public MainWindow()
		{
			InitializeComponent();
			_db = new DatabaseLayer();
			SetSelected("Close");
		}

		private void SetSelected(string name)
		{
			App.IsEnabled = true;
			Client.IsEnabled = true;
			Menu.IsEnabled = true;
			Order.IsEnabled = true;
			Soups.IsEnabled = true;
			Meals.IsEnabled = true;
			Deserts.IsEnabled = true;
			TerminalService.IsEnabled = true;
			switch (name)
			{
				case "App":
					frame.Content = new SubAppsPage(this, _db);
					App.IsEnabled = false;
					break;
				case "Client":
					frame.Content = (new ClientsPage(this, _db));
					Client.IsEnabled = false;
					break;
				case "Menu":
					frame.Content = (new MenuPage(this, _db));
					Menu.IsEnabled = false;
					break;
				case "Order":
					frame.Content = (new OrdersPage(this, _db));
					Order.IsEnabled = false;
					break;
				case "Soups":
					frame.Content = (new MenuItemPage(this, _db, new Soup()));
					Soups.IsEnabled = false;
					break;
				case "Meals":
					frame.Content = (new MenuItemPage(this, _db, new Meal()));
					Meals.IsEnabled = false;
					break;
				case "Deserts":
					frame.Content = (new MenuItemPage(this, _db, new Desert()));
					Deserts.IsEnabled = false;
					break;
				case "TerminalService":
					frame.Content = (new TerminalServicePage(this, _db));
					TerminalService.IsEnabled = false;
					break;
				case "Close":
					frame.Content = new StartPage();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void MenuButtonClick(object sender, RoutedEventArgs e)
		{
			SetSelected(((Button)e.Source).Name);
		}
	}
}