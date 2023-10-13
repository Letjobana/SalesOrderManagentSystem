using Imperial.SalesOrder.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imperial.SalesOrder.Identity
{
    public class UserStore : IUserClaimStore<IdentityUser, int>,
        IUserRoleStore<IdentityUser, int>,
        IUserPasswordStore<IdentityUser, int>,
        IUserSecurityStampStore<IdentityUser, int>,
        IUserTwoFactorStore<IdentityUser, int>,
        IUserPhoneNumberStore<IdentityUser, int>,
        IUserEmailStore<IdentityUser, int>,
        IUserLockoutStore<IdentityUser, int>,
        IQueryableUserStore<IdentityUser, int>,
        IUserLoginStore<IdentityUser, int>,
        IUserStore<IdentityUser, int>
    {
        private Repository repo;

        public IQueryable<IdentityUser> Users => throw new NotImplementedException();

        //Create a new UserStore with the Repository using the Default Connection String
        public UserStore()
        {
            repo = new Repository();
        }

        //Create a new UserStore with the specified Repository
        public UserStore(Repository repository)
        {
            repo = repository;
        }

        #region Private Methods
        private Task CreateUserAsync(IdentityUser user)
        {
            if (user == null)
                return Task.FromResult<object>(null);

            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("UserName", user.UserName),
                new SqlParameter("Email", user.Email),
                new SqlParameter("PasswordHash", user.PasswordHash),
                new SqlParameter("LockoutEnabled", user.LockoutEnabled),
                new SqlParameter("EmailConfirmed", user.EmailConfirmed),
                new SqlParameter("SecurityStamp", user.SecurityStamp),
                new SqlParameter("PhoneNumber", user.PhoneNumber),
                new SqlParameter("PhoneNumberConfirmed", user.PhoneNumberConfirmed),
                new SqlParameter("TwoFactorEnabled", user.TwoFactorEnabled),
                new SqlParameter("LockoutEndDateUtc", user.LockoutEndDateUtc),
                new SqlParameter("AccessFailedCount", user.AccessFailedCount),
                new SqlParameter("FirstName", user.FirstName),
                new SqlParameter("Surname", user.Surname)
            };

            user = repo.GetRecord<IdentityUser>("usp_Identity_CreateUser", lsParms);

            return Task.FromResult(user);
        }

        private Task UpdateUserAsync(IdentityUser user)
        {
            if (user == null)
                return Task.FromResult<object>(null);

            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("Id", user.Id),
                new SqlParameter("UserName", user.UserName),
                new SqlParameter("Email", user.Email),
                new SqlParameter("PasswordHash", user.PasswordHash),
                new SqlParameter("LockoutEnabled", user.LockoutEnabled),
                new SqlParameter("EmailConfirmed", user.EmailConfirmed),
                new SqlParameter("SecurityStamp", user.SecurityStamp),
                new SqlParameter("PhoneNumber", user.PhoneNumber),
                new SqlParameter("PhoneNumberConfirmed", user.PhoneNumberConfirmed),
                new SqlParameter("TwoFactorEnabled", user.TwoFactorEnabled),
                new SqlParameter("LockoutEndDateUtc", user.LockoutEndDateUtc),
                new SqlParameter("AccessFailedCount", user.AccessFailedCount),
                new SqlParameter("FirstName", user.FirstName),
                new SqlParameter("Surname", user.Surname)
            };

            user = repo.GetRecord<IdentityUser>("usp_Identity_UpdateUser", lsParms);

            return Task.FromResult(user);
        }
        #endregion

        #region IUserStore Implementation
        public Task CreateAsync(IdentityUser user)
        {
            return CreateUserAsync(user);
        }

        public Task<IdentityUser> FindByIdAsync(int userId)
        {
            if (userId == default)
                return Task.FromResult<IdentityUser>(null);

            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("Id", userId)
            };

            IdentityUser user = repo.GetRecord<IdentityUser>("usp_Identity_GetUserById", lsParms);

            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return Task.FromResult<IdentityUser>(null);

            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("UserName", userName)
            };

            IdentityUser user = repo.GetRecord<IdentityUser>("usp_Identity_GetUserByUserName", lsParms);

            return Task.FromResult(user);
        }

        public Task UpdateAsync(IdentityUser user)
        {
            return UpdateUserAsync(user);
        }

        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (repo != null)
            {
                repo = null;
            }
        }
        #endregion

        #region IUserClaimStore Implementation
        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("ID_User", user.Id)
            };

            List<UserClaim> userClaims = repo.GetRecords<UserClaim>("usp_Identity_GetUserClaims", lsParms);

            ClaimsIdentity identity = new ClaimsIdentity();
            foreach (UserClaim uc in userClaims)
                identity.AddClaim(new Claim(uc.ClaimType, uc.ClaimValue));

            return Task.FromResult<IList<Claim>>(identity.Claims.ToList());
        }

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserRoleStore Implementation
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("ID_User", user.Id)
            };

            List<string> roles = repo.GetRecords_String("usp_Identity_GetUserRoles", lsParms);
            {
                if (roles != null)
                {
                    return Task.FromResult<IList<string>>(roles);
                }
            }

            return Task.FromResult<IList<string>>(null);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserPasswordStore Implementation
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(user);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            IdentityUser u = FindByIdAsync(user.Id).Result;
            return Task.FromResult(u.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            IdentityUser u = FindByIdAsync(user.Id).Result;

            bool hasPassword = (u != null && !string.IsNullOrEmpty(u.PasswordHash));

            return Task.FromResult(hasPassword);
        }
        #endregion

        #region IUserSecurityStampStore Implementation
        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserTwoFactorStore Implementation
        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            UpdateUserAsync(user);

            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        #endregion

        #region IUserPhoneNumberStore Implementation
        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            UpdateUserAsync(user);

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            UpdateUserAsync(user);

            return Task.FromResult(0);
        }
        #endregion

        #region IUserEmailStore Implementation
        public Task SetEmailAsync(IdentityUser user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            UpdateUserAsync(user);

            return Task.FromResult(0);
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Task.FromResult<IdentityUser>(null);

            List<SqlParameter> lsParms = new List<SqlParameter>
            {
                new SqlParameter("Email", email)
            };

            IdentityUser user = repo.GetRecord<IdentityUser>("usp_Identity_GetUserByEmail", lsParms);

            return Task.FromResult(user);
        }
        #endregion

        #region IUserLockoutStore Implementation
        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            UpdateUserAsync(user);

            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            user.AccessFailedCount++;
            UpdateUserAsync(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            user.AccessFailedCount = 0;
            UpdateUserAsync(user);

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserLoginStore Implementation
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
