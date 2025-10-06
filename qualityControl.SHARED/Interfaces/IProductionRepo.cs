using qualityControl.SHARED.Models;

namespace qualityControl.SHARED.Interfaces
{
    public interface IProductionRepo
    {
        Task<IEnumerable<Production>> GetAllProductionsAsync();
        Task<Production?> GetProductionByIdAsync(int logref);
    }
}
