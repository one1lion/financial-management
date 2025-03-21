# FinanMan UI Components Guide

## Overview

The FinanMan UI is built using Blazor Server and consists of several key component categories organized by functionality. These components were originally part of separate UI libraries (FinanMan.BlazorUi and FinanMan.BlazorUi.SharedComponents) but are now integrated into the main WebApp project.

## Core UI Components

### Transaction Entry Components

Components for entering different transaction types:

- **DepositEntry.razor** - For entering deposit transactions
- **PaymentEntry.razor** - For entering payment transactions with line items
- **TransferEntry.razor** - For transferring funds between accounts
- **PaymentDetailsTable.razor** - Displays line items for payment transactions

### Transaction History Components

Components for displaying and interacting with transaction history:

- **TransactionHistoryGrid.razor** - Main grid for displaying transactions
- **BaseTransactionRow.razor** - Base component for transaction rows
- **DepositHistoryRow.razor** - Displays deposit transactions
- **PaymentHistoryRow.razor** - Displays payment transactions
- **TransferHistoryRow.razor** - Displays transfer transactions
- **EditTransactionModal.razor** - Modal for editing transactions
- **ConfirmDeleteTransactionModal.razor** - Confirmation dialog for deletions
- **ConfirmTogglePendingModal.razor** - Dialog for toggling transaction status

### Account Components

Components for managing accounts:

- **AccountSummary.razor** - Displays summary of account balances
- **AccountSummaryGrid.razor** - Grid showing account balances
- **AccountSummaryRow.razor** - Individual account row in the summary

### List Management Components

Components for managing lookup lists:

- **ListManagementPage.razor** - Main page for managing lookup lists
- **LookupTypeList.razor** - Selector for choosing which lookup type to manage
- **LookupListEdit.razor** - Editor for modifying lookup items
- **AddAccountForm.razor** - Form for adding a new account
- **AddAccountDialog.razor** - Dialog wrapper for account form

### Shared UI Components

Reusable UI components:

- **Card.razor** - Card container with header, body, and footer sections
- **ModalComponents** - Various modal dialog components
- **FlyoutComponents** - Slide-in panel components
- **DraggableComponents** - Components that can be moved by the user

### Layout Components

Page layout components:

- **MainLayout.razor** - Main application layout
- **TopBar.razor** - Application header with navigation
- **Footer.razor** - Application footer
- **AppBrand.razor** - Branding component
- **AppBarLinks.razor** - Navigation links

### Main Page Components

Landing page components:

- **MainPageHero.razor** - Hero section for landing page
- **TrackTransactionsSection.razor** - Feature section for landing page
- **ViewTransactionHistorySection.razor** - Feature section for landing page

### Utility Components

Utility and helper components:

- **Calculator.razor** - Simple calculator widget
- **Logo.razor** - Logo display component with different variations

## State Management

The UI components interact with several state management services:

- **IUiState** - Manages UI-specific state (dialogs, flyouts, language)
- **ILookupListState** - Manages lookup data cache
- **ITransactionsState** - Manages transaction data and operations

## CSS Structure

The UI uses a structured SCSS approach:

- **Root variables** - Defined in _Root.scss
- **Theme variables** - Colors, spacing, and sizing
- **Component-specific styles** - Co-located with components
- **Responsive layouts** - Media queries for different screen sizes

## JavaScript Interop

Several components use JavaScript interop for enhanced functionality:

- **super-dukasoft-interop.js** - Core interop functions
- **Calculator** - Uses interop for clipboard and focus management
- **DraggableContainer** - Uses interop for drag and drop functionality

## Usage Patterns

### Form Submission Pattern

Forms typically follow this pattern:

1. Bind to a view model
2. Validate input using data annotations or FluentValidation
3. Submit to appropriate service
4. Handle response and display errors if needed
5. Update state on success

### Modal Dialog Pattern

Modal dialogs typically:

1. Accept a Show parameter with two-way binding
2. Include title, body, and action buttons
3. Fire events for confirm/cancel actions
4. Handle their own state for validation

### State Management Pattern

State management follows:

1. Inject appropriate state service
2. Subscribe to change notifications
3. Update component state when notifications received
4. Unsubscribe in Dispose method