param location string
param appServicePlanId string

resource webApplication 'Microsoft.Web/sites@2023-12-01' = {
  name: 'messenger-webapp'
  location: location
  properties: {
    serverFarmId: appServicePlanId
  }
}

param publisherName string
param publisherEmail string

resource apiManagement 'Microsoft.ApiManagement/service@2023-05-01-preview' = {
  name: 'messenger-apiservice'
  location: location
  sku: {
    name: 'Consumption'
    capacity: 0
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}

resource api 'Microsoft.ApiManagement/service/apis@2023-05-01-preview' = {
  parent: apiManagement
  name: 'rbmessenger-api'
  properties: {
    displayName: 'Messenger API'
    serviceUrl: 'https://rb-messenger.azurewebsites.net'
    path: 'messenger'
    protocols: [
      'https'
    ]
  }
}

output apiManagementId string = apiManagement.id
output apiId string = api.id

