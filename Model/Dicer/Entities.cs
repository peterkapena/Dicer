using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dicer.Model.Dicer
{
    public class Person
    {
        [Key]
        string PersonID { get; set; }
        string PhoneNumber { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
#nullable enable

        string? IDNumber { get; set; }
        string? PassPortNumber { get; set; }
        string? OtherIdentityNumber { get; set; }
#nullable disable
    }

    public class Devices
    {
        [Key]
        string DeviceID { get; set; }
        string mcAddress { get; set; }
        string deviceType { get; set; }
        string deviceName { get; set; }

    }

    public class Gamed
    {
        [Key]
        string gamedID { get; set; }
        Person PersonID { get; set; }
        Devices DeviceID { get; set; }
    }
}
