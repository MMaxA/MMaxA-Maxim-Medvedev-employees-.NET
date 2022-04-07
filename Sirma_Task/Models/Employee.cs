using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sirma_Task.Models
{
    public class Employee
    {
        public int EmpId { get; set; }

        public int ProjectID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}