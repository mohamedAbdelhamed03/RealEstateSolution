﻿using Microsoft.AspNetCore.Identity;
using RealEstate.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Domain.IdentityEntities
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string? PersonName { get; set; }
		public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
	}
}
