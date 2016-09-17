using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class CreateEventViewModel : EventModel
    {
        [Display(Name="Number of tickets")]
        public Int32 NumberOfTickets { get; set; }
    }
}
