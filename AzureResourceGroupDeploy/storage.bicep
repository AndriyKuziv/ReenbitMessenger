param location string = resourceGroup().location

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-04-01' = {
  kind: 'BlobStorage'
  location: location
  name: 'messengerstorageaccount'
  sku:{
    name:'Standard_LRS'
  }
  properties:{
    accessTier: 'Hot'
    allowBlobPublicAccess: true
  }
}

resource blobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-04-01' = {
  name: '${storageAccount.name}/default/messenger-blobcontainer'
  properties: {
    publicAccess: 'Blob'
  }
}
