using qualityControl.SHARED.Dtos;

namespace qualityControl.SHARED.Interfaces
{
    public interface IWorkOrderRepo
    {
        Task<IEnumerable<WorkOrderDto>> GetAllWorkOrdersAsync();
        Task<WorkOrderDto?> GetWorkOrderByIdAsync(int logref);
        Task<IEnumerable<WorkOrderDto>> SearchWorkOrderAsync(string? query, bool onlyFinished = false, bool onlyNotFinished = false);
    }
}
