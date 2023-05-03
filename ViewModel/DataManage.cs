using System;
using DevExpress.Mvvm;
using Homework12.Model;
using Homework12.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace Homework12.ViewModel
{
    public class DataManage : ViewModelBase, INotifyPropertyChanged
    {
        #region Переменные

        // текущее окно
        private static Window? _currentWindow;
        
        #endregion

        #region Свойства

        // список всех клиентов
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

        // список всех счетов
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

        // свойства клиентов
        public string? ClientFirstName { get; set; }
        public string? ClientLastName { get; set; }

        // свойства счетов
        public decimal AccountSum { get; set; }
        public Account? AccountTarget { get; set; }

        // свойства для выделенных элементов
        public static Client? SelectedClient { get; set; }
        public static Account? SelectedAccount { get; set; }
        
        #endregion

        #region Команды операций с БД
        //
        private DelegateCommand? _refreshAccounts;
        public DelegateCommand RefreshAccounts => _refreshAccounts ??= new DelegateCommand(UpdateAccountsCheckMethod);

        // добавление клиента
        private DelegateCommand? _addNewClient;
        public DelegateCommand AddNewClient => _addNewClient ??= new DelegateCommand(AddNewClientMethod);

        // открытие счёта
        private DelegateCommand? _openNewAccount;
        public DelegateCommand OpenNewAccount => _openNewAccount ??= new DelegateCommand(OpenNewAccountMethod);

        // удаление клиента
        private DelegateCommand? _deleteClient;
        public DelegateCommand DeleteClient => _deleteClient ??= new DelegateCommand(DeleteClientMethod);

        // закрытие счёта
        private DelegateCommand? _closeAccount;
        public DelegateCommand CloseAccount => _closeAccount ??= new DelegateCommand(CloseAccountMethod);

        // пополнение счёта
        private DelegateCommand? _fundAccount;
        public DelegateCommand FundAccount => _fundAccount ??= new DelegateCommand(FundAccountMethod);

        // перевод средств
        private DelegateCommand? _transferFunds;
        public DelegateCommand TransferFunds => _transferFunds ??= new DelegateCommand(TransferFundsMethod);

        #endregion

        #region Методы операций с БД
        
        /// <summary>
        /// Метод добавления нового клиента
        /// </summary>
        private void AddNewClientMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            if (ClientFirstName == null || ClientFirstName.Replace(" ", "").Length == 0 || ClientLastName == null || ClientLastName.Replace(" ", "").Length == 0) return;
                DataBank.AddClient(ClientFirstName, ClientLastName);
                UpdateAllClientsView();
                _currentWindow?.Close();
                _currentWindow = null;
        }

        /// <summary>
        /// Метод добавления нового счёта
        /// </summary>
        private void OpenNewAccountMethod()
        {
            if (SelectedClient == null) return;
            DataBank.OpenAccount(SelectedClient);
            UpdateAllAccountsView(SelectedClient);
        }

        /// <summary>
        /// Метод удаления клиента
        /// </summary>
        private void DeleteClientMethod()
        {
            if (SelectedClient == null) return;
            DataBank.DeleteClient(SelectedClient);
            UpdateAllClientsView();
            UpdateAllAccountsView(SelectedClient);
        }

        /// <summary>
        /// Метод закрытия счёта
        /// </summary>
        private void CloseAccountMethod()
        {
            if (SelectedAccount == null) return;
            DataBank.CloseAccount(SelectedAccount);
            if (SelectedClient != null) UpdateAllAccountsView(SelectedClient);
        }

        /// <summary>
        /// Метод пополнения счёта
        /// </summary>
        private void FundAccountMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            DataBank.FundAccount(SelectedAccount, Convert.ToDecimal(AccountSum));
            UpdateAllAccountsView(SelectedClient);
            _currentWindow?.Close();
            _currentWindow = null;
        }

        /// <summary>
        /// Метод перевода средств
        /// </summary>
        private void TransferFundsMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            DataBank.TransferFunds(SelectedAccount, AccountTarget, Convert.ToDecimal(AccountSum));
            UpdateAllAccountsView(SelectedClient);
            _currentWindow?.Close();
            _currentWindow = null;
        }
        #endregion

        #region Обновление содержимиого списков

        /// <summary>
        /// Обновление списка клиентов
        /// </summary>
        private void UpdateAllClientsView()
        {
            AllClients = DataBank.GetAllClients();
            if (MainWindow.AllClientsView == null) return;
            MainWindow.AllClientsView.ItemsSource = null;
            MainWindow.AllClientsView.Items.Clear();
            MainWindow.AllClientsView.ItemsSource = AllClients;
            MainWindow.AllClientsView.Items.Refresh();
        }

        /// <summary>
        /// Промежуточный метод обновления списка счетов
        /// </summary>
        private void UpdateAccountsCheckMethod()
        {
            if (SelectedClient == null)
            {
                if (MainWindow.AllAccountsView != null) MainWindow.AllAccountsView.Visibility = Visibility.Hidden;
            }
            else
            {
                if (MainWindow.AllAccountsView != null) MainWindow.AllAccountsView.Visibility = Visibility.Visible;
                UpdateAllAccountsView(SelectedClient);
            }
        }

        /// <summary>
        /// Обновление списка счетов
        /// </summary>
        private void UpdateAllAccountsView(Client? client)
        {
                if (client == null) return;
                AllAccounts = DataBank.GetAllAccountsByClientId(client.Id);
                if (MainWindow.AllAccountsView == null) return;
                MainWindow.AllAccountsView.ItemsSource = null;
                MainWindow.AllAccountsView.Items.Clear();
                MainWindow.AllAccountsView.ItemsSource = AllAccounts;
                MainWindow.AllAccountsView.Items.Refresh();
        }

        #endregion

        #region Команды работы с окнами

        // команда на открытие окна добавления нового клиента
        private DelegateCommand? _openAddNewClientWin;
        public DelegateCommand OpenAddNewClientWin =>
            _openAddNewClientWin ??= new DelegateCommand(OpenAddNewClientWindowMethod);

        // команда на открытие окна пополнения счёта
        private DelegateCommand? _openAddFundsWin;
        public DelegateCommand OpenAddFundsWin =>
            _openAddFundsWin ??= new DelegateCommand(OpenAddFundsWindowCheckMethod);

        // команда на открытие окна перевода средств
        private DelegateCommand? _openTransferFundsWin;
        public DelegateCommand OpenTransferFundsWin =>
            _openTransferFundsWin ??= new DelegateCommand(OpenTransferFundsWindowMethod);

        // команда на закрытие окна
        public ICommand? CloseCommand { get; private set; }
        protected ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

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
        /// Промежуточный метод проверки, выделен ли пользователь, для открытия окна пополнения счёта
        /// </summary>
        private static void OpenAddFundsWindowCheckMethod()
        {
            OpenAddFundsWindowMethod(SelectedClient, SelectedAccount);
        }

        /// <summary>
        /// Метод открытия окна пополнения счёта
        /// </summary>
        private static void OpenAddFundsWindowMethod(Client? client, Account? account)
        {
            if (client == null || account == null) return;
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
            _currentWindow = window;
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        
        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Закрытие окна
        /// </summary>
        private void Close()
        {
            CurrentWindowService.Close();
        }

        #endregion

        #region INofifyPropertyChanged

        public new event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
