using System;
using System.Collections.Generic;
using System.Text;

namespace HXMail.Common
{
    public class PathInfo
    {
        public static string GetInstallPath
        {
            get {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase; ;
            }
        }

        public static string GetSysLogDir
        {
            get { return string.Format("{0}{1}", GetInstallPath, "log\\"); }
        }

        public static string GetErrorDir
        {
            get { return string.Format("{0}{1}", GetInstallPath, "errorlog\\"); }
        }

        public static string GetUserDir(string EmailAddress)
        {
            return string.Format("{0}{1}{2}\\", GetInstallPath, "UserDate\\", EmailAddress);
        }

        public static string GetUserTempDir(string EmailAddress)
        {
            return string.Format("{0}{1}", GetUserDir(EmailAddress), "Temp\\");
        }

        public static string GetUserAttachmentDir(string EmailAddress)
        {
            return string.Format("{0}{1}", GetUserDir(EmailAddress), "Attachment\\");
        }

        public static string GetUserLogDir(string EmailAddress)
        {
            return string.Format("{0}{1}", GetUserDir(EmailAddress), "log\\");
        }

        public static string GetUserErrorLogDir(string EmailAddress)
        {
            return string.Format("{0}{1}", GetUserDir(EmailAddress), "log\\UserErrorLog\\");
        }
    }
}
