using System.ComponentModel;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace DoctorsNearMe
{
    [ApiController]
    [Route("appointment")]
    public class AppointmentController : ControllerBase
    {
        const int PageSize = 10;

        readonly ILogger<AppointmentController> _logger;
        readonly AppDbContext _ctx;

        public AppointmentController(ILogger<AppointmentController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ctx.Appointment.AsNoTracking().ToListAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id = 1)
        {
            var appt = await _ctx.Appointment.SingleOrDefaultAsync(c => c.Id == id);

            if (appt == null)
            {
                return BadRequest(new { Error = "Appointment not found" });
            }

            return Ok(appt);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create()
        {
            var clinic = await _ctx.Clinic.SingleOrDefaultAsync(c => c.Id == 1);

            if (clinic == null)
            {
                return BadRequest(new { Error = "Clinic not found" });
            }

            await _ctx.Appointment.AddAsync(new Appointment
            {
                ReviewContent = "Muito bom!",
                ReviewedAt = DateTime.Now,
                Clinic = clinic,
                ReviewScore = ReviewScore.Positive,
                CreatedAt = DateTime.Now
            });

            await _ctx.SaveChangesAsync();

            return Created();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id = 1)
        {
            var appt = await _ctx.Appointment.SingleOrDefaultAsync(c => c.Id == id);

            if (appt == null)
            {
                return BadRequest(new { Error = "Appointment not found" });
            }

            _ctx.Remove(appt);

            await _ctx.SaveChangesAsync();

            return Created();
        }
    }
}
