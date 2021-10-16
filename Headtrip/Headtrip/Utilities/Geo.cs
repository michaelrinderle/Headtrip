/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

#pragma warning disable CS0168 

using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Headtrip.Utilities
{
    public static class Geo
    {
        public static async Task<Location> GetLocationCoordinates()
        {
            try
            {
                return await Geolocation.GetLastKnownLocationAsync();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return null;
        }
    }
}

#pragma warning restore CS0168
