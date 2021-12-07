$RESOURCE_GROUP = 'rg-container-apps-demo'
$LOCATION = "canadacentral"
$CONTAINERAPPS_ENVIRONMENT = 'env-container-apps-demo'
$LOG_ANALYTICS_WORKSPACE = "law-container-apps-demo"
$STORAGE_ACCOUNT = "stcontainerappsdemo"
$REGISTRY_LOGIN_SERVER = "ravana.azurecr.io"
$REGISTRY_USERNAME = "ravana"
$REGISTRY_PASSWORD = "EtPFbeN0l1rAc9z0QbHurrNs5cNYE/ef"

$WEB_APP_NAME = "capp-web-blazor-demo"
$WEB_APP_IMAGE = "ravana.azurecr.io/containers-apps-demo/web/blazor:latest"
$WEB_APP_ID = "web-blazor"

$CUSTOMERS_API_NAME = "capp-customers-api-demo"
$CUSTOMERS_API_IMAGE = "ravana.azurecr.io/containers-apps-demo/customers/api:latest"
$CUSTOMERS_API_ID = "customers-api"

$ORDERS_API_NAME = "capp-orders-api-demo"
$ORDERS_API_IMAGE = "ravana.azurecr.io/containers-apps-demo/orders/api:latest"
$ORDERS_API_ID = "orders-api"

$STORAGE_ACCOUNT_KEY=(az storage account keys list --resource-group $RESOURCE_GROUP --account-name $STORAGE_ACCOUNT --query '[0].value' --out tsv)

az monitor log-analytics workspace create `
    --resource-group $RESOURCE_GROUP `
    --workspace-name $LOG_ANALYTICS_WORKSPACE

$LOG_ANALYTICS_WORKSPACE_CLIENT_ID=(az monitor log-analytics workspace show --query customerId -g $RESOURCE_GROUP -n $LOG_ANALYTICS_WORKSPACE --out tsv)

$LOG_ANALYTICS_WORKSPACE_CLIENT_SECRET=(az monitor log-analytics workspace get-shared-keys --query primarySharedKey -g $RESOURCE_GROUP -n $LOG_ANALYTICS_WORKSPACE --out tsv)

az containerapp env create `
    --name $CONTAINERAPPS_ENVIRONMENT `
    --resource-group $RESOURCE_GROUP `
    --logs-workspace-id $LOG_ANALYTICS_WORKSPACE_CLIENT_ID `
    --logs-workspace-key $LOG_ANALYTICS_WORKSPACE_CLIENT_SECRET `
    --location "$LOCATION"

az storage account create `
    --name $STORAGE_ACCOUNT `
    --resource-group $RESOURCE_GROUP `
    --location "$LOCATION" `
    --sku Standard_RAGRS `
    --kind StorageV2
  
az containerapp create `
    --name $WEB_APP_NAME `
    --resource-group $RESOURCE_GROUP `
    --environment $CONTAINERAPPS_ENVIRONMENT `
    --image $WEB_APP_IMAGE `
    --registry-login-server $REGISTRY_LOGIN_SERVER `
    --registry-username $REGISTRY_USERNAME `
    --registry-password $REGISTRY_PASSWORD `
    --target-port 80 `
    --ingress 'external' `
    --min-replicas 1 `
    --max-replicas 1 `
    --enable-dapr `
    --dapr-app-port 80 `
    --dapr-app-id $WEB_APP_ID `
    --dapr-components ./dapr/components.yaml `
    --secrets storage-account-key=$STORAGE_ACCOUNT_KEY

az containerapp create `
    --name $CUSTOMERS_API_NAME `
    --resource-group $RESOURCE_GROUP `
    --environment $CONTAINERAPPS_ENVIRONMENT `
    --image $CUSTOMERS_API_IMAGE `
    --registry-login-server $REGISTRY_LOGIN_SERVER `
    --registry-username $REGISTRY_USERNAME `
    --registry-password $REGISTRY_PASSWORD `
    --target-port 80 `
    --ingress 'internal' `
    --min-replicas 1 `
    --max-replicas 1 `
    --enable-dapr `
    --dapr-app-port 80 `
    --dapr-app-id $CUSTOMERS_API_ID `
    --dapr-components ./dapr/components.yaml `
    --secrets storage-account-key=$STORAGE_ACCOUNT_KEY

az containerapp create `
    --name $ORDERS_API_NAME `
    --resource-group $RESOURCE_GROUP `
    --environment $CONTAINERAPPS_ENVIRONMENT `
    --image $ORDERS_API_IMAGE `
    --registry-login-server $REGISTRY_LOGIN_SERVER `
    --registry-username $REGISTRY_USERNAME `
    --registry-password $REGISTRY_PASSWORD `
    --target-port 80 `
    --ingress 'internal' `
    --min-replicas 1 `
    --max-replicas 1 `
    --enable-dapr `
    --dapr-app-port 80 `
    --dapr-app-id $ORDERS_API_ID `
    --dapr-components ./dapr/components.yaml `
    --secrets storage-account-key=$STORAGE_ACCOUNT_KEY

az containerapp update `
    --name $WEB_APP_NAME `
    --resource-group $RESOURCE_GROUP `
    --image $WEB_APP_IMAGE `
    --registry-login-server $REGISTRY_LOGIN_SERVER `
    --registry-username $REGISTRY_USERNAME `
    --registry-password $REGISTRY_PASSWORD

az monitor log-analytics query `
    --workspace $LOG_ANALYTICS_WORKSPACE_CLIENT_ID `
    --analytics-query "ContainerAppConsoleLogs_CL | where ContainerAppName_s == 'capp-web-blazor-demo' | project ContainerAppName_s, Log_s, TimeGenerated | take 5" `
    --out table

dapr run --app-id myapp --app-port 5000 -- dotnet run --components-path ..\dapr\components.yaml