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
	public partial class SubAppWindow : Window
	{
		private MyApplication app;
		private readonly DatabaseLayer db;
		public SubAppWindow(MyApplication app, DatabaseLayer db)
		{
			InitializeComponent();
			this.app = app;
			this.db = db;
			AppTypeName.Content = app.TypeName;
			AppId.Text = app.Id.ToString();
			AppIsRunning.IsChecked = app.IsRunning;
			AppName.Text = app.AppName;
			AppDevicesDataGrid.ItemsSource = app.Devices;
			//AppDevicesDataGrid.Columns[0].Visibility = Visibility.Collapsed;
		}

		private void CancelBtnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void SaveBtnClick(object sender, RoutedEventArgs e)
		{
			app.IsRunning = AppIsRunning.IsChecked == true;
			app.AppName = AppName.Text;
			db.UpdateSubApp(app);
			Close();
		}
	}
}
