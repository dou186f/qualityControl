using qualityControl.SHARED.Models;

namespace qualityControl.SHARED.Interfaces
{
    public interface IItemRepo
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int logref);
        Task<IEnumerable<Item>> SearchItemAsync(string? query);
    }
}
