using DataLayer;
using DataLayer.Data;
using System;
using System.Linq;
using System.Windows;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class TerminalServicePage : System.Windows.Controls.Page
	{
		private readonly DatabaseLayer _db;
		private readonly Window _owner;
		private MenuItem _item;
		public TerminalServicePage(Window owner, DatabaseLayer db, DataLayer.Data.MenuItem item)
		{
			_item = item;
			_owner = owner;
			_db = db;
			InitializeComponent();
			switch (item)
			{
				case Soup s:
					MyTitle.Content = $"Zoznam polievok";
					break;
				case Meal s:
					MyTitle.Content = $"Zoznam hl. jedál";
					break;
				case Desert s:
					MyTitle.Content = $"Zoznam dezertov";
					break;
			}
			ReloadData();
		}

		private void ReloadData()
		{
		}

		private void ClientSelect_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			BtnEdit_Click(this, null);
		}

		private void BtnAdd_Click(object sender, RoutedEventArgs e)
		{
			MenuItemDetailWindow window = new MenuItemDetailWindow(_db, (MenuItem)Activator.CreateInstance(_item.GetType())) { Owner = _owner };
			window.ShowDialog();
			ReloadData();
		}

		private void BtnEdit_Click(object sender, RoutedEventArgs e)
		{
			/*
			object c = clientSelect.SelectedItem;
			if (c != null)
			{
				MenuItemDetailWindow window = new MenuItemDetailWindow(_db, c as MenuItem);
				window.Owner = _owner;
				window.ShowDialog();
				ReloadData();
			}
			*/
		}
		private void BtnRemove_Click(object sender, RoutedEventArgs e)
		{
			/*
			if (clientSelect.SelectedItem is MenuItem)
			{
				_db.RemoveFromDatabase(clientSelect.SelectedItem);
				ReloadData();
			}
			*/
		}
	}
}
