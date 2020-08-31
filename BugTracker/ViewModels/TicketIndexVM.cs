using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class TicketIndexVM
    {
        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Ticket> UserTickets { get; set; }

        public TicketIndexVM()
        {
            Tickets = new HashSet<Ticket>();
            UserTickets = new HashSet<Ticket>();
        }
    }
}