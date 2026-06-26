<h1 align="center">🔭 StellarMinds — Web (MVC)</h1>

<p align="center">
  Frontend web de <b>StellarMinds</b>, un sistema de gestión para un club de astronomía:
  socios, equipos, observaciones y préstamos.<br/>
  Aplicación <b>ASP.NET Core MVC</b> que consume la <a href="https://github.com/lcorbo121/StellarMinds-API-SOLID">API REST StellarMinds</a>.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10-512BD4?style=flat&logo=dotnet&logoColor=white" alt=".NET 10"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white" alt="C#"/>
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91?style=flat&logo=dotnet&logoColor=white" alt="ASP.NET Core MVC"/>
  <img src="https://img.shields.io/badge/Razor-Views-blue?style=flat" alt="Razor"/>
</p>

---

## 📋 Descripción

**StellarMinds** es una plataforma para administrar la operativa de un club de astronomía. Esta aplicación es el **frontend MVC**: renderiza las vistas, gestiona la sesión del usuario y se comunica con la **API REST** (proyecto [StellarMinds-API-SOLID](https://github.com/lcorbo121/StellarMinds-API-SOLID)) para todas las operaciones de datos.

> 🎓 Proyecto académico — Obligatorio de **Programación 3** (Analista en TI, Universidad ORT Uruguay).

---

## 📸 Capturas de pantalla

> ⚠️ Reemplazá estas imágenes por tus capturas reales (ver instrucciones más abajo).

<table>
  <tr>
    <td width="50%" align="center">
      <b>Inicio de sesión</b><br/>
      <img src="screenshots/login.png" alt="Login" width="100%"/>
    </td>
    <td width="50%" align="center">
      <b>Equipos</b><br/>
      <img src="screenshots/equipos.png" alt="Equipos" width="100%"/>
    </td>
  </tr>
  <tr>
    <td width="50%" align="center">
      <b>Observaciones / Ranking</b><br/>
      <img src="screenshots/observaciones.png" alt="Observaciones" width="100%"/>
    </td>
    <td width="50%" align="center">
      <b>Préstamos</b><br/>
      <img src="screenshots/prestamos.png" alt="Préstamos" width="100%"/>
    </td>
  </tr>
</table>

---

## ✨ Funcionalidades

- 👥 **Usuarios** — inicio de sesión y gestión de sesión.
- 🧑‍🚀 **Socios** — alta, listado y detalle de socios del club.
- 🔭 **Equipos** — gestión de telescopios, cámaras, oculares y monturas (alta/edición/detalle por tipo).
- 🌌 **Observaciones** — registro de observaciones astronómicas, listados y **ranking**.
- 📦 **Préstamos** — préstamo y devolución de equipos, auditoría e informes (socios por telescopio).

---

## 🛠️ Stack

- **.NET 10** · **C#**
- **ASP.NET Core MVC** + **Razor Views**
- **HttpClient** tipado para consumir la API REST
- **Sesión** basada en cookies (`HttpOnly`, `Secure`)

---

## 🏗️ Arquitectura

```
Navegador  ──►  StellarMinds-MVC (este repo)  ──►  StellarMinds-API-SOLID (REST API)  ──►  Base de datos
                 Controllers + Razor Views          Lógica de negocio (SOLID)
```

El frontend no accede a la base de datos directamente: toda la lógica y persistencia viven en la **API**.

---

## 🚀 Cómo ejecutarlo localmente

**Requisitos:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
# 1. Clonar el repositorio
git clone https://github.com/lcorbo121/StellarMinds-MVC.git
cd StellarMinds-MVC/AppWeb/StellarMinds

# 2. Configurar la URL de la API (appsettings.json o variable de entorno)
#    "ApiBaseUrl": "https://localhost:7158/"   (o la URL de tu API publicada)

# 3. Restaurar y ejecutar
dotnet restore
dotnet run
```

La app queda disponible en `https://localhost:xxxx`. La URL de la API se configura con la clave **`ApiBaseUrl`** en `appsettings.json`.

> 💡 Necesitás la [API StellarMinds](https://github.com/lcorbo121/StellarMinds-API-SOLID) corriendo (local o publicada) para que la app funcione.

---

## 🔗 Proyecto relacionado

- [**StellarMinds-API-SOLID**](https://github.com/lcorbo121/StellarMinds-API-SOLID) — API REST (backend) construida aplicando principios SOLID.

---

<p align="center"><sub>Desarrollado por <a href="https://github.com/lcorbo121">Lucas Corbo</a></sub></p>
