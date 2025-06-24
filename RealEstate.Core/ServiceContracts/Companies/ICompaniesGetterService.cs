using RealEstate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts.Companies
{
	public interface ICompaniesGetterService
	{
		Task<IEnumerable<CompanyResponseDTO>> GetAllCompanies();
		Task<CompanyResponseDTO> GetCompanyById(Guid CompanyId);
		Task<IEnumerable<CompanyResponseDTO>> GetFilterdCompany(string searchBy, string searchString);
	}
}
