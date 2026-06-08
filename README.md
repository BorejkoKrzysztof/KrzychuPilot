# KrzychuPilot

KrzychuPilot is a small recruitment-task MVP that lets a user submit multiple prompts, stores them in a database, processes them in the background, and shows the current status and result in a React frontend.

## What is included

- ASP.NET Core backend with REST API for creating prompts and fetching prompt states
- Background worker that processes pending tasks and calls an LLM via Ollama
- SQL Server persistence for prompt tasks
- React + Vite frontend with polling / SignalR updates for live status changes
- Docker Compose setup for one-command startup

## Main technologies

- Backend: C#, ASP.NET Core 9, MediatR, Entity Framework Core, SignalR
- Frontend: React, TypeScript, Vite, Axios, React Query, SCSS
- Database: SQL Server 2022
- LLM runtime: Ollama
- Orchestration: Docker Compose

## Project structure

- Backend/KrzychuPilot.API - ASP.NET Core Web API
- Backend/KrzychuPilot.Application - commands, queries, DTOs, validation
- Backend/KrzychuPilot.Domain - domain models and enums
- Backend/KrzychuPilot.Infrastructure - background worker and persistence
- Frontend - React frontend UI

## Quick start (recommended)

1. Make sure Docker Desktop is running.
2. Copy the example environment file:

   ```sh
   copy .env.example .env
   ```

   On Linux/macOS use:

   ```sh
   cp .env.example .env
   ```

3. Start the full stack:

   ```sh
   docker compose up --build
   ```

4. Open the app:
   - Frontend: http://localhost:3000


5. To stop the stack:

   ```sh
   docker compose down
   ```

   To remove the SQL Server volume as well:

   ```sh
   docker compose down -v
   ```

## Environment variables

The root `.env` file is used by Docker Compose. The default values are already defined in `.env.example`.

Key variables:

- `DB_SA_PASSWORD` - SQL Server password
- `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER` - database connection settings
- `API_PORT`, `API_INTERNAL_PORT` - API host/container ports
- `FRONTEND_PORT`, `FRONTEND_INTERNAL_PORT` - frontend host/container ports
- `OLLAMA_URL`, `OLLAMA_PORT`, `OLLAMA_MODEL` - Ollama connection and model name

## How the app works

1. The user enters one or more prompts in the frontend.
2. The backend saves each prompt as a task in SQL Server.
3. A background worker picks up pending tasks and sends them to Ollama.
4. The task status moves through the flow:
   - Awaiting
   - Processing
   - Finished
   - Failed
5. The frontend updates the visible status/result through polling / SignalR notifications.

## Local development (optional)

If you want to run parts of the system locally instead of using Docker for everything:

### Backend

```sh
cd Backend
 dotnet restore
 dotnet run --project KrzychuPilot.API/KrzychuPilot.API.csproj
```

### Frontend

```sh
cd Frontend
npm install
npm run dev
```

### Database and Ollama

You can keep SQL Server and Ollama in Docker while running the API and frontend directly on your machine:

```sh
docker compose up krzychupilot-db ollama
```

If you run the backend locally, make sure the connection settings point to the correct host (for example, `localhost` instead of the container name `krzychupilot-db`).

## Ollama model note

The current setup uses the `phi3` model by default. If the model is not available in your Ollama instance, pull it once:

```sh
docker compose exec ollama ollama pull phi3
```

## Notes for reviewers

This project is intentionally kept small and focused on the  requirement:

- multiple prompts can be submitted at once
- status is tracked for every task
- processing runs asynchronously in the background
- Docker Compose makes the full environment easy to start

