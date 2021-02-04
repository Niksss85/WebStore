using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Identity
{
    public class Role : IdentityRole
    {
        public const string Administrtator = "Administrators";

        public const string Users = "Users";
        public string Description { get; set; }
    }
}
