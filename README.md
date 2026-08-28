# Gift of the Givers operations desk

A **prototype** web app for coordinating disaster relief work in the spirit of [Gift of the Givers Foundation](https://giftofthegivers.org/). The public can see where teams are deployed, donate money or goods, and volunteer. Staff can log incidents, review incoming aid, and keep donation totals in the open.

This is an independent prototype. It is **not** an official Gift of the Givers product.

## Features

Anyone can use the site without an account.

| Public | Staff (signed in) |
| --- | --- |
| Read active and closed incidents | Log a disaster and close it when the response ends |
| Donate money (named or anonymous) | Review monetary and goods donations |
| Donate goods against a category | Add goods categories beyond the defaults |
| Register as a volunteer | Open the operations desk and volunteer list |
| See public incoming totals | Same public totals, plus staff-only capture screens |

Default goods categories are **food parcels**, **bottled water**, and **clothing**. Staff can add others — for example hygiene packs — so donors can select them.

Anonymous gifts still count toward the totals. The donor name is never shown.

## Demo staff login

| | |
| --- | --- |
| Email | `operations@giftofthegivers.org` |
| Password | `Hope1992!` |

The first run creates a local SQLite database (`GiftOfTheGiversApp/app.db`), applies migrations, and seeds this user together with sample incidents and donations. The database file is gitignored, so each machine starts with a fresh copy.

## Getting started

You need the **.NET 8 SDK**, or Visual Studio 2022 with the **ASP.NET and web development** workload.

### Visual Studio 2022

1. Open `GiftOfTheGiversApp.sln`.
2. Set **GiftOfTheGiversApp** as the startup project.
3. Choose the **http** profile (port 43127) or **IIS Express**.
4. Press **F5**.

Step-by-step Visual Studio notes, including switching SQLite to LocalDB, are in [docs/VISUAL-STUDIO-STEPS.md](docs/VISUAL-STUDIO-STEPS.md).

### Command line

```bash
cd GiftOfTheGiversApp
dotnet restore
dotnet run --launch-profile http
```

Open [http://localhost:43127](http://localhost:43127).

## Try the prototype

1. Stay signed out. Open **Public totals**, **Incidents**, and **Donate**.
2. Record a named monetary donation and an anonymous goods donation. Check that the totals increase and that the anonymous name stays hidden.
3. Register as a volunteer.
4. Sign in with the demo staff account and open **Staff desk**.
5. Log a new incident and confirm it appears on the public incidents list.
6. Add a goods category, then donate against it.

## Project layout

```
GiftOfTheGiversApp.sln
GiftOfTheGiversApp/
  Controllers/     Home, incidents, donations, volunteers, operations, categories
  Models/          Incident, donations, goods category, volunteer application
  Views/           Public and staff Razor views
  Data/            EF Core context, Identity, SQLite, seed data
  Areas/Identity/  Login, register, and account pages
docs/              Visual Studio setup notes
```

Built with **ASP.NET Core 8**, **MVC**, **ASP.NET Identity**, **EF Core**, and **SQLite**.

## Prototype notes

- Visiting `/Operations` while signed out redirects to the login page.
- Passwords must include a symbol; the seeded password uses `!`.
- Do not enter real identity numbers or live payment card details. Records stay on the local SQLite file.
