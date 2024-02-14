using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController:ControllerBase
	{

		[HttpGet]
		public IEnumerable<VillaDTO> GetVillas()
		{
			return new List<VillaDTO>
			{
				new() {Id=1, Name = "Pool View" },
				new() {Id=2, Name = "Beach View"}
			};
		}
	}
}
