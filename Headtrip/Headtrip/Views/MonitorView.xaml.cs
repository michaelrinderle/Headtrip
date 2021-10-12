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
    public partial class MonitorView : ContentPage
    {
        private static MonitorViewModel viewModel;

        public MonitorView()
        {
            InitializeComponent();

            BindingContext = viewModel = 
                DependencyProvider.ServiceProvider.GetService<MonitorViewModel>();
        }

        private void LogItemsListview_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var target = viewModel.Logs[viewModel.Logs.Count - 1];
            LogItemsListview.ScrollTo(target, ScrollToPosition.MakeVisible, true);
        }
    }
}