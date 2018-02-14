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
		private readonly DatabaseLayer db;
		private DateTime forDate;
		private List<DataLayer.Data.Menu> menu;
		public MenuPage(DatabaseLayer db)
		{
			this.db = db;
			InitializeComponent();
			forDate = DateTime.Now.Date;
			forDate = forDate.AddDays(-12);
			LoadData();
		}

		private void LoadData()
		{
			dateLabel.Content = forDate.Date;

			menu = db.GetMenu(forDate.Date).ToList();
			menuDataGrid.ItemsSource = menu;
		}

		private void AddBtnClick(object sender, RoutedEventArgs e)
		{

		}

		private void EditBtnClick(object sender, RoutedEventArgs e)
		{

		}

		private void PrevBtnClick(object sender, RoutedEventArgs e)
		{
			forDate = forDate.AddDays(-1);
			LoadData();
		}

		private void NextBtnClick(object sender, RoutedEventArgs e)
		{
			forDate = forDate.AddDays(1);
			LoadData();
		}

		private void menuDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
