/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Android.Content;
using Com.Syncfusion.Charts;
using Headtrip.Interfaces;
using Java.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.SfChart.XForms.Droid;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Headtrip.Droid.Dependencies.AndroidPrintService))]
namespace Headtrip.Droid.Dependencies
{
    public class AndroidPrintService : IPrintService
    {
        Java.IO.File file;

        public void ExportAsPDF(string filename, Stream chartStream, Syncfusion.SfChart.XForms.SfChart chart)
        {
            try
            {
                //Create a new PDF document.
                var document = new PdfDocument();
                var page = document.Pages.Add();
                var graphics = page.Graphics;
                graphics.DrawImage(PdfImage.FromStream(chartStream), 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                document.Close(true);
                SavePDF(filename, stream);
            }
            finally
            {
                chartStream.Flush();
                chartStream.Close();
                var nativeChart = SfChartRenderer.GetNativeObject(typeof(SfChart), chart);
                // ((Com.Syncfusion.Charts.SfChart)nativeChart).DrawingCacheEnabled = false;
            }
        }

        private void SavePDF(string fileName, MemoryStream stream)
        {
            string root = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            var myDir = new Java.IO.File(root + "/Charts");
            myDir.Mkdir();

            if (file != null && file.Exists())
                file.Delete();

            file = new Java.IO.File(myDir, fileName);

            try
            {
                var outs = new FileOutputStream(file);
                outs.Write(stream.ToArray());
                outs.Flush();
                outs.Close();
            }
            catch (Exception) { }

            if (!file.Exists()) return;

            var path = Android.Net.Uri.FromFile(file);
            var extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
            var mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(path, mimeType);
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
        }
    }
}