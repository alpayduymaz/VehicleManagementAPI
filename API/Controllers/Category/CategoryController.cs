using BLL.EntityCore.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Category
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _CategoryRepository;
        public CategoryController(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        /// <summary>
        /// Tüm Category verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_CategoryRepository.FindBy(x => x.DataStatus == Entity.Shared.DataStatus.Activated).ToList());
        }

        /// <summary>
        /// Tekil Category verisini getirir.
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
            return Ok(_CategoryRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault());
        }

        /// <summary>
        /// Yeni Category kaydını oluşturur.
        /// </summary>
        /// <param name="val">Oluşturulacak Category kaydının bilgisidir.</param>
        [HttpPost, Route("Post")]
        public IActionResult Post([FromBody] Entity.Vehicle.Categories val)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = new DTO.Shared.DataAccessResult<object>();

            try
            {
                _CategoryRepository.Add(val);
                _CategoryRepository.Commit();
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
        /// Category kaydını günceller.
        /// </summary>
        /// <param name="id">Guncellenecek Category kaydının tekil bilgisidir.</param>
        /// <param name="val">Guncellenecek Category kaydının bilgisidir.</param>
        [HttpPost, Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] Entity.Vehicle.Categories val)
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
                var entity = _CategoryRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault();
                entity = val;
                _CategoryRepository.Update(entity);
                _CategoryRepository.Commit();
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
        /// Category kaydını siler.
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
                var entity = _CategoryRepository.GetSingle(x => x.Id == id);
                if (entity == null)
                {
                    res.Result = false;
                    res.ResultMessage = "Kayıt Bulunamadı";
                    return NotFound(res);
                }
                _CategoryRepository.Delete(entity);
                _CategoryRepository.Commit();
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
        /// Tüm DeActivated Category verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetAllDeactivatedCategorys")]
        public IActionResult GetAllDeactivatedCategorys()
        {
            return Ok(_CategoryRepository.FindBy(x => x.DataStatus == Entity.Shared.DataStatus.DeActivated).ToList());
        }
    }
}
