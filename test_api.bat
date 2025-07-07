@echo off
echo ========================================
echo Yedidim API Testing Setup
echo ========================================
echo.

echo Starting the backend server...
echo Server will be available at: https://localhost:7145
echo Swagger UI will be available at: https://localhost:7145/swagger
echo.

cd Server
echo Running: dotnet run
echo.
echo Press Ctrl+C to stop the server when done testing
echo.
dotnet run

pause 