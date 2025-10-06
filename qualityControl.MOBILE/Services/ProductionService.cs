using System.Net.Http.Json;
using qualityControl.SHARED.Models;

namespace qualityControl.MOBILE.Services
{
    public class ProductionService
    {
        private readonly HttpClient _http;
        public ProductionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Production?> GetProductionAsync(int logref)
        {
            try
            {
                return await _http.GetFromJsonAsync<Production>($"api/Production/{logref}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Production>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<Production>>("api/Production") ?? new();
        }
    }
}
