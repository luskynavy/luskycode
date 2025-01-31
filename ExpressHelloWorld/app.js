const express = require('express')
const app = express()
const port = 3000

app.get('/', (req, res) => {
  res.send('Hello World!')
})

app.get('/getList', (req, res) => {
  res.status(200);
  res.send('[{id:1,name:"John"},{id:2,name:"Marc"},{id:3,name:"Huggy"}]')
})

app.get('/reservations', (req, res) => {
  res.status(200);
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Methods", "GET");
  res.header("Access-Control-Allow-Headers", "X-Requested-With");
  res.send('[{"checkInDate":"2025-01-10","checkOutDate":"2025-01-14","guestName":"11","guestEmail":"a@b.c","roomNumber":10,"id":"1737982320051"},'+
  '{"checkInDate":"2023-01-10","checkOutDate":"2025-01-14","guestName":"22","guestEmail":"b@b.c","roomNumber":666,"id":"173720051"},'+
  '{"checkInDate":"2024-01-10","checkOutDate":"2025-01-14","guestName":"33","guestEmail":"c@b.c","roomNumber":33,"id":"222982320051"}]')
})


app.listen(port, () => {
  console.log(`Example app listening on port ${port}`)
})