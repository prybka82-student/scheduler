using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.App.Entities
{
    public enum TimeInterval
    {
        Never, 
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year
    }
}
