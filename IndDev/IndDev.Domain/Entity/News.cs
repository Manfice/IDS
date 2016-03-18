using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IndDev.Domain.Entity
{
    public class News
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public DateTime NewsTime { get; set; }

        [Display (Name = "Картинка заголовка:")]
        public byte[] Glyf { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        [Required]
        [MaxLength(130,ErrorMessage = "Заголовок не должен превышать 130 символов.")]
        [MinLength(20,ErrorMessage = "Заголовок должен содержать минимум 20 символов.")]
        [Display(Name = "Заголовок новости")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Превью")]
        public string ShortDescr { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"),AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Новостной блок")]
        public string FullNewsBody { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Источник")]
        public string Source { get; set; }

        [Required(ErrorMessage = "Укажите категорию новостей")]
        [Display(Name = "Категория новостей")]
        public string Category { get; set; }

        [Display(Name = "Публиковать новость?")]
        public bool Published { get; set; } 
    }
}