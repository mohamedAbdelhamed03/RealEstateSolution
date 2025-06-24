using RealEstate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts.Estates
{
	public interface IEstatesGetterService
	{
		Task<IEnumerable<EstateResponseDTO>> GetAllEstates();
		Task<EstateResponseDTO> GetEstateById(Guid EstateId);
		Task<IEnumerable<EstateResponseDTO>> GetFilterdEstate(string searchBy, string searchString);
	}
}
