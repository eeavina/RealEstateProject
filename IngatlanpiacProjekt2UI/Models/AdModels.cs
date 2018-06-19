using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//this model is aligned to two database tables and the adcontroller
namespace RealEstateProjectUI.Models
{
    public class AdModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A cím megadása kötelező")]
        [Display(Name = "Hirdetés címe")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A kerület megadása kötelező")]
        [Display(Name = "Kerület")]
        public string District { get; set; }

        [Required(ErrorMessage = "Az utca megadása kötelező")]
        [Display(Name = "Utca és házszám")]
        public string Street { get; set; }

        [Required(ErrorMessage = "A leírás megadása kötelező")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "A leírás nem lehet hosszabb mint 1000 karakter")]
        [Display(Name = "Leírás")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A méret megadása kötelező")]
        [Range(0, 100000,
            ErrorMessage = "Az méret 0 és 100000 négyzetméter között lehet")]
        [Display(Name = "Méret (nm-ben)")]
        public int Size { get; set; }

        [Required(ErrorMessage = "Az ár megadása kötelező")]
        [Range(0.01,5000,
            ErrorMessage = "Az árnak 0-nál nagyobbnak kell lennie és nem lehet nagyobb, mint 5000 millió")]
        [Display(Name = "Ár (millió Forint)")]
        public int Price { get; set; }

        public Nullable<int> PictureId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Kép")]
        public HttpPostedFileBase DataInHttpPostedFileBase { get; set; }

        [Display(Name = "Kép")]
        public string SourceString { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
    }
}