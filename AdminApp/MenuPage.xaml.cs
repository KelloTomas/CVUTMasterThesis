using DataLayer;
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
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class MenuPage : Page
	{
		private readonly DatabaseLayer _db;
		private DateTime _forDate;
		private List<DataLayer.Data.Menu> _menu;
		public MenuPage(DatabaseLayer db)
		{
			_db = db;
			InitializeComponent();
			_forDate = DateTime.Now.Date;
			LoadData();
		}

		private void LoadData()
		{
			dateLabel.Content = _forDate.Date;

			_menu = _db.GetMenu(_forDate.Date).ToList();
			menuDataGrid.ItemsSource = _menu;
		}

		private void AddBtnClick(object sender, RoutedEventArgs e)
		{
			MenuDetailWindow menuWindow = new MenuDetailWindow(_db, _forDate);
			menuWindow.ShowDialog();
			LoadData();
		}

		private void RemoveBtnClick(object sender, RoutedEventArgs e)
		{
			DataLayer.Data.Menu m = (DataLayer.Data.Menu)menuDataGrid.SelectedItem;
			_db.RemoveFromDatabase(m);
			LoadData();
		}

		private void PrevBtnClick(object sender, RoutedEventArgs e)
		{
			_forDate = _forDate.AddDays(-1);
			LoadData();
		}

		private void NextBtnClick(object sender, RoutedEventArgs e)
		{
			_forDate = _forDate.AddDays(1);
			LoadData();
		}

		private void menuDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
