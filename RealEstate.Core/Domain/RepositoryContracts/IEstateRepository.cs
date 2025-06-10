using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Domain.RepositoryContracts
{
	public interface IEstateRepository : IRepository<Estate>
	{
	}
}
