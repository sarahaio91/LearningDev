using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D2.Models
{
    public class Map
    {
        public int ID { set;  get; }
        public DateTime time{get; set;}
        public IDictionary<Hero, Position> heroPosition { set; get; }
    }
}