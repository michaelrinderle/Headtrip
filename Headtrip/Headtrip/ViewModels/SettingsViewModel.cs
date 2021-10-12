/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Data;
using Headtrip.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Headtrip.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public ICommand DumpDatabaseCommand => new Command((async) => { DumpDatabase(); });

        public ICommand WakelockCommand = new Command(() =>
        {
            var message = DeviceDisplay.KeepScreenOn ? "on" : "off";
            DeviceDisplay.KeepScreenOn = !DeviceDisplay.KeepScreenOn;
            DependencyService.Get<IMessageService>().ShortAlert($"Wake lock {message}");

        });

        public bool Wakelock
        {
            get
            {
                var wakelock = Preferences.Get("wake_lock", false);
                DeviceDisplay.KeepScreenOn = wakelock;
                return wakelock;
            }
            set
            {
                Preferences.Set("wake_lock", value);
                var message = value ? "on" : "off";
                DeviceDisplay.KeepScreenOn = value;
                DependencyService.Get<IMessageService>().ShortAlert($"Wake lock {message}");
                OnPropertyChanged();
            }
        }

        public int MagnometerSpeed
        {
            get
            {
                return (int) Preferences.Get("_MagnometerSpeed", (int) SensorSpeed.Default);
            }
            set
            {
                Preferences.Set("_MagnometerSpeed", (int) value);
                MonitorViewModel._magnetSpeed = (SensorSpeed) value;
                Magnetometer.Stop();
                Magnetometer.Start((SensorSpeed) value);
                OnPropertyChanged();
            }
        }

        public int AcceleratorSpeed
        {
            get
            {
                return (int) Preferences.Get("_AcceleratorSpeed", (int)SensorSpeed.Default);
            }
            set
            {
                Preferences.Set("_AcceleratorSpeed", (int) value);
                MonitorViewModel._acceleratorSpeed = (SensorSpeed) value;
                Accelerometer.Stop();
                Accelerometer.Start((SensorSpeed)value);
                OnPropertyChanged();
            }
        }

        public static int _ThresholdRange { get; set; } = (int) Preferences.Get("_ThresholdRange", 100);
        public int ThresholdRange
        {
            get =>_ThresholdRange;
            set
            {
                if (value == _ThresholdRange) return;
                Preferences.Set("_ThresholdRange", value);
                _ThresholdRange = value;
                OnPropertyChanged();
            }
        }

        public static bool _ThresholdAlarm { get; set; } = (bool) Preferences.Get("_ThresholdAlarm", false);
        public bool ThresholdAlarm
        {
            get => _ThresholdAlarm;
            set
            {
                if (value == _ThresholdAlarm) return;
                Preferences.Set("_ThresholdRange", value);
                _ThresholdAlarm = value;
                OnPropertyChanged();
            }
        }

        public int ThresholdAlarmVolume
        {
            get => (int)Preferences.Get("_ThresholdAlarmVolume", 10);
            set
            {
                Preferences.Set("_ThresholdAlarmVolume", value);
                MonitorViewModel.Alarm.Volume = (double) (value / 10);
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            Title = "Settings";
        }

        private async void DumpDatabase()
        {
            string action = await Application.Current.MainPage
                .DisplayActionSheet("Database Log Dump", "Cancel", null, "Dump DB", "Dump & Clear DB", "Clear DB");

            _ = Task.Run(async () =>
            {
                try
                {
                    switch (action)
                    {
                        case "Dump DB":
                            {
                                DependencyService.Get<ISqliteService>().ExportDb();
                                DependencyService.Get<IMessageService>().ShortAlert($"database exported");
                                break;
                            }
                        case "Dump & Clear DB":
                            {
                                DependencyService.Get<ISqliteService>().ExportDb();

                                using (var ctx = new SqliteContext())
                                {
                                    ctx.RemoveRange(ctx.LogEntrys);
                                }
                                DependencyService.Get<IMessageService>().ShortAlert($"database exported and cleared");
                                break;
                            }
                        case "Clear DB":
                            {
                                using (var ctx = new SqliteContext())
                                {
                                    ctx.RemoveRange(ctx.LogEntrys);
                                }

                                DependencyService.Get<IMessageService>().ShortAlert($"database cleared");
                                break;
                            }
                    }
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Error dumping database.", "Okay");
                }
            });
        }
    }
}
