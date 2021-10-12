/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Syncfusion.SfChart.XForms;
using System.IO;

namespace Headtrip.Interfaces
{
    public interface IPrintService
    {
        void ExportAsPDF(string filename, Stream chartStream, SfChart chart);
    }
}
