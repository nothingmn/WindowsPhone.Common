namespace RunKeeper8.Contracts.Services
{
    public interface IAccount
    {
        string ClientID { get; set; }
        string ClientSecret { get; set; }
        string AuthorizationURL { get; set; }
        string AccessTokenURL { get; set; }
        string DeAuthorizationURL { get; set; }
        string RedirectURL { get; set; }
        string Code { get; set; }
        string AccessToken { get; set; }

        string AuthorizationEndPoint();
        string TokenEndPoint();
    }
}