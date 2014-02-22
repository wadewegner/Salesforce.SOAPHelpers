using System.Xml.Serialization;

namespace WadeWegner.Salesforce.SOAPHelpers.Models
{
    [XmlRoot(Namespace = "urn:partner.soap.sforce.com", ElementName = "result", IsNullable = true)]
    public class LoginResult
    {
        [XmlElement(ElementName = "metadataServerUrl")]
        public string MetadataServerUrl;

        [XmlElement(ElementName = "passwordExpired")]
        public bool PasswordExpired;

        [XmlElement(ElementName = "sandbox")]
        public bool Sandbox;

        [XmlElement(ElementName = "serverUrl")]
        public string ServerUrl;

        [XmlElement(ElementName = "sessionId")]
        public string SessionId;

        [XmlElement(ElementName = "userId")]
        public string UserId;

        [XmlElement(ElementName = "userInfo")]
        public UserInfoResult UserInfo;
    }
}

