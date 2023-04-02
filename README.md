# oauth2-azure-starter-webapi

## Secrets
==secret.json== file
```
{
  "AzureAD": {
    "Instance": "https://login.microsoftonline.com",
    "TenantId": "{your-tenantid-here}",
    "ClientId": "{your-clientid-here}",
  }
}
```

## Building the docker image
```
docker build --rm -t manifesting-art/webapi:latest .
docker image ls
```

## Run the docker container
```
docker run --rm -p 5000:80 -p 5001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=44366 manifesting-art/webapi
```