namespace Products.ViewModels
{
    using Models;
    using System.Collections.ObjectModel;

    public class CategoriesViewModel
    {
        #region Properties
        public ObservableCollection<Category> Categories { get; set; }
        #endregion

        #region Constructors
        public CategoriesViewModel()
        {
            LoadCategories();
        }
        #endregion

        #region Methods
        void LoadCategories()
        {

        }
        #endregion
    }
}
