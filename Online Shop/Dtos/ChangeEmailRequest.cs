namespace Online_Shop.Dtos
{
    public class ChangeEmailRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
    }
}
