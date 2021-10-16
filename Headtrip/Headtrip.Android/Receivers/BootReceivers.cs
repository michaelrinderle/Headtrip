/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Android.App;
using Android.Content;
using Android.Widget;
using System;
using Xamarin.Essentials;

namespace Headtrip.Droid.Receivers
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action.Equals(Intent.ActionBootCompleted))
                {
                    if (Preferences.Get("start_on_boot", false))
                    {
                        Toast.MakeText(context, "headtripped!", ToastLength.Long).Show();
                        Intent i = new Intent(context, typeof(MainActivity));
                        i.AddFlags(ActivityFlags.NewTask);
                        context.StartActivity(i);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}