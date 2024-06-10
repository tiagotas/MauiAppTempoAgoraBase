using MauiAppTempoAgora.Models;
using System.Text.Json;

namespace MauiAppTempoAgora.Service
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisaoDoTempo(string cidade)
        {
            string appId = "3a1c2788d3f9ad95494df4258594b553";

            string url = $"http://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appId}";

            Tempo? tempo = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;

                    tempo = JsonSerializer.Deserialize<Tempo>(json);                    
                }
            }

            return tempo;
        }
    }
}
