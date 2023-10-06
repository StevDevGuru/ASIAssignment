using ASIAssignment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASIAssignment.Infrastructure.Models.ViewModel
{
    public class ContactVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public List<EmailVM> Email { get; set; }
    }
}
