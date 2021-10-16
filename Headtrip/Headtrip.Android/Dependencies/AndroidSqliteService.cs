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

[assembly: Xamarin.Forms.Dependency(typeof(Headtrip.Droid.Dependencies.AndroidSqliteService))]
namespace Headtrip.Droid.Dependencies
{
    public class AndroidSqliteService : ISqliteService
    {
        private string dbName = "headtrip.db3";

        [Obsolete]
        public void ExportDb()
        {
            var internalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
            var bytes = File.ReadAllBytes(internalPath);
            string path = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

            var fileCopyName = string.Format("headtrip_{0:dd-MM-yyyy_HH-mm-ss-tt}.db3", System.DateTime.Now);
            string dbPath = Path.Combine(path, "exports");

            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            string dpFilePath = Path.Combine(dbPath, fileCopyName);
            File.WriteAllBytes(dpFilePath, bytes);

        }

        public string GetDbPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
        }
    }
}