namespace Products.ViewModels
{
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

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

            var response = await apiService.Delete(
                "http://apiproducts.azurewebsites.net",
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
    }
}