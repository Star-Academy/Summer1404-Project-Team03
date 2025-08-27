
### Dockerize
 - Backend [Dockerfile](./etl_backend/etl_backend/Dockerfile)


### Keycloak Config 
  - Env Vars
    - `Keycloak__AuthServerUrl`, `Keycloak__AuthorizationUrl` :
      host of these server urls should be the host of Keycloak service in containerized network
    - `Keycloak__AuthServerUrlPublic` this should be set to host of Keycalok that end-user can reach Keycalok pages
      (especially login page)
    - other parameters like `Keycloak__ClientSecret` should be configured based on Keycloak realm settings.


### Database

#### Database Connection
 - Env Vars
   - Database__ConnectionString=Host=`host in contaner network`;Port=`port in contaner network`;Database=etl;Username=`Username`;Password=`Passwrod`;

#### Database Migration 

### Environments
 - Env Vars
   - `ASPNETCORE_ENVIRONMENT` :
     - `Test` for test
     - `Production` for production
     - `Development` for development
 
 - Branches
   - `backend-develop` for development
   - `backend-test` for tests
   - `main` for production
