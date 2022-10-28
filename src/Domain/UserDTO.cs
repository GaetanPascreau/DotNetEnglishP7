using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Domain
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName is mandatory")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }

        [Required(ErrorMessage = "FullName is mandatory")]
        public string FullName { get; set; }
    }
}
