using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repositories
{
	public class EstateRepository : Repository<Estate>, IEstateRepository
	{
		private readonly ApplicationDbContext _db;
		public EstateRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
	}
}
