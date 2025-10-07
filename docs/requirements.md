---
title: "Requisitos — Sistema de Indicação (MVP)"
owner: "product-engineering"
status: "draft"
last_review: "2025-10-03"
---

## 1. Escopo
Criar uma **SPA simples** (HTML/JS/CSS) integrada a uma **API ASP.NET Core Web API** (C# + EF Core + **SQL Server**) para **cadastro, login** e **perfil** com **pontuação por indicação** via link único.

## 2. Funcionais
### F1. Cadastro de usuário
- Campos: `name`, `email`, `password`
- Gera `refCode` único ao criar usuário (ex.: `8 char alfanumérico`)
- Se cadastro vier com `?ref=CODE` válido → incrementar `points` do indicador
**Aceitação**
- E-mail inválido/duplicado bloqueia cadastro (mensagem clara)
- Senha ≥ 8 caracteres com letras e números
- Sucesso → redireciona para `/profile`

### F2. Login
- Autenticação por `email + password`
- Retorna **JWT** e dados básicos
**Aceitação**
- Credenciais inválidas → 401 com mensagem amigável
- Token armazenado (localStorage) e usado nas chamadas autenticadas

### F3. Perfil
- Exibir `name`, `points`, `refLink = {ORIGIN}/register?ref={refCode}`
- Botão **Copiar Link**
**Aceitação**
- Copiar link dá feedback visual
- `points` corresponde ao persistido após recarregar

### F4. Responsividade
- Layout adaptado 360–1440px
**Aceitação**
- Sem overflow horizontal; inputs e botões acessíveis em mobile

## 3. Não-funcionais
- **API**: .NET 8 + EF Core + **SQL Server**; **bcrypt** para hash; **JWT**
- **Front**: HTML/JS/CSS puro (sem frameworks de UI)
- **Segurança**: validação server-side; CORS habilitado para o front
- **DevX**: Swagger habilitado; arquivo `.http` para testes

## 4. Modelo de dados (mínimo)
`User(id, name, email*, passwordHash, refCode*, points:int=0, referredById?:id)`

## 5. Endpoints
- `POST /auth/register { name, email, password }` (opcional `?ref=CODE`)
- `POST /auth/login { email, password }`
- `GET /me` (Bearer JWT)

## 6. Métricas de sucesso
- Taxa de conclusão de cadastro
- Nº de indicações válidas por usuário
- Zero frameworks de UI/CSS utilizados

## 7. Fora de escopo (MVP)
- Recuperação de senha, refresh token, perfis de admin, ranking público