using qualityControl.SHARED.Dtos;

namespace qualityControl.SHARED.Interfaces
{
    public interface IQualityControlResultRepo
    {
        Task<int> UpsertAsync(QualityControlResult dto);
        Task<Dictionary<int, (bool Result, int LogicalRef)>> GetResultsMapAsync(int workOrderRef);
        Task<QualityControlResult?> GetQCResultAsync(int logref);
    }
}
