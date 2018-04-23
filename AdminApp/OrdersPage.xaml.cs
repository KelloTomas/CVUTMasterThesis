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
    public partial class OrdersPage : Page
    {
        private readonly DatabaseLayer _db;
        private Client _forClient;
        private List<Order> _orders;
        private readonly Window _owner;
        public OrdersPage(Window owner, DatabaseLayer db)
        {
            _owner = owner;
            _db = db;
            InitializeComponent();
            clientSelect.ItemsSource = db.GetClients().ToList();
        }

        private void LoadData()
        {
            if (_forClient == null)
            {
                ordersDataGrid.ItemsSource = null;
            }
            else
            {

                var client = _db.GetOrders(_forClient.CardNumber);
                _orders = client.Orders.ToList();
                foreach (Order o in _orders)
                {
                    o.Client = client;
                }
                ordersDataGrid.ItemsSource = _orders;
            }
        }

        private void AddBtnClick(object sender, RoutedEventArgs e)
        {
            if (_forClient == null)
            {
                MessageBox.Show("Nevybrali ste klienta");
            }
            else
            {
                OrderDetailWindow menuWindow = new OrderDetailWindow(_db, _forClient);
                menuWindow.Owner = _owner;
                menuWindow.ShowDialog();
                LoadData();
            }
        }

        private void RemoveBtnClick(object sender, RoutedEventArgs e)
        {
            if (ordersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Nevybrali ste objednavku");
            }
            else
            {
                Order o = ordersDataGrid.SelectedItem as Order;
                _db.RemoveFromDatabase(o);
                LoadData();
            }
        }

        private void ClientSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _forClient = clientSelect.SelectedItem as Client;
            LoadData();
        }
    }
}
