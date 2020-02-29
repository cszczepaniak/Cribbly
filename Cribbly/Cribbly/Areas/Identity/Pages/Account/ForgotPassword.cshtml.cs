using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Cribbly.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager)//, IEmailSender emailSender)
        {
            _userManager = userManager;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                /*
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                string api = "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";
                var client = new SendGridClient(api);
                var from = new EmailAddress("szcz0047@umn.edu", "Cribbly Admin");
                var to = new EmailAddress(Input.Email);
                var subject = "Cribbly password reset";
                var body = "Hello!" +
                    "" +
                    "Thanks for participating in this year's Cribbage tournament. " +
                    "" +

                    "Please reset your password by <a href=" + HtmlEncoder.Default.Encode(callbackUrl) + ">clicking here</a>.";
                var msg = MailHelper.CreateSingleEmail(
                    from,
                    to,
                    subject,
                    null,
                    body
                );
                */
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, "Cribbly123");

                //var response = await client.SendEmailAsync(msg);

                /*await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
