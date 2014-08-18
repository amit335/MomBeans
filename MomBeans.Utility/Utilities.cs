/********************************************************************************
 * File Name    : Utilities.cs
 * Author       : Amit Kumar
 * Created On   : 12/05/2014
 * Description  : This is a utility class to store constants
 *******************************************************************************/
namespace MomBeans.Utility
{
    public static class Utilities
    {
        /// <summary>
        /// Error Messages
        /// </summary>
        public static class Errors
        {
            public const string DefaultUserError = "Error Adding the default user for the Application: ";
            public const string ReadError = "Error in Read: ";
        }

        /// <summary>
        /// Success Messages shown across the application
        /// </summary>
        public static class Success
        {
            public const string ResetMailSuccess = "Thank you! The reset password link has been sent to your mail id.";
        }

        /// <summary>
        /// Constants used across the application
        /// </summary>
        public static class Constants
        {
            public const string Admin = "Admin";
            public const string Active = "Active";
            public const string ContentEditor = "ContentEditor";
            public const string User = "User";
        }
    }
}

