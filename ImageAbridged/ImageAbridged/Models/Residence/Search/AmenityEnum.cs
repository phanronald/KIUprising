using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAbridged.Models.Residence.Search
{
	public enum AmenityEnum
	{
		InUnitWasherDryer = 1,
		Dishwasher = 4,
		AirConditioning = 16,
		Furnished = 128,
		FitnessCenter = 256,
		Pool = 512,
		Parking = 65536,
		Elevator = 524288,
		WheelchairAccess = 131072,
		WasherDryerHookup = 1048576,
		LaundryFacilities = 2097152,
		UtilitiesIncluded = 4194304,
		Lofts = 8388608,
	}
}
