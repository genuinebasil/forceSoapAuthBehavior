using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BizTalk.Salesforce.WcfBehaviors
{
    public class SessionHeader : MessageHeader
    {
        private string sessionIdField;
        
        public string sessionId
        {
            get
            {
                return this.sessionIdField;
            }
            set
            {
                this.sessionIdField = value;
            }
        }

        public SessionHeader(String _sessionId)
        {
            sessionId = _sessionId;
        }

        public override string Name
        {
            get { return (SessionHeaderNames.SessionHeaderName); }
        }

        public override string Namespace
        {
            get { return (SessionHeaderNames.SessionHeaderNamespace); }
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteElementString(SessionHeaderNames.SessionIdName, this.sessionId);
        }

        public static SessionHeader ReadHeader(XmlDictionaryReader reader)
        {
            if (reader.ReadToDescendant(SessionHeaderNames.SessionIdName, SessionHeaderNames.SessionHeaderNamespace))
            {
                String sessionId = reader.ReadElementString();
                return (new SessionHeader(sessionId));
            }
            else
            {
                return null;
            }
        }
    }

    public static class SessionHeaderNames
    {
        public const String SessionHeaderName = "SessionHeader";
        public const String SessionIdName = "sessionId";
        public const String SessionHeaderNamespace = "urn:enterprise.soap.sforce.com";
    }
}
