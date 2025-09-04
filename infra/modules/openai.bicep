// Module for Azure OpenAI Service deployment
// Creates an Azure OpenAI service with a GPT-4o-mini model deployment

@description('The name of the Azure OpenAI service')
param openAIName string

@description('The location for the OpenAI service')
param location string

@description('The name of the model deployment')
param deploymentName string

// Azure OpenAI Service
resource openAIAccount 'Microsoft.CognitiveServices/accounts@2024-10-01' = {
  name: openAIName
  location: location
  kind: 'OpenAI'
  sku: {
    name: 'S0'
  }
  properties: {
    customSubDomainName: openAIName
    publicNetworkAccess: 'Enabled'
    disableLocalAuth: true // Required by Microsoft security policy
  }
  tags: {
    Environment: 'Development'
    Service: 'OpenAI'
  }
}

// Model deployment for GPT-4o-mini
resource modelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
  parent: openAIAccount
  name: deploymentName
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-4o-mini'
      version: '2024-07-18'
    }
    versionUpgradeOption: 'OnceNewDefaultVersionAvailable'
    raiPolicyName: 'Microsoft.Default'
  }
  sku: {
    name: 'Standard'
    capacity: 50
  }
}

// Outputs
output openAIServiceName string = openAIAccount.name
output openAIEndpoint string = openAIAccount.properties.endpoint
output deploymentName string = modelDeployment.name
output openAIResourceId string = openAIAccount.id