using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorsNearMe;

[ApiController]
[Route("doctor")]
public class DoctorController : ControllerBase
{
    readonly ILogger<DoctorController> _logger;
    readonly AppDbContext _ctx;

    public DoctorController(ILogger<DoctorController> logger, AppDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    // [HttpGet("all-docs")]
    // public async Task<IActionResult> GetAllWithAppointments()
    // {
    //     return Ok(
    //         await _ctx.Doctor
    //             .Include(d => d.User)
    //             .Include(d => d.Appointments)
    //                 .ThenInclude(a => a.Patient)
    //                     .ThenInclude(p => p.User)
    //             .Select(d => d.ToDoctorWithAppointmentsDto())
    //             .ToListAsync()
    //     );
    // }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            await _ctx.Doctor
            .Include(d => d.User)
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

        if (await _ctx.Doctor.SingleOrDefaultAsync(d => d.User == user) != null)
        {
            return BadRequest(new { Error = "User is already a Doctor" });
        }

        await _ctx.AddAsync(new Doctor
        {
           User = user
        });

        await _ctx.SaveChangesAsync();

        return Created();
    }
}
