using System;
using System.Configuration;
using NUnit.Framework;

namespace WadeWegner.Salesforce.SOAPHelpers.FunctionalTests
{
    [TestFixture]
    public class Tests
    {
#pragma warning disable 618
        private static string _tokenRequestEndpointUrl = ConfigurationSettings.AppSettings["TokenRequestEndpointUrl"];
        private static string _securityToken = ConfigurationSettings.AppSettings["SecurityToken"];
        private static string _consumerKey = ConfigurationSettings.AppSettings["ConsumerKey"];
        private static string _consumerSecret = ConfigurationSettings.AppSettings["ConsumerSecret"];
        private static string _username = ConfigurationSettings.AppSettings["Username"];
        private static string _password = ConfigurationSettings.AppSettings["Password"] + _securityToken;
        private static string _organizationId = ConfigurationSettings.AppSettings["OrganizationId"];
#pragma warning restore 618

        [Test]
        public async void Login()
        {
            var salesforceClient = new SalesforceClient();
            var loginResult = await salesforceClient.Login(_username, _password, _organizationId);

            Assert.IsNotNull(loginResult);
        }

        [Test]
        public async void CreateObject()
        {
            var customObject = "MyCustomObject";
            var customField = "MyCustomField";

            var salesforceClient = new SalesforceClient();
            var loginResult = await salesforceClient.Login(_username, _password, _organizationId);

            var createObjectResult = await salesforceClient.CreateCustomObject(customObject, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createObjectResult);

            var createFieldResult = await salesforceClient.CreateCustomField(customObject, customField, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createFieldResult);
        }
    }
}
