# Dapr Enabed Azure Container Apps

The solution has three projects,
* Web App: ASP.NET Core Blazor Server App
  * Public facing app
  * Dapr enabled
  * Service invocation done through Dapr
* Customers API: ASP.NET Core Minimal API
  * Internal: Not exposed to the outside world
  * Dapr enabled
* Orders API: ASP.NET Core Minimal API
  * Internal: Not exposed to the outside world
  * Dapr enabled


## Setup Resources (PowerShell)

```powershell
# Install the Azure Container Apps extension to the CLI
az extension add `
  --source https://workerappscliextension.blob.core.windows.net/azure-cli-extension/containerapp-0.2.0-py2.py3-none-any.whl 

# Variables
$RESOURCE_GROUP = '<resource-group>'
$LOCATION = "canadacentral"
$CONTAINERAPPS_ENVIRONMENT = '<container-app-environment-name>'
$LOG_ANALYTICS_WORKSPACE = "<log-analytics-workspace>"
$STORAGE_ACCOUNT = "<storage-account-name>"

$WEB_APP_NAME = "<web-app-name>"
$WEB_APP_SERVICE_ID = "<web-app-service-id>"

$CUSTOMERS_API_NAME = "<customers-api-name>"
$CUSTOMERS_API_SERVICE_ID = "<customers-api-service-id>"

$ORDERS_API_NAME = "<orders-api-name>"
$ORDERS_API_SERVICE_ID = "<orders-api-service-id>"

# Create a Log Analytics workspace
az monitor log-analytics workspace create `
    --resource-group $RESOURCE_GROUP `
    --workspace-name $LOG_ANALYTICS_WORKSPACE

$LOG_ANALYTICS_WORKSPACE_CLIENT_ID=(az monitor log-analytics workspace show --query customerId -g $RESOURCE_GROUP -n $LOG_ANALYTICS_WORKSPACE --out tsv)

$LOG_ANALYTICS_WORKSPACE_CLIENT_SECRET=(az monitor log-analytics workspace get-shared-keys --query primarySharedKey -g $RESOURCE_GROUP -n $LOG_ANALYTICS_WORKSPACE --out tsv)

# Create Azure Container Apps environment
az containerapp env create `
    --name $CONTAINERAPPS_ENVIRONMENT `
    --resource-group $RESOURCE_GROUP `
    --logs-workspace-id $LOG_ANALYTICS_WORKSPACE_CLIENT_ID `
    --logs-workspace-key $LOG_ANALYTICS_WORKSPACE_CLIENT_SECRET `
    --location "$LOCATION"

# Create Storage Account
az storage account create `
    --name $STORAGE_ACCOUNT `
    --resource-group $RESOURCE_GROUP `
    --location "$LOCATION" `
    --sku Standard_RAGRS `
    --kind StorageV2

$STORAGE_ACCOUNT_KEY=(az storage account keys list --resource-group $RESOURCE_GROUP --account-name $STORAGE_ACCOUNT --query '[0].value' --out tsv)
  
# Create Container App for Web App
az containerapp create `
    --name $WEB_APP_NAME `
    --resource-group $RESOURCE_GROUP `
    --environment $CONTAINERAPPS_ENVIRONMENT `
    --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest `
    --target-port 80 `
    --ingress 'external' `
    --min-replicas 1 `
    --max-replicas 1 `
    --enable-dapr `
    --dapr-app-port 80 `
    --dapr-app-id $WEB_APP_SERVICE_ID `
    --dapr-components ./dapr/components.yaml `
    --secrets storage-account-key=$STORAGE_ACCOUNT_KEY

# Create Container App for Customers API
az containerapp create `
    --name $CUSTOMERS_API_NAME `
    --resource-group $RESOURCE_GROUP `
    --environment $CONTAINERAPPS_ENVIRONMENT `
    --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest `
    --target-port 80 `
    --ingress 'internal' `
    --min-replicas 1 `
    --max-replicas 1 `
    --enable-dapr `
    --dapr-app-port 80 `
    --dapr-app-id $CUSTOMERS_API_SERVICE_ID `
    --dapr-components ./dapr/components.yaml `
    --secrets storage-account-key=$STORAGE_ACCOUNT_KEY

# Create Container App for Orders API
az containerapp create `
    --name $ORDERS_API_NAME `
    --resource-group $RESOURCE_GROUP `
    --environment $CONTAINERAPPS_ENVIRONMENT `
    --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest `
    --target-port 80 `
    --ingress 'internal' `
    --min-replicas 1 `
    --max-replicas 1 `
    --enable-dapr `
    --dapr-app-port 80 `
    --dapr-app-id $ORDERS_API_SERVICE_ID `
    --dapr-components ./dapr/components.yaml `
    --secrets storage-account-key=$STORAGE_ACCOUNT_KEY
```