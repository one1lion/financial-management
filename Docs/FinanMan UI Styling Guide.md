# FinanMan UI Styling Guide

## Overview

FinanMan UI uses a structured SCSS system with CSS variables for theming. This document outlines the styling architecture and usage patterns.

## Core Style Files

### Base Styles

- **_Root.scss** - CSS variables and root settings
- **_ColorVariables.scss** - Color definitions
- **_FontSizing.scss** - Typography scale
- **_BreakPointVariables.scss** - Responsive breakpoints
- **_GlobalStyles.scss** - Global element styles
- **App.scss** - Main style import file

### Element Styles

- **_Buttons.scss** - Button styles
- **_Forms.scss** - Form control styles
- **_TablesAndGrids.scss** - Table and data grid styles
- **_CardOverride.scss** - Card component styles
- **_ModalOverride.scss** - Modal dialog styles

## CSS Variables

### Color Variables

```scss
// Brand colors
$purple1: #34005C;
$purple2: #230143;
$purple3: #200D29;
$purple4: #530296;
$purple5: #7900B8;
$purple6: #ED68FD;

// UI colors
$jet-black: #03060A;
$off-white: #F6F5F5;
$graphite-gray: #44494E;
$white: #FFFFFF;
$white-smoke: #F3F3F3;
$dim-gray: #6D6D6D;
```

### Root Variables

```scss
:root {
  // Base variables
  --base-font-size: #{$base-font-size};
  --primary-text-color: #f0f3f0;
  --primary-background-color: #{$jet-black};
  --font-family: "Segoe UI", Arial, sans-serif, serif;
  
  // Component-specific variables
  --button-background-color: #{$purple1};
  --card-background-color: var(--primary-background-color);
  --input-background-color: #03000A;
  
  // Spacing
  --default-padding: 2.4rem;
  --default-padding-mobile: .5rem;
  
  // Z-index layers
  --flyout-z-index: 2;
  --modal-z-index: calc(var(--flyout-z-index) + 1);
}
```

## Component-Specific Styling

Components use co-located CSS/SCSS files:

```
ComponentName.razor
ComponentName.razor.scss   // Source SCSS
ComponentName.razor.css    // Compiled CSS
ComponentName.razor.min.css // Minified version
```

Example component SCSS:

```scss
.component-wrapper {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 1rem;
  
  .header {
    grid-column: 1 / 3;
    font-size: var(--font-size-h3);
  }
  
  .content {
    background-color: var(--card-background-color);
  }
}
```

## Responsive Design

### Breakpoints

```scss
$bp-smoll-max: 480px;
$bp-medium-min: 480.1px;
$bp-medium-max: 930px;
$bp-large-min: 930.1px;
$bp-large-max: 1920px;
$bp-xlarge-min: 1920.1px;
```

### Responsive Patterns

Media query usage:

```scss
@media (max-width: #{$bp-smoll-max}) {
  // Mobile styles
}

@media (min-width: #{$bp-medium-min}) and (max-width: #{$bp-medium-max}) {
  // Tablet styles
}

@media (min-width: #{$bp-large-min}) {
  // Desktop styles
}
```

### Responsive Mixins

Common patterns defined as mixins:

```scss
@mixin narrow-section {
  .section {
    padding: var(--default-padding-mobile);
    flex-direction: column;
    
    .title {
      font-size: var(--font-size-h3);
    }
  }
}

@media (max-width: #{$bp-smoll-max}) {
  @include narrow-section;
}
```

## CSS Grid Usage

The application uses CSS Grid extensively:

```scss
.grid-layout {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  grid-template-areas:
    "hd hd hd hd hd hd"
    "sd sd mn mn mn mn"
    "ft ft ft ft ft ft";
  
  .header {
    grid-area: hd;
  }
  
  .sidebar {
    grid-area: sd;
  }
  
  .main {
    grid-area: mn;
  }
  
  .footer {
    grid-area: ft;
  }
}
```

## Theme Support

The application includes variables for theme support:

```scss
.light-theme {
  --primary-text-color: #{$darkteal};
  --primary-background-color: #f0f3f0;
  --border-glow: rgba(0, 0, 0, .3);
}
```

## Styling Deep Components

Cascading styles to child components using `::deep`:

```scss
.parent-component ::deep {
  .child-component {
    margin: 1rem;
    
    .button {
      background-color: var(--button-background-color);
    }
  }
}
```

## Build Process

Styles are processed using the SCSS compiler:

1. SCSS files are defined in `compilerconfig.json`
2. Compiled to CSS during build
3. Minified versions are generated
4. Included in the published application

## Usage Guidelines

1. Always use CSS variables for theming
2. Co-locate component styles with components
3. Use media queries for responsive design
4. Follow the naming convention for CSS selectors
5. Scope styles to component identifiers
6. Use `::deep` for styling child components