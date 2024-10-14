## Estrutura das Tabelas do Projeto GloboClima

O projeto utiliza duas tabelas principais no DynamoDB:

### 1. Tabela `Favorites`

| Atributo    | Tipo   | Observações            |
|-------------|--------|------------------------|
| Id          | String | Chave de Partição (PK) |
| City        | String | Nome da cidade         |
| Country     | String | Nome do país           |
| UserId      | String | ID do usuário          |

### 2. Tabela `Users`

| Atributo      | Tipo   | Observações                    |
|---------------|--------|--------------------------------|
| Username      | String | Chave de Partição (PK)         |
| Email         | String | Email do usuário               |
| FullName      | String | Nome completo do usuário       |
| Id            | String | ID único do usuário            |
| PasswordHash  | String | Hash da senha do usuário       |

### Como Recriar as Tabelas

Você pode recriar essas tabelas usando o DynamoDB na AWS ou o DynamoDB Local. Abaixo estão os arquivos JSON que podem ser usados para criar as tabelas.
