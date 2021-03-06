﻿namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Interfaces;
    using Models;
    using Services;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {
        #region Services
        NavigationService navigationService;
        #endregion

        #region Properties
        public Category Category { get; set; }

        public CategoriesViewModel Categories { get; set; }

        public EditCategoryViewModel EditCategory { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public LoginViewModel Login { get; set; }

        public MyProfileViewModel MyProfile { get; set; }

        public NewCategoryViewModel NewCategory { get; set; }

        public NewCustomerViewModel NewCustomer { get; set; }

        public NewProductViewModel NewProduct { get; set; }

        public ObservableCollection<Menu> MyMenu { get; set; }

        public PasswordRecoveryViewModel PasswordRecovery { get; set; }

        public ProductsViewModel Products { get; set; }

        public SyncViewModel Sync { get; set; }

        public TokenResponse Token { get; set; }

        public UbicationsViewModel Ubications { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            navigationService = new NavigationService();

            Login = new LoginViewModel();
            LoadMenu();
        }
        #endregion

        #region Sigleton
        static MainViewModel instance;
        
        public static MainViewModel GetInstance()
        {
            if(instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        void LoadMenu()
        {
            MyMenu = new ObservableCollection<Menu>();

            MyMenu.Add(new Menu
            {
                Icon = "ic_map",
                PageName = "UbicationsView",
                Title = "Ubications",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_settings",
                PageName = "MyProfileView",
                Title = "My Profile",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_sync",
                PageName ="SyncView",
                Title = "Sync Offline Operations",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginView",
                Title = "Close Sesion",
            });
        }

        public void RegisterDevice()
        {
            var register = DependencyService.Get<IRegisterDevice>();
            register.RegisterDevice();
        }
        #endregion

        #region Commands
        public ICommand NewCategoryCommand { get {return new RelayCommand(GoNewCategory); }  }

        async void GoNewCategory()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.NavigateOnMaster("NewCategoryView");
        }

        public ICommand NewProductCommand { get { return new RelayCommand(GoNewProduct); } }

        async void GoNewProduct()
        {
            NewProduct = new NewProductViewModel();
            await navigationService.NavigateOnMaster("NewProductView");
        }
        #endregion
    }
}