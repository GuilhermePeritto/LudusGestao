# Clean Architecture - Sistema de Reservas

## Visão Geral
O projeto segue o padrão Clean Architecture, separando responsabilidades em camadas independentes para garantir manutenibilidade, testabilidade e escalabilidade.

## Camadas

- **Domain**: Regras de negócio, entidades, enums, interfaces. Não depende de nenhuma outra camada.
- **Application**: Serviços de aplicação, DTOs, validações, mapeamentos, lógica de orquestração. Depende apenas do Domain.
- **Infrastructure**: Implementação de repositórios, contexto de dados, integrações externas, middlewares de segurança. Depende apenas do Domain.
- **API**: Camada de apresentação (controllers), middlewares HTTP, autenticação, documentação Swagger. Depende de Application e Infrastructure.

## Fluxo de Dependências
```
API -> Application -> Domain
API -> Infrastructure -> Domain
```

## Padrões Utilizados
- Repository Pattern
- Unit of Work
- CQRS
- Specification Pattern
- Mediator Pattern
- Multitenancy (Tenant por Empresa)
- JWT Authentication

## Benefícios
- Baixo acoplamento entre camadas
- Fácil manutenção e testes
- Isolamento de regras de negócio
- Pronto para escalar e evoluir
