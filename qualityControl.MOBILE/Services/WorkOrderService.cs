using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using qualityControl.SHARED.Dtos;

namespace qualityControl.MOBILE.Services
{
    public class WorkOrderService
    {
        private readonly HttpClient _http;
        public WorkOrderService(HttpClient http)
        {
            _http = http;
        }

        public async Task<WorkOrderDto?> GetWorkOrderAsync(int logref)
        {
            try
            {
                return await _http.GetFromJsonAsync<WorkOrderDto>($"api/WorkOrder/{logref}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<WorkOrderDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<WorkOrderDto>>("api/WorkOrder") ?? new();
        }

        public async Task<List<WorkOrderDto>> SearchWorkOrderAsync(string? query, bool onlyFinished = false, bool onlyNotFinished = false, CancellationToken ct = default)
        {
            var data = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(query))
            {
                data["q"] = query;
            }

            data["onlyFinished"] = onlyFinished ? "true" : "false";
            data["onlyNotFinished"] = onlyNotFinished ? "true" : "false";

            var url = QueryHelpers.AddQueryString("api/WorkOrder/search", data);

            try
            {
                return await _http.GetFromJsonAsync<List<WorkOrderDto>>(url, ct) ?? new();
            }
            catch (OperationCanceledException) 
            { 
                throw;
            }
            catch (HttpRequestException ex) 
            { 
                throw new Exception("API call failed.", ex);
            }
        }
    }
}
