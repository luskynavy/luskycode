import express, { Request, Response } from 'express'
import dotenv from "dotenv";
import mysql from 'mysql2/promise';
import fs from 'fs';
//import http from 'http';
import https from 'https';
import cors from 'cors';

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

    // Add a list of allowed origins.
    const allowedOrigins = ['http://localhost:5173'];

    const options: cors.CorsOptions = {
        origin: allowedOrigins
    };

    // Then pass these options to cors:
    app.use(cors(options));

    // Get certificate
    var privateKey = fs.readFileSync('sslcert/selfsigned.key', 'utf8');
    var certificate = fs.readFileSync('sslcert/selfsigned.crt', 'utf8');

    var credentials = { key: privateKey, cert: certificate };
    var httpsServer = https.createServer(credentials, app);

    //var httpServer = http.createServer(app);

    // Empty result for quick test
    app.get('/Products', async (req: Request, res: Response) => {
        let result = {
            "pageIndex": 1,
            "totalPages": 0,
            "data": [],
            "hasPreviousPage": false,
            "hasNextPage": false
        }
        res.status(200).json(result);
    });

    // Empty result for quick test
    app.get('/GroupProducts', async (req: Request, res: Response) => {
        let result = {
            "pageIndex": 1,
            "totalPages": 0,
            "data": [],
            "hasPreviousPage": false,
            "hasNextPage": false
        }
        res.status(200).json(result);
    });

    // GET /GroupSelectList
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
    });

    // Empty result for quick test
    app.get('/GetProductPrices', async (req: Request, res: Response) => {
        res.status(200).json("[]");
    });

    // GET /ProductsNames?search=a&group=fruits
    app.get('/ProductsNames', async (req: Request, res: Response) => {
        let nbParams = 0;
        let paramList = [];
        let query = "SELECT DISTINCT name FROM products";

        //optional param search using LIKE
        if (req.query.search) {
            nbParams++;
            paramList.push("%" + req.query.search + "%");
            query += " WHERE name LIKE ?";
        }

        //optional param group using equal
        if (req.query.group) {
            paramList.push(req.query.group);
            if (nbParams == 0) {
                query += " WHERE `GROUP`=?";
            } else {
                query += " AND `GROUP`=?";
            }
        }

        query += " ORDER BY name";
        const [results, fields]: [any, any] = await connection.query(
            query, paramList
        );

        let names: string[] = [];

        for (let i in results) {
            names.push(results[i].name);
        }
        res.status(200).json(names);
    });


    httpsServer.listen(port, () => {
        console.log(`App listening at https://localhost:${port}`)
    });

    //httpServer.listen(port, () => {
    //    console.log(`App listening at http://localhost:${port}`)
    //});
}

main();

