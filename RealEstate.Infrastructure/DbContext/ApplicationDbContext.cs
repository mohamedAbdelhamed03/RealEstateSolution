using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.DbContext
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public DbSet<Estate> estates { get; set; }
		public DbSet<Category> categories { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Estate>().HasData(
				new Estate
				{
					Id = Guid.Parse("65958D38-6DC4-454E-9DB4-A010506F9A8F"),
					Name = "Luxury Apartment",
					EstateNumber = "EST-001",
					Details = "A luxury apartment with modern amenities.",
					Rate = 250000.00,
					Sqft = 1200,
					Occupancy = 4,
					ImageUrl = "https://example.com/images/estate1.jpg",
					Amenity = "Pool, Gym, Parking",
					CategoryId = Guid.Parse("5428C770-B3B7-4509-A208-161957823537")
				},
				new Estate
				{
					Id = Guid.Parse("6F3811B2-FC13-4982-860A-0416AF514635"),
					Name = "Spacious Villa",
					EstateNumber = "EST-002",
					Details = "A spacious villa with a beautiful garden.",
					Rate = 500000.00,
					Sqft = 2500,
					Occupancy = 6,
					ImageUrl = "https://example.com/images/estate2.jpg",
					Amenity = "Garden, Garage, Security",
					CategoryId = Guid.Parse("46B6CC6A-DE9E-48D3-B6D1-9201D71169D6")
				},
				new Estate
				{
					Id = Guid.Parse("64009858-0CFE-4CDD-B0B6-A17BD02BA400"),
					Name = "Modern Duplex",
					EstateNumber = "EST-003",
					Details = "A modern duplex with two floors.",
					Rate = 350000.00,
					Sqft = 1800,
					Occupancy = 5,
					ImageUrl = "https://example.com/images/estate3.jpg",
					Amenity = "Balcony, Terrace, Parking",
					CategoryId = Guid.Parse("357E9CC3-62E0-44E6-AD74-782A73497FE9")
				},
				new Estate
				{
					Id = Guid.Parse("F929DD87-8C20-4CB9-B4A4-215B418AD2A1"),
					Name = "Residential House",
					EstateNumber = "EST-004",
					Details = "A cozy residential house in a quiet neighborhood.",
					Rate = 300000.00,
					Sqft = 1500,
					Occupancy = 4,
					ImageUrl = "https://example.com/images/estate4.jpg",
					Amenity = "Backyard, Garage, Security",
					CategoryId = Guid.Parse("3BB3DC78-4F92-4785-A942-E45DB3F38424")
				},
				new Estate
				{
					Id = Guid.Parse("148E17F1-0E65-4ED2-8320-8B2183F1018A"),
					Name = "Commercial Space",
					EstateNumber = "EST-005",
					Details = "A commercial space suitable for business operations.",
					Rate = 600000.00,
					Sqft = 3000,
					Occupancy = 10,
					ImageUrl = "https://example.com/images/estate5.jpg",
					Amenity = "Parking, Security, Air Conditioning",
					CategoryId = Guid.Parse("DF99C125-E092-4AA7-BBC1-A8AD04D1AADD")
				},
				new Estate
				{
					Id = Guid.Parse("CE338791-BC56-4EFA-923B-DF4B0D88E589"),
					Name = "Luxury Villa",
					EstateNumber = "EST-006",
					Details = "A luxury villa with a private pool.",
					Rate = 750000.00,
					Sqft = 3500,
					Occupancy = 8,
					ImageUrl = "https://example.com/images/estate6.jpg",
					Amenity = "Pool, Garden, Security",
					CategoryId = Guid.Parse("46B6CC6A-DE9E-48D3-B6D1-9201D71169D6")
				},
				new Estate
				{
					Id = Guid.Parse("A7FA520A-E357-4D25-AC34-B4CDF5EDD48E"),
					Name = "Contemporary Apartment",
					EstateNumber = "EST-007",
					Details = "A contemporary apartment with city views.",
					Rate = 400000.00,
					Sqft = 1600,
					Occupancy = 3,
					ImageUrl = "https://example.com/images/estate7.jpg",
					Amenity = "Gym, Parking, Balcony",
					CategoryId = Guid.Parse("5428C770-B3B7-4509-A208-161957823537")
				}
				);
			modelBuilder.Entity<Category>().HasData(
				new Category
				{
					Id = Guid.Parse("5428C770-B3B7-4509-A208-161957823537"),
					Name = "Apartment"
				},
				new Category
				{
					Id = Guid.Parse("46B6CC6A-DE9E-48D3-B6D1-9201D71169D6"),
					Name = "Villa"
				},
				new Category
				{
					Id = Guid.Parse("357E9CC3-62E0-44E6-AD74-782A73497FE9"),
					Name = "Duplex"
				},
				new Category
				{
					Id = Guid.Parse("3BB3DC78-4F92-4785-A942-E45DB3F38424"),
					Name = "Residential"
				},
				new Category
				{
					Id = Guid.Parse("DF99C125-E092-4AA7-BBC1-A8AD04D1AADD"),
					Name = "Commercial"
				}

			);
		}

	}
}
