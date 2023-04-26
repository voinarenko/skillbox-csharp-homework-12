using System.IO;
using System.Reflection;
using System.Windows;

namespace Homework12.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "MaterialDesignThemes.Wpf.dll"));   // фикс 'material design' для MVVM
            Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "MaterialDesignColors.dll"));       // фикс 'material design' для MVVM
            InitializeComponent();
        }
    }
}
