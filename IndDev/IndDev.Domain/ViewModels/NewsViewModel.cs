using System;
using System.ComponentModel.DataAnnotations;
using IndDev.Domain.Entity;
using System.Web;
using System.Web.Mvc;

namespace IndDev.Domain.ViewModels
{
    public class NewsViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(130, ErrorMessage = "Заголовок не должен превышать 130 символов.")]
        [MinLength(20, ErrorMessage = "Заголовок должен содержать минимум 20 символов.")]
        [Display(Name = "Заголовок новости")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Превью")]
        public string ShortDescr { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
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
        [Display(Name = "Картинка для новости:")]
        [Required(ErrorMessage = "Укажите фаил с изображением!")]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}