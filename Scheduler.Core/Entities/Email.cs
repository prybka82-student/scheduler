using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class Email
    {
        public Addressee From { get; set; }
        public Addressee To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
