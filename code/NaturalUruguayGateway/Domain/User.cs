using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    [Serializable]
    public class User : IPrincipal
    {
        public int Id { get; set; }
        [JsonIgnore]
        public UserRole Role { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public IIdentity Identity => null;

        public bool IsInRole(string role)
        {
            return Role.Equals(role);
        }
    }
}