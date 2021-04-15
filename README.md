[![NuGet Status](http://img.shields.io/nuget/v/Haka.Renderers.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Haka.Renderers)

<img 
    src="https://raw.githubusercontent.com/robinzevallos/haka-renderers/main/Haka.Renderers/icon.png" 
    width="100" 
    height="100">

## Haka.Renderers

Set of custom controls with renderers for Xamarin.Forms

### NuGet installation

Install the [Haka.Renderers NuGet package](https://www.nuget.org/packages/Haka.Renderers):

```powershell
PM> Install-Package Haka.Renderers
```

## Overview

### Frame Ripple
<img src="./screenshots/frame-ripple.gif">

```xml
<hkRenderers:FrameRipple
    BackgroundColor="#AEDDC9"
    BorderColor="#F58996"
    BorderWidth="5"
    CornerRadius="50, 0"
    Margin="20"
    Padding="20"
    RippleColor="Blue"
    >
</hkRenderers:FrameRipple>

<hkRenderers:FrameRipple
    BackgroundColor="#FFF"
    BorderColor="Blue"
    CornerRadius="15"
    Elevation="10"
    Margin="20"
    Padding="20"
    >
</hkRenderers:FrameRipple>
```
Properties:

| Property
| --- |
| BorderWidth |
| CornerRadius |
| Elevation |
| RippleColor |
| Tap |
| OnTap |

