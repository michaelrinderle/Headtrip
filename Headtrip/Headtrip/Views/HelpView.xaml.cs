/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Providers;
using Headtrip.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Headtrip.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpView : ContentPage
    {
        public HelpView()
        {
            InitializeComponent();
            
            BindingContext = 
                DependencyProvider.ServiceProvider.GetService<HelpViewModel>();
        }
    }
}