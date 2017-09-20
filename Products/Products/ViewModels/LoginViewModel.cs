namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        bool _isEnabled;
        bool _isRunning;
        bool _isToggled;
        string _email;
        string _password;
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if(_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsToggled)));
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();

            IsEnabled = true;
            IsToggled = true;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get { return new RelayCommand(Login); } }

        async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter an email.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a password.",
                    "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var response = await apiService.GetToken(
                "http://apiproducts.azurewebsites.net",
                Email,
                Password);

            if(response == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    "The service is not available, please try latter.",
                    "Accept");
                Password = null;
                return;
            }

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.ErrorDescription,
                    "Accept");
                Password = null;
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = response;
            mainViewModel.Categories = new CategoriesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CategoriesView());

            Email = null;
            Password = null;

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}
