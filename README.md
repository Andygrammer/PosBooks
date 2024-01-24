# PosBooks - FIAP Pós Tech

#### Especialização em Arquitetura de Sistemas .NET com Azure: Fase IV - Tech Challenge

# 0. Metadados

**Nome do Projeto:** PosBooks

**Desenvolvedores do Projeto:**

| Aluno                               | RM            |  
| --------------------------------    | ------------- | 
| André Marinho Valadão Batemarchi    | 348471        | 
| André Vinícius de Angelo Falcão     | 349140        | 
| Kaique Leonardo Gomes da Silva      | 349128        |
| Nathalia Lasagna Dias de souza      | 350089        |
| Rodrigo Castagnaro                  | 349122        |

**Tecnologias Utilizadas:**

| Tecnologia                               | Propósito                                                      |  
| -----------------------------------      | -------------------------------------------------------------- | 
| .NET 7                                   | API, Class Library, Worker Service                             |
| Microsoft SQL Server                     | Banco de Dados                                                 |
| RabbitMQ                                 | Mensageria                                                     |
| xUnit, Bogus e NSubstitute               | Testes unitários/integrados                                    |
| Visual Studio e VS Code             | Desenvolvimento                                                |
| GitHub                                   | Versionamento                                                  |
| Miro                                     | Planejamento das demandas do trabalho e desenhos de diagramas  |
| Trello                                   | Kanban das demandas                                            |
| Discord                                  | Comunicação da equipe                                          |

# 1. Desafio

O Tech Challenge #4 consiste em desenvolver dois projetos .NET que implementam o conceito de mensageria: um Producer que envia dados para um broker, como o RabbitMQ e um Consumer que deve ler e processar os dados enviados.

**Requisitos:**

- Usar como broker o RabbitMQ ou o Azure ServiceBus.
  
- A explicação da solução deve ser registrada em um vídeo e compartilhada no portal do aluno FIAP.

# 2. Nossa Solução

Primeiramente, definimos qual seria a aplicação: um sistema para empréstimo/aluguel de livros. Assim, a aplicação deve possibilitar as seguintes ações:

- \<GET> Listar livros disponíveis na biblioteca;

- \<GET> Obter um livro específico;

- \<POST> Alugar um livro específico, com notificação por e-mail sobre disponibilidade. Caso o livro não estiver disponível para aluguel, devemos armazenar o requisitante em uma lista de espera;

- \<POST> Devolver um livro específico, com atualização de sua disponibilidade.

Para implementar a mensageria, criamos os seguintes projetos:

- Uma **API** para o **Producer**;

- Um **WorkerService** para o **Consumer**;

- Um **Class Library** para centralizar os Models/DTOs;

- Um projeto de **testes** para os testes unitários e integrados da aplicação.

Diagrama representando a ideia da aplicação:

![](./res/Diagrama%20da%20Aplicação.png "Diagrama da Aplicação")

As tecnologias usadas para concretizar a ideia são:

- **RabbitMQ, com MassTransit** para mensageria;

- **System.Net.Mail** para envio de e-mails;

- **SQL Server, com Entity Framework** para o banco de dados (BD) da aplicação;

- **Bogus** (geração de dados) e **NSubstitute** (mocks) para testes;

- **Docker Compose** para a execução da solução.

As tabelas criadas no BD dos Consumers são as seguintes:

![](./res/Diagrama%20ER%20Consumer.png "Diagrama de Entidade-Relacionamento")

A tabela criada no BD do Producer para os testes integrados é a seguinte:

![](./res/Diagrama%20ER%20Producer.png "Diagrama de Entidade-Relacionamento")

## 2.1. Arquitetura Proposta

Em virtude do que apresentamos até aqui, desenhamos a seguinte arquitetura para atender os requisitos de nossa aplicação.

![](./res/Diagrama%20da%20Arquitetura.png "Diagrama de Arquitetura")

De acordo com a arquitetura, temos o seguinte fluxo:

1. O cliente (usuário) consome a API (Produtor) com o intuito de emprestar ou devolver um livro.

2. A API se comunica com o servidor do RabbitMQ conteinerizado: cria e envia mensagens para o exchange.

3. O exchange recebe as mensagens da API e as encaminha (roteia) para as filas correspondentes usando o padrão fanout.

4. Os Consumidores (projeto WorkerService) consomem as mensagens que estão nas filas. Sendo que:

- O primeiro consumidor é responsável pelo aluguel de livros. Então, manipula informações de aluguel de livros e envio de e-mails de notificação (envia um e-mail em caso de sucesso ou um e-mail caso o livro já tenha sido alugado).

- O segundo consumidor é responsável pela devolução de livros. Então, manipula as informações de devolução de livros.

- As informações de aluguel/devolução de livros ficam armazenadas em um BD SQL Server conteinerizado.

## 2.2. Código Desenvolvido

Explicamos a seguir o código desenvolvido para a solução **PosBooks.sln**.

**Projeto API PosBooks (Producer):**

- Contém os Controllers, Services, classe de contexto e migrations do BD.

- Os endpoints fornecem as funcionalidades para o aluguel de livros, conforme o diagrama da aplicação apresentado anteriormente.

- A API é documentada com o Swagger.

- O appsettings possui as configurações de BD (connection string) e mensageria (nomes das filas, servidor, usuário e senha).

- A classe Program contém as configurações da API, de BD e do MassTransit para fornecer os endpoints aos consumers.

**Projeto PosBooksConsumer:**

- Contém o Worker, Services, classes de eventos (emprestar e devolver livros), classe de contexto e migrations do BD.

- A classe Program contém as configurações de serviços, de BD e do MassTransit para consumo.

- O appsettings possui as configurações de BD (connection string), mensageria (nomes das filas, servidor, usuário e senha) e e-mail (servidor SMTP, porta SMTP, usuário e senha de aplicação).

**Projeto PosBooksCore:**

- Contém todos os Models/DTOs/ViewModels das entidades criadas: Books, Clients e WaitList.

**Projetos PosBooksTest e PosBooksConsumerTests**

- Contém os testes unitários e integrados da aplicação, com BD SQL Server.

**Pasta res:** recursos usados por este documento.

**Outras pastas:** armazenam informações de configurações das IDEs utilizadas.

## 2.3. Docker Compose Criado

Na raiz deste repositório temos o **docker-compose.yml**. Ele foi desenvolvido para criar um container para o servidor do RabbitMQ e um container para o BD da aplicação.

# 3. Observações

Apresentamos aqui os pontos de destaque para a apresentação de nossa solução:

1. Criaremos os containers com o comando: **docker compose up**.

2. Executaremos os projetos do Producer (API) e do Consumer (WorkerService) ao mesmo tempo. 

# 4. Conclusões

Uma solução que implementa mensageria permite a criação de sistemas distribuídos. Esses sistemas têm características relevantes, como desacoplamento e escalabilidade. É possível usar um broker, como o RabbitMQ para implementar aplicações que são escaláveis e desacopladas. Por exemplo, podemos criar funcionalidades para a aplicação desenvolvida 
sem comprometer o funcionamento total do sistema, pois a dependência entre os componentes é mínima (em comparação a modelos tradicionais, de monolitos). O RabbitMQ também garante resiliência, evitando o comprometimento de mensagens trocadas. Ainda assim, destacamos que a elaboração de uma camada de mensageria não é trivial. O projeto pode se tornar mais complexo e o gerenciamento de estados não é simples. Portanto, uma equipe que deseja utilizar mensageria, deve se atentar se realmente é necessário esse tipo de solução e como vão tratar as complexidades inerentes a esse modelo.

# 5. Referências

1. [Documentação da Microsoft para envio de e-mail](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailmessage?view=net-7.0)

2. [Documentação do Docker Compose](https://docs.docker.com/compose/)

3. [Documentação do MassTransit](https://masstransit.io/documentation/concepts)

4. [Documentação do RabbitMQ](https://www.rabbitmq.com/documentation.html)

