# FinanMan Solution Structure

## Project Overview
FinanMan is a personal financial management application designed to track transactions across multiple accounts including:
- Deposits (money coming in)
- Payments (money going out)
- Transfers (money moving between accounts)

The solution follows a layered architecture with separation of concerns between data models, business logic, and presentation.

## Core Projects

### Database Layer
- **FinanMan.Database.Models** - Core entity models and shared types
- **FinanMan.Database** - EF Core DbContext and database access

### Service Layer
- **FinanMan.Shared** - View models, interfaces, and shared logic
- **FinanMan.SharedLocalization** - Localization resources
- **FinanMan.SharedServer** - Server-side implementations of services
- **FinanMan.SharedClient** - Client-side implementations of services

### Presentation Layer
- **FinanMan.BlazorUi** - Blazor UI components library
- **FinanMan.BlazorUi.SharedComponents** - Reusable UI components library
- **FinanMan.App** - Blazor WebApp (main host application)
  - References the component libraries
  - Serves as the primary entry point for users

## Key Concepts

### Transaction Types
The application handles three main transaction types:
- **Deposits** - Money coming into an account (external → account)
- **Payments** - Money going out of an account (account → external)
- **Transfers** - Money moving between accounts (account → account)

### Data Models
- **Transaction** - Base entity for all financial transactions
- **Account** - Represents financial accounts (checking, savings, credit cards)
- **Lookup Lists** - Various reference data (account types, categories, etc.)

### State Management
The application uses state services to manage:
- **Lookup Lists** - Reference data like account types, categories, etc.
- **Transaction History** - List of transactions across accounts
- **UI State** - UI-specific state like language, dialog visibility, etc.

## Technology Stack
- **.NET 9.0** - Target framework
- **Blazor Server** - UI framework
- **Entity Framework Core** - Data access
- **SQL Server** - Database
- **FluentValidation** - Model validation