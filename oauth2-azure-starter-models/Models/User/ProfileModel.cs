namespace oauth2_azure_starter_models.Models.User
{
    public class ProfileModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
