using System.Xml.Serialization;

namespace WadeWegner.Salesforce.SOAPHelpers.Models
{
    [XmlRoot(Namespace = "urn:partner.soap.sforce.com", ElementName = "userInfo", IsNullable = true)]
    public class UserInfoResult
    {
        [XmlElement(ElementName = "accessibilityMode")]
        public bool AccessibilityMode;

        [XmlElement(ElementName = "currencySymbol")]
        public string CurrencySymbol;

        [XmlElement(ElementName = "orgAttachmentFileSizeLimit")]
        public int OrgAttachmentFileSizeLimit;

        [XmlElement(ElementName = "orgDefaultCurrencyIsoCode")]
        public string OrgDefaultCurrencyIsoCode;

        [XmlElement(ElementName = "orgDisallowHtmlAttachments")]
        public bool OrgDisallowHtmlAttachments;

        [XmlElement(ElementName = "orgHasPersonAccounts")]
        public bool OrgHasPersonAccounts;

        [XmlElement(ElementName = "organizationId")]
        public string OrganizationId;

        [XmlElement(ElementName = "organizationMultiCurrency")]
        public bool OrganizationMultiCurrency;

        [XmlElement(ElementName = "organizationName")]
        public string OrganizationName;

        [XmlElement(ElementName = "profileId")]
        public string ProfileId;

        [XmlElement(ElementName = "roleId")]
        public string RoleId;

        [XmlElement(ElementName = "sessionSecondsValid")]
        public int SessionSecondsValid;

        [XmlElement(ElementName = "userDefaultCurrencyIsoCode")]
        public string UserDefaultCurrencyIsoCode;

        [XmlElement(ElementName = "userEmail")]
        public string UserEmail;

        [XmlElement(ElementName = "userFullName")]
        public string UserFullName;

        [XmlElement(ElementName = "userId")]
        public string UserId;

        [XmlElement(ElementName = "userLanguage")]
        public string UserLanguage;

        [XmlElement(ElementName = "userLocale")]
        public string UserLocale;

        [XmlElement(ElementName = "userName")]
        public string UserName;

        [XmlElement(ElementName = "userTimeZone")]
        public string UserTimeZone;

        [XmlElement(ElementName = "userType")]
        public string UserType;

        [XmlElement(ElementName = "userUiSkin")]
        public string UserUiSkin;
    }
}
