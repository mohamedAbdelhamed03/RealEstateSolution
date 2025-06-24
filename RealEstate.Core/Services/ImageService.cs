using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services
{
	public class ImageService
	{
		public void DeleteImageFromFileSystem(string? imageLocalPath)
		{
			if (!string.IsNullOrEmpty(imageLocalPath))
			{
				var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), imageLocalPath);
				FileInfo file = new FileInfo(oldFilePathDirectory);
				if (file.Exists)
				{
					file.Delete();
				}
			}
		}
	}
}
