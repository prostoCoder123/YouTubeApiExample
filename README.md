# YouTubeApiExample

ASP.NET Razor Pages app with Google external authentication to access YouTube Data API v3

## Google external login setup

ASP.NET Core makes it easy to sign-in using existing credentials from external authentication providers like Google.
Considering Google as an external authentication provider first you need to create credentials and project on

1. Go to the [Credentials page](https://console.developers.google.com/apis/credentials).
2. Click Create credentials > OAuth client ID.
3. Select the Web application application type.
4. Name your OAuth 2.0 client and click Create

Next, open the Credentials menu, select the created OAuth 2.0 client, set up the url of your ASP.NET Core app and get the Client ID and Client Secret:

1. Set the url in the Authorized redirect URIs section (https://localhost:7020/signin-google)
2. Save the Client ID and Client Secret for use in the app's configuration.
3. Use SecretManager to store the retrieved tokens

[For more information please read this guide](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-9.0)
