namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        bool _isRefreshing;
        List<Product> products;
        ObservableCollection<Product> _products;
        string _filter;
        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }

        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                if(_products != value)
                {
                    _products = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Products)));
                }
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    Search();
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }
        #endregion

        #region Constructors
        public ProductsViewModel(List<Product> products)
        {
            instance = this;

            this.products = products;

            apiService = new ApiService();
            dialogService = new DialogService();

            Products = new ObservableCollection<Product>(products.OrderBy(p => p.Description));
        }
        #endregion

        #region Sigleton
        static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Methods
        public void Add(Product product)
        {
            IsRefreshing = true;
            products.Add(product);
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        public async Task Delete(Product product)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.Delete(
                urlAPI,
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            products.Remove(product);
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));

            IsRefreshing = false;
        }
        public void Update(Product product)
        {
            IsRefreshing = true;
            var oldProduct = products
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();
            oldProduct = product;
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand SearchCommand { get { return new RelayCommand(Search); } }

        void Search()
        {
            IsRefreshing = true;

            if (string.IsNullOrEmpty(Filter))
            {
                Products = new ObservableCollection<Product>(
                    products.OrderBy(c => c.Description));
            }
            else
            {
                Products = new ObservableCollection<Product>(products
                    .Where(c => c.Description.ToLower().Contains(Filter.ToLower()))
                    .OrderBy(c => c.Description));
            }

            IsRefreshing = false;
        }
        #endregion
    }
}