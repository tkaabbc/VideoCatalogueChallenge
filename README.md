# VideoCatalogue

A simple video catalogue demo built with **.NET 8 MVC** on the backend and **TypeScript + KnockoutJS + axios** on the frontend.

---

### Tech Stack
- **Backend**: ASP.NET Core MVC 8
- **Frontend**: TypeScript, KnockoutJS, axios for HTTP request
- **Build/Test**: `dotnet`, `npm`, Moq/xUnit for unit tests

---

### Key Directory Layout
```
VideoCatalogue.Web/
 ├── Controllers/      # MVC controllers
 ├── Services/         # VideoService + storage providers
 ├── Scripts/          # TS source
 ├── Views/            # Razor views + page-specific CSS
 ├── Validators/       # FluentValidation validators (e.g., file upload rules)
 ├── Models/           # DTOs / view models shared across layers
 ├── Common/           # Shared constants & helpers (file size, extensions, errors)
 ├── Configuration/    # Options bound from appsettings (e.g., storage paths)
 └── wwwroot/          # Compiled JS, CSS, static assets

VideoCatalogue.Tests/
 ├── Controllers/VideoApiControllerTests.cs
 ├── Services/VideoServiceTests.cs
 ├── Utils/Helper.cs   # shared test helpers
```

---

### Running the Project

1. **Backend**
   - `cd VideoCatalogue.Web`
   - `dotnet run` (or `dotnet watch`)

2. **Frontend TypeScript build**
   - `cd VideoCatalogue.Web`
   - `npm install` (first time)
   - `npm run dev:ts` (watch & compile TS to `wwwroot/js`)

3. **Open in browser**
   - After backend launches, navigate to `http://localhost:{port}` (check console output for actual port)

---

### Running Unit Tests
```
cd VideoCatalogue.Tests
dotnet test
```
Tests currently cover:
- `VideoService` logic (file saving, catalogue ordering)
- `VideoApiController` behaviors (validation, service interactions)

---

### Future Enhancements
- Add unit tests for `LocalFileStorageProvider`
- Add frontend unit tests for utility functions
- Add structured logging

Hope you enjoy reviewing the project! Feel free to reach out with any questions.