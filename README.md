# YouTubeApiExample

ASP.NET Razor Pages app with Google external authentication to access YouTube Data API v3

## Google external login setup

ASP.NET Core makes it easy to sign-in using existing credentials from external authentication providers like Google.
Considering Google as an external authentication provider first you need to create credentials to obtain an access token from Google's authorization servers:

1. Go to the [Credentials page](https://console.developers.google.com/apis/credentials).
2. Click Create credentials > OAuth client ID.
3. Select the Web application application type.
4. Name your OAuth 2.0 client and click Create

Next, open the Credentials menu, select the created OAuth 2.0 client, set up the url of your ASP.NET Core app and get the Client ID and Client Secret:

1. Set the url in the Authorized redirect URIs section (https://localhost:7020/signin-google)
2. Save the Client ID and Client Secret for use in the app's configuration.
3. Use SecretManager to store the retrieved tokens

[For more information please read this guide](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-9.0)

The Secret Manager is designed mostly for development stage.
The user secrets.json file is only merged with appsettings.json when IsDevelopment = true.
For other environments (staging, production), you can use some secure secrets storage service, for example, Key Vault and environment variables.
Secrets should NOT be stored in appsettings, as it is plain text.
You should use secrets (which is basically another json file outside of the source code), and then environment variables when running in production.