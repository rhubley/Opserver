namespace Opserver.F5Status.Data
{
    public static class F5StatusRoles
    {
        public const string Admin = nameof(F5StatusModule) + ":" + nameof(Admin);
        public const string Viewer = nameof(F5StatusModule) + ":" + nameof(Viewer);
    }
}
