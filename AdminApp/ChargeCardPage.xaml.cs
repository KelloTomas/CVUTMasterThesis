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
	public partial class ChargeCardPage : Page
	{
		private readonly DatabaseLayer _db;
        private readonly Window _owner;
        public ChargeCardPage(Window owner, DatabaseLayer db)
		{
            _owner = owner;
            _db = db;
			InitializeComponent();
            ReloadData();
		}


		private void ChargeBtnClick(object sender, RoutedEventArgs e)
		{
            Client c = clientSelect.SelectedItem as Client;
            ChargeCardDetailWindow window = new ChargeCardDetailWindow(_db, c);
            window.Owner = _owner;
            window.ShowDialog();
            ReloadData();
        }

        private void ReloadData()
        {
            clientSelect.ItemsSource = _db.GetClients().ToList();
        }


        private void clientSelect_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChargeBtnClick(this, null);
        }
    }
}
