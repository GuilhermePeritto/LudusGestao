# Value Objects - Ludus Gestão

## Visão Geral

Value Objects são objetos que representam conceitos do domínio e possuem validação e comportamento específicos. No sistema Ludus Gestão, eles garantem a integridade dos dados e encapsulam regras de negócio.

## Value Objects Implementados

### 1. Documento

Representa documentos brasileiros (CPF e CNPJ) com validação completa.

#### Características
- **Validação**: Algoritmo oficial de validação de CPF e CNPJ
- **Formatação**: Formatação automática (XXX.XXX.XXX-XX para CPF, XX.XXX.XXX/XXXX-XX para CNPJ)
- **Detecção**: Identifica automaticamente se é CPF ou CNPJ
- **Limpeza**: Remove caracteres especiais automaticamente

#### Uso
```csharp
// CPF válido
var documento = new Documento("123.456.789-09");
Console.WriteLine(documento.Tipo); // TipoDocumento.CPF
Console.WriteLine(documento.ToString()); // "123.456.789-09"

// CNPJ válido
var documento = new Documento("11.222.333/0001-81");
Console.WriteLine(documento.Tipo); // TipoDocumento.CNPJ
Console.WriteLine(documento.ToString()); // "11.222.333/0001-81"

// Exceção para documento inválido
var documento = new Documento("123.456.789-10"); // CPF inválido
// ArgumentException: "CPF inválido"
```

#### Validação de CPF
1. Verifica se tem 11 dígitos
2. Verifica se não são todos iguais
3. Calcula primeiro dígito verificador
4. Calcula segundo dígito verificador
5. Compara com os dígitos fornecidos

#### Validação de CNPJ
1. Verifica se tem 14 dígitos
2. Verifica se não são todos iguais
3. Calcula primeiro dígito verificador
4. Calcula segundo dígito verificador
5. Compara com os dígitos fornecidos

### 2. Email

Representa endereços de email com validação de formato.

#### Características
- **Validação**: Regex robusta para formato de email
- **Normalização**: Converte para lowercase
- **Limpeza**: Remove espaços em branco

#### Uso
```csharp
// Email válido
var email = new Email("usuario@exemplo.com");
Console.WriteLine(email.Endereco); // "usuario@exemplo.com"

// Email inválido
var email = new Email("email-invalido");
// ArgumentException: "E-mail inválido"
```

#### Regex de Validação
```regex
^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$
```

### 3. Telefone

Representa números de telefone brasileiros.

#### Características
- **Validação**: Suporte a fixo (10 dígitos) e celular (11 dígitos)
- **Formatação**: Formatação automática brasileira
- **Detecção**: Identifica automaticamente o tipo
- **Limpeza**: Remove caracteres especiais

#### Uso
```csharp
// Telefone fixo
var telefone = new Telefone("(11) 3333-4444");
Console.WriteLine(telefone.Tipo); // TipoTelefone.Fixo
Console.WriteLine(telefone.ToString()); // "(11) 3333-4444"

// Celular
var telefone = new Telefone("(11) 99999-8888");
Console.WriteLine(telefone.Tipo); // TipoTelefone.Celular
Console.WriteLine(telefone.ToString()); // "(11) 99999-8888"

// Telefone inválido
var telefone = new Telefone("123");
// ArgumentException: "Telefone inválido"
```

#### Regras de Validação
- **Fixo**: 10 dígitos (DDD + número)
- **Celular**: 11 dígitos (DDD + 9 + número)
- **Detecção**: Celular se começar com 9 no 3º dígito

## Implementação nas Entidades

### Exemplo de Uso
```csharp
public class Cliente : BaseEntity, ITenantEntity
{
    public string Documento { get; set; } // Armazena como string, mas pode ser validado
    public string Email { get; set; }     // Armazena como string, mas pode ser validado
    public string Telefone { get; set; }  // Armazena como string, mas pode ser validado
    
    // Método para validar documento
    public void ValidarDocumento()
    {
        if (!string.IsNullOrEmpty(Documento))
        {
            var documento = new Documento(Documento);
            Documento = documento.Numero; // Armazena apenas números
        }
    }
    
    // Método para validar email
    public void ValidarEmail()
    {
        if (!string.IsNullOrEmpty(Email))
        {
            var email = new Email(Email);
            Email = email.Endereco; // Armazena normalizado
        }
    }
    
    // Método para validar telefone
    public void ValidarTelefone()
    {
        if (!string.IsNullOrEmpty(Telefone))
        {
            var telefone = new Telefone(Telefone);
            Telefone = telefone.Numero; // Armazena apenas números
        }
    }
}
```

## Validação nos DTOs

### Exemplo de Validador
```csharp
public class CreateClienteValidator : AbstractValidator<CreateClienteDTO>
{
    public CreateClienteValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty()
            .Must(BeValidDocument)
            .WithMessage("Documento inválido");
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(BeValidEmail)
            .WithMessage("Email inválido");
            
        RuleFor(x => x.Telefone)
            .NotEmpty()
            .Must(BeValidPhone)
            .WithMessage("Telefone inválido");
    }
    
    private bool BeValidDocument(string documento)
    {
        try
        {
            new Documento(documento);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private bool BeValidEmail(string email)
    {
        try
        {
            new Email(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private bool BeValidPhone(string telefone)
    {
        try
        {
            new Telefone(telefone);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## Benefícios

1. **Integridade**: Garante que apenas dados válidos sejam aceitos
2. **Reutilização**: Lógica de validação centralizada
3. **Manutenibilidade**: Mudanças na validação em um só lugar
4. **Testabilidade**: Fácil de testar isoladamente
5. **Documentação**: Código auto-documentado

## Extensibilidade

Para adicionar novos Value Objects:

1. Criar a classe no namespace `LudusGestao.Domain.ValueObjects`
2. Implementar validação no construtor
3. Implementar `ToString()` se necessário
4. Adicionar testes unitários
5. Atualizar documentação 