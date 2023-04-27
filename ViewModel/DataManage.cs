using Homework12.Model;
using Homework12.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

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

        #region Команды открытия окон

        /// <summary>
        /// Команда на открытие окна добавления нового клиента
        /// </summary>
        private RelayCommand? _openAddNewClientWin;
        public RelayCommand OpenAddNewClientWin
        {
            get
            {
                return _openAddNewClientWin ?? new RelayCommand(obj =>
                {
                    OpenAddNewClientWindowMethod();
                });
            }
        }

        /// <summary>
        /// Команда на открытие окна пополнения счёта
        /// </summary>
        private RelayCommand? _openAddFundsWin;
        public RelayCommand OpenAddFundsWin
        {
            get
            {
                return _openAddFundsWin ?? new RelayCommand(obj =>
                {
                    OpenAddFundsWindowMethod();
                });
            }
        }

        /// <summary>
        /// Команда на открытие окна перевода средств
        /// </summary>
        private RelayCommand? _openTransferFundsWin;
        public RelayCommand OpenTransferFundsWin
        {
            get
            {
                return _openTransferFundsWin ?? new RelayCommand(obj =>
                {
                    OpenTransferFundsWindowMethod();
                });
            }
        }

        #endregion

        #region Методы открытия окон

        /// <summary>
        /// Метод открытия окна добавления нового клиента
        /// </summary>
        private void OpenAddNewClientWindowMethod()
        {
            var newClientWindow = new AddNewClientWindow();
            SetCenterPositionAndOpen(newClientWindow);
        }

        /// <summary>
        /// Метод открытия окна пополнения счёта
        /// </summary>
        private void OpenAddFundsWindowMethod()
        {
            var newFundWindow = new AddFundsWindow();
            SetCenterPositionAndOpen(newFundWindow);
        }

        /// <summary>
        /// Метод открытия окна перевода средств
        /// </summary>
        private void OpenTransferFundsWindowMethod()
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
