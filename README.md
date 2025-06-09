# Sistema de Gestão de Clientes - CRUD com .NET e Redis

## 📝 Descrição do Projeto

Sistema de cadastro de clientes com operações CRUD (Create, Read, Update, Delete), desenvolvido em .NET 8 com arquitetura MVC, utilizando NHibernate para persistência e Redis para cache distribuído. O projeto inclui:

- Cadastro de clientes com telefones
- Validação de regras de negócio
- Cache inteligente com Redis
- Interface web responsiva

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **NHibernate** - ORM para mapeamento objeto-relacional
- **Redis** - Cache distribuído para melhor performance
- **SQL Server** - Banco de dados relacional

### Frontend
- HTML5 semântico
- CSS3 moderno (Flexbox, Grid)
- JavaScript vanilla (ES6+)
- Fetch API para comunicação assíncrona

### Infraestrutura
- Docker (para container do Redis)
- Visual Studio Code (ambiente de desenvolvimento)

## 🗂️ Estrutura do Projeto

```
ClienteCRUD/
├── src/
│   ├── ClienteCRUD.Core/       # Modelos de domínio
│   ├── ClienteCRUD.Infra/      # Infraestrutura (NHibernate, Redis)
│   └── ClienteCRUD.Web/        # Camada web (Controllers, Views)
│       └── wwwroot/            # Arquivos estáticos
│           ├── css/
│           ├── js/
│           └── index.html      # Página principal
├── docker-compose.yml          # Configuração do Redis
└── README.md                   # Este arquivo
```

## 🔧 Configuração do Ambiente

### Pré-requisitos
- .NET 8 SDK
- Docker Desktop (para Redis)
- SQL Server (local ou Docker)

### Passos para Execução

1. **Inicie o Redis**:
   ```bash
   docker-compose up -d
   ```

2. **Configure a conexão com o banco**:
   Edite `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "ClienteDB": "Server=.;Database=ClienteCRUD;Integrated Security=true;",
       "Redis": "localhost:6379"
     }
   }
   ```

3. **Rode o script bootstrap**:
   Copie de `/db/bootstrap.sql`:
   ```SQL
   CREATE DATABASE ClienteCrud;
   
   USE ClienteCrud;
   CREATE TABLE Cliente(
        Id         INT PRIMARY KEY IDENTITY
       ,DtCriacao  DATETIME DEFAULT GETDATE()
       ,Ativo      BIT DEFAULT 1
       ,Nome       NVARCHAR(100) NOT NULL
       ,Sexo       NVARCHAR(20)
       ,Endereco   NVARCHAR(200)
   );
   
   CREATE TABLE Telefone(
        Id         INT PRIMARY KEY IDENTITY
       ,DtCriacao  DATETIME DEFAULT GETDATE()
       ,Ativo      BIT DEFAULT 1
       ,Numero     NVARCHAR(20)
       ,ClienteId  INT
   );
   
   ALTER TABLE Telefone 
       ADD CONSTRAINT FK_Telefone_Cliente 
       FOREIGN KEY (ClienteId) REFERENCES Cliente(Id);
   ```

4. **Execute a aplicação**:
   ```bash
   dotnet run
   ```

5. **Acesse no navegador**:
   ```
   http://localhost:5000
   ```

## 📊 Funcionalidades Principais

### Cadastro de Clientes
- Nome (obrigatório)
- Sexo (Masculino/Feminino/Outro)
- Endereço completo
- Lista de telefones

### Gestão de Telefones
- Múltiplos telefones por cliente
- Apenas um telefone marcado como "ativo"
- Validação de formato

### Cache Inteligente
- Consultas primeiro no Redis
- Atualização automática do cache
- Invalidação em operações de escrita

## 🎯 Regras de Negócio Implementadas

1. **Validação de Cliente**:
   - Nome é obrigatório
   - Sexo deve ser uma das opções pré-definidas

2. **Gestão de Telefones**:
   - Um cliente pode ter 0 ou N telefones
   - Apenas um telefone pode estar ativo por cliente
   - Validação de formato do número

3. **Performance**:
   - Cache de consultas frequentes
   - Atualização assíncrona do cache

## 🚀 Rotas da API (Endpoints)

| Método | Rota                | Descrição                     |
|--------|---------------------|-------------------------------|
| GET    | /api/cliente        | Lista todos os clientes       |
| GET    | /api/cliente/{id}   | Obtém um cliente específico   |
| POST   | /api/cliente        | Cria um novo cliente          |
| PUT    | /api/cliente/{id}   | Atualiza um cliente           |
| DELETE | /api/cliente/{id}   | Remove um cliente             |
| GET    | /api/cliente/{id}/telefones | Lista telefones do cliente |

## 📌 Melhores Práticas Aplicadas

- **Injeção de Dependência**: Controllers e serviços devidamente injetados
- **Separação de Responsabilidades**: Arquitetura em camadas
- **Tratamento de Erros**: Middleware global para exceptions
- **Logging**: Registro de operações importantes
- **CORS**: Configuração segura para requisições entre origens

## 📈 Próximos Passos (Roadmap)

1. Conclusão de testes de caso do front-end;
2. Validação de endpoints do back-end

## 📄 Licença
**Cópia proibida;** Projeto para fins de portfólio. Todos os direitos são reservados.
``` 
