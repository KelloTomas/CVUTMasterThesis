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
		private DatabaseLayer db;
		public MainWindow()
		{
			InitializeComponent();
			frame.Navigated += frame_Navigated;
			db = new DatabaseLayer();
		}

		private void ApplicationButtonClick(object sender, RoutedEventArgs e)
		{
			frame.Navigate(new SubAppsPage(db));
		}

		void frame_Navigated(object sender, NavigationEventArgs e)
		{
			frame.NavigationService.RemoveBackEntry();
		}

		private void ClientsButtonClick(object sender, RoutedEventArgs e)
		{
			frame.Navigate(new ClientsPage(db));
		}

		private void ListMenuButtonClick(object sender, RoutedEventArgs e)
		{
			frame.Navigate(new MenuPage(db));
		}

		private void OrdersButtonClick(object sender, RoutedEventArgs e)
		{
			frame.Navigate(new OrdersPage(db));
		}
	}
}
