using RealEstate.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Domain.RepositoryContracts
{
	public interface IUnitOfWork
	{
		IEstateRepository EstateRepository { get; }
		ICategoryRepository CategoryRepository { get; }

	}
}
