namespace Products.ViewModels
{
    using Models;

    public class MainViewModel
    {
        #region Properties
        public CategoriesViewModel Categories { get; set; }

        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }

        public TokenResponse Token { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            Login = new LoginViewModel();
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
    }
}
