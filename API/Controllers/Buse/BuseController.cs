using BLL.EntityCore.Abstract;
using DTO.Vehicle;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Buse
{
    [ApiController]
    [Route("[controller]")]
    public class BuseController : Controller
    {
        private readonly IBuseRepository _buseRepository;
        public BuseController(IBuseRepository buseRepository)
        {
            _buseRepository = buseRepository;
        }
        /// <summary>
        /// Tüm Buse verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_buseRepository.GetAll(x => x.DataStatus == Entity.Shared.DataStatus.Activated).ToList());
        }

        /// <summary>
        /// Tekil Buse verisini getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var res = new DTO.Shared.DataAccessResult<object>();

            if (id == null)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
            return Ok(_buseRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault());
        }

        /// <summary>
        /// Yeni Buse kaydını oluşturur.
        /// </summary>
        /// <param name="val">Oluşturulacak Buse kaydının bilgisidir.</param>
        [HttpPost, Route("Post")]
        public IActionResult Post([FromBody] Entity.Vehicle.Buse val)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = new DTO.Shared.DataAccessResult<object>();

            try
            {
                _buseRepository.Add(val);
                _buseRepository.Commit();
                res.Result = true;
                res.ResultMessage = "İşlem Başarılı";
                res.Object = val;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
        }

        /// <summary>
        /// Buse kaydını günceller.
        /// </summary>
        /// <param name="id">Guncellenecek Buse kaydının tekil bilgisidir.</param>
        /// <param name="val">Guncellenecek Buse kaydının bilgisidir.</param>
        [HttpPost, Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] Entity.Vehicle.Buse val)
        {
            var res = new DTO.Shared.DataAccessResult<object>();

            if (id == null)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }

            try
            {
                var entity = _buseRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault();
                entity = val;
                _buseRepository.Update(entity);
                _buseRepository.Commit();
                res.Result = true;
                res.ResultMessage = "Kayıt Güncellendi";
                res.Object = entity;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }

        }

        /// <summary>
        /// Buse kaydını siler.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("Delete/{id}")]
        [Produces("application/json")]
        public IActionResult Delete(int id)
        {

            var res = new DTO.Shared.DataAccessResult<object>();
            try
            {
                var entity = _buseRepository.GetSingle(x => x.Id == id);
                if (entity == null)
                {
                    res.Result = false;
                    res.ResultMessage = "Kayıt Bulunamadı";
                    return NotFound(res);
                }
                _buseRepository.Delete(entity);
                _buseRepository.Commit();
                res.Result = true;
                res.ResultMessage = "Kayıt Silindi";
                res.Object = entity;
                return NoContent();
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
        }

        /// <summary>
        /// Buse kayıtlarını renge göre filtreler.
        /// </summary>
        /// <param name="val">Oluşturulacak Buse kaydının bilgisidir.</param>
        [HttpGet, Route("ColorFilter")]
        public IActionResult ColorFilter([FromBody] VehicleColorFilter filterDto)
        {
            var res = new DTO.Shared.DataAccessResult<object>();

            if (filterDto == null)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }

            try
            {
                var entities = _buseRepository.FindBy(x => x.Color == filterDto.Color && x.DataStatus == Entity.Shared.DataStatus.Activated).ToList();

                res.Result = true;
                res.Object = entities;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }

        }


        /// <summary>
        /// Far değerini değiştirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("ChangeHeadlight/{id:int}")]
        public IActionResult ChangeHeadlight(int id)
        {
            var res = new DTO.Shared.DataAccessResult<object>();

            if (id == null)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
            try
            {
                var entity = _buseRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault();
                entity.Headlight = entity.Headlight == false ? entity.Headlight = true : entity.Headlight = false;
                _buseRepository.Update(entity);
                _buseRepository.Commit();
                res.Result = true;
                res.ResultMessage = entity.Headlight == false ? "Farlar Açıldı" : "Farlar Kapatıldı";
                res.Object = entity;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
        }
    }
}
