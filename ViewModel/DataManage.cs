using Homework12.Model;
using Homework12.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Prism.Commands;
using RelayCommand = CommunityToolkit.Mvvm.Input.RelayCommand;

namespace Homework12.ViewModel
{
    public class DataManage: INotifyPropertyChanged
    {
        /// <summary>
        /// Список всех клиентов
        /// </summary>
        private List<Client> _allClients = DataBank.GetAllClients();
        public List<Client> AllClients
        {
            get => _allClients;
            set
            {
                _allClients = value;
                OnPropertyChanged("AllClients");
            }
        }

        /// <summary>
        /// Список всех счетов
        /// </summary>
        private List<Account> _allAccounts = DataBank.GetAllAccounts();
        public List<Account> AllAccounts
        {
            get => _allAccounts;
            set
            {
                _allAccounts = value;
                OnPropertyChanged("AllAccounts");
            }
        }

        public string? ClientFirstName;
        public string? ClientLastName;

        #region Команды опреаций с БД

        private RelayCommand? _addNewClient;
        public RelayCommand AddNewClient
        {
            get
            {
                return _addNewClient ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    var resultStr = "";
                    if (ClientFirstName == null || ClientFirstName.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControl(window, "Box");
                    }
                    else
                    {
                        resultStr = DataBank.AddClient(ClientFirstName, ClientLastName);
                    }
                });
            }
        }
        

        #endregion

        #region Команды открытия окон

        /// <summary>
        /// Команда на открытие окна добавления нового клиента
        /// </summary>
        private DelegateCommand? _openAddNewClientWin;
        public DelegateCommand OpenAddNewClientWin =>
            _openAddNewClientWin ??= new DelegateCommand(OpenAddNewClientWindowMethod);

        /// <summary>
        /// Команда на открытие окна пополнения счёта
        /// </summary>
        private DelegateCommand? _openAddFundsWin;
        public DelegateCommand OpenAddFundsWin =>
            _openAddFundsWin ??= new DelegateCommand(OpenAddFundsWindowMethod);

        /// <summary>
        /// Команда на открытие окна перевода средств
        /// </summary>
        private DelegateCommand? _openTransferFundsWin;
        public DelegateCommand OpenTransferFundsWin =>
            _openTransferFundsWin ??= new DelegateCommand(OpenTransferFundsWindowMethod);

        #endregion

        #region Методы открытия окон

        /// <summary>
        /// Метод открытия окна добавления нового клиента
        /// </summary>
        private static void OpenAddNewClientWindowMethod()
        {
            var newClientWindow = new AddNewClientWindow();
            SetCenterPositionAndOpen(newClientWindow);
        }

        /// <summary>
        /// Метод открытия окна пополнения счёта
        /// </summary>
        private static void OpenAddFundsWindowMethod()
        {
            var newFundWindow = new AddFundsWindow();
            SetCenterPositionAndOpen(newFundWindow);
        }

        /// <summary>
        /// Метод открытия окна перевода средств
        /// </summary>
        private static void OpenTransferFundsWindowMethod()
        {
            var newTransferWindow = new TransferFundsWindow();
            SetCenterPositionAndOpen(newTransferWindow);
        }

        /// <summary>
        /// Установка общих параметров для диалоговых окон
        /// </summary>
        /// <param name="window">окно</param>
        private static void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        
        #endregion

        #region Вспомогательные методы

        public void SetRedBlockControl(Window window, string blockName)
        {
            var block = window.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }

        #endregion

        #region INofifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
