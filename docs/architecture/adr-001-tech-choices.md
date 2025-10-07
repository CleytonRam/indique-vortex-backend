---
title: "ADR-0001 — Escolhas Técnicas"
owner: "tech-lead"
status: "draft"
last_review: "2025-10-03"
---

## Decisão
- **API**: ASP.NET Core Web API (.NET 8) pela rapidez, tooling e Swagger nativo
- **Persistência**: **SQL Server** (SQL Server 2022 local) com **EF Core** (migrations)
- **Auth**: JWT + hash de senha com bcrypt
- **Front**: HTML + JS + CSS (sem frameworks)

## Alternativas consideradas
- MySQL 8 (familiaridade prévia, porém padronizamos no ecossistema Microsoft)
- ASP.NET Identity (mais robusto que o necessário para o MVP)
- Frameworks CSS (evitar para cumprir requisito de simplicidade)

## Consequências
- Entrega rápida mantendo EF Core
- Migrations estáveis via `Microsoft.EntityFrameworkCore.SqlServer`