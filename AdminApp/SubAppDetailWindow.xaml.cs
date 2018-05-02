using DataLayer;
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
using System.Windows.Shapes;

namespace AdminApp
{
    /// <summary>
    /// Interaction logic for SubAppWindow.xaml
    /// </summary>
    public partial class SubAppDetailWindow : Window
    {
        private MyApplication _app;
        private readonly DatabaseLayer _db;
        public SubAppDetailWindow(MyApplication app, DatabaseLayer db)
        {
            InitializeComponent();
            _app = app;
            _db = db;

            ReloadData();
            //AppDevicesDataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void ReloadData()
        {
            AppTypeName.Content = _app.TypeName;
            AppId.Text = _app.Id.ToString();
            AppIsRunning.IsChecked = _app.IsRunning;
            AppName.Text = _app.AppName;
            AppDevicesDataGrid.ItemsSource = _app.Devices;
            var servingPlaces = _db.Get(new ServingPlace()).ToList();
            servingPlaceCombo.ItemsSource = servingPlaces;
            if (_app.ServingPlace != null)
                servingPlaceCombo.SelectedItem = servingPlaces.FirstOrDefault(o => o.Id == _app.ServingPlace.Id);
        }

        private void CancelBtnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            _app.IsRunning = AppIsRunning.IsChecked == true;
            _app.AppName = AppName.Text;
            _app.ServingPlace = servingPlaceCombo.SelectedItem as ServingPlace;
            _db.UpdateSubApp(_app);
            Close();
        }
    }
}
