# oauth2-azure-starter-webapi

## Building the docker image
```
docker build --rm -t manifesting-art/webapi:latest .
docker image ls
```

## Run the docker container
```
docker run --rm -p 8080:80 -p 44366:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=44366 manifesting-art/webapi
```