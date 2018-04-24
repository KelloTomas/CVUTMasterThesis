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
	public partial class MenuItemDetailWindow : Window
	{
		private readonly DatabaseLayer _db;
		private readonly MenuItem _menuItem;

		public MenuItemDetailWindow(DatabaseLayer db, MenuItem menuItem)
		{
			InitializeComponent();
			_menuItem = menuItem;
			_db = db;
			LoadData();
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			_menuItem.Name = ItemName.Text;
			_menuItem.Description = Description.Text;
			_db.Add(_menuItem);
			Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LoadData()
		{
			if (_menuItem.Id != 0)
			{
				ItemName.Text = _menuItem.Name;
				ItemName.IsReadOnly = true;
			}
			Description.Text = _menuItem.Description;
		}
	}
}
