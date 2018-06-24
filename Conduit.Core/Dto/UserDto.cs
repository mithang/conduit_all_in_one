using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Conduit.Common.Dto
{
    public class UserDto
    {
        
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
       
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

        public string Bio { get; set; }
        /// <summary>
        /// Username đã được mã hóa
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// UserDto dan gelen password ile username'in kriptolanmış hali.
        /// </summary>
        public string Hash { get; set; }

    }
}
