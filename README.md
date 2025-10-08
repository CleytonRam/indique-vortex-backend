# 🚀 Indique Vortex - Backend (API)

> Sistema de indicação (referral system) desenvolvido com ASP.NET Core Web API + Entity Framework Core + SQL Server

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-Express-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

---

##  Sobre o Projeto

API REST para gerenciar sistema de cadastro de usuários com **pontuação por indicação**. Quando um usuário se cadastra usando o link de indicação de outro usuário, o indicador ganha **1 ponto**.

###  Funcionalidades

-  Cadastro de usuários com validação
-  Autenticação JWT
-  Sistema de indicação por link único (`refCode`)
-  Pontuação automática para indicadores
-  Perfil do usuário com estatísticas
-  Hash de senhas com BCrypt

---

##  Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web moderno e performático
- **Entity Framework Core 9.0** - ORM para acesso ao banco de dados
- **SQL Server Express** - Banco de dados relacional
- **BCrypt.Net** - Hash seguro de senhas
- **JWT Bearer** - Autenticação stateless

### Ferramentas
- **Swagger/OpenAPI** - Documentação interativa da API
- **Visual Studio 2022** - IDE de desenvolvimento

### Por que essas tecnologias?

**ASP.NET Core + SQL Server:**  
Escolhi o ecossistema Microsoft pela integração nativa entre as ferramentas, facilitando o desenvolvimento e deploy. O .NET 8 oferece performance excepcional e o Entity Framework Core simplifica o acesso aos dados com migrations e LINQ.

**JWT + BCrypt:**  
Implementação de autenticação moderna e segura. JWT permite escalabilidade (stateless) e BCrypt garante hash seguro de senhas com salt automático.

---

##  Estrutura do Projeto

```
indique-vortex-backend/
├── api/
│   ├── Controllers/          # Endpoints da API
│   │   ├── AuthController.cs      # Register, Login
│   │   └── UsersController.cs     # Profile (/me)
│   ├── Services/             # Lógica de negócio
│   │   ├── UserService.cs         # CRUD e regras de indicação
│   │   └── JwtService.cs          # Geração/validação de tokens
│   ├── Models/               # Entidades do banco
│   │   └── User.cs                # Modelo de usuário
│   ├── DTOs/                 # Objetos de transferência
│   ├── Data/                 # Contexto EF Core + Migrations
│   │   ├── AppDbContext.cs
│   │   └── Seed/SeedData.cs       # Dados iniciais
│   ├── Migrations/           # Migrations do banco
│   ├── appsettings.json      # Configurações (JWT, ConnectionString)
│   └── Program.cs            # Configuração da aplicação
├── docs/                     # Documentação do projeto
└── README.md
```

---

##  Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server Developer
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (opcional, mas recomendado)
- [Git](https://git-scm.com/)

---

##  Como Executar o Projeto

### 1️ Clone o repositório

```bash
git clone https://github.com/CleytonRam/indique-vortex-backend.git
cd indique-vortex-backend/api
```

### 2️ Configure o banco de dados

O arquivo `appsettings.json` já vem configurado para SQL Server local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ReferralDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

> **Nota:** Se necessário, ajuste a connection string para o seu ambiente.

### 3️ Rode as migrations

O Entity Framework Core criará o banco automaticamente ao iniciar a aplicação. Se preferir fazer manualmente:

```bash
dotnet ef database update
```

### 4️ Execute a aplicação

```bash
dotnet run
```

A API estará rodando em:
- 🌐 HTTP: `http://localhost:5172`
- 🔒 HTTPS: `https://localhost:7203`
- 📖 Swagger: `https://localhost:7203/swagger`

### 5️ Seed de Dados (Opcional)

O projeto inclui um seed automático que cria **5 usuários de teste** na primeira execução:

| Email | Senha | Pontos |
|-------|-------|--------|
| ana.silva@email.com | Senha1234 | Aleatório (0-10) |
| bruno.oliveira@email.com | Senha1234 | Aleatório (0-10) |
| carla.santos@email.com | Senha1234 | Aleatório (0-10) |
| diego.souza@email.com | Senha1234 | Aleatório (0-10) |
| elena.costa@email.com | Senha1234 | Aleatório (0-10) |

---

## 📡 Endpoints da API

### 🔓 Públicos (Sem autenticação)

#### **POST** `/api/auth/register`
Cadastra um novo usuário. Opcionalmente aceita um código de indicação.

**Query Params (opcional):**
- `refCode` - Código de indicação de 8 caracteres

**Body:**
```json
{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "Senha1234"
}
```

**Response (201):**
```json
{
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@email.com",
    "refCode": "A1B2C3D4",
    "points": 0,
    "refLink": "http://localhost:3000/register?ref=A1B2C3D4",
    "referredByName": null
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

#### **POST** `/api/auth/login`
Autentica um usuário existente.

**Body:**
```json
{
  "email": "joao@email.com",
  "password": "Senha1234"
}
```

**Response (200):**
```json
{
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@email.com",
    "refCode": "A1B2C3D4",
    "points": 5,
    "refLink": "http://localhost:3000/register?ref=A1B2C3D4",
    "referredByName": null
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

###  Protegidos (Requer JWT)

#### **GET** `/api/users/me`
Retorna o perfil do usuário autenticado.

**Headers:**
```
Authorization: Bearer {seu_token_jwt}
```

**Response (200):**
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao@email.com",
  "refCode": "A1B2C3D4",
  "points": 5,
  "refLink": "http://localhost:3000/register?ref=A1B2C3D4",
  "referredByName": "Maria Santos"
}
```

---

##  Testando a API

### Swagger UI
Acesse `https://localhost:7203/swagger` para testar os endpoints interativamente.

<!-- Adicione aqui uma screenshot do Swagger -->
![Swagger Screenshot](docs/screenshots/swagger.png)

### Arquivo .http (VS Code REST Client)
O projeto inclui exemplos no arquivo `docs/api/http-examples.md`:

```http
### Registro
POST http://localhost:5172/api/auth/register
Content-Type: application/json

{
  "name": "Teste User",
  "email": "teste@email.com",
  "password": "Senha1234"
}

### Login
POST http://localhost:5172/api/auth/login
Content-Type: application/json

{
  "email": "teste@email.com",
  "password": "Senha1234"
}
```

---

##  Fluxo de Indicação

1. **Usuário A** se cadastra e recebe um `refCode` único (ex: `A1B2C3D4`)
2. **Usuário A** compartilha seu link: `http://localhost:3000/register?ref=A1B2C3D4`
3. **Usuário B** acessa o link e se cadastra
4. **Usuário A** ganha **+1 ponto** automaticamente
5. No perfil de **Usuário B** aparece: `"referredByName": "Usuário A"`

---

##  Segurança

- ✅ **Senhas hasheadas** com BCrypt (salt automático)
- ✅ **JWT com expiração** de 2 horas
- ✅ **Validação de issuer e audience** nos tokens
- ✅ **CORS configurado** para aceitar apenas frontend autorizado
- ✅ **Validações server-side** em todos os inputs
- ✅ **HTTPS habilitado** em produção

---

##  Colaboração com IA

Durante o desenvolvimento, utilizei o **DeepSeek** como ferramenta de apoio para:

### Como utilizei:
- Esclarecer dúvidas sobre recursos específicos do **C# e ASP.NET Core**
- Validar escolhas arquiteturais e entender diferentes abordagens
- Compreender melhores práticas de segurança e estruturação de código
- Debugar erros específicos do Entity Framework Core

### Abordagem:
Minha metodologia foi **focar no entendimento prático** através de prompts direcionados, evitando simplesmente copiar código. Sempre busquei entender o *porquê* de cada solução antes de implementar.

### Principais aprendizados:
- **Arquitetura em camadas:** Separação clara entre Controllers (apresentação), Services (lógica de negócio) e Data (persistência)
- **Repository Pattern:** Embora não implementado completamente no MVP, entendi os benefícios de abstrair o acesso a dados
- **Service Layer:** Importância de manter controllers enxutos e delegar lógica de negócio para services
- **Segurança JWT:** Configuração correta de `ValidateIssuer`, `ValidateAudience` e `ClockSkew`
- **Boas práticas EF Core:** Uso de migrations, indexes únicos e relacionamentos `OnDelete(Restrict)`

A IA foi fundamental para **acelerar meu aprendizado** em conceitos que eu não dominava completamente, mas todo o código foi escrito e compreendido por mim.

---

##  Repositórios Relacionados

- **Frontend (SPA):** [indique-vortex-frontend](https://github.com/CleytonRam/indique-vortex-frontend)

---

##  Autor

**Cleyton Glauco Ramsay Zaina Castrillon**

- GitHub: [@CleytonRam](https://github.com/CleytonRam)
- Email: cleytonglauco@gmail.com
- LinkedIn: [Seu LinkedIn](https://www.linkedin.com/in/cleyton-ramsay-8637b525b/)

---

##  Licença

Este projeto foi desenvolvido como parte do processo seletivo para estágio no **Laboratório Vortex - UNIFOR**.

---

##  Agradecimentos

Agradeço ao **Laboratório Vortex** pela oportunidade de participar deste processo seletivo desafiador e enriquecedor! 🚀