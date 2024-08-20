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
        public async Task<IActionResult> Get()
        {
            return Ok(await _ctx.Clinic.AsNoTracking().ToListAsync());
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateClinicDto ccDto)
        {
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var p = gf.CreatePoint(new Coordinate(ccDto.Location.Long, ccDto.Location.Lat));

            //GeometryFactory.CreatePointFromInternalCoord(new Coordinate(ccDto.Location.Lat, ccDto.Location.Long));

            await _ctx.Clinic.AddAsync(new Clinic()
            {
                Name = ccDto.Name,
                Location = p
            });

            await _ctx.SaveChangesAsync();

            _logger.LogCritical($"{p.X}, {p.Y}");

            return Ok();
        }
    }
}
