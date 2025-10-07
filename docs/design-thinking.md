---
title: "Design Thinking — 1-pager"
owner: "product-engineering"
status: "draft"
last_review: "2025-10-02"
---

## Nota
A solução é **SPA** com rotas `#/register` e `#/profile`, evitando recargas de página.

## Problema
Facilitar cadastro e indicações por link (ref) com visualização de pontos.

## Personas
- Indicador (Ana, 22) — compartilhar link e acompanhar pontos
- Indicado (Bruno, 23) — cadastro rápido partindo do link

## Jornada
1. Ana copia link no `#/profile`
2. Bruno abre `#/register?ref=XXXX`, preenche, envia
3. API cria conta (conta ponto p/ Ana se `ref` válido)
4. Ana vê pontos em `#/profile` (reload ok)

## Riscos
Ref inválido; e-mail duplicado; senha fraca

## Métricas
% cadastros; indicações/usuário
