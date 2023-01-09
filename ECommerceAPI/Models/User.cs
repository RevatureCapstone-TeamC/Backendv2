using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public bool IfAdmin { get; set; }

        /* public User() { } */

        /* public User(string firstName, string lastName, string email, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
        } */
    }
}