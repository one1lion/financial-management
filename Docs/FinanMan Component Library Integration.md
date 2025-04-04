﻿# Component Library Usage Guide

## Overview

This guide outlines how to effectively use and maintain the FinanMan component libraries within a Blazor WebApp project.

## Project Structure

The FinanMan solution consists of several projects, each serving a specific purpose. Below is the directory structure for the component libraries and the Blazor WebApp. Note that the WebApp project's App.razor file serves as the entry point for the component libraries. It also serves as the API host for the Blazor WebApp. However, the API simply forwards requests to the Server-side service library.

The FinanMan.App.Client project was scaffolded during the initial project creation process and primarily handles the client-side setup within its Program.cs file. It references the FinanMan.BlazorUi library but does not include any other component libraries.

```
FinanMan.App/ (Blazor WebApp)
├── Components/
│   └── App.razor - Points to FinanMan.BlazorUi.Routes
├── Controllers/ - The API controllers for the WebApp
└── Program.cs - Entry point for the Blazor WebApp

FinanMan.Client/ (Standard Client UI Library)
└── Program.cs - Entry point for the client-side application

FinanMan.BlazorUi/ (Component Library)
├── Components/
├── Routes.razor
├── wwwroot/
└── _Imports.razor

FinanMan.BlazorUi.SharedComponents/ (Shared Component Library)
└── [Shared components]
```

## Key Configuration

### WebApp Project References

The WebApp needs to reference:

1. FinanMan.BlazorUi
2. FinanMan.BlazorUi.SharedComponents
3. Core service libraries (Database, Shared, etc.)

### _Imports.razor in WebApp

```razor
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.AspNetCore.Components.WebAssembly.Http
@using Microsoft.JSInterop
@using FinanMan.App
@using FinanMan.App.Components
@using FinanMan.BlazorUi
@using FinanMan.BlazorUi.Components
@using FinanMan.BlazorUi.SharedComponents
```

### Static Asset References

Use the `_content` prefix for referencing static resources from component libraries:

```html
<!-- Reference to static content in component libraries -->
<link rel="stylesheet" href="_content/FinanMan.BlazorUi/css/app.css" />
<script src="_content/FinanMan.BlazorUi/js/script.js"></script>
<img src="_content/FinanMan.BlazorUi/images/logo.png" />
```

### Service Registration

Register all services required by the component libraries in WebApp's Program.cs:

```csharp
builder.Services
    .AddFinanManLocalization()
    .AddStateManagement()
    .AddJavaScriptModules()
    .SetupDbContext(builder.Configuration)
    .AddServerServices()
    .AddFluentValidation();
```

### Component References

Use fully qualified namespaces when referencing components in custom pages:

```razor
@using FinanMan.BlazorUi.Components.TransactionHistoryComponents
@using FinanMan.BlazorUi.SharedComponents.Card

<TransactionHistoryGrid />
<Card>
    <CardHeader>My Card</CardHeader>
    <CardBody>Content here</CardBody>
</Card>
```

### Assembly Registration

Register component library assemblies with the WebApp for routing:

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(FinanMan.App.Client._Imports).Assembly,
        typeof(FinanMan.BlazorUi._Imports).Assembly);
```

### JavaScript Interop

Use proper content paths for JavaScript interop:

```csharp
// Reference JS from component library
await JS.InvokeVoidAsync("loadScript", "_content/FinanMan.BlazorUi/js/script.js");
```

## App.razor Configuration

The App.razor file in the WebApp serves as the host for the component library:

```razor
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <base href="/" />
    <link rel="shortcut icon" href="_content/FinanMan.BlazorUi/favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="_content/FinanMan.BlazorUi/css/app.css" />
    <link rel="stylesheet" href="FinanMan.App.styles.css" />

    <HeadOutlet @rendermode="InteractiveAuto" />
</head>

<body>
    <FinanMan.BlazorUi.Routes @rendermode="InteractiveAuto" />
    <script src="_framework/blazor.web.js"></script>
</body>

</html>
```

## Integration Test Checklist

After integration, verify:

- [ ] All components render correctly
- [ ] Styles are applied properly
- [ ] JavaScript functionality works
- [ ] Navigation between pages works
- [ ] Forms submit correctly
- [ ] State management functions as expected
- [ ] Modals and dialogs display correctly
- [ ] Responsive layouts work on all screen sizes

## Common Issues

### CSS/JS Loading Issues

**Problem:**
Styles or scripts not loading from component libraries.

**Solution:**
Ensure all references use the `_content/[LibraryName]/` prefix.

### Component Initialization

**Problem:**
Components failing to initialize due to missing service registrations.

**Solution:**
Ensure all services required by component libraries are registered in the WebApp's Program.cs.

### Assembly Reference Issues

**Problem:**
Components from libraries not found or not routing correctly.

**Solution:**
Verify that component library assemblies are properly registered with AddAdditionalAssemblies.
