using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cribbly.Models;
using Cribbly.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Cribbly.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //New ApplicationUser object
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    HasTeam = false,
                    TeamId = 0
                };
                //Add user record to DB
                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                
                //Logging info
                if (result.Succeeded)
                {

                    //Create roles if they do not exist
                    var roleInit = new RoleInit(_roleManager);
                    await roleInit.CreateRoles();

                    //Give them the role of "user"
                    IdentityResult addUser = await _userManager.AddToRoleAsync(user, "User");

                    _logger.LogInformation("User created a new account with password.");
                    
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id },
                        protocol: Request.Scheme);

                    //Send confirmation email
                    string api = "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";
                    var client = new SendGridClient(api);
                    var from = new EmailAddress("szcz0047@umn.edu", "Cribbly Admin");
                    var to = new EmailAddress(Input.Email);
                    var subject = "Welcome to Cribbly!";
                    var body = "Hello!" +
                        "" +
                        "Thanks for participating in this year's Cribbage tournament. " +
                        "" +

                        "Please confirm your account by <a href=" + HtmlEncoder.Default.Encode(callbackUrl) + ">clicking here</a>.";
                    var msg = MailHelper.CreateSingleEmail(
                        from,
                        to, 
                        subject, 
                        null,
                        body
                    );

                    var response = await client.SendEmailAsync(msg);

                    //Sign in and redirect to home page
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
