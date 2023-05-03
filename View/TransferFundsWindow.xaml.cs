using Homework12.ViewModel;
using System.Windows;

namespace Homework12.View
{
    /// <summary>
    /// Interaction logic for TransferFundsWindow.xaml
    /// </summary>
    public partial class TransferFundsWindow : Window
    {
        public TransferFundsWindow()
        {
            InitializeComponent();
            DataContext = new DataManage();
        }
    }
}
