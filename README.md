# PlantWatering API

Uma API REST desenvolvida em **.NET 10 (C#)** utilizando a abordagem de **Minimal APIs** para gerenciamento e monitoramento do ciclo de rega de plantas.

---

## Objetivo do Projeto
Este projeto é concebido no contexto da disciplina de **DevOps** da puc pr, servindo como base prática e evolutiva para aplicação de boas práticas de Engenharia de Software e DevOps, como por exemplo:
1. **Controle de Versão e Git Flow:** Commits atômicos, convenção semântica (*Conventional Commits*) e fluxo com Pull Requests.
2. **Integração Contínua (CI):** Pipelines automatizados de build, análise estática e execução de testes.
3. **Entrega/Implantação Contínua (CD):** Containerização com Docker e deploy automatizado em ambientes de nuvem.
4. **Monitoramento e Alertas:** Notificações de integridade de builds e métricas de saúde da API.

---

## Stack Tecnológica
- **Linguagem:** C# 13 / .NET 10
- **Paradigma:** Minimal APIs (ASP.NET Core)
- **Persistência:** Em memória (`Thread-Safe In-Memory Repository`)
- **Documentação:** OpenAPI / Swagger (Endpoints nativos)

---

## Arquitetura e Estrutura da Aplicação
A solução foi planejada focando em uma organização limpa e desacoplada:

```text
PlantWatering-API/
├── .github/
│   └── workflows/
│       └── ci-cd.yml
├── .dockerignore
├── .gitignore
├── docker-compose.yml
├── Dockerfile
├── PlantWatering.sln
├── README.md
└── PlantWatering.Api/
    ├── PlantWatering.Api.csproj
    ├── Program.cs
    ├── Domain/
    │   ├── Models/
    │   └── Enums/
    ├── Data/
    │   ├── Repositories/
    │   └── Interfaces/
    └── Endpoints/
```

---

## Como Executar Localmente

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download) instalado **ou** [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado.

### Opção 1: Executando com .NET CLI
```bash
# restaurar dependencias e compilar a solucao
dotnet build

# executar o projeto da API
dotnet run --project PlantWatering.Api
```

### Opção 2: Executando com Docker Compose
```bash
# subir a aplicacao em container com build automatico
docker compose up --build

# parar e remover os containers
docker compose down
```

---

## 📋 Roadmap da Disciplina
- [x] **Etapa 1:** Configuração do repositório, branch de feature, commits e Pull Request.
- [x] **Etapa 2:** Configuração dos Workflows no GitHub Actions para CI e CD.
- [x] **Etapa 3:** Containerização com Docker (`Dockerfile` e `docker-compose`).
- [ ] **Etapa 4:** Execução do Kubernetes Playground e prática com os componentes do Kubernetes.
- [ ] **Etapa 5:** Configuração de alertas automatizados do GitHub Actions (Discord, Slack, Microsoft Teams ou Telegram).
- [ ] **Etapa 6:** Criação de pelo menos cinco testes unitários e execução automática dos testes nas Pull Requests via GitHub Actions.