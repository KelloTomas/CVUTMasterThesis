using DataLayer;
using DataLayer.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class OrdersPage : Page
	{
		private readonly DatabaseLayer db;
		private Client forClient;
		private List<Order> orders;
		public OrdersPage(DatabaseLayer db)
		{
			this.db = db;
			InitializeComponent();
			clientSelect.ItemsSource = db.GetClients().ToList();
		}

		private void LoadData()
		{
			orders = db.GetOrders(forClient.CardNumber).Orders.ToList();
			ordersDataGrid.ItemsSource = orders;
		}

		private void AddBtnClick(object sender, RoutedEventArgs e)
		{

		}

		private void RemoveBtnClick(object sender, RoutedEventArgs e)
		{
			Order m = (Order)ordersDataGrid.SelectedItem;
			//db.RemoveOrder(m.IdOrder);
			LoadData();
		}

		private void clientSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			forClient = clientSelect.SelectedItem as Client;
			LoadData();
		}
	}
}
