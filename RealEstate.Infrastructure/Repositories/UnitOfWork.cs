using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repositories
{

	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;
		public IEstateRepository EstateRepository { get; private set; }
		public ICategoryRepository CategoryRepository { get; private set; }
		public ICompanyRepository CompanyRepository { get; private set; }


		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			EstateRepository = new EstateRepository(_db);
			CategoryRepository = new CategoryRepository(_db);
			CompanyRepository = new CompanyRepository(_db);
		}
	}
}
