/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headtrip.Models
{
    public class LogEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public double EMF { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Altitude { get; set; }
    }
}
