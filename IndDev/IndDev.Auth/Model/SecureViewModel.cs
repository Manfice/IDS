using System.ComponentModel.DataAnnotations;

namespace IndDev.Auth.Model
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Укажите свой адрес электронной почты.")]
        [Display(Name = "Идентификатор пользователя:")]
        [EmailAddress]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class ResetPasswordVm
    {
        [Required(ErrorMessage = "Укажите пароль!")]
        [Display(Name = "ПАРОЛЬ")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль должен быть минимум из 6 символов.")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "ПОДТВЕРЖДЕНИЕ ПАРОЛЯ")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Укажите название компании!")]
        [Display(Name = "НАЗВАНИЕ КОМПАНИИ")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Укажите ваш адрес электронной почты.")]
        [Display(Name = "E-mail АДРЕС")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Укажите пароль!")]
        [Display(Name = "ПАРОЛЬ")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage = "Пароль должен быть минимум из 6 символов.")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "ПОДТВЕРЖДЕНИЕ ПАРОЛЯ")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Укажите ваш регион, откуда Вы.")]
        [Display(Name = "ВАШ ГОРОД")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Укажите ваш контактный телефон!")]
        [Display(Name = "ТЕЛЕФОН ДЛЯ СВЯЗИ")]
        [Phone]
        public string Phone { get; set; }

        public string ReturnUrl { get; set; }
    }
}