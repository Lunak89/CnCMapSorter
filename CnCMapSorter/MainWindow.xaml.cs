using System.Windows;

namespace CnCMapSorter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			MoveCnCMapFiles.IntoNamedSubFolders();
		}
	}
}
