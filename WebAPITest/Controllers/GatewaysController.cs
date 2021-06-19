using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models;

namespace WebAPITest.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class GatewaysController : ControllerBase
    {
        private readonly TodoContext _context;

        public GatewaysController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Gateways
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGateways()
        {
            return await _context.Gateways.ToListAsync();
        }

        // GET: api/Gateways/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> GetGateway(string id)
        {
            var gateway = await _context.Gateways.Include(d => d.PeripheralDevices).FirstOrDefaultAsync(x => x.SerialNumber == id);

            if (gateway == null)
            {
                return NotFound();
            }

            return gateway;
        }

        // GET: api/Gateways/{id}/Devices
        [HttpGet("{id}/Devices")]
        public async Task<ActionResult<ICollection<Device>>> GetDevices(string id)
        {
            var gateway = await _context.Gateways.Include(d => d.PeripheralDevices).FirstOrDefaultAsync(x => x.SerialNumber == id);

            if (gateway == null)
            {
                return NotFound();
            }

            return Ok(gateway.PeripheralDevices);
        }

        // GET: api/Gateways/{id}
        [HttpGet("{id}/Devices/{deviceId}")]
        public async Task<ActionResult<Device>> GetDevice(string id, long deviceId)
        {
            var gateway = await _context.Gateways.Include(d => d.PeripheralDevices).FirstOrDefaultAsync(x => x.SerialNumber == id);

            if (gateway == null)
            {
                return NotFound();
            }

            var device = gateway.PeripheralDevices.FirstOrDefault(x => x.UID == deviceId);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        [HttpPost]
        public async Task<ActionResult<Gateway>> PostGateway(Gateway gateway)
        {
            if (!IPAddress.TryParse(gateway.IP, out _))
            {
                return BadRequest("That's not an IP address");
            }

            var existsGW = await _context.Gateways.Include(d => d.PeripheralDevices).AnyAsync(x => x.SerialNumber == gateway.SerialNumber);

            if (existsGW)
            {
                return Conflict();
            }

            _context.Gateways.Add(gateway);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GatewayExists(gateway.SerialNumber))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGateway", new { id = gateway.SerialNumber }, gateway);
        }

        [HttpPost("{id}/Devices")]
        public async Task<ActionResult<Device>> PostDevices(string id, Device device)
        {
            var gateway = await _context.Gateways.Include(d => d.PeripheralDevices).FirstOrDefaultAsync(x => x.SerialNumber == id);

            if (gateway == null)
            {
                return NotFound();
            }

            if (gateway.PeripheralDevices.Any(x => x.UID == device.UID))
            {
                return Conflict();
            }

            if (gateway.PeripheralDevices.Count == 10)
            {
                return Forbid();
            }

            gateway.PeripheralDevices.Add(device);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeviceExists(id, device.UID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGateway", new { id = device.Guid, vendor = device.Vendor }, device);
        }

        // DELETE: api/Gateways/{id}
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGateway(string id)
        {
            var gateway = await _context.Gateways.Include(d => d.PeripheralDevices).FirstOrDefaultAsync(x => x.SerialNumber == id);
            if (gateway == null)
            {
                return NotFound();
            }

            _context.Gateways.Remove(gateway);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        [HttpDelete("{id}/Devices/{guid}")]
        public async Task<IActionResult> DeleteDevice(string id, long guid)
        {
            var gateway = await _context.Gateways.Include(d => d.PeripheralDevices).FirstOrDefaultAsync(x => x.SerialNumber == id);
            if (gateway == null)
            {
                return NotFound();
            }

            var device = gateway.PeripheralDevices.FirstOrDefault(x => x.Guid == guid);
            if (device != null)
            {
                gateway.PeripheralDevices.Remove(device);
            }
            
            await _context.SaveChangesAsync();

            return Ok(guid);
        }

        private bool GatewayExists(string id)
        {
            return _context.Gateways.Any(e => e.SerialNumber == id);
        }

        private bool DeviceExists(string id, long deviceId)
        {
            return _context.Gateways.Any(e => e.SerialNumber == id && e.PeripheralDevices.Any(d => d.UID == deviceId));
        }
    }
}
