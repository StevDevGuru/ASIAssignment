using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASIAssignment.Infrastructure.Models.ViewModel
{
    public class EmailVM
    {
        public long Id { get; set; }
        public long ContactId { get; set; }
        public bool IsPrimary { get; set; }
        public string Address { get; set; }
    }
}
