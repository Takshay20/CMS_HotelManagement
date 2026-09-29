# Royal Paradise Hotel — Dynamic Hotel Booking CMS

A fully dynamic **ASP.NET Core MVC (.NET 8)** hotel booking website with a custom **Admin CMS Panel**,
built with **Dapper + SQL Server**, **jQuery/AJAX**, and **Repository + Service** architecture.

Converted from a static HTML/CSS site into a complete content-managed system: every section of every
public page (sliders, About content, "Why Choose Us", Rooms, Gallery, Facilities, Navbar, Footer, Social
links, Site settings) is editable from the Admin Panel — no code changes needed to update content.

---

## 1. Tech Stack

| Layer            | Technology                                   |
|-------------------|-----------------------------------------------|
| Framework          | ASP.NET Core MVC (.NET 8)                     |
| Data Access        | Dapper (micro-ORM) + Stored Procedures        |
| Database            | Microsoft SQL Server                          |
| Frontend            | Razor Views, jQuery, AJAX, Bootstrap (admin)  |
| Auth                 | Cookie Authentication (Role claim: Admin / Customer) |
| Alerts (Admin)       | SweetAlert2                                   |
| Password Hashing     | PBKDF2 (Rfc2898DeriveBytes) — no external package needed |

---

## 2. Project Structure

```
CMS_HotelBooking/
│
├── Areas/Admin/                  # Admin CMS panel (Area)
│   ├── Controllers/              # 18 controllers - one per module (AJAX JSON endpoints)
│   ├── Views/                    # Drawer + Table UI per module
│   │   └── Shared/               # _AdminLayout, _Slidebar, _Header, _Footer
│   └── Views/_ViewStart.cshtml   # Sets default Admin layout
│
├── Controllers/                  # Public site controllers (Home, About, Room, Gallery,
│                                  #   Contact, Booking, Account)
├── Data/                         # IDbConnectionFactory (SqlConnection factory)
├── Helpers/                      # PasswordHelper (PBKDF2), FileUploadHelper
├── Models/                       # POCOs mapped 1:1 with SQL tables
├── ViewModels/                   # Form-binding + upload wrapper models
├── Repository/
│   ├── BaseRepository.cs         # Shared SqlConnection creation
│   ├── Interfaces/                # One interface per module
│   └── Implementations/           # Dapper + stored-procedure calls
├── Services/
│   ├── Interfaces/
│   └── Implementations/           # Thin business layer over Repositories
├── ViewComponents/                # NavbarViewComponent, FooterViewComponent
│                                   #  (render dynamic navbar/footer on every page)
├── Views/                         # Public site Razor views
│   ├── Home, About, Room, Gallery, Contact, Account, Booking
│   └── Shared/_Layout.cshtml, Shared/Components/{Navbar,Footer}/Default.cshtml
├── wwwroot/
│   ├── css/site.css, js/site.js   # ORIGINAL static-site CSS/JS - untouched
│   ├── images/                    # Original site images (used as seed-data paths)
│   ├── uploads/                   # Admin-uploaded images land here (rooms, gallery, slider...)
│   └── Admin/css/, Admin/js/      # Admin panel styling + one JS file per module
├── SQL/
│   ├── 01_Schema.sql
│   ├── 02_StoredProcedures.sql
│   ├── 03_StoredProcedures_Room.sql
│   ├── 04_StoredProcedures_Content.sql
│   ├── 05_StoredProcedures_SiteConfig.sql
│   └── 06_SeedData.sql
├── Program.cs                     # DI registration, auth, admin auto-seed, routing
└── appsettings.json                # Connection string lives here
```

---

## 3. Database Setup

1. Open **SQL Server Management Studio** (or Azure Data Studio) connected to your local SQL Server.
2. Run the scripts **in this exact order** from the `SQL/` folder:
   1. `01_Schema.sql` - creates the `RoyalParadiseHotelDb` database and all 18 tables
   2. `02_StoredProcedures.sql`
   3. `03_StoredProcedures_Room.sql`
   4. `04_StoredProcedures_Content.sql`
   5. `05_StoredProcedures_SiteConfig.sql`
   6. `06_SeedData.sql` - inserts starter content (rooms, categories, sliders, facilities, gallery, etc.) using the images already bundled in `wwwroot/images`

3. Open `appsettings.json` and update the connection string if your SQL Server instance name / credentials are different:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=RoyalParadiseHotelDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

---

## 4. Running the Project

```bash
cd CMS_HotelBooking
dotnet restore
dotnet run
```

The first time the app starts, it automatically creates a default **Admin** login (if no Admin user
exists yet) - no manual SQL insert needed for this:

```
Admin Panel URL : /Admin/Dashboard/Dashboard  (or click "Admin Panel" after logging in)
Email           : admin@royalparadise.com
Password        : Admin@123
```

> Change this password from the database (or add an admin "change password" screen) before using this
> in production.

Customer registration is available at `/Account/Register` - registration always creates a **Customer**
role account; there is no public way to self-register as Admin (per the requirement).

---

## 5. How the CMS Works

- **One cookie scheme, two roles.** `Users` table has a `Role` column (`Admin` / `Customer`). After
  login, Admins are redirected to `/Admin/Dashboard/Dashboard`; Customers go to the homepage.
- **Every Admin module** (Sliders, Rooms, Gallery, Facilities, Why-Choose-Us, Navbar, Footer, Social
  Media, Site Settings, About page, Feedback approval, Contact messages, Bookings, Users) follows the
  same pattern:
  - A Razor view with a table + a slide-out "drawer" form (`Areas/Admin/Views/{Module}/{Module}.cshtml`)
  - A matching `wwwroot/Admin/js/{Module}.js` file doing all CRUD via **jQuery `$.ajax`**
  - A controller action group: `GetAll`, `GetById`, `Save`, `Delete` returning JSON (`ResponseModel`)
- **Sliders** are page-scoped via a `PageKey` column (`Home`, `Room`, `About`, `Gallery`, `Contact`) -
  the admin can add/remove slides per page independently, exactly as requested.
- **Rooms** support a cover image, multiple gallery images (room-detail-page slider), and a checklist of
  Amenities (master list managed in its own "Amenity" module) - all without touching Room's original CSS.
- **Bookings** are created by logged-in customers from a room's detail page and start in `Pending`
  status; only the Admin's Booking screen can mark them `Approved`/`Rejected`, per the requirement that
  all bookings need admin approval.
- **Guest Feedback** submitted from the Home page starts as unapproved and only shows in the
  testimonials section once an Admin approves it.
- **Navbar and Footer** are rendered through `NavbarViewComponent` / `FooterViewComponent` so the same
  dynamic markup renders on every page without duplicating queries in every controller - fully editable
  from Admin -> Navbar / Footer / Social Media / Site Setting, with the original CSS/JS completely
  untouched.

---

## 6. Known Simplifications (documented intentionally)

Given the size of the original static site, a couple of pragmatic simplifications were made so the CMS
stays maintainable:

- The About page's many static sub-sections (Story / Luxury / Reception / CTA blocks) keep their
  original static copy; only the sections that map to real content types (hero slider, welcome text,
  facilities, why-choose-us, counters) are wired to the database. These can be turned into their own
  CMS modules later using the exact same Repository/Service/Controller/View/JS pattern used everywhere
  else in this project.
- Room `Amenities` are stored as a comma-separated list of `AmenityId`s on the `Room` row (rather than a
  many-to-many join table) to keep the admin form simple - this is easy to normalize into a join table
  later if needed.

---

## 7. Credentials Recap

| Role      | Email                        | Password   |
|-----------|-------------------------------|------------|
| Admin     | admin@royalparadise.com        | Admin@123  |
| Customer  | *(register your own at `/Account/Register`)* | -- |
