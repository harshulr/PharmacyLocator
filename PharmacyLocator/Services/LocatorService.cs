using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Web;

namespace PharmacyLocator.Services
{
	public class LocatorService
	{
		public Pharmacy GetNearestPharmacy(double lat, double lon)
		{
			//Import CSV - This can be stored in Database as well
			List<Pharmacy> pharmacies = new List<Pharmacy>();
			string path = @"C:\Users\harshul\source\repos\PharmacyLocator\PharmacyLocator\pharmacies.csv";

			using (var reader = new StreamReader(path))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					Pharmacy pharmacy = Pharmacy.FillPharmacy(line);
					if (pharmacy != null)
					{
						pharmacies.Add(pharmacy);
					}
				}
			}
			//Import CSV COMPLETE

			var MaxLat = lat + 0.5; //Each degree of Latitude is approximately 69 miles
			var MinLat = lat - 0.5; //Each degree of Longitude = cosine (latitude in decimal degrees) * length of degree (miles) at equator
			var MaxLon = lon + 0.5; //Latitude can be obtained from above, Length of degree at equator - 69.172 miles
			var MinLon = lon - 0.5; //Degree of Longitude at 39 Degrees Latitude is around 53.43 miles 										
									//Since Min and Max Latitude in give dataset is 38.88323 & 39.24578, I added only 0.5 to form radius
									//Same for longitude as well, Min and Max longitude is -95.77866 & -94.25503 

			//Checking for pharmacies within radius of given lat + 0.5, lon + 0.5, which is basically (lat+0.5)^2 + (lon+0.5)^2)
			List<Pharmacy> pharmaciesNearUser = pharmacies.FindAll(u => u.Latitude < MaxLat && u.Latitude > MinLat && u.Longitude < MaxLon && u.Longitude > MinLon);


			List<Pharmacy> pharmaciesToCheck = pharmacies;

			//if there are pharmacies in our radius then check the distance between user and only those pharmacies, else check distance with all the pharmacies
			if (pharmaciesNearUser.Count > 0)
			{
				pharmaciesToCheck = pharmaciesNearUser;
			}

			//Initiate variables for checking
			double distance;
			int count = 0;
			Pharmacy nearestPharmacy = new Pharmacy();
			GeoCoordinate userLocation = new GeoCoordinate(lat, lon);

			//Start the checking process
			foreach (var pharmacy in pharmaciesToCheck)
			{
				//GetDistanceTo is a C# Method that is available to check the distance between two co-ordinates
				//GetDistanceTo provides the value in meters, so we are multiplying with the below number to convert into miles 
				distance = (userLocation.GetDistanceTo(pharmacy.Location)) * 0.000621371192;
				if (count == 0)
				{
					nearestPharmacy.Name = pharmacy.Name;
					nearestPharmacy.Address = pharmacy.Address;
					nearestPharmacy.City = pharmacy.City;
					nearestPharmacy.State = pharmacy.State;
					nearestPharmacy.Zipcode = pharmacy.Zipcode;
					nearestPharmacy.Distance = distance;

				}
				//if distance is less then Nearest Pharmacy, then Update the nearestPharmacy
				if (nearestPharmacy.Distance > distance)
				{
					nearestPharmacy.Name = pharmacy.Name;
					nearestPharmacy.Address = pharmacy.Address;
					nearestPharmacy.City = pharmacy.City;
					nearestPharmacy.State = pharmacy.State;
					nearestPharmacy.Zipcode = pharmacy.Zipcode;
					nearestPharmacy.Distance = distance;
				}

				//The counter can be incremented or set to any number other then 0, Since we only require it for 1st
				//I have set it to increment because, if in future we try to expand the function then It might be useful (Left it more open-ended)
				count++;
			}

			//Return the nearestPharmacyDetails
			return nearestPharmacy;
		}
	}

	public class Pharmacy
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public int Zipcode { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public GeoCoordinate Location { get; set; }
		public double Distance { get; set; }

		public static Pharmacy FillPharmacy(string csvLine)
		{
			string[] values = csvLine.Split(',');

			Pharmacy pharmacy = new Pharmacy();

			if (values[4] == "\"zip\"")
				return null;

			pharmacy.Name = values[0];
			pharmacy.Address = values[1];
			pharmacy.City = values[2];
			pharmacy.State = values[3];
			pharmacy.Zipcode = Convert.ToInt32(values[4]);
			pharmacy.Latitude = Convert.ToDouble(values[5]);
			pharmacy.Longitude = Convert.ToDouble(values[6]);
			pharmacy.Location = new GeoCoordinate(pharmacy.Latitude, pharmacy.Longitude);

			return pharmacy;
		}


	}
}