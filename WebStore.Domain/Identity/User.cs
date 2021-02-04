using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Identity
{
    public class User:IdentityUser
    {
        public const string Administrtator = "Admin";

        public const string DefaultAdminPassword = "AdPass";
        public string Description { get; set; }
    }
}
