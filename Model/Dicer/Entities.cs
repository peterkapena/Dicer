using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dicer.Model.Dicer
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 PersonID { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
#nullable enable

        public string? IDNumber { get; set; }
        public string? PassPortNumber { get; set; }
        public string? OtherIdentityNumber { get; set; }
#nullable disable
    }

    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 DeviceID { get; set; }
        public string mcAddress { get; set; }
        public string deviceTypeName { get; set; }
        public string deviceOwnerName { get; set; }

    }

    public class Gamed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 GamedID { get; set; }
        public string PersonID { get; set; }
        public string DeviceID { get; set; }
        public DateTime Date { get; set; }
    }
}
