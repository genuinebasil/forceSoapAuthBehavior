using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BizTalk.Salesforce.WcfBehaviors
{
    public class SfdcSoapAuthBehavior : IEndpointBehavior
    {
        // Set the behavior configuration to use later
        public SfdcSoapAuthBehavior(SoapAuthConfiguration configuration)
        {
            BehaviorConfiguration = configuration;
        }

        public SoapAuthConfiguration BehaviorConfiguration { get; set; }

        // Doesn't do anything
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        // Add inspector to collection
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new SfdcSoapAuthInspector(BehaviorConfiguration));
        }

        // Doesn't do anything
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        { }

        // Doesn't do anything
        public void Validate(ServiceEndpoint endpoint)
        { }
    }

    public class SfdcSoapAuthBehaviorExtension : BehaviorExtensionElement 
    {
        // Used to reflect the type and let the user set the property values
        public override Type BehaviorType
        {
            get { return typeof(SfdcSoapAuthBehavior); }
        }

        [ConfigurationProperty("SfdcUrl", DefaultValue = "SfdcUrl", IsRequired = true)]
        public string SfdcUrl
        {
            get { return (string)base["SfdcUrl"]; }
            set { base["SfdcUrl"] = value; }
        }

        [ConfigurationProperty("SfdcUsername", DefaultValue = "Username", IsRequired = true)]
        public string SfdcUsername
        {
            get { return (string)base["SfdcUsername"]; }
            set { base["SfdcUsername"] = value; }
        }

        [ConfigurationProperty("SfdcPassword", DefaultValue = "Password", IsRequired = true)]
        public string SfdcPassword
        {
            get { return (string)base["SfdcPassword"]; }
            set { base["SfdcPassword"] = value; }
        }

        [ConfigurationProperty("SfdcSecurityToken", DefaultValue = "Token", IsRequired = true)]
        public string SfdcSecurityToken
        {
            get { return (string)base["SfdcSecurityToken"]; }
            set { base["SfdcSecurityToken"] = value; }
        }

        private ConfigurationPropertyCollection properties = null;

        // Return the properties of this behavior extension
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new ConfigurationPropertyCollection();
                    this.properties.Add(new ConfigurationProperty("SfdcUrl", typeof(string), "", ConfigurationPropertyOptions.IsRequired));
                    this.properties.Add(new ConfigurationProperty("SfdcUsername", typeof(string), "", ConfigurationPropertyOptions.IsRequired));
                    this.properties.Add(new ConfigurationProperty("SfdcPassword", typeof(string), "", ConfigurationPropertyOptions.IsRequired));
                    this.properties.Add(new ConfigurationProperty("SfdcSecurityToken", typeof(string), "", ConfigurationPropertyOptions.IsRequired));
                }

                return properties;
            }
        }

        // Return the behavior with the populated property values
        protected override object CreateBehavior()
        {
            return new SfdcSoapAuthBehavior(
                 new SoapAuthConfiguration()
                 {
                     SfdcUrl = SfdcUrl,
                     SfdcUsername = SfdcUsername,
                     SfdcPassword = SfdcPassword,
                     SfdcSecurityToken = SfdcSecurityToken
                 });
        }
    }
}
