using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.DAL.Models
{
    public class Customer
    {
#nullable disable
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
#nullable enable
        //? = nullable
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? StateOrProvinceAbbr { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
#nullable disable
        //Navigation property
        //Customer - Order is  1 to many relationship
        public ICollection<Order> Orders { get; set; }
    }
}
