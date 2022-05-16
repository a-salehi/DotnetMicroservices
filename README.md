## Microservices Architecture and Implementation on .NET 6

![alt text](https://user-images.githubusercontent.com/1147445/110304529-c5b70180-800c-11eb-832b-a2751b5bda76.png)

## Securing Microservices with IdentityServer4, OAuth2 and OpenID Connect fronted by Ocelot API Gateway

![alt text](https://user-images.githubusercontent.com/78356597/168614439-fbb9a83c-fcdc-4b76-9925-9a8cfd9d699b.png)

### Run The Project

You will need the following tools:

- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)

- [.Net Core 6 or later](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)


#### Installing

Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)

1. Clone the repository

2. Once Docker for Windows is installed, go to the Settings > Advanced option, from the Docker icon in the system tray, to configure the minimum amount of memory and CPU like so: (Or in the file path C:\Users\<UserName>\.wslconfig)

- Memory: 4 GB

- CPU: 2

3. At the root directory which include docker-compose.yml files, run below command:

````
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
````

