﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Domain.Entities
{
	[Owned]
	public class RefreshToken
	{
		[Key]
		public Guid Id { get; set; }
		public string? Token { get; set; }
		public DateTime ExpiresOn { get; set; }
		public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
		public DateTime CreatedOn { get; set; }
		public DateTime? RevokedOn { get; set; }
		public bool IsActive => RevokedOn == null && !IsExpired;

	}
}
