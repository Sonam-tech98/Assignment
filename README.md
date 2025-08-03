## 🗄️ Database Setup

To create the SQL Server database:

1. Open **SQL Server Management Studio (SSMS)**
2. Open the file at `Assignment/Database/AssignmentDB.sql`
3. Execute the script to create the database and seed data (if any)

Make sure to update your `appsettings.json` with your connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SONAM\\SQLEXPRESS;Database=TestingApi1;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
