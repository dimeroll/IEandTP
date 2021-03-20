using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab1_ICtaTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly Lab1DBContext _context;

        public ChartsController(Lab1DBContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var roles = _context.Roles.Include(b => b.TrainWorkers).ToList();

            List<object> roleWorkers = new List<object>();
            roleWorkers.Add(new[] { "Роль", "Кількість працівників" });

            foreach (var c in roles)
            {
                roleWorkers.Add(new object[] { c.RoleName, c.TrainWorkers.Count() });
            }
            return new JsonResult(roleWorkers);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var trainTypes = _context.TrainTypes.Include(b => b.Trains).ToList();

            List<object> TrainsForType = new List<object>();
            TrainsForType.Add(new[] { "Тип", "Кількість потягів" });

            foreach (var c in trainTypes)
            {
                TrainsForType.Add(new object[] { c.TrainTypeName, c.Trains.Count() });
            }
            return new JsonResult(TrainsForType);
        }
    }
}
