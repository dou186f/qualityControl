using Microsoft.AspNetCore.Mvc;
using qualityControl.SHARED.Interfaces;

namespace qualityControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProductionController : ControllerBase
    {
        private readonly IProductionRepo _repo;
        public ProductionController(IProductionRepo repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllProductionsAsync());

        [HttpGet("{logref:int}")]
        public async Task<IActionResult> GetById(int logref)
        {
            var i = await _repo.GetProductionByIdAsync(logref);
            return i is null ? NotFound() : Ok(i);
        }
    }
}
