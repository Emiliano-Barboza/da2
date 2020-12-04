namespace NaturalUruguayGateway.HelperInterface.Configuration
{
    public interface IConfigurationManager
    {
        string ConnectionString { get; }
        string LogoutRedirectUrl { get; }
        string DefaultPassword { get; }
        string AuthenticationSchema { get; }
        string LodgmentsFolder { get; }
        string SpotsFolder { get; }
        string ResourcesFolder { get; }
        string ImportThirdPartyAssembliesFolder { get; }
        string BaseUrl { get; }
    }
}