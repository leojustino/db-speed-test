using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.Entities
{
    public class Customer : Base
    {
        public Customer()
        {
            Orders = new List<Order>();
        }

        public string Name { get; set; }

        public virtual IList<Order> Orders { get; set; }
    }
}
