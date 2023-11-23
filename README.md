
# Sistema Básico de Gerenciamento de Banco de Talentos (Desafio de processo seletivo)

Um sistema básico para fazer o gerenciamento de um banco de talentos, com autenticação e autorização por perfil, com opção de fazer o armazenamento do curriculo direto no banco de dados (só por ser um protótipo, o ideal seria salvar isso num Amazon S3 por exemplo), consiste em uma Aplicação Web MVC e uma Api, as páginas da aplicação Web são simples, geradas automaticamentes via Scaffold (era um requisito), porém com uma ou outra adição para tentar da um feedback melhor para o usuário, como um Toast notification por exemplo.

Tentei aplicar uma arquitetura próxima ao DDD, orientada a domínio, separada por camadas de Presentation, Application, Service, Domain, Infra, estou utilizando também alguns padrões de projetos, como injeção de dependência, unit of work, repositories.

## Stack utilizada

**Front-end:** Razor Pages, HTML5, CSS, JavaScript, JQuery

**Back-end:** C#, .Net Framework 4.8, Entity Framework, Identity, SendGrid

**Banco de dados:** SQL Server Local DB

## Algumas observações antes de rodar o projeto pela primeira vez

- Será necessário rodar as migrations para criar o banco e também os perfis de usuários, se necessário, trocar a connectionString no Web.Config das duas aplicações
 **<connectionStrings>
    <add name="DBContext" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DesafioThera;Integrated Security=True" providerName="System.Data.SqlClient" />
  </connectionStrings>**

- Caso queira utilizar o SendGrid, será necessário inserir a chave no arquivo Web.config das duas aplicações, dentro da seção de **<appSettings>**
  **<add key="mailApi" value="INSIRA A CHAVE DO SENDGRID" />**
  Precisará também mudar no arquivo MailService, dentro da camada de CrossCutting/Identity/Configuration o valor do from para corresponder ao seu sender do SendGrid.

## Documentação da Aplicação WEB MVC (Pasta DesafioThera)
**Tipo de Autenticação:** Cookie.

- O botão de **Registrar** no menu inicial não ta com nenhuma proteção de autorização, justamente para poder testar o sistema.
- O sistema WEB MVC está com Identity, utilizando Claims, além de perfis, os 3 perfis possuem suas respectivas claims que permitem acessar os dados após logado.
- O menu só vai mostrar abas relacionadas a entidade que o perfil logado pode pelo menos visualizar, os botões de Editar, Detalhes e Desativar só ficam disponíveis também de acordo com a permissão do perfil logado.
- A senha de cadastro é gerada pelo sistema e enviada por e-mail via SendGrid, mas eu coloquei umas viewbag para retornar ela na view pós cadastro, assim mesmo se der problema no envio do e-mail, será possível testar o sistema

## Documentação da API (Pasta DesafioApi)

**Tipo de Autenticação:** JWT Bearer.

#### Autenticar no sistema e receber o token

```http
  POST /api/users/login
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `Email` | `string` | **Obrigatório**. Email do usuário |
| `Password` | `string` | **Obrigatório**. Senha do usuário |

## Administradores ##

#### Obter todos os administradores cadastrados

```http
  GET /api/administrators
```
#### Obter administrador por ID

```http
  GET /api/administrators/{id}

```
#### Cadastrar novo administador

```http
  POST /api/administrators
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `Email`      | `string` | **Obrigatório**. Email do administrador |
| `Cpf`      | `string` | **Obrigatório**. Cpf do administrador |
| `Name`      | `string` | **Obrigatório**. Nome do administrador |
| `NickName`      | `string` | **Obrigatório**. O apelido que será exibido |
| `ProfileId`      | `int` | O ID do do perfil de administrador, que é 1 |

#### Editar administrador

```http
  PUT /api/administrators/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `Email`      | `string` | **Obrigatório**. Email do administrador |
| `Cpf`      | `string` | **Obrigatório**. Cpf do administrador |
| `Name`      | `string` | **Obrigatório**. Nome do administrador |
| `NickName`      | `string` | **Obrigatório**. O apelido que será exibido |
| `ProfileId`      | `int` | O ID do do perfil de administrador, que é 1 |

#### Desativar administrador

```http
  DELETE /api/administrators/{id}
```

## Secretárias(os) ##

#### Obter todas(os) as(os) secretárias(os) cadastradas(os) no sistema

```http
  GET /api/secretaries
```
#### Obter secretária(o) por ID

```http
  GET /api/secretaries/{id}

```
#### Cadastrar nova(o) secretária(o)

```http
  POST /api/secretaries
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `Email`      | `string` | **Obrigatório**. Email da(o) secretária(o) |
| `Cpf`      | `string` | **Obrigatório**. Cpf da(o) secretária(o) |
| `Name`      | `string` | **Obrigatório**. Nome da(o) secretária(o) |
| `NickName`      | `string` | **Obrigatório**. O apelido que será exibido |
| `ProfileId`      | `int` | O ID do do perfil de secretária(o), que é 2 |

#### Editar secretária(o)

```http
  PUT /api/secretaries/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `Email`      | `string` | **Obrigatório**. Email da(o) secretária(o) |
| `Cpf`      | `string` | **Obrigatório**. Cpf da(o) secretária(o) |
| `Name`      | `string` | **Obrigatório**. Nome da(o) secretária(o) |
| `NickName`      | `string` | **Obrigatório**. O apelido que será exibido |
| `ProfileId`      | `int` | O ID do do perfil de secretária(o), que é 2 |

#### Desativar secretária(o)

```http
  DELETE /api/secretaries/{id}
````

## Leitores(as) ##

#### Obter todos os leitores(as) cadastrados no sistema

```http
  GET /api/readers
```
#### Obter leitor(a) por ID

```http
  GET /api/readers/{id}
```
#### Cadastrar novo leitor(a)

```http
  POST /api/readers
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `Email`      | `string` | **Obrigatório**. Email do(a) leitor(a) |
| `Cpf`      | `string` | **Obrigatório**. Cpf do(a) leitor(a) |
| `Name`      | `string` | **Obrigatório**. Nome do(a) leitor(a) |
| `NickName`      | `string` | **Obrigatório**. O apelido que será exibido |
| `ProfileId`      | `int` | O ID do do perfil de leitor(a), que é 3 |

#### Editar leitor(a)

```http
  PUT /api/readers/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `Email`      | `string` | **Obrigatório**. Email do(a) leitor(a) |
| `Cpf`      | `string` | **Obrigatório**. Cpf do(a) leitor(a) |
| `Name`      | `string` | **Obrigatório**. Nome do(a) leitor(a) |
| `NickName`      | `string` | **Obrigatório**. O apelido que será exibido |
| `ProfileId`      | `int` | O ID do do perfil de leitor(a), que é 3 |

#### Desativar leitor(a)

```http
  DELETE /api/readers/{id}
```
## Talentos ##

#### Obter todos os talentos cadastrados no sistema

```http
  GET /api/talents
```
#### Obter talento por ID

```http
  GET /api/talents/{id}
```

#### Obter Curriculo por ID do Talento

```http
  GET /api/talents/{id}/resume
```

#### Cadastrar novo talento

```http
  POST /api/talents
```
##### **Content-Type:** multipart/form-data

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `FullName`      | `string` | **Obrigatório**. Nome do talento |
| `Email`      | `string` | **Obrigatório**. Email do talento |
| `Cpf`      | `string` | **Obrigatório**. Cpf do talento) |
| `Resume`      | `string` | **Obrigatório**. Curriculo do Talento(PDF,DOC,DOCX) |

#### Editar talento

```http
  PUT /api/talents/{id}
```
##### **Content-Type:** multipart/form-data

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `FullName`      | `string` | **Obrigatório**. Nome do talento |
| `Email`      | `string` | **Obrigatório**. Email do talento |
| `Cpf`      | `string` | **Obrigatório**. Cpf do talento) |
| `Resume`      | `string` | **Obrigatório**. Curriculo do Talento(PDF,DOC,DOCX) |

#### Desativar talento

```http
  DELETE /api/talents/{id}
```
