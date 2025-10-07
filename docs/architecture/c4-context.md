title: "C4 — Contexto e Containers"
owner: "tech-lead"
status: "draft"
last_review: "2025-10-03"
---

## Contexto (C1)
Usuário final quer se cadastrar e convidar amigos por um link único que dá pontos ao indicador.

## Containers (C2)
- **Front (HTML/JS/CSS)**: páginas `/register` e `/profile`; chama API REST; guarda JWT
- **API (ASP.NET Core Web API)**: endpoints `register/login/me`; regra de negócio de indicação
- **DB (SQL Server)**: tabela `Users`

Fluxo:
[User] → [Front SPA] → (HTTP JSON) → [API C#] → [SQL Server]

## Componentes (visão leve)
- **AuthController**: `POST /auth/register`, `POST /auth/login`
- **UsersController** (ou rota `GET /me`)
- **UserService**: cadastro, login, geração de `refCode`, contagem de pontos
- **UserRepository/DbContext**: EF Core