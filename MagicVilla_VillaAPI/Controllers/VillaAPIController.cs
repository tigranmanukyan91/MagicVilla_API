using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ILogger<VillaAPIController> _logger;
		private readonly ApplicationDbContext _db;
        public VillaAPIController(ILogger<VillaAPIController> logger,ApplicationDbContext db)
        {
			_logger = logger;
			_db = db;
        }

        [HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			_logger.LogInformation("Getting all villas");
			return Ok(_db.Villas.ToList());
		}


		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public ActionResult<VillaDTO> GetVilla(int id)
		{
			if (id == 0)
			{
				_logger.LogError("Get villa Error with id " + id);
				return BadRequest();
			}
			var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
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
		public ActionResult<VillaDTO> createVilla([FromBody] VillaDTO villaDto)
		{
			if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
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
			villaDto.Id = _db.Villas.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			Villa model = new()
			{
				Id = villaDto.Id,
				Name = villaDto.Name,
				Details = villaDto.Details,
				Sqft = villaDto.Sqft,
				Occupancy = villaDto.Occupancy,
				Amenity = villaDto.Amenity,
				ImageUrl = villaDto.ImageUrl,
				Rate = villaDto.Rate,
			};
			_db.Villas.Add(model);
			_db.SaveChanges();
			//returns the route to the created object
			return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
		}


		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
			_db.Villas.Remove(villa);
			_db.SaveChanges();

			return NoContent();
		}

		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
		{
			if (villaDTO == null || id != villaDTO.Id)
			{
				return BadRequest();
			}
			Villa model = new()
			{
				Id = villaDTO.Id,
				Name = villaDTO.Name,
				Details = villaDTO.Details,
				Sqft = villaDTO.Sqft,
				Occupancy = villaDTO.Occupancy,
				Amenity = villaDTO.Amenity,
				ImageUrl = villaDTO.ImageUrl,
				Rate = villaDTO.Rate,
			};
			_db.Villas.Update(model);
			_db.SaveChanges();

			return NoContent();
		}

		[HttpPatch("{id:int}",Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
		{
            if (patchDTO == null || id == 0)
            {
				return BadRequest();
            }
			var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
				return NotFound();
            }
			VillaDTO villaDTO = new()
			{
				Name = villa.Name,
				Details = villa.Details,
				Sqft = villa.Sqft,
				Occupancy = villa.Occupancy,
				Amenity = villa.Amenity,
				Rate = villa.Rate,
				Id = villa.Id,
				ImageUrl = villa.ImageUrl,
			};

			patchDTO.ApplyTo(villaDTO, ModelState);
            if (!ModelState.IsValid)
            {
				return BadRequest();
            }

			Villa model = new()
			{
				Id = villaDTO.Id,
				Name = villaDTO.Name,
				Details = villaDTO.Details,
				Sqft = villaDTO.Sqft,
				Occupancy = villaDTO.Occupancy,
				Amenity = villaDTO.Amenity,
				ImageUrl = villaDTO.ImageUrl,
				Rate = villaDTO.Rate,
			};

			_db.Update(model);
			_db.SaveChanges();
			return NoContent();
        }
	}
}
