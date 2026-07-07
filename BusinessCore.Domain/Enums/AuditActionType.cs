namespace BusinessCore.Domain.Enums
{
    public enum AuditActionType
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        Login = 3,
        Logout = 4,
        PasswordChange = 5,
        StatusChange = 6,
        Export = 7,
        Import = 8
    }
}