using DataLayer;
using DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class MenuPage : System.Windows.Controls.Page
    {
		private readonly DatabaseLayer _db;
		private DateTime _forDate;
		private List<Menu> _menu;
        private readonly Window _owner;
        public MenuPage(Window owner, DatabaseLayer db)
		{
            _owner = owner;
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
            menuWindow.Owner = _owner;
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
