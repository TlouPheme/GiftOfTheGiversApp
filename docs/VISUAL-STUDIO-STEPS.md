# How to build the Gift of the Givers app in Visual Studio 2022

This is the C# / ASP.NET Core MVC path for APPR6312 Part 1 Section C. You can **open the solution that is already in this repository**, or **recreate it from scratch** so you can screenshot every step for 3.1 / 3.3.

---

## A. Fast path — open this project (recommended)

### 1. Install Visual Studio 2022

1. Download Visual Studio 2022 Community from https://visualstudio.microsoft.com/downloads/
2. Run the installer.
3. Tick **ASP.NET and web development**.
4. On the right, confirm these are included:
   - .NET 8.0 Runtime (or .NET 8 SDK)
   - .NET Framework 4.8 targeting pack is optional
   - Git for Windows (optional)
5. Click **Install**. Restart the PC if the installer asks.

### 2. Get the code onto your PC

If you cloned with Origin in WSL, the folder is already on disk. From File Explorer, go to that folder.

If you are starting from a zip: extract it, for example to:

`C:\Users\tloup\source\repos\GiftOfTheGiversApp`

You should see:

- `GiftOfTheGiversApp.sln`
- `GiftOfTheGiversApp\GiftOfTheGiversApp.csproj`

### 3. Open the solution

1. Double-click `GiftOfTheGiversApp.sln`.
2. If Visual Studio asks to trust the project, choose **Trust**.
3. Wait until the bottom status bar says **Ready** (NuGet packages restore automatically).

### 4. Set the startup project

1. In **Solution Explorer**, right-click **GiftOfTheGiversApp** (the project, not the solution).
2. Choose **Set as Startup Project**.
3. On the toolbar, open the profile dropdown next to the green Play button.
4. Choose **http** (port 43127) **or** **IIS Express**.

### 5. Run

1. Press **F5** (run with debugger) or **Ctrl+F5** (run without debugger).
2. A browser should open on the Gift of the Givers home page.
3. If a firewall prompt appears, allow access on private networks.

### 6. Sign in as staff

1. Click **Sign in** (or **Staff desk**, which will bounce you to login).
2. Email: `operations@giftofthegivers.org`
3. Password: `Hope1992!`
4. You should land on the operations desk.

The first run creates/updates `GiftOfTheGiversApp\app.db` (SQLite) and seeds incidents, donations, categories and the staff user.

### 7. Screenshots to take for the POE

Take these while the app is running:

1. Visual Studio with Solution Explorer showing Models, Controllers, Views, Data.
2. Home page with public totals.
3. Donate — monetary form.
4. Donate — goods form.
5. Identity login page.
6. Operations desk after login.
7. New incident form.
8. Public totals after a donation.
9. **SQL Server Object Explorer is not required** while you use SQLite. If you switched to LocalDB (section D), screenshot the tables there.

---

## B. From-scratch path — create the same app yourself

Use this if the brief wants evidence that **you** created an ASP.NET Core MVC project with Individual Accounts.

### 1. Create the project

1. Open Visual Studio 2022.
2. **Create a new project**.
3. Search for **ASP.NET Core Web App (Model-View-Controller)**.
4. Choose that template (C#). Click **Next**.
5. Project name: `GiftOfTheGiversApp`
6. Location: your DIS / APPR6312 folder.
7. Tick **Place solution and project in the same directory** if you want a flatter folder. Click **Next**.
8. Framework: **.NET 8.0 (Long Term Support)**.
9. Authentication type: **Individual Accounts**.
10. Leave **Configure for HTTPS** ticked.
11. Click **Create**.

Visual Studio now scaffolds:

- `Program.cs` — services and HTTP pipeline
- `Data\ApplicationDbContext.cs` — Identity database
- `Areas\Identity` — login / register Razor Pages
- `Controllers\HomeController.cs`
- `Views\Shared\_Layout.cshtml`
- Bootstrap under `wwwroot`

That is the same starting point this repository used.

### 2. Confirm Individual Accounts (login)

1. Press F5 once.
2. Click **Register**, create a test user, then **Login**.
3. Screenshot the Identity login page for 3.3.

For this POE we seed a staff user instead of making the marker register. After you copy in `Data\DbInitializer.cs` and the models below, that account is created on first run.

### 3. Add the Gift of the Givers models

In Solution Explorer, right-click **Models** → **Add** → **Class** for each of these (or copy the files from this repo):

| Class | Purpose |
| --- | --- |
| `Incident` | Disaster: title, location, start/end date, description, required aid, active flag |
| `GoodsCategory` | Food parcels, water, clothing, plus staff-defined categories |
| `MonetaryDonation` | Date, amount in rand, anonymous flag, donor name |
| `GoodsDonation` | Date, item count, category, description, anonymous flag |
| `VolunteerApplication` | Public volunteer form |

Use `[Required]`, `[Range]`, `[DataType(DataType.Date)]` and `[Display]` so MVC can validate the forms.

### 4. Extend the database context

Open `Data\ApplicationDbContext.cs` and add `DbSet<>` properties:

```csharp
public DbSet<Incident> Incidents => Set<Incident>();
public DbSet<GoodsCategory> GoodsCategories => Set<GoodsCategory>();
public DbSet<MonetaryDonation> MonetaryDonations => Set<MonetaryDonation>();
public DbSet<GoodsDonation> GoodsDonations => Set<GoodsDonation>();
public DbSet<VolunteerApplication> VolunteerApplications => Set<VolunteerApplication>();
```

### 5. Add a migration

1. **Tools** → **NuGet Package Manager** → **Package Manager Console**.
2. Default project: `GiftOfTheGiversApp`.
3. Run:

```
Add-Migration AddReliefEntities
Update-Database
```

If you are on the copy from this repo, the migration is already under `Data\Migrations`. You only need `Update-Database` if the `.db` file is missing.

### 6. Seed data and the staff user

Copy `Data\DbInitializer.cs`. In `Program.cs`, after `var app = builder.Build();` call:

```csharp
await DbInitializer.SeedAsync(app.Services);
```

Also set:

```csharp
options.SignIn.RequireConfirmedAccount = false;
```

so the seeded staff user can sign in without email confirmation.

Add `app.UseAuthentication();` **before** `app.UseAuthorization();`.

### 7. Add MVC controllers

Right-click **Controllers** → **Add** → **Controller** → **MVC Controller - Empty**.

| Controller | Public actions | Staff actions (`[Authorize]`) |
| --- | --- | --- |
| `HomeController` | Index (totals), Transparency, Privacy | — |
| `IncidentsController` | Index | Create, Close |
| `DonationsController` | Create, CreateMoney, CreateGoods | Index (all gifts) |
| `VolunteersController` | Create | Index |
| `CategoriesController` | — | Index, Create |
| `OperationsController` | — | Index (desk) |

`[Authorize]` is what blocks the public from logging fake disasters. Unauthenticated users are redirected to `/Identity/Account/Login`.

### 8. Add views

For each action, right-click the action name in the controller → **Add View** → **Razor View**.

Copy the `.cshtml` files from this repo if you want the Gift of the Givers green layout:

- `Views\Shared\_Layout.cshtml` — dark green navbar, Donate / Incidents / Staff desk
- `Views\Shared\_IncidentCard.cshtml`
- `wwwroot\css\site.css` — branding colours
- Views for Home, Incidents, Donations, Volunteers, Categories, Operations

### 9. Anti-forgery and validation

Every POST form must include:

```html
<form asp-action="Create" method="post">
```

Razor adds the anti-forgery token automatically. Controller actions that accept POST should have `[ValidateAntiForgeryToken]`.

Add this at the bottom of forms:

```html
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

That hooks up jQuery unobtrusive validation for `[Required]` fields.

### 10. Run and test

Work through this checklist (same as 3.3 in the Word document):

1. Public totals visible while signed out.
2. Monetary donation of R0 is rejected.
3. Named monetary donation increases the money total.
4. Anonymous goods donation increases item count and shows **Anonymous**.
5. `/Operations` while signed out → login page.
6. Wrong password → Identity error.
7. Staff login → operations desk.
8. New incident appears on `/Incidents`.
9. Close incident moves it to the closed list.
10. New category appears on the goods donate form.

---

## C. What to click in Visual Studio day to day

| Task | Where |
| --- | --- |
| Edit C# | Solution Explorer → `.cs` file |
| Edit a page | `Views\...\.cshtml` |
| Add a class | Right-click folder → Add → Class |
| Add a controller | Right-click Controllers → Add → Controller |
| Add a view | Right-click an action → Add View |
| See SQL tables (LocalDB only) | View → SQL Server Object Explorer |
| Package Manager Console | Tools → NuGet Package Manager → Package Manager Console |
| Run | F5 |
| Stop | Shift+F5 |
| Commit | Git Changes window (or Origin/GitHub Desktop) |

---

## D. Optional: switch SQLite to LocalDB (SQL Server)

Only do this if the lecturer insists on SQL Server / LocalDB.

1. In **Package Manager Console**:

```
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Uninstall-Package Microsoft.EntityFrameworkCore.Sqlite
```

2. In `Program.cs` change `UseSqlite` to `UseSqlServer`.
3. In `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GiftOfTheGiversApp;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

4. Remove `app.db` from the project if Visual Studio still copies it.
5. Package Manager Console:

```
Remove-Migration
Add-Migration InitialSqlServer
Update-Database
```

If you already have the SQLite migrations, it is cleaner to keep SQLite for Part 1 unless the rubric names SQL Server.

---

## E. Common Visual Studio errors

**“Unable to start program” / port in use**  
Close the extra browser tab, Shift+F5, or change the port in `Properties\launchSettings.json`.

**Login fails for the demo user**  
Password is `Hope1992!` with the exclamation mark. Identity requires a digit, upper case, lower case and a symbol.

**Changes to `.cshtml` do not show**  
Ctrl+Shift+F5 (restart) or save the view and refresh. Razor views compile on the fly in Development.

**Red squiggles after copy-paste**  
Build → **Rebuild Solution**. Wait for NuGet restore.

**HTTPS certificate warning**  
If you run the **https** profile, click **Yes** when Visual Studio asks to trust the IIS Express / Kestrel certificate. The **http** profile avoids that prompt.

---

## F. What to put in the Word document

Paste screenshots from section A.7 into `docs/APPR6312-Part1-SectionC-Tlou.docx`. Add your student number and group names on the cover. Do not paste the whole source code into Word unless the rubric asks for it — the marker should open the `.sln`.
