using Microsoft.AspNetCore.Mvc;
using qualityControl.SHARED.Interfaces;

namespace qualityControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class WorkOrderController : ControllerBase
    {
        private readonly IWorkOrderRepo _repo;
        public WorkOrderController(IWorkOrderRepo repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllWorkOrdersAsync());

        [HttpGet("{logref:int}")]
        public async Task<IActionResult> GetById(int logref)
        {
            var i = await _repo.GetWorkOrderByIdAsync(logref);
            return i is null ? NotFound() : Ok(i);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery(Name = "q")] string? q, [FromQuery] bool onlyFinished = false, [FromQuery] bool onlyNotFinished = false)
        {
            if (onlyFinished && onlyNotFinished) return BadRequest("Completion filters cannot both be enabled.");
            var data = await _repo.SearchWorkOrderAsync(q, onlyFinished, onlyNotFinished);
            return Ok(data);
        }
    }
}
