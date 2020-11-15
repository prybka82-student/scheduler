using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.App.Entities
{
    public class Customer : Addressee
    {
        public decimal Discount { get; set; }
    }
}
