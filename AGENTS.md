# AGENTS.md

## Project

ASP.NET Core 10.0 MVC financial management app. Single entity (`Lancamento` - financial entry) with EF Core + SQL Server LocalDB / PostgreSQL.

## Commands

```bash
dotnet build                  # Build
dotnet run                    # Run (http://localhost:5121)
dotnet ef migrations add Name # Add migration (requires EF tools: dotnet tool install --global dotnet-ef)
dotnet ef database update     # Apply migrations
```

## Architecture

- **Default route**: `FinanceiroController` (not HomeController) - `Controllers/FinanceiroController.cs:30`
- **DbContexts**: `Data/AppDbContext.cs` (SQL Server), `Data/AppPostgresContext.cs` (PostgreSQL)
- **Model**: `Models/Lancamento.cs` - Id, Descricao, Valor, Tipo, Categoria, Data
- **Views**: `Views/Financeiro/` for CRUD operations
- **Databases**: SQL Server LocalDB (`DefaultConnection`) + PostgreSQL (`PostgreSqlConnection`)

## Multi-Database

- Toggle between SQL Server / PostgreSQL via dropdown in the dashboard UI
- Active database is stored in session (`SelectedDb` key)
- Both contexts registered in DI - controller checks session to route queries
- Each database needs its own migrations (run `dotnet ef` with `--context` flag for each)

## Conventions

- Portuguese naming for business concepts (Lancamento, Financeiro, Receita, Despesa)
- `Tipo` field values: "Receita" (income) / "Despesa" (expense)
- HTTPS redirect is commented out in `Program.cs:23`
- No test suite exists
