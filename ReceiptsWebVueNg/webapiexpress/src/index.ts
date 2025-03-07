import express, { Request, Response } from 'express'
import dotenv from "dotenv";
import mysql from 'mysql2/promise';
import fs from 'fs';
//import http from 'http';
import https from 'https';

async function main() {
    dotenv.config({ path: './config.env' });

    let connection: mysql.Connection;

    try {
        // Create the connection to database
        connection = await mysql.createConnection({
            host: process.env.MYSQL_HOST,
            user: process.env.MYSQL_USER,
            database: process.env.MYSQL_DB
        });
    } catch (error) {
        console.log(error);
    }

    const app = express()
    const port = process.env.PORT || 3000;

    // Get certificate
    var privateKey = fs.readFileSync('sslcert/selfsigned.key', 'utf8');
    var certificate = fs.readFileSync('sslcert/selfsigned.crt', 'utf8');

    var credentials = { key: privateKey, cert: certificate };
    var httpsServer = https.createServer(credentials, app);

    //var httpServer = http.createServer(app);

    app.get('/GroupSelectList', async (req: Request, res: Response) => {
        const [results, fields]: [any, any] = await connection.query(
            "SELECT DISTINCT `group` FROM products ORDER BY `group`"
        );
        let groups: string[] = [];

        //Default  empty entry for select
        groups.push("");

        for (let i in results) {
            groups.push(results[i].group);
        }
        res.status(200).json(groups);
    })

    httpsServer.listen(port, () => {
        console.log(`App listening at https://localhost:${port}`)
    });

    //httpServer.listen(port, () => {
    //    console.log(`App listening at http://localhost:${port}`)
    //});
}

main();

