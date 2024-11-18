using Microsoft.AspNetCore.Mvc;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Services;

namespace TruckDeliveryPlatform.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("distance")]
        public IActionResult GetDistance(int fromId, int toId)
        {
            try
            {
                var from = _context.Locations.Find(fromId);
                var to = _context.Locations.Find(toId);
                var config = _context.SystemConfigs.First();

                if (from == null || to == null)
                    return BadRequest("Invalid locations");

                var distance = DistanceCalculator.CalculateDistance(from, to);
                var cost = (decimal)distance * config.PricePerKilometer + config.BaseFee;

                return Ok(new { distance, cost });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating distance: {ex.Message}");
            }
        }
    }
} 