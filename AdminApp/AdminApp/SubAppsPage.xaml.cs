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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for SubAppsPage.xaml
	/// </summary>
	public partial class SubAppsPage : Page
	{
		private readonly DatabaseLayer db;
		public SubAppsPage(DatabaseLayer db)
		{
			InitializeComponent();
			this.db = db;
			UpdateData();
		}

		private void UpdateData()
		{
			List<MyApplication> apps = db.GetSubApps().ToList();
			appsDataGrid.ItemsSource = apps;
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
		}

		private void appsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{

			MyApplication subApp = appsDataGrid.SelectedItem as MyApplication;
			if (subApp == null)
				return;
			SubAppWindow appWindow = new SubAppWindow(subApp, db);
			appWindow.ShowDialog();
			UpdateData();
		}
	}
}
