using System;
using System.Net;
using System.Net.Mail;
using System.Web.Http;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class ErrorController : ApiController
    {
        private const string S = "this";
        // GET api/error
        public string Get(string method, string exception, string fullEx)
        {
            Database.InsertPhoneErrorToDb(method, exception, fullEx);
            //EmailError(method, exception, fullEx);

            return S;
        }

        private void EmailError(string s1, string s2, string s3)
        {
            var m = new MailMessage();
            var SmtpServer = new SmtpClient("smtp.gmail.com");
            m.From = new MailAddress("gercarroll@gmail.com");
            m.To.Add("gcwpdev@gmail.com");
            m.Subject = "GAA App Error Phone" + DateTime.Now;
            m.Body = s1 + ";    " + s2 + ";     " + s3;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("gercarroll@gmail.com", "Ggc12003412598");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(m);
        }
    }
}