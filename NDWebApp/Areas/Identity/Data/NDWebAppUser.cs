using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace NDWebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the NDWebAppUser class
public class NDWebAppUser : IdentityUser
{
    [DisplayName("Ansattnummer")]
    public int empNr { get; set; }

    [DisplayName("Fornavn")]
    public string empFname { get; set; } = "";

    [DisplayName("Etternavn")]
    public string empLname { get; set; } = "";

    [DisplayName("Team ID")]
    public int? teamId { get; set; }
}

