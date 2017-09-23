namespace Products.ViewModels
{
    using Models;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Services;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoriesViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        ObservableCollection<Category> _categories;
        #endregion

        #region Properties
        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if(_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Categories)));
                }
            }
        }
        #endregion

        #region Constructors
        public CategoriesViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            LoadCategories();
        }
        #endregion

        #region Methods
        async void LoadCategories()
        {
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var response = await apiService.GetList<Category>(
                "http://apiproducts.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var categories = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
        }
        #endregion
    }
}
