using Homework12.ViewModel;
using System.Windows;

namespace Homework12.View
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow(string text)
        {
            InitializeComponent();
            DataContext = new DataManage();
            MessageText.Text = text;
        }
    }
}
