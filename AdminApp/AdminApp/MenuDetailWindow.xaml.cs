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
	public partial class MenuDetailWindow : Window
	{
		private readonly DataLayer.DatabaseLayer _db;
		private readonly DataLayer.Data.Menu _menu;

		public MenuDetailWindow(DataLayer.DatabaseLayer db, DateTime forDate)
		{
			InitializeComponent();
			_db = db;
			_menu = new DataLayer.Data.Menu()
			{
				ForDate = forDate
			};
			LoadData();
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			_menu.Items[0] = soupsCombo.SelectedItem as DataLayer.Data.MenuItem;
			_menu.Items[1] = mealsCombo.SelectedItem as DataLayer.Data.MenuItem;
			_menu.Items[2] = desertsCombo.SelectedItem as DataLayer.Data.MenuItem;
			_db.Add(_menu);
			Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LoadData()
		{
			date.Content = _menu.ForDate;
			soupsCombo.ItemsSource = _db.GetTable(new Soup()).ToList();
			soupsCombo.SelectedIndex = 0;
			mealsCombo.ItemsSource = _db.GetTable(new Meal()).ToList();
			mealsCombo.SelectedIndex = 0;
			desertsCombo.ItemsSource = _db.GetTable(new Desert()).ToList();
			desertsCombo.SelectedIndex = 0;
		}
	}
}
