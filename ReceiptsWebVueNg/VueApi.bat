cd webapi\bin\Debug\net8.0
start "" webapi.exe
cd ..\..\..\..\vueapp
start "" "http://localhost:5173"
npm run preview