/********************************************************************************
 * File Name    : MailSender.cs 
 * Author       : Amit Kumar
 * Created On   : 12/05/2014
 * Description  : This class has a mail sending functionality
 *******************************************************************************/

using System;
using System.Net.Mail;
using System.Reflection;
using log4net;

namespace AriRick.Utility
{
    /// <summary>
    /// This class has a mail sending functionality
    /// </summary>
    public class MailSender
    {
        //Error logging
        private static ILog Log
        {
            get { return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType +
                MethodBase.GetCurrentMethod().Name); }
        }

        /// <summary>
        /// Method to Send mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendMail(string subject, string to, string body)
        {
            try
            {
                //Create mail message object
                var mailMessage = new MailMessage { Subject = subject, Body = body };
                mailMessage.To.Add(new MailAddress(to));
                mailMessage.IsBodyHtml = true;
                var client = new SmtpClient();

                // Attempt to send the email                
                client.Send(mailMessage);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("Error in SendMail. " + exception.Message, exception);
                return false;
            }
        }
    }
}
