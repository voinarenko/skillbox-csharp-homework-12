using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Homework12.ViewModel;

namespace Homework12.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListView? AllClientsView;
        public static ListView? AllAccountsView;
        public MainWindow()
        {
            Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "MaterialDesignThemes.Wpf.dll"));   // фикс 'material design' для MVVM
            Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "MaterialDesignColors.dll"));       // фикс 'material design' для MVVM
            InitializeComponent();
            DataContext = new DataManage();
            AllClientsView = ViewAllClients;
            AllAccountsView = ViewAllAccounts;
        }

        private void ViewAllClients_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (DataManage)DataContext;
            vm.RefreshAccounts.Execute();
        }
    }
}
