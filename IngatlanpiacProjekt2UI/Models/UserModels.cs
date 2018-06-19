using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstateProjectUI.Models
{
    public class UserModels
    {
        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0}nak legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó még egyszer")]
        [Compare("Password", ErrorMessage = "A jelszó és a jelszó ellenőrző nem egyezik.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Jogosultság")]
        public int UserRoleId { get; set; }

        public string Salt { get; set; }
    }
}