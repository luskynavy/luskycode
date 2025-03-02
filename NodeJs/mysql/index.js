// Get the client
import mysql from 'mysql2/promise';


// A simple SELECT query
try {
    console.log("Node.js + Mysql2");

    console.time("dbtest");

    // Create the connection to database
    const connection = await mysql.createConnection({
        host: 'localhost',
        user: 'root',
        database: 'test',
    });

    // Create the table
    await connection.execute("CREATE TABLE IF NOT EXISTS `test`.`tabletest` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = MYISAM;");

    // Get the size of table
    var [results, fields] = await connection.query(
      'SELECT COUNT(*) as nb FROM tabletest'
    );
    console.log("\n" + results[0]['nb'] + " elements in tabletest");

    // Insert many rows
    const maxInsert = 10000;
    console.log("\nfor i:0->" + maxInsert + " INSERT INTO tabletest (id, col1, col2) VALUES (i, 2, 3)");
    for(let i = 0; i < maxInsert; i++) {
        await connection.execute("INSERT INTO tabletest (id, col1, col2) VALUES (?, 2, 3)", [i]);
    }

    // Get the size of table
    [results, fields] = await connection.query(
        'SELECT COUNT(*) as nb FROM tabletest'
    );
    console.log("\n" + results[0]['nb'] + " elements in tabletest");

    // Read last line
    console.log("\nlast element in table :");
    [results, fields] = await connection.query(
    'SELECT * FROM tabletest WHERE id = ? - 1', [maxInsert]
    );
    for(let i in results[0])
    {
        console.log(i + ": " + results[0][i]);
    }

    console.log("");

    // Delete the table
    await connection.execute("DROP TABLE `test`.`tabletest`;");

    connection.end();

    //3.112s for 10 000 elements on i5 8265U
    console.timeEnd("dbtest");

    console.log("Done");
} catch (err) {
    console.log(err);
}


