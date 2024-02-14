﻿using MagicVilla_VillaAPI.Data;
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
			return VillaStore.villaList;
		}


		[HttpGet("id")]
		public VillaDTO GetVilla(int id)
		{
			return VillaStore.villaList.FirstOrDefault(u => u.Id == id);
		}
	}
}