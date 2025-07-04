# Banco de Dados - Sistema de Reservas

## Visão Geral
O banco utiliza PostgreSQL e segue o modelo relacional, com suporte a multitenancy (EmpresaId em todas as entidades).

## Principais Tabelas

- **Empresa**: id, nome, cnpj, email, endereço, etc.
- **Filial**: id, nome, código, endereço, cidade, estado, cep, telefone, email, cnpj, responsável, ativo, dataAbertura, empresaId (FK)
- **Local**: id, label, subtitulo, nome, tipo, intervalo, valorHora, capacidade, descricao, comodidades, situacao, cor, horaAbertura, horaFechamento, filialId (FK), empresaId (FK)
- **Cliente**: id, label, subtitulo, nome, documento, email, telefone, endereco, observacoes, situacao, dataCadastro, empresaId (FK)
- **Reserva**: id, cliente, clienteId (FK), local, localId (FK), data, horaInicio, horaFim, situacao, cor, esporte, observacoes, valor, dataCadastro, empresaId (FK)
- **Recebivel**: id, cliente, clienteId (FK), descricao, valor, dataVencimento, situacao, reservaId (FK), dataCadastro, empresaId (FK)
- **Usuario**: id, nome, email, telefone, cargo, filialId (FK), grupoId, ativo, ultimoAcesso, foto, permissoesCustomizadas, dataCadastro, senhaHash, empresaId (FK)

## Relacionamentos
- Uma **Empresa** possui várias **Filiais**.
- Uma **Filial** possui vários **Locais** e **Usuários**.
- Um **Local** pode ter várias **Reservas**.
- Um **Cliente** pode ter várias **Reservas** e **Recebíveis**.
- Uma **Reserva** pode gerar vários **Recebíveis**.

## Multitenancy
- Todas as tabelas principais possuem o campo `empresaId` para garantir isolamento dos dados por tenant.
