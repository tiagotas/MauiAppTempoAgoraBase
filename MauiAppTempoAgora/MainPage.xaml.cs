using Microsoft.Maui.Devices.Sensors;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public MainPage()
        {
            InitializeComponent();
        }

        // https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key
        // https://stackoverflow.com/questions/75174113/maui-windows-platform-cant-access-location

        public async Task<string> GetCachedLocation()
        {
            Platform.MapServiceToken = "Ak76gvzfN2uHruYAG8pNyE_2ipU9lV5MaN0YQ0k9WP8b8kRaKWpp4hr4ZFJaA1E4";

            //await DisplayAlert("Token de Mapas:", Platform.MapServiceToken, "OK");

            try
            {

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                string reverso = await GetGeocodeReverseData(location.Latitude, location.Longitude);

                lbl_reverso.Text = reverso;
                await DisplayAlert("Reverso", reverso, "OK");



                Console.WriteLine("-------------------------------------------");
                Console.WriteLine(location);
                Console.WriteLine(reverso);
                Console.WriteLine("-------------------------------------------");

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
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

            return "None";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string teste = await GetCachedLocation();            

            lbl_localizacao.Text = teste;            
        }

        private async Task<string> GetGeocodeReverseData(double latitude = 47.673988, double longitude = -122.121513)
        {
            IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);

            

            Placemark placemark = placemarks?.FirstOrDefault();

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

            return "";
        }
    }

}
