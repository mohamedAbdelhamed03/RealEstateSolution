﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Helpers
{
	class ValidationHelper
	{
		internal static void ModelValidation(object obj)
		{

			ValidationContext validationContext = new ValidationContext(obj);
			List<ValidationResult> validationResult = new List<ValidationResult>();

			bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
			if (!isValid)
			{
				throw new ArgumentException(validationResult.FirstOrDefault()?.ErrorMessage);

			}

		}
	}
}
