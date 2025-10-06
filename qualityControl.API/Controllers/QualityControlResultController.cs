using Microsoft.AspNetCore.Mvc;
using qualityControl.SHARED.Dtos;
using qualityControl.SHARED.Interfaces;

namespace qualityControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class QcController : ControllerBase
    {
        private readonly IQualityControlResultRepo _repo;
        public QcController(IQualityControlResultRepo repo) => _repo = repo;

        [HttpPost("result")]
        public async Task<IActionResult> Upsert([FromBody] QualityControlResult dto,
        [FromServices] IQualityControlRepo qcRepo,
        [FromServices] IQualityControlResultRepo resultRepo)
        {
        if (dto is null) return BadRequest("The body is empty.");
        if (dto.WorkOrderRef <= 0) return BadRequest("Invalid WorkOrderRef.");
        if (dto.QcRef <= 0) return BadRequest("Invalid QcRef.");

        var def = await qcRepo.GetQualityControlByIdAsync(dto.QcRef);
        if (def is null) return BadRequest("Invalid QcRef.");
        dto.Name  = def.Name;
        dto.SetRef = def.SetRef;

        var logicalRef = await resultRepo.UpsertAsync(dto);
        return Ok(new { logicalRef });
        }

        [HttpGet("{logref:int}")]
        public async Task<IActionResult> Get(int logref)
        {
            if (logref <= 0) return BadRequest("Parameter is invalid.");
            var data = await _repo.GetQCResultAsync(logref);
            return data is null ? NotFound() : Ok(data);
        }

        [HttpGet("checklist")]
        public async Task<ActionResult<List<QcChecklistItemDto>>> GetChecklist(
        [FromQuery] int workOrderRef,
        [FromServices] IWorkOrderRepo woRepo,
        [FromServices] IItemRepo itemRepo,
        [FromServices] IQualityControlRepo qcRepo,
        [FromServices] IQualityControlResultRepo resultRepo)
        {
            if (workOrderRef <= 0) return BadRequest("Invalid WorkOrderRef.");

            var wo = await woRepo.GetWorkOrderByIdAsync(workOrderRef);
            if (wo is null) return NotFound("Work order not found.");
            var itemRef = wo.ItemRef;


            var item = await itemRepo.GetItemByIdAsync(itemRef);
            if (item is null) return NotFound("No related item found.");
            var setRef = item.QccSetRef;

            var defs = await qcRepo.GetBySetRefAsync(setRef);


            var map  = await resultRepo.GetResultsMapAsync(workOrderRef);

            var list = defs.Select(d =>
            {
                var has = map.TryGetValue(d.LogicalRef, out var entry);
                return new QcChecklistItemDto
                {
                    QcRef  = d.LogicalRef,
                    Name   = d.Name ?? "",
                    SetRef = d.SetRef,
                    MinVal = d.MinVal,
                    MaxVal = d.MaxVal,
                    Result = has ? entry.Result : (bool?)null,
                    ResultLogicalRef = has ? entry.LogicalRef : null
                };
            })
            .OrderBy(x => x.QcRef)
            .ToList();

            return Ok(list);
        }
    }
}
