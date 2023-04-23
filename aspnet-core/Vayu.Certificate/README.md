Run this project to getting certificate files.
For Azure App Service
Upload cert to your app service
open Azure Could Shell --> az webapp config appsettings set --name <app-name> --resource-group <resource-group-name> --settings WEBSITE_LOAD_CERTIFICATES=<comma-separated-certificate-thumbprints>