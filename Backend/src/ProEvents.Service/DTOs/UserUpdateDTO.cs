namespace ProEvents.Service.DTOs
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        // Armazenar o token, caso atualizar a senha não deslogar o Usuário
        public string Token { get; set; }
    }
}