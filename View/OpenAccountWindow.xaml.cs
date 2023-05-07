using Homework12.ViewModel;
using System.Windows;

namespace Homework12.View
{
    /// <summary>
    /// Interaction logic for OpenAccountWindow.xaml
    /// </summary>
    public partial class OpenAccountWindow : Window
    {
        public OpenAccountWindow()
        {
            InitializeComponent();
            DataContext = new DataManage();
        }
    }
}
