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
using Headtrip.Utilities;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Headtrip.ViewModels
{
    public class MonitorViewModel : BaseViewModel
    {
        public ICommand ClearLogCommand => new Command((async) => { ClearLog(); });

        public static SensorSpeed _magnetSpeed = (SensorSpeed) Preferences.Get("_MagnometerSpeed", (int)SensorSpeed.Default);

        public static SensorSpeed _acceleratorSpeed = (SensorSpeed) Preferences.Get("_AcceleratorSpeed", (int)SensorSpeed.Default);

        public static ISimpleAudioPlayer Alarm { get; set; }

        private float Alpha = 0.8f;

        private float[] Gravity = new float[3];

        private float[] Magnetic = new float[3];

        private float _GimbalX { get; set; }
        public float GimbalX
        {
            get => _GimbalX;
            set
            {
                _GimbalX = value;
                OnPropertyChanged(nameof(GimbalX));
            }
        }

        private float _GimbalY { get; set; }
        public float GimbalY
        {
            get => _GimbalY;
            set
            {
                _GimbalY = value;
                OnPropertyChanged(nameof(GimbalY));
            }
        }

        private float _GimbalZ { get; set; }
        public float GimbalZ
        {
            get => _GimbalZ;
            set
            {
                _GimbalZ = value;
                OnPropertyChanged(nameof(GimbalZ));
            }
        }

        private double _MaxEMF { get; set; } = 0;
        public double MaxEMF
        {
            get => _MaxEMF;
            set
            {
                _MaxEMF = value;
                OnPropertyChanged();
            }
        }

        private float _EMF { get; set; }
        public float EMF
        {
            get => _EMF;
            set
            {
                _EMF = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EmfPoint> _Timeline = new ObservableCollection<EmfPoint>();
        public ObservableCollection<EmfPoint> Timeline
        {
            get
            {
                return _Timeline;
            }
            set
            {
                _Timeline = value;
                OnPropertyChanged(nameof(Timeline));
            }
        }

        private ObservableCollection<LogEntry> _LogItems { get; set; }
        public ObservableCollection<LogEntry> LogItems
        {
            get
            {
                if (_LogItems == null)
                    _LogItems = new ObservableCollection<LogEntry>();
                return _LogItems;
            }
            set
            {
                _LogItems = value;
                OnPropertyChanged();
            }
        }

        public List<LogEntry> Logs = new List<LogEntry>();

        public MonitorViewModel()
        {
            Title = "Monitor";

            InitializeSensors();
            InitializeAlarm();
        }

        private void InitializeSensors()
        {
            if (!Magnetometer.IsMonitoring)
                Magnetometer.Start(_magnetSpeed);

            if (!Accelerometer.IsMonitoring)
                Accelerometer.Start(_acceleratorSpeed);

            Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private void InitializeAlarm()
        {
            Alarm = CrossSimpleAudioPlayer.Current;
            Alarm.Load("alarm.wav");
            Alarm.Loop = true;
            Alarm.Volume = (double) (Preferences.Get("_ThresholdAlarmVolume", 10) / 10); 
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            // isolating force
            Gravity[0] = Alpha * Gravity[0] + (1 - Alpha) * e.Reading.Acceleration.X;
            Gravity[1] = Alpha * Gravity[1] + (1 - Alpha) * e.Reading.Acceleration.Y;
            Gravity[2] = Alpha * Gravity[2] + (1 - Alpha) * e.Reading.Acceleration.Z;
        }

        private void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            Magnetic[0] = e.Reading.MagneticField.X;
            Magnetic[1] = e.Reading.MagneticField.Y;
            Magnetic[2] = e.Reading.MagneticField.Z;

            OnSensorUpdate();
        }

        private void OnSensorUpdate()
        {
            // get orignal values from magnetic gimbals
            float[] originalValues = Magnetic;
            float[] R = new float[9];
            float[] I = new float[9];

            // check if matrix can be rotated
            if (!Emf.GetRotationMatrix(R, I, Gravity, Magnetic)) return;

            // convert sensor values             
            float[] A_D = originalValues;
            float[] A_W = new float[3];

            A_W[0] = R[0] * A_D[0] + R[1] * A_D[1] + R[2] * A_D[2];
            A_W[1] = R[3] * A_D[0] + R[4] * A_D[1] + R[5] * A_D[2];
            A_W[2] = R[6] * A_D[0] + R[7] * A_D[1] + R[8] * A_D[2];

            float emf = Emf.ConvertToEmfReading(A_W[0], A_W[1], A_W[2]);

            // update ui/chart with new converted sensor data
            UpdateUI(A_W, emf);

            UpdateChart(emf);

            // log entry if meets user threshold
            if ((int) emf > SettingsViewModel._ThresholdRange)
            {
                CreateLogEntry(A_W, emf);
                
                if(SettingsViewModel._ThresholdAlarm)
                {
                    if (!Alarm.IsPlaying)
                    {
                        Alarm.Play();
                    }
                }     
            }
            else
            {
                if (Alarm.IsPlaying)
                {
                    Alarm.Stop();
                }
            }
        }

        private void UpdateUI(float[] convertedSensorValues, float emf)
        {
            GimbalX = convertedSensorValues[0];
            GimbalY = convertedSensorValues[1];
            GimbalZ = convertedSensorValues[2];
            EMF = emf;

            double newReading = Math.Round(emf);
            if (MaxEMF < newReading)
            {
                MaxEMF = newReading;
            }
            else
            {
                MaxEMF = MaxEMF;
            }
        }

        private void UpdateChart(float emf)
        {
            if (Timeline.Count > 10000)
                Timeline.Clear();

            Timeline.Add(new EmfPoint(DateTime.Now, emf));
        }

        private void CreateLogEntry(float[] convertedSensorValues, float emf)
        {
            var geo = Geo.GetLocationCoordinates().Result;
            if (geo == null) geo = new Location();

            _ = Task.Run(() =>
            {
                LogEntry log = new LogEntry()
                {
                    Created = DateTime.Now,
                    X = convertedSensorValues[0],
                    Y = convertedSensorValues[1],
                    Z = convertedSensorValues[2],
                    EMF = emf,
                    Latitude = geo.Latitude,
                    Longitude = geo.Longitude,
                    Altitude = geo.Altitude,
                };

                using (var sql = new SqliteContext())
                {
                    Logs.Add(log);
                    sql.LogEntrys.Add(log);
                    sql.SaveChanges();
                }

                LogItems.Add(log);
            });
        }

        private void ClearLog()
        {
            SettingsViewModel._ThresholdRange = 250;
            MaxEMF = 0;
            Timeline.Clear();
            LogItems.Clear();
            Logs.Clear();
        }
    }
}
