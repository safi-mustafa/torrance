using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;

namespace IdentityProvider.Data.IdentityStore.ClaimHelpers
{
    public static class ClaimsHelper
    {
        public static async Task<List<Claim>> GetClaims(ToranceUser user, ToranceContext db)
        {
            var roleId = await db.UserRoles.Where(x => x.UserId == user.Id).Select(x=>x.RoleId).FirstOrDefaultAsync();
            var roleName = await db.Roles.Where(x => x.Id == roleId).Select(x => x.Name).FirstOrDefaultAsync();
            var empId = await db.Employees.Where(x => x.UserId == user.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var email = !string.IsNullOrEmpty(user.Email) ? user.Email : "";
            var fullName = string.IsNullOrEmpty(user.NormalizedUserName) ? "" : user.NormalizedUserName;
            var claims = new List<Claim>();
            claims.Add(new Claim("Role", roleName));
            claims.Add(new Claim("EmployeeId", empId.ToString()));
            claims.Add(new Claim("Email", email.ToString()));
            claims.Add(new Claim("Id", empId.ToString()));
            claims.Add(new Claim("FullName", fullName.ToString()));
            return claims;
        }
    }
}
