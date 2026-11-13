# CampusRooms – Lab Exercise 02

**Student:** Justin Johnson  
**Course:** CWEB2302 Web Development II  
**College:** Excelsior Community College – School of Computer Studies  
**Semester:** 1, 2026/27  

**Assignment:** CampusRooms: buildings, logins and a JSON API  
**Due:** Sunday 15 November 2026, 11:59 pm  
**Weight:** 15% (100 marks)

---

## What this project does

An ASP.NET Core 8 MVC application that manages teaching rooms on campus.

- Rooms belong to buildings (one-to-many relationship)
- Staff can add and edit rooms; only Admins can delete
- Identity authentication + roles (Admin / Staff)
- JSON API at `/api/rooms` for the timetabling program
- Raw SQL query for large rooms

## How to run

1. Open the solution in Visual Studio 2022 (or later) or use the .NET 8 SDK.
2. Update the connection string in `appsettings.json` if needed (LocalDB is the default).
3. In Package Manager Console:

   ```
   Update-Database
   ```

4. Press **F5**.  
5. Register these test accounts (then restart the app so the Admin role is applied):

   | Email              | Role after restart |
   |--------------------|--------------------|
   | admin@ecc.test     | Admin              |
   | staff@ecc.test     | (make Staff via /users) |
   | user@ecc.test      | none               |

6. Test the API with the included `CampusRooms.http` file (update the `@host` port).

## Project structure

```
CampusRooms/
├── Controllers/
│   ├── RoomsController.cs      # MVC pages + /rooms/seats/{min}
│   ├── BuildingsController.cs  # Building details page
│   ├── UsersController.cs      # Admin users page + Make Staff
│   └── ApiRoomsController.cs   # JSON API
├── Models/
│   ├── Building.cs
│   ├── Room.cs
│   ├── RoomDto.cs
│   └── RoomInput.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Views/
├── CampusRooms.http            # 12 API test requests
└── appsettings.json            # AdminEmail + connection string
```

## Features completed

- [x] Part A – Buildings linked to Rooms (Tasks 1–4)
- [x] Part B – Logins and roles (Tasks 5–7)
- [x] Part C – JSON API (Tasks 8–10)
- [x] Part D – Builds cleanly from migrations (Task 11)

## Submission

Public GitHub repository:  
https://github.com/justinjohnson/CampusRooms   ← replace with your actual repo URL after pushing

---

*Justin Johnson – CWEB2302 Lab Exercise 02*
