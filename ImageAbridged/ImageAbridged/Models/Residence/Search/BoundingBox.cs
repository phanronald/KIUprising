using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAbridged.Models.Residence.Search
{
	public class BoundingBox
	{
		public LowerRight LowerRight { get; set; }
		public UpperLeft UpperLeft { get; set; }
	}
}
