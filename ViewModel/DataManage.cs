using System;
using DevExpress.Mvvm;
using Homework12.Model;
using Homework12.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using static Homework12.Model.DataBank;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace Homework12.ViewModel
{
    public class DataManage : ViewModelBase, INotifyPropertyChanged
    {
        #region Переменные

        // текущее окно
        private static Window? _currentWindow;
        private static Window? _infoWindow;

        #endregion

        #region Свойства

        // список всех клиентов
        private List<Client> _allClients = GetAllClients();
        public List<Client> AllClients
        {
            get => _allClients;
            set
            {
                _allClients = value;
                OnPropertyChanged();
            }
        }

        // список всех счетов
        private List<Account> _allAccounts = GetAllAccounts();
        public List<Account> AllAccounts
        {
            get => _allAccounts;
            set
            {
                _allAccounts = value;
                OnPropertyChanged();
            }
        }

        // счета, отобранные по ID клиента
        public static List<Account>? SelectedAccounts { get; set; }

        // свойства клиентов
        public string? ClientFirstName { get; set; }
        public string? ClientLastName { get; set; }

        // свойства счетов
        public AccountType AccType { get; set; }
        public decimal AccountSum { get; set; }
        public Account? AccountTarget { get; set; }

        // свойства для выделенных элементов
        public static Client? SelectedClient { get; set; }
        public static Account? SelectedAccount { get; set; }
        
        // информационная строка
        public static string? InfoText { get; set; }

        #endregion

        #region Передача данных типов счёта в ComboBox

        private AccountType _accountType;

        public AccountType SelectedType
        {
            get => _accountType;
            set
            {
                if (_accountType == value) return;
                _accountType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды операций с БД
        //
        private DelegateCommand? _refreshAccounts;
        public DelegateCommand RefreshAccounts => 
            _refreshAccounts ??= new DelegateCommand(UpdateAccountsCheckMethod);

        // добавление клиента
        private DelegateCommand? _addNewClient;
        public DelegateCommand AddNewClient => 
            _addNewClient ??= new DelegateCommand(AddNewClientMethod);

        // открытие счёта
        private DelegateCommand? _openNewAccount;
        public DelegateCommand OpenNewAccount => 
            _openNewAccount ??= new DelegateCommand(OpenNewAccountMethod);

        // удаление клиента
        private DelegateCommand? _deleteClient;
        public DelegateCommand DeleteClient => 
            _deleteClient ??= new DelegateCommand(DeleteClientMethod);

        // закрытие счёта
        private DelegateCommand? _closeAccount;
        public DelegateCommand CloseAccount => 
            _closeAccount ??= new DelegateCommand(CloseAccountMethod);

        // пополнение счёта
        private DelegateCommand? _fundAccount;
        public DelegateCommand FundAccount => 
            _fundAccount ??= new DelegateCommand(FundAccountMethod);

        // перевод средств
        private DelegateCommand? _transferFunds;
        public DelegateCommand TransferFunds => 
            _transferFunds ??= new DelegateCommand(TransferFundsMethod);

        #endregion

        #region Методы операций с БД
        
        /// <summary>
        /// Метод добавления нового клиента
        /// </summary>
        private void AddNewClientMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            if (ClientFirstName == null || ClientFirstName.Replace(" ", "").Length == 0 || ClientLastName == null || ClientLastName.Replace(" ", "").Length == 0) return;
                AddClient(ClientFirstName, ClientLastName);
                UpdateAllClientsView();
                _currentWindow?.Close();
                _currentWindow = null;
        }

        /// <summary>
        /// Метод добавления нового счёта
        /// </summary>
        private void OpenNewAccountMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            if (SelectedClient == null) return;
            InfoText = OpenAccount(SelectedClient, AccType);
            OpenInfoWindowMethod();
            UpdateAllAccountsView(SelectedClient);
            _currentWindow?.Close();
            _currentWindow = null;
        }

        /// <summary>
        /// Метод удаления клиента
        /// </summary>
        private void DeleteClientMethod()
        {
            if (SelectedClient == null) return;
            DeleteClient(SelectedClient);
            UpdateAllClientsView();
            UpdateAllAccountsView(SelectedClient);
        }

        /// <summary>
        /// Метод закрытия счёта
        /// </summary>
        private void CloseAccountMethod()
        {
            if (SelectedAccount == null) return;
            CloseAccount(SelectedAccount);
            if (SelectedClient != null) UpdateAllAccountsView(SelectedClient);
        }

        /// <summary>
        /// Метод пополнения счёта
        /// </summary>
        private void FundAccountMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            FundAccount(SelectedAccount, Convert.ToDecimal(AccountSum));
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
            TransferFunds(SelectedAccount, AccountTarget, Convert.ToDecimal(AccountSum));
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
            AllClients = GetAllClients();
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
                AllAccounts = GetAllAccountsByClientId(client.Id);
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

        // команда на открытие окна открытия нового счёта
        private DelegateCommand? _openOpenNewAccountWin;
        public DelegateCommand OpenOpenNewAccountWin => 
            _openOpenNewAccountWin ??= new DelegateCommand(OpenNewAccountWindowMethod);

        // команда на открытие окна пополнения счёта
        private DelegateCommand? _openAddFundsWin;
        public DelegateCommand OpenAddFundsWin =>
            _openAddFundsWin ??= new DelegateCommand(OpenAddFundsWindowMethod);

        // команда на открытие окна перевода средств
        private DelegateCommand? _openTransferFundsWin;
        public DelegateCommand OpenTransferFundsWin =>
            _openTransferFundsWin ??= new DelegateCommand(OpenTransferFundsWindowMethod);

        // команда на закрытие информационного окна
        private DelegateCommand? _closeInfoWin;
        public DelegateCommand CloseInfoWin =>
            _closeInfoWin ??= new DelegateCommand(CloseInfoWindowMethod);

        // команда на закрытие окна
        public ICommand? CloseCommand { get; private set; }
        protected ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

        #endregion

        #region Методы работы с окнами

        /// <summary>
        /// Метод открытия окна добавления нового клиента
        /// </summary>
        private static void OpenAddNewClientWindowMethod()
        {
            var newClientWindow = new AddNewClientWindow();
            SetCenterPositionAndOpen(newClientWindow);
        }

        /// <summary>
        /// Метод открытия окна открытия нового счёта
        /// </summary>
        private static void OpenNewAccountWindowMethod()
        {
            var newAccountWindow = new OpenAccountWindow();
            SetCenterPositionAndOpen(newAccountWindow);
        }

        /// <summary>
        /// Метод открытия информационного окна
        /// </summary>
        private static void OpenInfoWindowMethod()
        {
            if (InfoText != null) _infoWindow = new InfoWindow(InfoText);
            if (_infoWindow == null) return;
            _infoWindow.Owner = Application.Current.MainWindow;
            _infoWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _infoWindow.ShowDialog();
        }

        /// <summary>
        /// Метод закрытия информационного окна
        /// </summary>
        private void CloseInfoWindowMethod()
        {
            CloseCommand = new DelegateCommand(Close);
            _infoWindow?.Close();
            _infoWindow = null;
        }
        
        /// <summary>
        /// Метод открытия окна пополнения счёта
        /// </summary>
        private static void OpenAddFundsWindowMethod()
        {
            if (SelectedClient == null || SelectedAccount == null) return;
            var newFundWindow = new AddFundsWindow();
            SetCenterPositionAndOpen(newFundWindow);
        }

        /// <summary>
        /// Метод открытия окна перевода средств
        /// </summary>
        private static void OpenTransferFundsWindowMethod()
        {
            if (SelectedClient == null || SelectedAccount == null) return;
            var donorAccount = SelectedAccount.Number;
            SelectedAccounts = GetAllAccountsByClientId(SelectedClient.Id);
            SelectedAccounts.RemoveAll(a => a.Number == donorAccount);
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

    #region Конвертер Enum в Collection

    [ValueConversion(typeof(Enum), typeof(IEnumerable<ValueDescription>))]
    public class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelper.GetAllValuesAndDescriptions(value.GetType());
        }
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    #endregion
}
