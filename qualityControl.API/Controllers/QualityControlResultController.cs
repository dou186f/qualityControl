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
            [FromServices] IWorkOrderRepo woRepo,
            [FromServices] IItemRepo itemRepo,
            [FromServices] IQualityControlRepo qcRepo)
        {
            var error = await ValidateAsync(dto, woRepo, itemRepo, qcRepo);
            if (error is not null) return error;
            var logicalRef = await _repo.UpsertAsync(dto);
            return Ok(new { logicalRef });
        }

        [HttpPost("results")]
        public async Task<IActionResult> UpsertBatch([FromBody] List<QualityControlResult>? results,
            [FromServices] IWorkOrderRepo woRepo,
            [FromServices] IItemRepo itemRepo,
            [FromServices] IQualityControlRepo qcRepo)
        {
            if (results is null || results.Count == 0) return BadRequest("Select at least one result.");
            var keys = new HashSet<(int, int)>();
            foreach (var dto in results)
            {
                var error = await ValidateAsync(dto, woRepo, itemRepo, qcRepo);
                if (error is not null) return error;
                if (!keys.Add((dto.WorkOrderRef, dto.QcRef)))
                    return BadRequest("Duplicate work order and test result.");
            }
            var logicalRefs = await _repo.UpsertBatchAsync(results);
            return Ok(new { logicalRefs });
        }

        private async Task<IActionResult?> ValidateAsync(QualityControlResult? dto,
            IWorkOrderRepo woRepo, IItemRepo itemRepo, IQualityControlRepo qcRepo)
        {
            if (dto is null) return BadRequest("The body contains an empty result.");
            if (dto.WorkOrderRef <= 0) return BadRequest("Invalid WorkOrderRef.");
            if (dto.QcRef <= 0) return BadRequest("Invalid QcRef.");
            var wo = await woRepo.GetWorkOrderByIdAsync(dto.WorkOrderRef);
            if (wo is null) return NotFound("Work order not found.");
            var item = await itemRepo.GetItemByIdAsync(wo.ItemRef);
            if (item is null) return NotFound("No related item found.");
            var def = await qcRepo.GetQualityControlByIdAsync(dto.QcRef);
            if (def is null) return NotFound("Quality control test not found.");
            if (item.QccSetRef is null or <= 0 || def.SetRef != item.QccSetRef)
                return BadRequest("The test does not belong to the work order item's quality control set.");
            dto.Name = def.Name;
            dto.SetRef = def.SetRef;
            return null;
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
