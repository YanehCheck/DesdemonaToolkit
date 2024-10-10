![Desdemona Toolkit Logo](https://github.com/YanehCheck/DesdemonaToolkit/blob/master/images/logo.png)
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
![License](https://img.shields.io/badge/license-GPL--3.0-EA2824)
![Language](https://img.shields.io/badge/language-C%23-CA2824)
![OS](https://img.shields.io/badge/OS-windows-AA2824)
![.NET](https://img.shields.io/badge/.NET-8.0-8A2824)
## About

Desdemona Toolkit is free and open-source desktop application for exploring and manage your Fortnite account and items without giving out your credentials. 

## Features

⭐ **Browse & Share**: Browse every skin, emote, and cosmetic you've unlocked, with all unlockable styles included. Share your items as an image or export it to Fortnite.GG.  
⭐ **Customizable**: Create filters to organize and annotate your items exactly how you want.  
⭐ **Periodically updated**: New tools and patches being added constantly. Enjoy fast reward code redeemer!  
⭐ **Secure**: Your login information is never shared with any third parties. Desdemona Toolkit communicates directly with the Epic Games API.  
⭐ **Open-Source**: All the code is licensed under GPL-3.0, ensuring transparency and extensibility.  

### ⚠️Desdemona Toolkit is currently in early active development. You should expect minor bugs, breaking changes and lack of features early-on.⚠️

## Installation and set-up

- [Download the latest release](https://github.com/YanehCheck/DesdemonaToolkit/releases/latest) (or compile the source code yourself using .NET8)
    - Self-contained version includes .NET8 runtime (recommended if you have no idea what that means)
    - Framework-dependent version requires .NET8 runtime pre-installed
- Extract the archive into a new directory
- Run `DesdemonaToolkit.exe`
- On home screen, under `Fetch Items` choose the recommended option, so the application loads in item and style data
    - If the release is older and some items are not showing up, you can also then use the `Items - Fortnite.GG` option to get the latest data from www.fortnite.gg
- Authenticate using your authorization code
- Use the app! Browse your inventory, export it and so on!

## Usage

See [Wiki](https://github.com/YanehCheck/DesdemonaToolkit/wiki) for FAQ, usage and on how to create your own filter.

## The future and what is in the making for v1.0.0 release

The long-term goal for this project is to become "toolkit" for real. I would consider the current locker feature pretty top-notch, but user-friendly customizable features take a long time to make and currently I just don't have time to implement all of them. My solution is to integrate most of the api as a REPL module. This will unlock almost unlimited and newest use-cases at the cost of user-friendliness.

#### EGS API REPL Page
- Very powerful feature for advanced users
- Support for most notable API endpoints
- No boilerplate information required compared to calling the API yourself
- Built-in doc pages

The DTKF language was implemented for customizable, easy and shareable locker filtering and I think it fulfilled my vision. I would love to implement similar scripting language, DTKAPI, as a facade for the API REPL page.

#### EGS API DTKAPI Language
- Extra useful functions (stringify json, etc...)
- Persistent aliases for commands
- Shareable script files

I will end it off with early draft of the language.
```
/alias "profile" "/QueryProfile profileId=athena"
profileVar << /profile
winsVar << /json-get-section wins <profileVar>
/RandomApiEndpoint profileId=athena {"loadout": 5, wins: <winsVar>}
```

###
  
## Gallery
<img width="750" src="https://github.com/YanehCheck/DesdemonaToolkit/blob/master/images/img-export.png"> </img>
![Home Page](https://github.com/YanehCheck/DesdemonaToolkit/blob/master/images/screenshot1.png)
![Inventory Page](https://github.com/YanehCheck/DesdemonaToolkit/blob/master/images/screenshot2.png)
