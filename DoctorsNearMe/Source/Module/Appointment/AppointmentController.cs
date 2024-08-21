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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id = 1)
    {
        var appt = await _ctx.Appointment
            .AsNoTracking()
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .SingleOrDefaultAsync(a => a.Id == id);

        if (appt == null)
        {
            return BadRequest(new { Error = "Appointment not found" });
        }

        return Ok(appt.ToAppointmentDto());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateAppointmentDto ca)
    {
        var clinic = await _ctx.Clinic.SingleOrDefaultAsync(c => c.Id == ca.ClinicId);

        if (clinic == null)
        {
            return BadRequest(new { Error = "Clinic not found" });
        }

        var doctor = await _ctx.Doctor.Include(d => d.User).SingleOrDefaultAsync(c => c.User.Id == ca.DoctorUserId);

        if (doctor == null)
        {
            return BadRequest(new { Error = "Doctor not found" });
        }

        var patient = await _ctx.Patient.Include(d => d.User).SingleOrDefaultAsync(c => c.User.Id == ca.PatientUserId);

        if (patient == null)
        {
            return BadRequest(new { Error = "Patient not found" });
        }

        if (doctor.User == patient.User)
        {
            return BadRequest(new { Error = "Can't make an Appointment where the Doctor and Patient are the same person" });
        }

        var appt = new Appointment
        {
            Clinic = clinic,
            Patient = patient,
            Doctor = doctor,
            CreatedAt = DateTime.Now
        };

        await _ctx.Appointment.AddAsync(appt);
        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { appt.Id }, appt.ToAppointmentDto());
    }

    [HttpPatch("review")]
    public async Task<IActionResult> Review(ReviewAppointmentDto ra)
    {
        var appt = await _ctx.Appointment
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .SingleOrDefaultAsync(c => c.Id == ra.AppointmentId);

        if (appt == null)
        {
            return BadRequest(new { Error = "Appointment not found" });
        }

        if (appt.State != AppointmentState.Over)
        {
            return BadRequest(new { Error = "Appointment is not over" });
        }

        if (appt.ReviewedAt != null)
        {
            return BadRequest(new { Error = "Appointment has been reviewed already" });
        }

        if (DateTime.Now > appt.EndedAt.Value.AddDays(7))
        {
            return BadRequest(new { Error = "Can't review an Appointment 7 days after its end" });
        }

        var user = await _ctx.User.SingleOrDefaultAsync(c => c.Id == ra.PatientUserId);

        if (user == null)
        {
            return BadRequest(new { Error = "User not found" });
        }

        if (appt.Patient.User != user)
        {
            return BadRequest(new { Error = "User is not the patient of this appointment" });
        }

        appt.ReviewContent = ra.ReviewContent;
        appt.ReviewScore = ra.ReviewScore;
        appt.ReviewedAt = DateTime.Now;

        await _ctx.SaveChangesAsync();

        return Ok(appt);
    }

    [HttpPatch("end")]
    public async Task<IActionResult> End(EndAppointmentDto ea)
    {
        var appt = await _ctx.Appointment
            .Include(a => a.Doctor)
                .ThenInclude(p => p.User)
            .SingleOrDefaultAsync(c => c.Id == ea.AppointmentId);

        if (appt == null)
        {
            return BadRequest(new { Error = "Appointment not found" });
        }

        if (appt.EndedAt != null)
        {
            return BadRequest(new { Error = "Appointment is over already" });
        }

        var docUser = await _ctx.User.SingleOrDefaultAsync(c => c.Id == ea.DoctorUserId);

        if (docUser == null)
        {
            return BadRequest(new { Error = "User not found" });
        }

        if (appt.Doctor.User != docUser)
        {
            return BadRequest(new { Error = "User is not the Doctor of this Appointment" });
        }

        appt.EndedAt = DateTime.Now;
        appt.State = AppointmentState.Over;

        await _ctx.SaveChangesAsync();

        return Ok(appt.ToAppointmentDto());
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
