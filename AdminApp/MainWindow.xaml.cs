﻿using DataLayer;
using DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseLayer db;
        public MainWindow()
        {
            InitializeComponent();
            frame.Navigated += frame_Navigated;
            db = new DatabaseLayer();
        }

        private void SetSelected(string name)
        {
            App.IsEnabled = true;
            Client.IsEnabled = true;
            Menu.IsEnabled = true;
            Order.IsEnabled = true;
            Soups.IsEnabled = true;
            Meals.IsEnabled = true;
            Deserts.IsEnabled = true;
            switch (name)
            {
                case "App":
                    App.IsEnabled = false;
                    break;
                case "Client":
                    Client.IsEnabled = false;
                    break;
                case "Menu":
                    Menu.IsEnabled = false;
                    break;
                case "Order":
                    Order.IsEnabled = false;
                    break;
                case "Soups":
                    Soups.IsEnabled = false;
                    break;
                case "Meals":
                    Meals.IsEnabled = false;
                    break;
                case "Deserts":
                    Deserts.IsEnabled = false;
                    break;
            }
        }

        private void ApplicationButtonClick(object sender, RoutedEventArgs e)
        {
            SetSelected(((Button)e.Source).Name);
            frame.Navigate(new SubAppsPage(this, db));
        }

        void frame_Navigated(object sender, NavigationEventArgs e)
        {
            frame.NavigationService.RemoveBackEntry();
        }

        private void ClientsButtonClick(object sender, RoutedEventArgs e)
        {
            SetSelected(((Button)e.Source).Name);
            frame.Navigate(new ClientsPage(this, db));
        }

        private void ListMenuButtonClick(object sender, RoutedEventArgs e)
        {
            SetSelected(((Button)e.Source).Name);
            frame.Navigate(new MenuPage(this, db));
        }

        private void OrdersButtonClick(object sender, RoutedEventArgs e)
        {
            SetSelected(((Button)e.Source).Name);
            frame.Navigate(new OrdersPage(this, db));
        }

        private void MenuItemButtonClick(object sender, RoutedEventArgs e)
        {
            SetSelected(((Button)e.Source).Name);
            switch (((Button)e.Source).Name)
            {
                case "Soups":
                    frame.Navigate(new MenuItemPage(this, db, new Soup()));
                    break;
                case "Meals":
                    frame.Navigate(new MenuItemPage(this, db, new Meal()));
                    break;
                case "Deserts":
                    frame.Navigate(new MenuItemPage(this, db, new Desert()));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
