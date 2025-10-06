using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using qualityControl.SHARED.Models;

namespace qualityControl.MOBILE.Services
{
    public class ItemService
    {
        private readonly HttpClient _http;
        public ItemService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Item?> GetItemAsync(int logref)
        {
            try
            {
                return await _http.GetFromJsonAsync<Item>($"api/Item/{logref}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<Item>>("api/Item") ?? new();
        }

        public async Task<List<Item>> SearchItemAsync(string? query, CancellationToken ct = default)
        {
            var data = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(query))
            {
                data["query"] = query;
            }

            var url = QueryHelpers.AddQueryString("api/Item/search", data);

            try
            {
                return await _http.GetFromJsonAsync<List<Item>>(url, ct) ?? new();
            }
            catch (OperationCanceledException) 
            { 
                throw;
            }
            catch (HttpRequestException) 
            { 
                return new(); 
            }
        }
    }
}
