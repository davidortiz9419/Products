namespace Products.Services
{
    using System.Threading.Tasks;
    using Xamarin.Forms;
    public class DialogService
    {
        public async Task ShowMessage(string title, string message, string button)
        {
            await Application.Current.MainPage.DisplayAlert(
                title, 
                message, 
                button);
        }

        public async Task<bool> ShowConfirm(string title, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(
                title, 
                message, 
                "Yes", "No");
        }
    }
}
