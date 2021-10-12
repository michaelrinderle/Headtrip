/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Interfaces;
using System;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(Headtrip.UWP.Dependencies.WindowsSqliteService))]
namespace Headtrip.UWP.Dependencies
{
    public class WindowsSqliteService : ISqliteService
    {
        private string dbName = "headtrip.db3";

        public void ExportDb()
        {
            var internalPath = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Headtrip", dbName); ;
            var bytes = File.ReadAllBytes(internalPath);
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            var fileCopyName = string.Format("headtrip_{0:dd-MM-yyyy_HH-mm-ss-tt}.db3", System.DateTime.Now);
            string dbPath = Path.Combine(path, fileCopyName);
            File.WriteAllBytes(dbPath, bytes);
        }

        public string GetDbPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Headtrip");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, dbName);

        }
    }
}
