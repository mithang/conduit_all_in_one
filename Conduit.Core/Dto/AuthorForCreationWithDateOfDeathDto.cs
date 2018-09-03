using System;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Common.Dto
{
    public class AuthorForCreationWithDateOfDeathDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset? DateOfDeath { get; set; }
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
    }
}
