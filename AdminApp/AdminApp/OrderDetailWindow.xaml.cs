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
using System.Windows.Shapes;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MenuDetailWindow.xaml
	/// </summary>
	public partial class OrderDetailWindow : Window
	{
		private readonly DataLayer.DatabaseLayer _db;
		private readonly Client _forClient;

		public OrderDetailWindow(DataLayer.DatabaseLayer db, Client forClient)
		{
			InitializeComponent();
			_db = db;
			_forClient = forClient;
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			if (menuDataGrid.SelectedIndex == -1)
			{
				MessageBox.Show("Select menu to order", "Warning");
				return;
			}
			Order order = new Order{
				Client = _forClient.Id,
				ForDate = date.DisplayDate,
				IdMenu = (menuDataGrid.SelectedItem as DataLayer.Data.Menu).IdMenu,
				Served = false,
			};
			_db.Add(order);
			Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LoadData(DateTime forDate)
		{
			menuDataGrid.ItemsSource = _db.GetMenu(forDate.Date).ToList();
		}

		private void date_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if( DateTime.TryParse(date.Text, out DateTime t))
				LoadData(t);
		}

		private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			LoadData((DateTime)date.SelectedDate);
		}
	}
}
