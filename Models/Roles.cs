namespace TaskManager.Models
{
    public static class RolesConsts{
        public const string ADMINISTRATOR = "administrator";

        public const string EMPLOYEE = "employee";
    }
    public enum Roles
    {
        none,
        employee,
        administrator
    }
}