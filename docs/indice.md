---
title: "Guia do Projeto (Índice)"
owner: "docs-owners"
status: "active"
last_review: "2025-10-02"
tags: [overview, onboarding, mvp, spa]
---

# Projeto — Sistema de Indicação (MVP) — **SPA**

**Stack**
- **Front (SPA)**: HTML + JavaScript (ES6 módulos) + CSS (sem frameworks de UI), roteamento por **hash** (`#/register`, `#/profile`)
- **API**: ASP.NET Core Web API (C#) + JWT
- **Banco**: MySQL 8 (EF Core)

> Objetivo: cadastro/login e perfil com **link de indicação** (`#/register?ref=CODE`) que **soma pontos** para quem indicou.

## 1) Documentos essenciais
- Requisitos: [`./requirements.md`](./requirements.md)
- Design Thinking (1-pager): [`./design-thinking.md`](./design-thinking.md)
- ADR (escolhas técnicas): [`./architecture/adr-0001-tech-choices.md`](./architecture/adr-0001-tech-choices.md)
- C4 (Contexto/Containers): [`./architecture/c4-context.md`](./architecture/c4-context.md)
- Exemplos de API (`.http`): [`./api/http-examples.http`](./api/http-examples.http)

## 2) Arquitetura em 5 minutos

**Fluxo alto nível**
 Usuário → Front SPA (HTML/JS/CSS) → HTTP JSON → API C# → SQL Server

- **Front SPA**: views renderizadas em runtime dentro de `#app`, rotas por hash.
- **API**: `POST /auth/register`, `POST /auth/login`, `GET /me`.
- **Dados**: `User(id, name, email*, passwordHash, refCode*, points, referredById?)`.

## 3) Dev Workflow (simples)
- Branches: `dev` → PR → `main`
- Commits: `feat(spa): add router`
- PR checklist: DoD verde

## 4) Setup rápido
- API: ver `appsettings.Development.json` (JWT + ConnectionString) e rodar `dotnet run`
- Front SPA: definir `API_BASE_URL` e servir `web/` com um server (Live Server ou `python -m http.server 5173`)

## 5) DoD (SPA)
- [ ] Rotas `#/register`, `#/profile` (guard de auth no profile)
- [ ] Validações no register (email, senha forte)
- [ ] Indicação via `?ref=CODE` (hash query) incrementa pontos
- [ ] Botão “Copiar link” em `/profile`
- [ ] Swagger ok + `.http` funcional
- [ ] CSS responsivo básico

