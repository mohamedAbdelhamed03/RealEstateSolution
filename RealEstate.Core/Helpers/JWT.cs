﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Helpers
{
	public class JWT
	{
		public string? Key { get; set; }
		public string? Issuer { get; set; }
		public string? Audience { get; set; }
		public double DurationInDays { get; set; }
		public double DurationInMinutes { get; set; }
		public double DurationInHours { get; set; }
	}
}
