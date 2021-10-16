using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Headtrip.Models
{
    public class SensorSpeeds
    {
        public SensorSpeeds(int index, SensorSpeed sensorSpeed)
        {
            Index = index;
            SensorSpeed = sensorSpeed;
        }

        int Index { get; set; }
        SensorSpeed SensorSpeed { get; set; }
    }
}
