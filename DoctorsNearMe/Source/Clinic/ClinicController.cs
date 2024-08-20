using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace DoctorsNearMe
{
    [ApiController]
    [Route("/")]
    public class ClinicController : ControllerBase
    {
        const int PageSize = 10;

        readonly ILogger<ClinicController> _logger;
        readonly AppDbContext _ctx;

        public ClinicController(ILogger<ClinicController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ctx.Clinic.AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}", Name = "Get by Id")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var clinic = await _ctx.Clinic.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            
            if (clinic == null)
            {
                return BadRequest("Clinic not found");
            }

            return Ok(clinic);
        }

        [HttpGet("near-me")]
        public async Task<IActionResult> FindNearMe(
            [FromQuery] LongLat longLat,
            [FromQuery][DefaultValue(1)] int page = 1
        )
        {
            var pt = new LongLat(longLat.Long, longLat.Lat).ToPoint();

            var clinics = await _ctx.Clinic
                .AsNoTracking()
                .OrderBy(c => c.Location.Distance(pt))
                .Skip(PageSize * (page - 1))
                .Take(PageSize)
                .ToListAsync();

            return Ok(clinics.ToClinicsNearMeDto(pt, _logger));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateOrUpdateClinicDto ccDto)
        {
            // var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            // var p = gf.CreatePoint(new Coordinate(ccDto.LongLat.Long, ccDto.LongLat.Lat));

            await _ctx.Clinic.AddAsync(new Clinic()
            {
                Name = ccDto.Name,
                Location = ccDto.LongLat.ToPoint()
            });

            await _ctx.SaveChangesAsync();

            //_logger.LogCritical($"{p.X}, {p.Y}");

            return Created();
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateClinic([FromRoute] int id, CreateOrUpdateClinicDto ucDto)
        {
            var clinic = await _ctx.Clinic.SingleOrDefaultAsync(c => c.Id == id);
            
            if (clinic == null)
            {
                return BadRequest("Clinic not found");
            }

            clinic.Location = ucDto.LongLat.ToPoint();
            clinic.Name = ucDto.Name;

            await _ctx.SaveChangesAsync();

            return Ok(clinic);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteClinicDto dcDto)
        {
            _ctx.Clinic.Remove(new Clinic
            {
                Id = dcDto.Id
            });

            await _ctx.SaveChangesAsync();

            return Ok();
        }

        
    }
}
