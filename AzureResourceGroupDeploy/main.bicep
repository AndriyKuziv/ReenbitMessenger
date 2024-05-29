param location string = deployment().location

targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'messenger-rg'
  location: location
}

module appServicePlan './app-service-plan.bicep' = {
  scope: resourceGroup
  name: 'messenger-app-plan'
  params:{
    location: resourceGroup.location
  }
}

module appService './web-app.bicep' = {
  scope: resourceGroup
  name: 'messenger-webapp'
  params:{
    location: resourceGroup.location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    publisherName: 'AndriiKuziv'
    publisherEmail: 'andrii.kuziv.kn.2021@edu.lpnu.ua'
  }
}

module storage './storage.bicep' = {
  scope: resourceGroup
  name: 'messenger-storage'
  params:{
    location: resourceGroup.location
  }
}
