using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DekkOnlineMVC.Models;
using Framework;
using DekkOnlineMVC.Models.HelperClasses;
using Entity;
using DekkOnlineMVC.Engine;

namespace DekkOnlineMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        DatabaseFunctions db2 = new DatabaseFunctions();
        dekkOnlineEntities db = new dekkOnlineEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private Controller _ctr;

        public AccountController(Controller ctr)
        {
            this._ctr = ctr;
        }
        public AccountController()
        {
            this._ctr = null;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                if (HttpContext != null)
                {
                    return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                else
                {
                    return _signInManager ?? _ctr.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }

            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if (HttpContext != null)
                {
                    return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                else {
                    return _userManager ?? _ctr.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }

            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult _LoginForm(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _LoginForm(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var usr = db.AspNetUsers.Where(s => s.Email == model.usuEmail).FirstOrDefault();
            if (usr == null)
            {
                ModelState.AddModelError(constClass.error, "El usuario o la contraseña son incorrectas.");
                return PartialView("Account/_LoginForm", model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.usuEmail, model.usuPassword, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        Users user = new Users();
                        Framework.ShoppingCart shoppingCart = new Framework.ShoppingCart();
                        string isUser = user.IdUser(model.usuEmail);
                        System.Web.HttpContext.Current.Session["SessionUser"] = isUser;
                        var usercookie = Security.GetIdUser(this);
                        if (usercookie != null || usercookie != "" || usercookie.Length < 1)
                        {
                            bool updateCookieDelevery = shoppingCart.updateDeleveryType(isUser, usercookie);
                        }
                        if (returnUrl != "")
                            model.scriptJS = "document.location.replace('" + returnUrl + "');";
                        else
                            model.scriptJS = "document.location.replace('" + Url.Action("Index", "Home") + "');";
                        return PartialView("Account/_LoginForm", model);
                    //return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError(constClass.error, "El usuario o la contraseña son incorrectas.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        /// <summary>
        /// Funcion que valida si hay que redirigir  a alguna pantalla
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        protected object hasReturnUrl(string returnUrl)
        {
            if (returnUrl == String.Empty || returnUrl == "" || returnUrl == null)
                return null;
            return new { returnUrl = returnUrl };
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult _RegisterForm()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _RegisterForm(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usr = db.AspNetUsers.Where(s => s.Email == model.usuEmail).FirstOrDefault();
                if (usr != null)
                {
                    ModelState.AddModelError(constClass.info, "El correo electrónico ya está registrado. Inicia sesión o recupera la contraseña.");
                    return PartialView("Account/_RegisterForm", model);
                }
                var user = new ApplicationUser { UserName = model.usuEmail, Email = model.usuEmail };
                var result = await UserManager.CreateAsync(user, model.usuPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    Users users = new Users();
                    string idUser = users.IdUser(model.usuEmail);
                    System.Web.HttpContext.Current.Session["SessionUser"] = idUser;

                    users.EnvioCorreo(model.usuEmail, model.usuPassword);
                    users.UpdateRoleUser(model.usuEmail);
                    var promo = users.CreateRandomPromoCode();
                    bool validatepromo = users.SavePromoCode(promo, model.usuEmail);
                    if (validatepromo == true)
                    {

                    }
                    else
                    {
                        while (validatepromo == false)
                        {
                            promo = users.CreateRandomPromoCode();
                            validatepromo = users.SavePromoCode(promo, model.usuEmail);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.usuEmail);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Redirecciona a la página del proveedor de credenciales. (Facebook,etc)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _LoginFormSocialNetworks(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("LoginExternalConfirmAsync", "Account", hasReturnUrl(returnUrl)));
        }

        /// <summary>
        /// Recibe el Callback con la información del proveedor de credenciales.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> LoginExternalConfirmAsync(string returnUrl = null)
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("Index", "Home", hasReturnUrl(returnUrl));
            //Tries to log in and if it has success redirects to admin page.
            string logInUrl = await externalLoginSignInSync(loginInfo, returnUrl);
            if (logInUrl != null)
                return Redirect(logInUrl);

            //Is not registered in the database register claims in the database and try to loggin again
            var claimName = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var claimEmail = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            LoginExternalConfirmViewModel model = new LoginExternalConfirmViewModel();
            splitClaimName(model, claimName);
            model.usuEmail = claimEmail != null ? claimEmail.Value : String.Empty;
            model.usuEmailConfirmar = model.usuEmail;
            model.LoginProvider = loginInfo.Login.LoginProvider;

            if (await externalLoginCreateUser_Provider(model, loginInfo))
            {
                logInUrl = await externalLoginSignInSync(loginInfo, returnUrl);
                if (logInUrl != null)
                    return Redirect(logInUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Inicia la sesión del usuario cuando ya esta logeado.
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        protected async Task<string> externalLoginSignInSync(ExternalLoginInfo loginInfo, string returnUrl)
        {
            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                case SignInStatus.LockedOut:
                    string userId = db2.getUserIdByProviderKey(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey);
                    var user = db2.getUserById(userId);
                    if (user != null)
                    {
                        if (returnUrl == null)
                            return Url.Action("Index", "Home", null);
                        else
                        {
                            return Url.Action("Index", "Profile");
                        }
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Divide el nombre del usuario que se obtiene del proveedor externo.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="claimName"></param>
        protected void splitClaimName(LoginExternalConfirmViewModel model, Claim claimName)
        {
            string names = claimName != null ? claimName.Value : string.Empty;
            if (names != string.Empty)
            {
                var split = names.Split(new char[] { ' ' });
                int counter = 0;
                foreach (string name in split)
                {
                    if (counter < 1)
                        model.usuNombre += " " + name;
                    else
                        model.usuApellido += " " + name;
                    counter++;
                }
            }
        }

        /// <summary>
        /// Crea el usuario si se logea si este no existe en la bd y e inicio sesion desde un proveedor externo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        protected async Task<bool> externalLoginCreateUser_Provider(LoginExternalConfirmViewModel model, ExternalLoginInfo loginInfo)
        {
            //Aqui crear la contraseña
            Users newpass = new Users();
            PasswordHasher passhash = new PasswordHasher();
            
            string pass = newpass.CreateRandomPassword(7);
           string pass2 = passhash.HashPassword(pass);
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.usuEmail,
                Email = model.usuEmail,
                EmailConfirmed = true,
                PasswordHash = pass2
            };

            var userChecked = db.AspNetUsers.Where(s=>s.Email == model.usuEmail).FirstOrDefault();
            if (userChecked == null)
            {
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    Users users = new Users();
                    string idUser = users.IdUser(model.usuEmail);
                    System.Web.HttpContext.Current.Session["SessionUser"] = idUser;

                    users.EnvioCorreo(model.usuEmail, pass);
                    users.UpdateRoleUser(model.usuEmail);
                    var promo = users.CreateRandomPromoCode();
                    bool validatepromo = users.SavePromoCode(promo, user.Email);
                    if (validatepromo == true)
                    {

                    }
                    else
                    {
                        while (validatepromo == false)
                        {
                            promo = users.CreateRandomPromoCode();
                            validatepromo = users.SavePromoCode(promo, model.usuEmail);
                        }
                    }
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                    else
                    {
                        Global_Functions.saveErrors(result.Errors.ToString(), false);
                        AddErrors(result);
                    }
                }
                else
                {
                    Global_Functions.saveErrors(result.Errors.ToString(), false);
                    AddErrors(result);
                }
            }
            else
            {
                user.Id = userChecked.Id;
                //Inserta en la base de datos para que ese usuario pueda ingresar por ese proveedor con la misma cuenta de correo.
                var result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                if (result.Succeeded)
                {
   
                    return true;
                }
                else
                {
                    Global_Functions.saveErrors(result.Errors.ToString(), false);
                    AddErrors(result);
                }
            }

            return false;
        }


        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.Session["SessionUser"] = "";
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}