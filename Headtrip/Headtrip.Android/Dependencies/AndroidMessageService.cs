/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Android.App;
using Android.Widget;
using Headtrip.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Headtrip.Droid.Dependencies.AndroidMessageService))]
namespace Headtrip.Droid.Dependencies
{

    public class AndroidMessageService : IMessageService
    {
        public void LongAlert(string msg)
        {
            Toast.MakeText(Application.Context, msg, ToastLength.Long).Show();
        }

        public void ShortAlert(string msg)
        {
            Toast.MakeText(Application.Context, msg, ToastLength.Short).Show();
        }
    }
}