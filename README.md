# Salesforce.SOAPHelpers

This .NET library provides a way for interacting with Salesforce's Force.com SOAP APIs.

### Getting Started

1. Install the NuGet package: `Install-Package WadeWegner.Salesforce.SOAPHelpers`
2. Create an instance of the `SalesforceClient`:

  `var salesforceClient = new SalesforceClient();`

3. Call login and accept the results:

  `var loginResult = await salesforceClient.Login(_username, _password, _organizationId);`

3. Create the custom object by calling `CreateCustomObject`:

        var customObject = "MyCustomObject";
        var createObjectResult =
          await salesforceClient.CreateCustomObject(
            customObject, loginResult.SessionId, loginResult.MetadataServerUrl);`

4. Create the custom field by calling `CreateCustomField`:

        var customField = "MyCustomField";
        var createFieldResult =
          await salesforceClient.CreateCustomField(
            customObject, customField, loginResult.SessionId, loginResult.MetadataServerUrl);

Enjoy!
