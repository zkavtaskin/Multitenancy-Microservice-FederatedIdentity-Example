namespace Web.Models.Group
{
    public class UserSuggestionModel
    {
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public bool IsInvited { get; set; }
    }
}