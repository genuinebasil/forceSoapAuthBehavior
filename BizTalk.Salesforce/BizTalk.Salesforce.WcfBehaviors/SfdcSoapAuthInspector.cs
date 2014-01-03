using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace BizTalk.Salesforce.WcfBehaviors
{
    public class SfdcSoapAuthInspector : IClientMessageInspector
    {
        // Accept property values as part of constructor
        public SfdcSoapAuthInspector(SoapAuthConfiguration behaviorConfiguration)
        {
            BehaviorConfiguration = behaviorConfiguration;
        }

        public SoapAuthConfiguration BehaviorConfiguration { get; set; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState) { }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();

            var sessionId = SfdcTokenManager.GetSession(
                        BehaviorConfiguration.SfdcUrl,
                        BehaviorConfiguration.SfdcUsername,
                        BehaviorConfiguration.SfdcPassword,
                        BehaviorConfiguration.SfdcSecurityToken);

            request.Headers.Add(new SessionHeader(sessionId));
                        
            //request is sent by reference, no need to return anything
            return null;
        }
    }

    public struct SoapAuthConfiguration
    {
        public string SfdcUrl { get; set; }
        public string SfdcUsername { get; set; }
        public string SfdcPassword { get; set; }
        public string SfdcSecurityToken { get; set; }
    }

}
