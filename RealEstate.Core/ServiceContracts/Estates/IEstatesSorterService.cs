using RealEstate.Core.DTOs;
using RealEstate.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts.Estates
{
	public interface IEstatesSorterService
	{
		Task<IEnumerable<EstateResponseDTO>> SortEstatesAsync(string sortBy, SortedOrderOptions sortedOrder);
	}
}
