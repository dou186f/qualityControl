using Microsoft.AspNetCore.Mvc;
using qualityControl.SHARED.Interfaces;
using qualityControl.SHARED.Models;

namespace qualityControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class QualityControlController : ControllerBase
    {
        private readonly IQualityControlRepo _repo;
        public QualityControlController(IQualityControlRepo repo) => _repo = repo;
  
        [HttpGet]
        public async Task<ActionResult<List<QualityControl>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{logref:int}")]
        public async Task<IActionResult> GetById(int logref)
        {
            var i = await _repo.GetQualityControlByIdAsync(logref);
            return i is null ? NotFound() : Ok(i);
        }

    }
}
