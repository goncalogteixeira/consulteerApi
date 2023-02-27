namespace Consulteer.API.Authorization.Model
{
    public class PermissionModel
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsModel> RoleClaims { get; set; }
    }
    public class RoleClaimsModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
