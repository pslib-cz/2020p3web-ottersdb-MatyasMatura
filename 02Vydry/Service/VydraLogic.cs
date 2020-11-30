using _02Vydry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _02Vydry.Service
{
    public class VydraLogic
    {
        public void PlaceLocationSplit(Vydra Vydra, string newPlace)
        {
            string[] data;
            data = newPlace.Split(";");
            Vydra.LocationId = int.Parse(data[0]);
            Vydra.PlaceName = data[1];
        }
    }
}
