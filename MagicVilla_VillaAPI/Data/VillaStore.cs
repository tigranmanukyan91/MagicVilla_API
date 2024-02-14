using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
	public static class VillaStore
	{
		public static List<VillaDTO> villaList = new List<VillaDTO>
			{
				new () {Id=1, Name = "Pool View", Occupancy = 4, Sqft = 100 },
				new () {Id=2, Name = "Beach View", Occupancy = 3, Sqft = 50}
			};
	}
}
