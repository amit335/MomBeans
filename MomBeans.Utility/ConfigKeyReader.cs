/********************************************************************************
 * File Name    : ConfigKeyReader.cs 
 * Author       : Amit Kumar
 * Created On   : 18/08/2014
 * Description  : This class helps in reading the web.config keys
 *******************************************************************************/

using System;
using System.Configuration;
using System.Reflection;
using log4net;

namespace MomBeans.Utility
{
    public static class ConfigKeyReader
    {
        //Error logging
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType +
            MethodBase.GetCurrentMethod().Name);

        //Error To Address
        public static readonly string ErrorEmail =
          ReadConfigKeys(ConfigurationManager.AppSettings["errorToAddress"], "errorToAddress");

        //Error email subject
        public static readonly string ErrorEmailSubject =
            ReadConfigKeys(ConfigurationManager.AppSettings["errorMailSubject"], "errorMailSubject");

        //First User user name
        public static readonly string DefaultUserName =
            ReadConfigKeys(ConfigurationManager.AppSettings["DefaultUserName"], "DefaultUserName");

        //First User password
        public static readonly string DefaultUserPassword =
            ReadConfigKeys(ConfigurationManager.AppSettings["DefaultUserPassword"], "DefaultUserPassword");

        
        /// <summary>
        /// Reading the configuration keys and throwing exception if the key is absent
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static string ReadConfigKeys(string key, string keyName)
        {
            try
            {
                if (String.IsNullOrEmpty(key))
                {
                    throw new Exception("The WebConfig key '" + keyName + "' is not present.");
                }
                return key;
            }
            catch (Exception exception)
            {
                Log.Error("WebConfig Key Missing. " + exception.Message, exception);
                return String.Empty;
            }
        }
    }
}
