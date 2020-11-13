﻿using Scheduler.App.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scheduler.App.Entities
{
    public class ActionResult<T>
    {
        public ResultType Result { get; set; }
        public IEnumerable<T> Data { get; set; }

        public ActionResult(ResultType result, IEnumerable<T> data = null)
        {
            Data = data ?? Enumerable.Empty<T>();
            Result = result;
        }
    }
}