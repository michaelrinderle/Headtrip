/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Headtrip.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; }

        public HelpViewModel()
        {
            Title = "Help";
            // OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }
    }
}
