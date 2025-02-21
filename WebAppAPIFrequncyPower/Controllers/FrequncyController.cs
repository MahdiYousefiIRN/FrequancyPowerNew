using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppAPIFrequncyPower.DataContext;
using WebAppAPIFrequncyPower.Model;

namespace WebAppAPIFrequncyPower.Controllers
{
    public class FrequncyController : Controller
    {

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
                var frequencies = _context.DbSetFrequency.OrderByDescending(f => f.Timestamp).Take(10).ToList();
                return Ok(frequencies);
            }

            [HttpPost]
            public IActionResult PostFrequency([FromBody] PowerFrequencyData frequencyRecord)
            {
                if (frequencyRecord.Frequency < 59)
                {
                    var alert = new Alert()
                    {
                        Timestamp = DateTime.Now,
                        Message = $"Frequency dropped below threshold: {frequencyRecord.Frequency} Hz",
                        Severity = "High"
                    };
                    _context.DbSetAlerts.Add(alert);
                }

                _context.SaveChanges();

                return CreatedAtAction(nameof(GetFrequencies), new { id = frequencyRecord.Id }, frequencyRecord);
            }
        }
    }
}
