# Documentação da API - Sistema de Reservas

## Autenticação
- Todas as rotas (exceto login/registro) exigem JWT Bearer Token.
- O token deve conter o claim do tenant (EmpresaId).
- Exemplo de header:

```
Authorization: Bearer {seu_token_jwt}
```

---

## Endpoints Principais

### Locais
- `GET /api/Local` - Lista todos os locais
- `GET /api/Local/{id}` - Detalhe de um local
- `POST /api/Local` - Cria um local
- `PUT /api/Local/{id}` - Atualiza um local
- `DELETE /api/Local/{id}` - Remove um local

### Filiais
- `GET /api/Filial` - Lista todas as filiais
- `GET /api/Filial/{id}` - Detalhe de uma filial
- `POST /api/Filial` - Cria uma filial
- `PUT /api/Filial/{id}` - Atualiza uma filial
- `DELETE /api/Filial/{id}` - Remove uma filial

### Clientes
- `GET /api/Cliente` - Lista todos os clientes
- `GET /api/Cliente/{id}` - Detalhe de um cliente
- `POST /api/Cliente` - Cria um cliente
- `PUT /api/Cliente/{id}` - Atualiza um cliente
- `DELETE /api/Cliente/{id}` - Remove um cliente

### Recebíveis
- `GET /api/Recebivel` - Lista todos os recebíveis
- `GET /api/Recebivel/{id}` - Detalhe de um recebível
- `POST /api/Recebivel` - Cria um recebível
- `PUT /api/Recebivel/{id}` - Atualiza um recebível
- `DELETE /api/Recebivel/{id}` - Remove um recebível

### Reservas
- `GET /api/Reserva` - Lista todas as reservas
- `GET /api/Reserva/{id}` - Detalhe de uma reserva
- `POST /api/Reserva` - Cria uma reserva
- `PUT /api/Reserva/{id}` - Atualiza uma reserva
- `DELETE /api/Reserva/{id}` - Remove uma reserva

### Usuários
- `GET /api/Usuario` - Lista todos os usuários
- `GET /api/Usuario/{id}` - Detalhe de um usuário
- `POST /api/Usuario` - Cria um usuário
- `PUT /api/Usuario/{id}` - Atualiza um usuário
- `DELETE /api/Usuario/{id}` - Remove um usuário

---

## Observações
- Todos os endpoints CRUD respeitam o tenant do usuário autenticado (multitenancy).
- Para testar, gere um token JWT válido e inclua no header Authorization.
- Consulte o Swagger em `/swagger` para exemplos de payloads e schemas.
- Healthcheck disponível em `/health`.
- Rate limiting global: 100 requisições por minuto.
