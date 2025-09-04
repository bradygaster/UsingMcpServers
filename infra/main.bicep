// Azure OpenAI Resource Group and Service Deployment
// This template creates a new resource group and deploys an Azure OpenAI service with a GPT-4o-mini model deployment

targetScope = 'subscription'

@description('The name of the resource group')
param resourceGroupName string = 'rg-openai-${uniqueString(subscription().id)}'

@description('The location for the resource group and all resources')
param location string = 'eastus'

@description('The name of the Azure OpenAI service')
param openAIName string = 'openai-${uniqueString(resourceGroupName)}'

@description('The name of the model deployment')
param deploymentName string = 'gpt-4o-mini'

// Create the resource group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
  tags: {
    Environment: 'Development'
    Purpose: 'OpenAI Demo'
  }
}

// Deploy the OpenAI service using a module
module openAI 'modules/openai.bicep' = {
  name: 'openai-deployment'
  scope: resourceGroup
  params: {
    openAIName: openAIName
    location: location
    deploymentName: deploymentName
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output openAIServiceName string = openAI.outputs.openAIServiceName
output openAIEndpoint string = openAI.outputs.openAIEndpoint
output deploymentName string = openAI.outputs.deploymentName