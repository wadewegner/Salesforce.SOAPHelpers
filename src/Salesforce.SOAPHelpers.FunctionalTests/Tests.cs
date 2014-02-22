using System;
using NUnit.Framework;

namespace WadeWegner.Salesforce.SOAPHelpers.FunctionalTests
{
    [TestFixture]
    public class Tests
    {
        const string UserName = "wade@sfdcapi.com";
        const string Password = "Pa$$w0rd!";
        const string OrgId = "00Di0000000icUBEAY";

        [Test]
        public async void Login()
        {
            var salesforceClient = new SalesforceClient();
            var loginResult = await salesforceClient.Login(UserName, Password, OrgId);

            Assert.IsNotNull(loginResult);
        }

        [Test]
        public async void CreateObject()
        {
            var customObject = "MyCustomObject";
            var customField = "MyCustomField";

            var salesforceClient = new SalesforceClient();
            var loginResult = await salesforceClient.Login(UserName, Password, OrgId);

            var createObjectResult = await salesforceClient.CreateCustomObject(customObject, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createObjectResult);

            var createFieldResult = await salesforceClient.CreateCustomField(customObject, customField, loginResult.SessionId, loginResult.MetadataServerUrl);
            Assert.IsNotNull(createFieldResult);
        }
    }
}
