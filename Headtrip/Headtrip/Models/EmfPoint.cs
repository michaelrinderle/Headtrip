/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using System;
using System.ComponentModel;

namespace Headtrip.Models
{
    public class EmfPoint : INotifyPropertyChanged
    {
        public DateTime _XValue { get; set; }
        public DateTime XValue
        {
            get { return _XValue; }
            set
            {
                _XValue = value;
                RaisePropertyChanged(nameof(XValue));
            }
        }

        public float _YValue { get; set; }
        public float YValue
        {
            get { return _YValue; }
            set
            {
                _YValue = value;
                RaisePropertyChanged(nameof(YValue));
            }
        }

        public EmfPoint(DateTime xValue, float yValue)
        {
            _XValue = xValue;
            YValue = yValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}