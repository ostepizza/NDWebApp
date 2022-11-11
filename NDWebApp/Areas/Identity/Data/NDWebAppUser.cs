using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NDWebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the NDWebAppUser class
public class NDWebAppUser : IdentityUser
{
    public int empNr { get; set; }
    public string empFname { get; set; }
    public string empLname { get; set; }
}

