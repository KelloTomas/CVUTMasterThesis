using DataLayer;
using DataLayer.Data;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class ClientsPage
	{
		private readonly DatabaseLayer _db;
		private readonly Window _owner;
		public ClientsPage(Window owner, DatabaseLayer db)
		{
			_owner = owner;
			_db = db;
			InitializeComponent();
			ReloadData();
		}

		private void ReloadData()
		{
			clientSelect.ItemsSource = _db.GetClients().ToList();
		}

		private void ClientSelect_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			BtnEdit_Click(this, null);
		}

		private void BtnAdd_Click(object sender, RoutedEventArgs e)
		{
			new ClientDetailWindow(_db, new Client())
			{
				Owner = _owner
			}.ShowDialog();
			ReloadData();
		}

		private void BtnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (clientSelect.SelectedItem is Client c)
			{
				new ClientDetailWindow(_db, c)
				{
					Owner = _owner
				}.ShowDialog();
				ReloadData();
			}
		}
		private void BtnCharge_Click(object sender, RoutedEventArgs e)
		{
			if (clientSelect.SelectedItem is Client c)
			{
				new ChargeCardDetailWindow(_db, c)
				{
					Owner = _owner
				}.ShowDialog();
				ReloadData();
			}
		}
		private void BtnSetCard_Click(object sender, RoutedEventArgs e)
		{
			if (clientSelect.SelectedItem is Client c)
			{
				new SetCardDetailWindow(_db, c)
				{
					Owner = _owner
				}.ShowDialog();
				ReloadData();
			}
		}

		private void BtnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (clientSelect.SelectedItem is Client c)
			{
				_db.RemoveFromDatabase(c);
				ReloadData();
			}
		}


		private void DialogChargeMoney_Closing(object sender, DialogClosingEventArgs eventArgs)
		{
			if (Equals(eventArgs.Parameter, true))
			{
				if (clientSelect.SelectedItem is Client c)
				{
					c.Balance += float.Parse(Balance.Text.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);
					_db.Add(c);
					ReloadData();
				}
			}
			Balance.Text = string.Empty;
		}
	}
}
