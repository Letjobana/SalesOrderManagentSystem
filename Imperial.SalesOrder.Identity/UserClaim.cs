using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Imperial.SalesOrder.Identity
{
    public class UserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public static class ClaimExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst("UserId");

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }

        public static string GetUserFullName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claimFN = claimsIdentity?.FindFirst("FirstName");
            Claim claimLN = claimsIdentity?.FindFirst("Surname");

            string firstName = claimFN == null ? "" : claimFN.Value + " ";
            string surname = claimLN == null ? "" : claimLN.Value;

            return firstName + surname;
        }
    }
}
