/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Headtrip.iOS.Dependencies.IosSqliteService))]
namespace Headtrip.iOS.Dependencies
{
    public class IosSqliteService : ISqliteService
    {
        private string dbName = "headtrip.db3";

        public void ExportDb()
        {
           
        }

        public string GetDbPath()
        {
            return dbName;
        }
    }
}
