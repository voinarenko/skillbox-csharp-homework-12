using Homework12.ViewModel;
using System.Windows;

namespace Homework12.View
{
    /// <summary>
    /// Interaction logic for AddFundsWindow.xaml
    /// </summary>
    public partial class AddFundsWindow : Window
    {
        public AddFundsWindow()
        {
            InitializeComponent();
            DataContext = new DataManage();
        }
    }
}
