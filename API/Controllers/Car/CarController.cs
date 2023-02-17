using BLL.EntityCore.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Car
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet, Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_carRepository.GetAll().ToList());
        }

        /// <summary>
        /// Yeni Incident kaydını onaylanan incidentdemand üzerinden oluşturur.
        /// </summary>
        /// <param name="val">IncidentStaff kaydının bilgisidir.</param>
        [HttpPost, Route("Post")]
        public IActionResult Post([FromBody] Entity.Vehicle.Car val)
        {
            try
            {
                _carRepository.Add(val);
                _carRepository.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
