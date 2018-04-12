using DataLayer;
using DataLayer.Data;
using System;
using System.Linq;
using System.Windows;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MenuDetailWindow.xaml
	/// </summary>
	public partial class OrderDetailWindow : Window
	{
		private readonly DatabaseLayer _db;
		private readonly Client _forClient;

		public OrderDetailWindow(DatabaseLayer db, Client forClient)
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
				Client = _forClient,
				ForDate = date.DisplayDate,
				IdMenu = (menuDataGrid.SelectedItem as Menu).IdMenu,
				Served = null,
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

		private void date_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			LoadData((DateTime)date.SelectedDate);
		}
	}
}
