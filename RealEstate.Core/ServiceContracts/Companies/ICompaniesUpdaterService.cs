using RealEstate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts.Companies
{
	public interface ICompaniesUpdaterService
	{
		Task<CompanyResponseDTO> UpdateCompany(CompanyUpdateDTO companyUpdateDTO);

	}
}
