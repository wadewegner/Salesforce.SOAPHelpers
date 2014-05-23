using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WadeWegner.Salesforce.SOAPHelpers.Models;

namespace WadeWegner.Salesforce.SOAPHelpers
{
    public class SalesforceClient
    {
        private const string UserAgent = "forcedotcom-soap-toolkit-dotnet";
        private HttpClient _httpClient;

        public SalesforceClient()
        {
            _httpClient = new HttpClient();
        }

        public SalesforceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> Login(string userName, string password, string orgId)
        {
            var url = "https://login.salesforce.com/services/Soap/u/29.0/" + orgId;
            var soap = string.Format(@"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
    <soapenv:Body>
        <login xmlns=""urn:partner.soap.sforce.com"">
            <username>{0}</username>
            <password>{1}</password>
        </login>
    </soapenv:Body>
</soapenv:Envelope>", userName, password);

            var content = new StringContent(soap, Encoding.UTF8, "text/xml");

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri(url);
                request.Method = HttpMethod.Post;
                request.Content = content;

                request.Headers.Add("SOAPAction", "login");

                var responseMessage = await httpClient.SendAsync(request);
                var response = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    var resultXml = XDocument.Parse(response);
                    var result = resultXml.Descendants(XNamespace.Get("urn:partner.soap.sforce.com") + "result").First();
                    var serializer = new XmlSerializer(typeof(LoginResult));

                    using (var stringReader = new StringReader(result.ToString()))
                    {
                        var loginResult = (LoginResult) serializer.Deserialize(stringReader);
                        return loginResult;
                    }
                }

                throw new Exception("Failed login");
            }
        }

        private async Task<string> Create(string query, string sessionId, string metadataServerUrl)
        {
            var wsdlNamespace = "http://soap.sforce.com/2006/04/metadata";
            var header = "";
            var action = "create";

            var soap = string.Format(
@"<soapenv:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
    xmlns:cmd=""{0}"" 
    xmlns:apex=""http://soap.sforce.com/2006/08/apex"">
	<soapenv:Header>
		<cmd:SessionHeader>
			<cmd:sessionId>{1}</cmd:sessionId>
		</cmd:SessionHeader>
		{2}
	</soapenv:Header>
	<soapenv:Body>
		<{3} xmlns=""{4}"">
			{5}
		</{6}>
	</soapenv:Body>
</soapenv:Envelope>", wsdlNamespace, sessionId, header, action, wsdlNamespace, query, action);

            var content = new StringContent(soap, Encoding.UTF8, "text/xml");
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(metadataServerUrl);
            request.Method = HttpMethod.Post;
            request.Content = content;

            request.Headers.Add("SOAPAction", action);

            var responseMessage = await _httpClient.SendAsync(request);
            var response = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                return response;
            }

            throw new Exception("Failed create object");
        }

        public async Task<CreateResult> CreateCustomField(string customObject, string fieldName, string sessionId, string metadataServerUrl, bool externalId = false)
        {
            var customFieldQuery = string.Format(
@"<metadata xsi:type=""CustomField"" xmlns:cmd=""http://soap.sforce.com/2006/04/metadata"">
	<fullName>{0}.{1}</fullName>
	<label>{1}</label>
	<length>100</length>
	<type>Text</type>
	<externalId>{2}</externalId>
</metadata>", customObject, fieldName, externalId); // TODO: pass this in for flexibility

            var customFieldResponse = await Create(customFieldQuery, sessionId, metadataServerUrl);
            var resultXml = XDocument.Parse(customFieldResponse);
            var result = resultXml.Descendants(XNamespace.Get("http://soap.sforce.com/2006/04/metadata") + "result").First();
            
            if (result == null) return null;

            var serializer = new XmlSerializer(typeof(CreateResult));
            using (var stringReader = new StringReader(result.ToString()))
            {
                var createResult = (CreateResult) serializer.Deserialize(stringReader);
                return createResult;
            }
        }

        public async Task<CreateResult> CreateCustomObject(string customObject, string sessionId, string metadataServerUrl)
        {
            var customObjectQuery = string.Format(
@"<metadata xsi:type=""CustomObject"" xmlns:cmd=""http://soap.sforce.com/2006/04/metadata"">
	<fullName>{0}__c</fullName>
	<label>{0}</label>
	<pluralLabel>{0}</pluralLabel>
	<deploymentStatus>Deployed</deploymentStatus>
	<sharingModel>ReadWrite</sharingModel>
	<nameField>
		<label>ID</label>
		<type>AutoNumber</type>
	</nameField>
</metadata>", customObject);

            var customObjectResponse = await Create(customObjectQuery, sessionId, metadataServerUrl);
            var resultXml = XDocument.Parse(customObjectResponse);
            var result = resultXml.Descendants(XNamespace.Get("http://soap.sforce.com/2006/04/metadata") + "result").First();
            
            if (result == null) return null;

            var serializer = new XmlSerializer(typeof(CreateResult));
            using (var stringReader = new StringReader(result.ToString()))
            {
                var createResult = (CreateResult) serializer.Deserialize(stringReader);
                return createResult;
            }
        }

        public async Task<T> CreateConnectedApp<T>(string fullName, string label, string contactEmail, string callbackUrl, string sessionId, string metadataServerUrl)
        {
            var createConnectedApQuery = string.Format(
@"<metadata xsi:type=""ConnectedApp"" xmlns:cmd=""http://soap.sforce.com/2006/04/metadata"">
	<fullName>{0}</fullName>
	<version>29.0</version>
	<label>{1}</label>
	<contactEmail>{2}</contactEmail>
	<oauthConfig>
		<callbackUrl>{3}</callbackUrl>
		<scopes>Full</scopes>
        <scopes>RefreshToken</scopes>
	</oauthConfig>
</metadata>", fullName, label, contactEmail, callbackUrl);

            var customObjectResponse = await Create(createConnectedApQuery, sessionId, metadataServerUrl);

            var resultXml = XDocument.Parse(customObjectResponse);
            var result = resultXml.Descendants(XNamespace.Get("http://soap.sforce.com/2006/04/metadata") + "result").First();

            //if (result == null) return null;

            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(result.ToString()))
            {
                var createResult = (T)serializer.Deserialize(stringReader);
                return createResult;
            }
        }
    }
}