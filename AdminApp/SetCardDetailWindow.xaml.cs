﻿using DataLayer;
using DataLayer.Data;
using System;
using System.Linq;
using System.Windows;

namespace AdminApp
{
    /// <summary>
    /// Interaction logic for MenuDetailWindow.xaml
    /// </summary>
    public partial class SetCardDetailWindow : Window
    {
        private readonly DatabaseLayer _db;
        private readonly Client _client;

        public SetCardDetailWindow(DatabaseLayer db, Client client)
        {
            InitializeComponent();
            _client = client;
            _db = db;
            LoadData();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
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
            FullName.Content = _client.FullName;
			CardNumber.Text = _client.CardNumber;

		}
    }
}
