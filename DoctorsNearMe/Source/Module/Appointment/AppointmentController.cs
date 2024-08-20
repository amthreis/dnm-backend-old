using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorsNearMe;

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
    public async Task<IActionResult> GetAll(
        [FromQuery] int? doctorId,
        [FromQuery] int? patientId
    )
    {
        return Ok(await _ctx.Appointment
            .AsNoTracking()
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.Patient)
                .ThenInclude(d => d.User)
            .Where(a => patientId == null || a.Patient.User.Id == patientId)
            .Where(a => doctorId == null || a.Doctor.User.Id == doctorId)
            .Select(a => a.ToAppointmentDto())
            .ToListAsync());
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

        return NoContent();
    }
}
