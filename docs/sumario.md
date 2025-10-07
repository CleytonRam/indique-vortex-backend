---
title: "Sumário de Documentação"
owner: "docs-owners"
status: "active"
last_review: "2025-10-03"
tags: [toc, docs]
---

# Sumário

## 1. Visão Geral
- [Guia do Projeto (Índice)](./indice.md)
- [Requisitos — Sistema de Indicação](./requirements.md)
- [Design Thinking (1-pager)](./design-thinking.md)

## 2. Arquitetura
- [ADR-0001 — Escolhas Técnicas](./architecture/adr-0001-tech-choices.md)
- [C4 — Contexto/Containers](./architecture/c4-context.md)

## 3. API e Contratos
- Swagger (runtime): `/swagger`
- Exemplo de chamadas: [`./api/http-examples.http`](./api/http-examples.http)

## 4. Implementação
- **API (C#)** — ASP.NET Core Web API + EF Core + SQL Server
  Entidade: `User(id, name, email*, passwordHash, refCode*, points, referredById?)`
- **Front (HTML/JS/CSS)** — Páginas `/register` e `/profile`; `API_BASE_URL` centralizado no JS

## 5. Qualidade e Entrega
- **DoD**: ver [Índice](./indice.md#6-definição-de-pronto-dod)
- **Testes manuais**: Swagger e arquivo `.http`
- **Checklist de PR**
  - [ ] Requisitos atendidos e validados
  - [ ] Swagger on
  - [ ] Front responsivo sem frameworks
  - [ ] README atualizado