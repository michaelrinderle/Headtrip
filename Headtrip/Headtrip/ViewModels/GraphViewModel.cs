/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Data;
using Headtrip.Interfaces;
using Headtrip.Models;
using Microsoft.EntityFrameworkCore;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Headtrip.ViewModels
{
    public class GraphViewModel : BaseViewModel
    {
        public ICommand OpenDatabaseCommand => new Command((async) => { OpenDatabase(); });

        public ICommand PrintGraphCommand => new Command((async) => 
        {
            _ = Task.Run(() => PrintDatabase());
            
        });

        private ObservableCollection<EmfPoint> _GraphData = new ObservableCollection<EmfPoint>();
        public ObservableCollection<EmfPoint> GraphData
        {
            get => _GraphData;
            set
            {
                _GraphData = value;
                OnPropertyChanged(nameof(GraphData));
            }
        }

        public GraphViewModel()
        {
            Title = "Graph";
        }

        private async void OpenDatabase()
        {
            string action = await Application.Current.MainPage
                .DisplayActionSheet("Open Database", "Cancel", null, "Current DB", "Exported DB");

            _ = Task.Run(async () =>
            {
                try
                {
                    switch (action)
                    {
                        case "Current DB":
                            {
                                using(var ctx = new SqliteContext())
                                {
                                    var entries = ctx.LogEntrys.Select(x => new EmfPoint(x.Created, (float)x.EMF)).ToList();

                                    Device.BeginInvokeOnMainThread(() => {
                                        GraphData = new ObservableCollection<EmfPoint>(entries);
                                    });
                                }

                                break;
                            }
                        case "Exported DB":
                            {
                                var customFileType =
                                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                                {
                                    { DevicePlatform.iOS, new[] { "public.database" } }, // or general UTType values
                                    { DevicePlatform.Android, new[] { "application/octet-stream" } },
                                    { DevicePlatform.UWP, new[] { ".db3", ".db3", ".sqlite" } },
                                });

                                var options = new PickOptions
                                {
                                    PickerTitle = "Select Database",
                                    FileTypes = customFileType
                                };

                                var resultPath = await FilePicker.PickAsync(options);
                                if(resultPath != null)
                                {
                                    var optionsBuilder = new DbContextOptionsBuilder<SqliteContext>()
                                   .UseSqlite($"Filename={resultPath.FullPath}");

                                    using (var ctx = new SqliteContext(optionsBuilder.Options))
                                    {
                                        var entries = ctx.LogEntrys.Select(x => new EmfPoint(x.Created, (float)x.EMF)).ToList();

                                        Device.BeginInvokeOnMainThread(() => {
                                            GraphData = new ObservableCollection<EmfPoint>(entries);
                                        });
                                    }
                                }
                                else
                                {
                                    DependencyService.Get<IMessageService>().ShortAlert($"no database selected.");
                                }

                                break;
                            }

                        default:
                            return;

                    }

                    //DependencyService.Get<IMessageService>().ShortAlert($"database imported.");
                }
                catch(Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Error importing database.", "Okay");
                }
            });


        }
    
        private void PrintDatabase()
        {
            if (GraphData.Count() != 0)
            {
                var chart = new SfChart()
                {
                    Title = new ChartTitle()
                    {
                        Text = "Headtrip EMF Graph"
                    }
                };

                DateTimeAxis primaryAxis = new DateTimeAxis()
                {
                    Title = new ChartAxisTitle()
                    {
                        Text = "Time"
                    }
                };

                NumericalAxis secondaryAxis = new NumericalAxis()
                {
                    Title = new ChartAxisTitle()
                    {
                        Text = "EMF"
                    },
                    Maximum = 250
                };

                chart.PrimaryAxis = primaryAxis;
                chart.SecondaryAxis = secondaryAxis;

                LineSeries series = new LineSeries();

                series.ItemsSource = GraphData.Select(x => new ChartDataPoint(x.XValue, x.YValue)).ToList(); ;
                series.Color = Color.CornflowerBlue;


                chart.Series.Add(series);

                DependencyService.Get<IPrintService>().ExportAsPDF("Chart.pdf", chart.GetStream(), chart);

                chart = null;
            }
        }
    }
}
