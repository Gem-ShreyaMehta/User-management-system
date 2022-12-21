using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginPageViaRepositoryPattern.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public string fullname { get; set; }
        public string email{ get; set; }

        public string password { get; set; }
        public string confirmpass { get; set; }
    }
}
