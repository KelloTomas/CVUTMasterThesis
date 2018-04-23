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
	public partial class ClientsPage : Page
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
            ClientDetailWindow window = new ClientDetailWindow(_db, new Client());
            window.Owner = _owner;
            window.ShowDialog();
            ReloadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Client c = clientSelect.SelectedItem as Client;
            if(c!= null)
            {

            ClientDetailWindow window = new ClientDetailWindow(_db, c);
            window.Owner = _owner;
            window.ShowDialog();
            ReloadData();
            }
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            Client c = clientSelect.SelectedItem as Client;
            _db.RemoveFromDatabase(c);
            ReloadData();
        }
    }
}
