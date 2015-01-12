using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D2.Models
{
    public class Hero
    {
        public int ID { set;  get; }
        public string name { set;  get; }
        public int currentHealth { set; get; }
        public int maxHealth { set; get; }
        public int currentLV { set; get; }

        public virtual Position currentPosition { set; get; }
    }
}