import sqlite3 from "sqlite3";

const execute = async (db, sql, params = []) => {
    if (params && params.length > 0) {
      return new Promise((resolve, reject) => {
        db.run(sql, params, (err) => {
          if (err) reject(err);
          resolve();
        });
      });
    }
    return new Promise((resolve, reject) => {
      db.exec(sql, (err) => {
        if (err) reject(err);
        resolve();
      });
    });
  };

const fetchFirst = async (db, sql, params) => {
    return new Promise((resolve, reject) => {
      db.get(sql, params, (err, row) => {
        if (err) reject(err);
        resolve(row);
      });
    });
  };

console.log("Node.js + SQLite3");


console.time("dbtest");

const filename = "test.db";
const db = new sqlite3.Database(filename);

// Create the table
await execute (db, `
    CREATE TABLE IF NOT EXISTS tabletest (
        id INT NULL,
        col1 INT NULL,
        col2 INT NULL
    )
`);

// Get the size of table
let nb = await fetchFirst(db, 'SELECT COUNT(*) as nb FROM tabletest');
console.log("\n" + nb['nb'] + " elements in tabletest");

// Insert many rows
const maxInsert = 10000;
console.log("\nfor i:0->" + maxInsert + " INSERT INTO tabletest (id, col1, col2) VALUES (i, 2, 3)");
for(let i = 0; i < maxInsert; i++) {
    //db.run("INSERT INTO tabletest (id, col1, col2) VALUES (" + i + ", 2, 3)");
    await execute(db, "INSERT INTO tabletest (id, col1, col2) VALUES (?, 2, 3)", [i]);
}

// Get the size of table
nb = await fetchFirst(db, 'SELECT COUNT(*) as nb FROM tabletest');
console.log("\n" + nb['nb'] + " elements in tabletest");

// Read last line
console.log("\nlast element in table :");
let last = await fetchFirst(db, 'SELECT * FROM tabletest WHERE id = ? - 1', [maxInsert]);
for(let i in last)
{
    console.log(i + ": " + last[i]);
}

console.log("");

// Delete the table
db.run("DROP TABLE tabletest");

db.close();

//18.749s for 10 000 elements on i5 8265U
console.timeEnd("dbtest");

console.log("Done");