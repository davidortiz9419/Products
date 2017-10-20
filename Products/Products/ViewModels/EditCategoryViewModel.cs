﻿namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditCategoryViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        #endregion
        DialogService dialogService;
        NavigationService navigationService;

        #region Attributes
        bool _isEnabled;
        bool _isRunning;
        Category category;
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
                if (_isEnabled != value)
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

        public string Description { get; set; }
        #endregion

        #region Constructors
        public EditCategoryViewModel(Category category)
        {
            this.category = category;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Description = category.Description;
            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        async void Save()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a category description.");
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
                    connection.Message);
                return;
            }

            category.Description = Description;
            var mainViewModel = MainViewModel.GetInstance();
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.Put(
                urlAPI,
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            var categoriesViewModel = CategoriesViewModel.GetInstance();
            categoriesViewModel.Update(category);

            await navigationService.BackOnMaster();

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}