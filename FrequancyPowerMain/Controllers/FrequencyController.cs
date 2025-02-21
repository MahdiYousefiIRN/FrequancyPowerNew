using FrequancyPowerMain.DataContex;
using FrequancyPowerMain.Model;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class FrequencyController : ControllerBase
{
    private readonly PowerGridContext _context;

    public FrequencyController(PowerGridContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetFrequencies()
    {
        var frequencies = _context.FrequencyRecords.OrderByDescending(f => f.Timestamp).Take(10).ToList();
        return Ok(frequencies);
    }

    [HttpPost]
    public IActionResult PostFrequency([FromBody] FrequencyRecord frequencyRecord)
    {
        if (frequencyRecord.Frequency < 59)
        {
            var alert = new Alert
            {
                Timestamp = DateTime.Now,
                Message = $"Frequency dropped below threshold: {frequencyRecord.Frequency} Hz",
                Severity = "High"
            };
            _context.Alerts.Add(alert);
        }

        _context.FrequencyRecords.Add(frequencyRecord);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetFrequencies), new { id = frequencyRecord.Id }, frequencyRecord);
    }
}