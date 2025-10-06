using Microsoft.AspNetCore.Mvc;
using qualityControl.SHARED.Interfaces;

namespace qualityControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ItemController : ControllerBase
    {
        private readonly IItemRepo _repo;
        public ItemController(IItemRepo repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllItemsAsync());

        [HttpGet("{logref:int}")]
        public async Task<IActionResult> GetById(int logref)
        {
            var i = await _repo.GetItemByIdAsync(logref);
            return i is null ? NotFound() : Ok(i);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? query)
        {
            var data = await _repo.SearchItemAsync(query);
            return Ok(data);
        }
    }
}
