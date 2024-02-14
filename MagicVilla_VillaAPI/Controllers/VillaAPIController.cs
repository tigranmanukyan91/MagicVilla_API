using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController:ControllerBase
	{

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			return Ok(VillaStore.villaList);
		}


		[HttpGet("id",Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		
		public ActionResult<VillaDTO> GetVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
				return NotFound();
            }
            return Ok(villa);
		}


		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<VillaDTO> createVilla([FromBody]VillaDTO villaDto)
		{
			if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower()==villaDto.Name.ToLower()) != null)
			{
				ModelState.AddModelError("CustomError", "Villa already exists");
				return BadRequest(ModelState);
			}
            {
                
            }
            if (villaDto == null)
            {
				return BadRequest();
            }
            if (villaDto.Id > 0)
            {
				return StatusCode(StatusCodes.Status500InternalServerError);
            }
			villaDto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			VillaStore.villaList.Add(villaDto);
			//returns the route to the created object
			return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }
	}
}
