using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace back_end_s7.Models
{
    public class ManagerRuoli : RoleProvider
    {
        FornoDbContext db = new FornoDbContext();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var roles = new string[] { };

            var userRole = db.Utenti.FirstOrDefault(u => u.Username == username)?.Ruolo;
            if (userRole != null)
            {
                roles = new string[] { userRole };
            }

            var adminRole = db.Amministratori.FirstOrDefault(a => a.Username == username)?.Ruolo;
            if (adminRole != null)
            {
                roles = new string[] { adminRole };
            }

            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userRole = db.Utenti.FirstOrDefault(u => u.Username == username)?.Ruolo;
            if (userRole != null && userRole.Equals(roleName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var adminRole = db.Amministratori.FirstOrDefault(a => a.Username == username)?.Ruolo;
            if (adminRole != null && adminRole.Equals(roleName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}