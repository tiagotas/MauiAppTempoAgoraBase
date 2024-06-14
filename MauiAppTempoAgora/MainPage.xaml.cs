using Microsoft.Maui.Devices.Sensors;
using System;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource _cancelTokenSource;
        bool _isCheckingLocation;

        public MainPage()
        {
            InitializeComponent();
        }

        // https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key
        // https://stackoverflow.com/questions/75174113/maui-windows-platform-cant-access-location


        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                _cancelTokenSource = new CancellationTokenSource();

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                Location? location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);


                if (location != null)
                {
                    //string reverso = await GetGeocodeReverseData(location.Latitude, location.Longitude);

                    lbl_latitude.Text = location.Latitude.ToString();
                    lbl_longitude.Text = location.Longitude.ToString();


                    //lbl_localizacao.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                    //lbl_reverso.Text = reverso;
                    //await DisplayAlert("Reverso", reverso, "OK");

                    Debug.WriteLine("-------------------------------------------");
                    Debug.WriteLine(location);
                    //Debug.WriteLine(reverso);
                    Debug.WriteLine("-------------------------------------------");
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Erro: Dispositivo não Suporta", fnsEx.Message, "OK");

            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await DisplayAlert("Erro: Localização Desabilitada", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Erro: Permissão", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Erro: ", ex.Message, "OK");
            }
        }

        private async Task<string> GetGeocodeReverseData(double latitude = 47.673988, double longitude = -122.121513)
        {
            IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);

            Placemark placemark = placemarks?.FirstOrDefault();

            Debug.WriteLine("-------------------------------------------");
            Debug.WriteLine(placemarks.Count());
            Debug.WriteLine("-------------------------------------------");

            if (placemark != null)
            {
                return
                    $"AdminArea:       {placemark.AdminArea}\n" +
                    $"CountryCode:     {placemark.CountryCode}\n" +
                    $"CountryName:     {placemark.CountryName}\n" +
                    $"FeatureName:     {placemark.FeatureName}\n" +
                    $"Locality:        {placemark.Locality}\n" +
                    $"PostalCode:      {placemark.PostalCode}\n" +
                    $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                    $"SubLocality:     {placemark.SubLocality}\n" +
                    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    $"Thoroughfare:    {placemark.Thoroughfare}\n";

            }

            return "Nada";
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            // lat = longitude = 37.421998333333335
            //long = -122.084
            double latitude = Convert.ToDouble(lbl_latitude.Text);
            double longitude = Convert.ToDouble(lbl_longitude.Text);

            lbl_reverso.Text = await GetGeocodeReverseData(latitude, longitude);
        }
    }

}
