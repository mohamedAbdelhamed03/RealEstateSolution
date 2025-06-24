using RealEstate.Core.DTOs;
using RealEstate.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts.Companies
{
	public interface ICompaniesSorterService
	{
		Task<IEnumerable<CompanyResponseDTO>> SortCompaniesAsync(string sortBy, SortedOrderOptions sortedOrder);

	}
}
