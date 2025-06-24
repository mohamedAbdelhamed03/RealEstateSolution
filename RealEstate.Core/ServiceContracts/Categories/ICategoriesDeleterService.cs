using RealEstate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts.Categories
{
	public interface ICategoriesDeleterService
	{
		Task<CategoryResponseDTO> DeleteCategory(Guid? categoryId);
	}
}
