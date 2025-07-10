#  Azure Functions in Visual Studio (.NET + C++ DLL)
This POC solution to let Cpp code run in Azure Functions contains two test implementations of a simple Cpp calculator that only contains an Add method. One sample implementation uses the DllImport (P/Invoke) and the second implementation with CLI uses a NuGet package that has been provided by @LohnClassicTeam. The solution contains a class library that contains the core implementations of the test calculators where either the Cpp DLL is imported or the NuGet package is referenced. The Azure Functions that use the different calculators are located in the Azure Function project. 


## With DllImport 
1. The DLLs need to be copied into the project folder (dll folder)
2. A new calculator class need to be created where every method of the DLL is imported by the DLlImport (CalculateByDllImport.cs)
3. An Azure Function class need to be created where a new function is defined that uses the method from the created calculator from the class library 
4. For testing, start the project and click the function link to trigger the local function (e.g. http://localhost:7895/api/CalculateByDllImport)

## With CLI 
1. @LohnClassicTeam provided a NuGet package which need to be located in the ´local-packages´ folder and an additional NuGet.config file is required to be able to use the package 
2. A new calculator class need to be created that defines a new method where a Calculator object (from the NuGet package) is created from which the static add method is called 
3. An Azure Function class need to be created where a new function is defined that uses the method from the created calculator from the class library 
4. For testing, start the project and click the function link to trigger the local function (e.g. http://localhost:7895/api/CalculateByCLI)

## Deploy/Publish Azure Function 
### Local Deployment 
1. Right click on the project and click publish 
2. Create a new profile and choose Azure -> Azure Function App 
3. Create a new function instance - if you do not already have a Resource Group, Storage Account and Application Insights, create them beforehand in the Azure Portal 
4. Create and Publish 

These settings can look like this: 
![alt text](img/deploy-function-app-from-vs.png)

### Bicep File 
ℹ️ You can only test this with your personal subscription, you will not have permissions in the RZL tenant. 

With a Bicep file, all Azure prerequisites can be deployed, as long as there is already a resource group existing. 
The created Bicep file provisions a Windows-based Azure Function App in westeurope, along with:
- A Storage Account
- An App Service Plan (consumption tier)
- An Application Insights instance
- GitHub Actions integration for CI/CD from the

The Function App is configured to run .NET 9 isolated worker.

#### Manually run Bicep file in Azure CLI 
To manually test the Bicep file and deploy the resources, following command can be executed in the Azure CLI - before the main.bicep file need to be uploaded to the cli so it can be accessed (if you are not running the cli locally)

``` sh
az deployment group create \
  --resource-group <your-resource-group> \
  --template-file main.bicep
```

### Deploy Azure Function with GitHub Actions 
ℹ️ Our RZL Azure Portal is not yet set up, so this only works with your personal subscription. 

When the required resources have been deployed with the Bicep file, GitHub Actions can be used to deploy any Azure Function to the prepared Function App. 
A yml file (workflow) was created that performs the deployment. 

#### Adjust yml file
1. When you want the action to be triggered on a feature branch, change the on -> push -> branch to your branch 
2. Adjust the app-name to your custom Function App if you renamed it in the Bicep file or want to use a different Function App 

#### GitHub Repository Secrets 
Create a new GitHub repository secret with the name "AZURE_CREDENTIALS". This is necessary for the Azure login and to be able to access the Function App. To get the secret value, enter following command in the Azure CLI: 

``` sh
az ad sp create-for-rbac --name "my-github-action-deployer" --role contributor \
    --scopes /subscriptions/<your-subscription-id>/resourceGroups/<your-resource-group> \
    --sdk-auth
```

This will output a JSON block like:

``` json
{
  "clientId": "...",
  "clientSecret": "...",
  "subscriptionId": "...",
  "tenantId": "...",
  "activeDirectoryEndpointUrl": "...",
  "resourceManagerEndpointUrl": "...",
  ...
}
```

Copy the full json block in the secret. 