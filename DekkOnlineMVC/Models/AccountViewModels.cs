using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DekkOnlineMVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    //public class LoginViewModel
    //{
    //    [Required]
    //    [Display(Name = "Email")]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [Display(Name = "Remember me?")]
    //    public bool RememberMe { get; set; }
    //}

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Favor de ingresar el correo.")]
        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "Favor de ingresar una cuenta de correo valida")]
        public string usuEmail { get; set; }

        [Required(ErrorMessage = "Favor de ingresar la contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string usuPassword { get; set; }

        [Display(Name = "No cerrar sesión")]
        public bool RememberMe { get; set; }

        public string scriptJS { get; set; }
    }

    //public class RegisterViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email1 { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password1 { get; set; }

    //    //[DataType(DataType.Password)]
    //    //[Display(Name = "Confirm password")]
    //    //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    //public string ConfirmPassword { get; set; }
    //}

    public class RegisterViewModel
    {
        public string usuId { get; set; }

        //[Required(ErrorMessage = "Favor de ingresar el nombre.")]
        //[MaxLength(50), Display(Name = "Nombre")]
        //public string usuNombre { get; set; }

        //[Required(ErrorMessage = "Favor de ingresar el apellido.")]
        //[MaxLength(50), Display(Name = "Apellidos")]
        //public string usuApellido { get; set; }

        [Required(ErrorMessage = "Favor de ingresar el correo.")]
        [EmailAddress(ErrorMessage = "Favor de ingresar una cuenta de correo valida")]
        [Display(Name = "Correo")]
        public string usuEmail { get; set; }

        //[EmailAddress(ErrorMessage = "Favor de ingresar una cuenta de correo valida")]
        //[Display(Name = "Confirmar correo")]
        //[System.ComponentModel.DataAnnotations.CompareAttribute("usuEmail", ErrorMessage = "El correo electrónico no coincide.")]
        //public string usuEmailConfirm { get; set; }

        //[Required(ErrorMessage = "Favor de seleccionar un rol.")]
        //[Display(Name = "Rol")]
        //public string usuRol { get; set; }

        [Required(ErrorMessage = "Favor de ingresar la contraseña")]
        [StringLength(100, ErrorMessage = "La longitud de la contraseña debe ser al menos de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string usuPassword { get; set; }

        public string scriptJS { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirmar Contraseña")]
        //[System.ComponentModel.DataAnnotations.CompareAttribute("usuPassword", ErrorMessage = "La contraseña y la confirmacion de contraseña no coinciden.")]
        //public string usuPasswordConfirm { get; set; }

        //[Display(Name = "Telefono")]
        //[DataType(DataType.PhoneNumber)]
        //public string usuTelefono { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Favor de ingresar el correo.")]
        [EmailAddress(ErrorMessage = "Favor de ingresar una cuenta de correo valida")]
        [Display(Name = "Correo")]
        public string usuEmail { get; set; }

        public string scriptJS { get; set; }
    }

    public class LoginExternalConfirmViewModel
    {
        [Required(ErrorMessage = "Favor de ingresar el nombre.")]
        [MaxLength(50), Display(Name = "Nombre(s)")]
        public string usuNombre { get; set; }

        [Required(ErrorMessage = "Favor de ingresar el apellido.")]
        [MaxLength(50), Display(Name = "Apellido(s)")]
        public string usuApellido { get; set; }

        [Required(ErrorMessage = "Favor de ingresar el correo.")]
        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "Favor de ingresar una cuenta de correo válida")]
        public string usuEmail { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar correo")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("usuEmail", ErrorMessage = "El correo no coincide, favor de verificarlo.")]
        public string usuEmailConfirmar { get; set; }

        public string LoginProvider { get; set; }
    }


}
