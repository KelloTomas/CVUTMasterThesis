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
    public partial class MenuDetailWindow : Window
    {
        private readonly DatabaseLayer _db;
        private readonly Menu _menu;

        public MenuDetailWindow(DatabaseLayer db, DateTime forDate)
        {
            InitializeComponent();
            _db = db;
            _menu = new Menu()
            {
                ForDate = forDate
            };
            LoadData();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _menu.Items[0] = soupsCombo.SelectedItem as MenuItem;
            _menu.Items[1] = mealsCombo.SelectedItem as MenuItem;
            _menu.Items[2] = desertsCombo.SelectedItem as MenuItem;
            if (string.IsNullOrWhiteSpace(price.Text))
                price.Text = "0";
            _menu.Price = float.Parse(price.Text);
            _menu.ServingPlace = servingPlaceCombo.SelectedItem as ServingPlace;
            _db.Add(_menu);
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoadData()
        {
            date.Content = _menu.ForDate;
            soupsCombo.ItemsSource = _db.Get(new Soup()).ToList();
            soupsCombo.SelectedIndex = 0;
            mealsCombo.ItemsSource = _db.Get(new Meal()).ToList();
            mealsCombo.SelectedIndex = 0;
            desertsCombo.ItemsSource = _db.Get(new Desert()).ToList();
            desertsCombo.SelectedIndex = 0;
            servingPlaceCombo.ItemsSource = _db.Get(new ServingPlace()).ToList();
            servingPlaceCombo.SelectedIndex = 0;
        }
    }
}
