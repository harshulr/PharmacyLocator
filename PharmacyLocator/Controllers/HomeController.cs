using PharmacyLocator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyLocator.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Locate(double latitude, double longitude)
		{
			LocatorService service = new LocatorService();
			Pharmacy nearestPharmacy = service.GetNearestPharmacy(latitude, longitude);

			//Declared a anonymous type object to get the fields that are needed to be sent back
			var JsonPharmacyObject = new
			{
				Name = nearestPharmacy.Name,
				Address = nearestPharmacy.Address,
				City = nearestPharmacy.City,
				State = nearestPharmacy.State,
				ZipCode = nearestPharmacy.Zipcode,
				Distance = nearestPharmacy.Distance.ToString() + " Miles"
			};

			return Json(JsonPharmacyObject, JsonRequestBehavior.AllowGet);
		}
	}
}