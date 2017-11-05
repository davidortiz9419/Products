namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SyncViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        bool _isEnabled;
        bool _isRunning;
        double _progress;
        string _message;
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
                if(_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if(_progress != value)
                {
                    _progress = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Progress)));
                }
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if(_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
        #endregion

        #region Constructors
        public SyncViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Message = "Press sync button to start";
            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SyncCommand { get { return new RelayCommand(Sync); } }

        async void Sync()
        {
            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message);
                return;
            }

            var products = dataService.Get<Product>(false)
                .Where(p => p.PendingToSave)
                .ToList();
            if(products.Count == 0)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    "There are not products to sync.");
                return;
            }
            var mainViewModel = MainViewModel.GetInstance();
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            double progress = (double)1 / products.Count;
            Progress = 0;
            foreach (var product in products)
            {
                var response = await apiService.Post(
                urlAPI,
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

                if (response.IsSuccess)
                {
                    product.PendingToSave = false;
                    dataService.Update(product);
                }

                Progress += progress;
            }

            IsRunning = false;
            IsEnabled = true;

            await dialogService.ShowMessage(
                "Confirmation",
                "Sync Ok");
            Progress = 0;
            await navigationService.BackOnMaster();
        }
        #endregion
    }
}