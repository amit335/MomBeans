/********************************************************************************
 * File Name    : LogRepository.cs
 * Author       : Amit Kumar
 * Created On   : 18/8/2014
 * Description  : This is Repository class for logging exceptions
 *******************************************************************************/

using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using AriRick.Utility;
using log4net;
using log4net.Appender;
using log4net.Core;
using MomBeans.Utility;

namespace MomBeans.DB.Repository
{
    public class LogAppender : IAppender,IDisposable
    {
        #region Variables

        private bool _disposedValue;
        private MomBeansEntities _momBeansContext;

        #endregion Variables

        #region Property

        //Error logging
        protected static ILog Log
        {
            get { return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType +
                MethodBase.GetCurrentMethod().Name); }
        }

        /// <summary>
        /// Get or set instance of GrootEntities
        /// </summary>
        public MomBeansEntities Context
        {
            get { return _momBeansContext = _momBeansContext ?? new MomBeansEntities(); }
        }

        /// <summary>
        /// Get/Set Name 
        /// </summary>
        public string Name { get; set; }

        #endregion


        public void Close()
        {

        }

        /// <summary>
        /// This method is responsible for logging the exception-logs in the database
        /// </summary>
        /// <param name="loggingEvent"></param>
        public void DoAppend(LoggingEvent loggingEvent)
        {
            var log = new Log();
            try
            {
                var httpCtx = HttpContext.Current; //Current context object 
                var request = httpCtx == null ? null : httpCtx.Request;

                //Create a Log object
                log = new Log
                {
                    Date = loggingEvent.TimeStamp,
                    Level = loggingEvent.Level.Name,
                    Logger = loggingEvent.LoggerName,
                    Message = loggingEvent.RenderedMessage,
                    Thread = loggingEvent.ThreadName,
                    Exception = loggingEvent.GetExceptionString(),
                    ServerName =
                        httpCtx == null
                            ? Environment.MachineName
                            : httpCtx.Server.MachineName ?? Environment.MachineName,
                    IPAddr = request == null ? null : request.UserHostAddress,
                    URL = request == null ? null : request.RawUrl,
                    Referrer =
                        request == null
                            ? null
                            : request.UrlReferrer == null ? null : request.UrlReferrer.OriginalString,
                    User = httpCtx == null ? null : httpCtx.User == null ? null : httpCtx.User.Identity.Name,
                    UserAgent = request == null ? null : request.UserAgent,
                    SessionTable = string.Empty
                };

                //Save in Error in Log table
                Context.Logs.Add(log);
                Context.SaveChanges();


                //send the error email 
                SendErrorEmail(loggingEvent.TimeStamp.ToString(CultureInfo.InvariantCulture),
                               loggingEvent.Level.Name, loggingEvent.LoggerName,
                               loggingEvent.RenderedMessage, log.Exception, log.UserAgent, log.IPAddr, log.URL,
                               log.Referrer, log.User, log.ServerName);
            }
            catch
            {
                //send the error email 
                SendErrorEmail(loggingEvent.TimeStamp.ToString(CultureInfo.InvariantCulture),
                                                loggingEvent.Level.Name, loggingEvent.LoggerName,
                                                loggingEvent.RenderedMessage, log.Exception, log.UserAgent, log.IPAddr,
                                                log.URL, log.Referrer, log.User, log.ServerName);


            }
        }

        /// <summary>
        /// Method to pass the error details to the MailSender class to send mail.
        /// </summary>
        /// <param name="logDate"></param>
        /// <param name="logLevelName"></param>
        /// <param name="logger"></param>
        /// <param name="logMessage"></param>
        /// <param name="exception"></param>
        /// <param name="logUserAgent"></param>
        /// <param name="logIpAddress"></param>
        /// <param name="logUrl"></param>
        /// <param name="logReferrer"></param>
        /// <param name="loguser"></param>
        /// <param name="logservername"></param>
        /// <returns></returns>
        public void SendErrorEmail(string logDate, string logLevelName, string logger, string logMessage,
                                   string exception, string logUserAgent, string logIpAddress, string logUrl,
                                   string logReferrer, string loguser, string logservername)
        {

            //Prepare the Error Message Body
            var sbBody = new StringBuilder(string.Empty);

            sbBody.Append("LogDate :" + logDate + Environment.NewLine)
                .Append("LogLevelName :" + logLevelName + Environment.NewLine)
                .Append("Logger :" + logger + Environment.NewLine)
                .Append("LogMessage :" + logMessage + Environment.NewLine)
                .Append("Exception :" + exception + Environment.NewLine)
                .Append("LogUserAgent :" + logUserAgent + Environment.NewLine)
                .Append("IP Address :" + logIpAddress + Environment.NewLine)
                .Append("LogUrl :" + logUrl + Environment.NewLine)
                .Append("Referrer :" + logReferrer + Environment.NewLine)
                .Append("User :" + loguser + Environment.NewLine)
                .Append("Server :" + logservername);

            var emailAddress = ConfigKeyReader.ErrorEmail;
            if (String.IsNullOrEmpty(emailAddress))
            {
                return;
            }
            var errorMailSubject = ConfigKeyReader.ErrorEmailSubject;

            //Mail Sender SendMail function invoked to send mail
            MailSender.SendMail(errorMailSubject, emailAddress, sbBody.ToString());
        }

        #region " Dispose Pattern"

        /// <summary>
        /// This code is added to correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// IDisposable event
        /// </summary>
        /// <param name="disposing">IsDisposing</param>
        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_momBeansContext != null)
                    {
                        _momBeansContext.Dispose();
                    }
                }
            }
            _disposedValue = true;
        }

        #endregion
    }
}


