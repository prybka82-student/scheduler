using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.App.Entities
{
    public class Person : DbEntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
    }
}
