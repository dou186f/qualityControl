using System.Net.Http.Json;
using qualityControl.SHARED.Dtos;
using qualityControl.SHARED.Models;

namespace qualityControl.MOBILE.Services
{
    public class QualityControlService
    {
        private readonly HttpClient _http;
        public QualityControlService(HttpClient http)
        {
            _http = http;
        }
            
        public async Task<QualityControlResult?> GetQualityControlResultAsync(int logref)
        {
            try
            {
                return await _http.GetFromJsonAsync<QualityControlResult>($"api/Qc/{logref}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<QualityControl?> GetQualityControlAsync(int logref)
        {
            try
            {
                return await _http.GetFromJsonAsync<QualityControl>($"api/QualityControl/{logref}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<QcChecklistItemDto>> GetChecklistAsync(int workOrderRef)
        {
            var url = $"api/Qc/checklist?workOrderRef={workOrderRef}";
            return await _http.GetFromJsonAsync<List<QcChecklistItemDto>>(url) ?? new List<QcChecklistItemDto>();
        }

        public async Task<int> UpsertResultAsync(int workOrderRef, int qcRef, bool result)
        {
            var resp = await _http.PostAsJsonAsync("api/Qc/result", new
            {
                WorkOrderRef = workOrderRef,
                QcRef        = qcRef,
                Result       = result
            });
            resp.EnsureSuccessStatusCode();

            var payload = await resp.Content.ReadFromJsonAsync<Dictionary<string, int>>();
            return payload is not null && payload.TryGetValue("logicalRef", out var id) ? id : 0;
        }

        public async Task SaveAllAsync(int workOrderRef, IEnumerable<QcChecklistItemDto> items)
        {
            foreach (var x in items.Where(i => i.Result.HasValue))
                _ = await UpsertResultAsync(workOrderRef, x.QcRef, x.Result!.Value);
        }
    }
}
