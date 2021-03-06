﻿using System;
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
        public async void CreateObjectAndField()
        {
            var customObject = "MyCustomObject";
            var standardObject = "Account";
            var customField = "MyCustomField__c";

            var salesforceClient = new SalesforceClient();
            var loginResult = await salesforceClient.Login(_username, _password, _organizationId);

            var createObjectResult = await salesforceClient.CreateCustomObject(customObject, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createObjectResult);

            var createFieldResult = await salesforceClient.CreateCustomField(customObject + "__c", customField, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createFieldResult);

            var createFieldResult2 = await salesforceClient.CreateCustomField(standardObject, customField, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createFieldResult2);
        }

        [Test]
        public async void CreateConnectedApp()
        {
            var salesforceClient = new SalesforceClient();
            var loginResult = await salesforceClient.Login(_username, _password, _organizationId);

            var fullName = "fullName";
            var label = "label";
            var contactEmail = "contact@email.com";
            var callbackUrl = "callback://url";

            var connectedAppResult = await salesforceClient.CreateConnectedApp<dynamic>(
                fullName,
                label,
                contactEmail,
                callbackUrl,
                loginResult.SessionId,
                loginResult.MetadataServerUrl);

            Assert.IsNotNull(connectedAppResult);
        }
    }
}
