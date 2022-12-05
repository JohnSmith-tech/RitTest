using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitProject
{
    class Machinery
    {
        private int id { get; set; }
        private string name { get; set; }
        private double lat { get; set; }
        private double lng { get; set; }

        public int Id
        {
            get { return id; }
           
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Lat
        {
            get { return lat; }
            set {lat = value; }
        }

        public double Lng
        {
            get { return lng; }
            set { lng = value; }
        }

        public Machinery(int id, string name, double lat, double lng)
        {
            this.id = id;
            this.name = name;
            this.lat = lat;
            this.lng = lng;
        }
    }
}
