using System.Drawing;

namespace TechLanches.Adapter.API.Options
{
    public class AuthenticationCognitoOptions
    {
        public string Region { get; set; }
        public string CognitoPoolId { get; set; }
        public string CognitoClientId { get; set; }
        public string CognitoClientSecret { get; set; }
        public string CognitoUri { get => $"https://cognito-idp.{Region}.amazonaws.com/{CognitoPoolId}"; }
    }
}