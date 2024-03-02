![Hackathon Logo](docs/images/hackathon.png?raw=true "Hackathon Logo")

# Sitecore Hackathon 2024

- MUST READ: **[Submission requirements](SUBMISSION_REQUIREMENTS.md)**
- [Entry form template](ENTRYFORM.md)

## Team name

Team 451 Unavailable For Legal Reasons

## Category

Best use of AI

## Description

<!-- TODO -->

## Module Purpose

<!-- TODO -->

## Video link

<!-- TODO -->

## Pre-requisites and Dependencies

- Windows 11
- Visual Studio 2022
- Some Docker engine, for example [Docker Desktop](https://desktop.docker.com/win/stable/amd64/Docker%20Desktop%20Installer.exe)

## Installation instructions

### Setup (once)

#### Initialize solution

1. Run `.\Init.ps1 -LicenseXmlPath "<C:\path\to\license.xml>"`
1. Run `dotnet tool restore`

### Startup

1. Run `.\Invoke-Build.ps1` if you have build tools such as `msbuild`, `nuget` in your path **OR** publish `Platform` project in Visual Studio.
1. Run `docker-compose up -d --build`
1. Run `dotnet sitecore login --authority https://id.team451.localhost --cm https://cm.team451.localhost --allow-write true`
1. Run `dotnet sitecore index schema-populate`
1. Run `dotnet sitecore ser push`

### Configuration

An API Key for OpenAI is necessary. It can be specified in one of the following ways:
* Set OPENAI_APIKEY environment variable on host, eg. by running ``[System.Environment]::SetEnvironmentVariable("OPENAI_APIKEY", "<yourkey>")``
* Set OPENAI_APIKEY in ``.env`` file (and avoid committing that change) and run with docker compose
* Set OPENAI_APIKEY environment variable in ``src\news-mixer\code\Properties\launchSettings.json`` and run with Visual Studio

## Usage instructions

<!-- TODO -->

### Comments

<!-- TODO -->
