using Microsoft.AspNetCore.Identity;
using ProEvents.Domain.Enum;

namespace ProEvents.Domain.Identity
{

    // IdentityUser<int>: <int> o ID dessa tabela estará associado com o tipo passado
    // no caso de exemplo vai ser um tipo inteiro, o certo é um GUID

    // ao herdar de IdentityUser herda algumas propriedades padrão do Identity

    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Title Title { get; set; }
        public string Description { get; set; }
        public UserType UserType { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}