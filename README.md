# Portfolio Performance API

A RESTful API for managing investment portfolios, assets, and transactions with performance tracking capabilities.

## Features

- **Portfolio Management**: Create, read, update, and delete investment portfolios
- **Asset Management**: Manage financial assets with current prices
- **Transaction Recording**: Record buy/sell transactions with automatic portfolio updates
- **Performance Tracking**: Calculate portfolio performance, gains/losses, and asset allocation
- **CQRS Architecture**: Clean separation of commands and queries using MediatR
- **Validation**: Comprehensive input validation using FluentValidation
- **Global Exception Handling**: Standardized error responses
- **SQLite Database**: Lightweight, file-based database for development

## Technology Stack

- **.NET 9.0**
- **Entity Framework Core** with SQLite
- **MediatR** for CQRS pattern
- **FluentValidation** for input validation
- **Swagger/OpenAPI** for API documentation
- **CQRS** with Command/Query separation

## Project Structure

```
PortfolioAPI/
├── src/
│   ├── PortfolioAPI.WebApi/          # API controllers, middleware
│   ├── PortfolioAPI.Application/     # Use cases, DTOs, interfaces
│   ├── PortfolioAPI.Domain/          # Entities, value objects, domain logic
│   ├── PortfolioAPI.Infrastructure/  # Data access, repositories
│   └── PortfolioAPI.Shared/          # Common utilities, extensions
├── tests/
│   ├── PortfolioAPI.UnitTests/
│   └── PortfolioAPI.IntegrationTests/
└── docs/
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code


The API will be available at `https://localhost:7000` and Swagger documentation at `https://localhost:7000/swagger`.

## API Endpoints

### Portfolios

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/portfolios` | Create a new portfolio |
| GET | `/api/portfolios` | Get all portfolios |
| GET | `/api/portfolios/{id}` | Get portfolio by ID |
| PUT | `/api/portfolios/{id}` | Update portfolio |
| DELETE | `/api/portfolios/{id}` | Delete portfolio |
| GET | `/api/portfolios/{id}/performance` | Get portfolio performance |
| GET | `/api/portfolios/{id}/allocation` | Get asset allocation |
| GET | `/api/portfolios/{id}/transactions` | Get transaction history |

### Assets

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/assets` | Create a new asset |
| GET | `/api/assets` | Get all assets |
| GET | `/api/assets/{id}` | Get asset by ID |
| PUT | `/api/assets/{id}` | Update asset |
| DELETE | `/api/assets/{id}` | Delete asset |

### Transactions

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/transactions` | Record a transaction |

## Usage Examples

### Create a Portfolio

```bash
  {
    "name": "Portfolio 1"
  }

### Create an Asset

```bash
  {
    "name": "Apple Inc.",
    "currentPrice": 150.00
  }
```

### Record a Buy Transaction

```bash
  {
    "portfolioId": "portfolio-guid-here",
    "assetId": "asset-guid-here",
    "transactionType": 1,
    "quantity": 10,
    "price": 150.00,
    "date": "2025-07-12T10:00:00Z"
  }
```

### Get Portfolio Performance

```bash
https://localhost:7000/api/portfolios/{portfolio-id}/performance?startDate=2024-01-01&endDate=2024-12-31"

```

### Get Asset Allocation

```bash
"https://localhost:7000/api/portfolios/{portfolio-id}/allocation"
```


## Seed Data

The application comes with 2 pre-seeded assets:
- Apple Inc. ($150.00)
- Microsoft Corp. ($300.00)

## Performance Calculations

- **Total Value**: Sum of (Current Price × Quantity) for all assets
- **Unrealized Gain/Loss**: (Current Price - Average Cost Basis) × Quantity
- **Realized Gain/Loss**: Calculated from completed sell transactions
- **Asset Allocation**: (Asset Value / Total Portfolio Value) × 100



