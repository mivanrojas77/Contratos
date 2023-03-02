using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LInde.CorreoElectronico
{
    public class Correo
    {
        public void sendEmail()
        {
            //String userName = "moises.rojas@linde.com";
            String userName = "c7ss74";
            String password = "Colombia/2022";
            MailMessage msg = new MailMessage("moises.rojas@linde.com ", " mivan.rojas@gmail.com ");
            msg.Subject = "Correo de prueba";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name: Abuelito dime tu" );
            sb.AppendLine("Mobile Number: 57316520000" );
            sb.AppendLine("Email: mivan.rojas@gmail.com" );
            sb.AppendLine("Drop Downlist Name: 01234" );
            msg.Body = sb.ToString();
            //Attachment attach = new Attachment(Server.MapPath("folder/" + ImgName));
            //msg.Attachments.Add(attach);
            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Credentials = new System.Net.NetworkCredential(userName, password);
            SmtpClient.Host = "smtp.office365.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.Send(msg);
        }
    }
}
