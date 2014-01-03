using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalk.Salesforce.WcfBehaviors
{
    public static class SfdcTokenManager
    {
        private static DateTime _sessionLastAccessDate = DateTime.Now;
        private static string _sessionId = string.Empty;

        public static string GetSession(string sfdcurl, string uname, string password, string token)
        {
            //get current date time
            DateTime now = DateTime.Now;
            //get the difference from the last time the token was accessed
            TimeSpan diff = now.Subtract(_sessionLastAccessDate);

            //if this is the first time we're calling class, or the token is stale, get a new token
            if (_sessionId == string.Empty || (diff.TotalMinutes >= 60))
            {
                //refresh token

                try
                {
                    SFSvcRef.SforceService proxy = new SFSvcRef.SforceService();
                    proxy.Url = sfdcurl;
                    SFSvcRef.LoginResult result = proxy.login(uname, password + token);
                    _sessionId = result.sessionId;
                    System.Diagnostics.EventLog.WriteEntry("Application", "New SFDC token acquired");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("Application", "Error getting token: " + ex.ToString());
                }
            }
            else
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "Existing SFDC token returned");
            }

            //update last access time since session is good for an hour since last call
            _sessionLastAccessDate = DateTime.Now;

            //give the session ID to the caller
            return _sessionId;
        }
    }
}
