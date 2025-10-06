using qualityControl.SHARED.Models;

namespace qualityControl.SHARED.Interfaces
{
    public interface IQualityControlRepo
    {
        Task<IEnumerable<QualityControl>> GetAllAsync();
        Task<IEnumerable<QualityControl>> GetBySetRefAsync(int? setref);
        Task<QualityControl?> GetQualityControlByIdAsync(int logref);
    }
}
