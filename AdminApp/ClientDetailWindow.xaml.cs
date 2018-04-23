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
    public partial class ClientDetailWindow : Window
    {
        private readonly DatabaseLayer _db;
        private readonly Client _client;

        public ClientDetailWindow(DatabaseLayer db, Client client)
        {
            InitializeComponent();
            _client = client;
            _db = db;
            LoadData();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _client.FirstName = FirstName.Text;
            _client.LastName = LastName.Text;
            _client.CardNumber = CardNumber.Text;
            _db.Add(_client);
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoadData()
        {
            FirstName.Text = _client.FirstName;
            LastName.Text = _client.LastName;
            CardNumber.Text = _client.CardNumber;
        }
    }
}
