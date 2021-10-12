using Headtrip.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Headtrip.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}