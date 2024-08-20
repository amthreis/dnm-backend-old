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
        //_logger.LogCritical($"patientId = {patientId}, doctorId = {doctorId}");

        return Ok(await _ctx.Appointment
            .AsNoTracking()
            .Include(a => a.Doctor)
                .ThenInclude(d => d!.User)
            .Include(a => a.Patient)
                .ThenInclude(p => p!.User)
            .Where(a => 
                (patientId == null || a.Patient!.User.Id == patientId.Value) 
                && (doctorId == null || a.Doctor!.User.Id == doctorId.Value)
            )
            .Select(a => a.ToAppointmentDto())
            .ToListAsync());
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(int userId = 1)
    {
        var appt = await _ctx.Appointment.SingleOrDefaultAsync(c => c.Id == userId);

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

        var user = new Appointment
        {
            ReviewContent = "Muito bom!",
            ReviewedAt = DateTime.Now,
            Clinic = clinic,
            ReviewScore = ReviewScore.Positive,
            CreatedAt = DateTime.Now
        };

        var app = await _ctx.Appointment.AddAsync(user);

        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { user.Id }, user);
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> Delete(int userId = 1)
    {
        var appt = await _ctx.Appointment.SingleOrDefaultAsync(c => c.Id == userId);

        if (appt == null)
        {
            return BadRequest(new { Error = "Appointment not found" });
        }

        _ctx.Remove(appt);

        await _ctx.SaveChangesAsync();

        return NoContent();
    }
}
