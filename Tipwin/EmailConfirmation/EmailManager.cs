using System;
using System.Net.Mail;
using System.Web;

namespace Tipwin.EmailConfirmation
{
    public class EmailManager
    {

        private const string EmailFrom = "noreplay@gmail.com";
        public static void SendConfirmationEmail(string userName)
        {
            var user = System.Web.Security.Membership.GetUser(userName.ToString());
            var confirmationGuid = user.ProviderUserKey.ToString();
            var verifyUrl = HttpContext.Current.Request.Url.GetLeftPart
               (UriPartial.Authority) + "/Account/Verify/" + confirmationGuid;

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(EmailFrom, user.Email))
                {
                    message.Subject = "Please Verify your Account";
                    message.Body = "<html><head><meta content=\"text/html; charset = utf - 8\" /></head><body><p>Dear " + user.UserName +
        ", </p><p>To verify your account, please click the following link:</p>"
        + "<p><a href=\"" + verifyUrl + "\" target=\"_blank\">" + verifyUrl + ""
        + "</a></p><div>Best regards,</div><div>Someone</div><p>Do not forward "
        + "this email. The verify link is private.</p></body></html>";

                    message.IsBodyHtml = true;

                    client.EnableSsl = true;
                    client.Send(message);
                };
            };
        }
    }
}
