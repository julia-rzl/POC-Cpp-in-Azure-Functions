@description('Name of the Azure Function App')
param functionAppName string = 'LohnAzureFunction'

@description('Deployment region')
param location string = 'westeurope'

@description('Unique name for the storage account (auto-generated)')
var storageAccountName = uniqueString(resourceGroup().id, 'lohnstorageaccount')

@description('Name of the Application Insights instance')
var appInsightsName = 'lohnapplicationinsights'

@description('Name of the App Service Plan')
var hostingPlanName = '${functionAppName}-plan'

//
// STORAGE ACCOUNT
//
resource storage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
  }
}

//
// APP SERVICE PLAN 
//
resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  kind: 'functionapp'
  properties: {
    reserved: false // Windows
  }
}

//
// APPLICATION INSIGHTS
//
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

//
// FUNCTION APP (Windows, GitHub Actions Deployment)
//
resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {
      use32BitWorkerProcess: false
      netFrameworkVersion: 'v9.0' 
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storage.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
      ]
    }
  }
}

//
// GITHUB ACTION DEPLOYMENT CONFIGURATION
//
resource sourceControl 'Microsoft.Web/sites/sourcecontrols@2022-03-01' = {
  name: 'web'
  parent: functionApp
  properties: {
    repoUrl: 'https://github.com/julia-rzl/POC-Cpp-in-Azure-Functions'
    branch: 'poc/cpp-in-azure-functions'
    isManualIntegration: false
    isGitHubAction: true
  }
}
