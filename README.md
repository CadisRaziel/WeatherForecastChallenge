## Instruções de Configuração

### 1. Clone o Repositório

```bash
git clone <URL_DO_REPOSITORIO>
cd <NOME_DO_REPOSITORIO>
```

### 2. Container docker
docker run --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=rhythms" -p 1433:1433 -d mcr.microsoft.com/mssql/server

### 3. Configure a String de Conexão
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NomeDoBanco;User Id=sa;Password=rhythms;TrustServerCertificate=True;"
  }
}

```

### 4. Rodar as migrations
- cd src/Infrastructure
``` bash
dotnet ef database update
```
