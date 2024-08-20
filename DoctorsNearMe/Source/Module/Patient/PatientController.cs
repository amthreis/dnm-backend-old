using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorsNearMe;

[ApiController]
[Route("patient")]
public class PatientController : ControllerBase
{
    readonly ILogger<PatientController> _logger;
    readonly AppDbContext _ctx;

    public PatientController(ILogger<PatientController> logger, AppDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            await _ctx.Patient
            .Include(d => d.User)
            .Select(d => d.ToPatientDto())
            .ToListAsync()
        );
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDoctorDto cd)
    {
        var user = await _ctx.User.SingleOrDefaultAsync(u => u.Id == cd.UserId);

        if (user == null)
        {
            return BadRequest(new { Error = "User not found" });
        }

        if (await _ctx.Patient.SingleOrDefaultAsync(d => d.User == user) != null)
        {
            return BadRequest(new { Error = "User is already a Patient" });
        }

        await _ctx.Patient.AddAsync(new Patient
        {
           User = user
        });

        await _ctx.SaveChangesAsync();

        return Created();
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> Delete(int userId)
    {
        var user = await _ctx.User.SingleOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return BadRequest(new { Error = "User not found" });
        }

        if (await _ctx.Patient.SingleOrDefaultAsync(d => d.User == user) == null)
        {
            return BadRequest(new { Error = "User NOT a Doctor" });
        }

        _ctx.Patient.Remove(new Patient
        {
           User = user
        });

        await _ctx.SaveChangesAsync();

        return NoContent();
    }
}
