import express, { Request, Response } from 'express'
import dotenv from "dotenv";
import mysql from 'mysql2/promise';
import fs from 'fs';
//import http from 'http';
import https from 'https';
import cors from 'cors';
import { Db } from './db';

async function main() {
    dotenv.config({ path: './config.env' });

    let connection: mysql.Connection;
    let db = new Db();

    try {
        // Create the connection to database
        db.connect();

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
    app.get('/Products/:id', async (req: Request, res: Response) => {
        let result = {
            "id": 1,
            "name": "BRIOCHE",
            "group": "VIENNOISERIE INDUSTRIEL",
            "price": 3.08,
            "dateReceipt": "2025-03-08T23:03:28.552Z",
            "sourceName": "string",
            "sourceLine": 10,
            "fullData": "BRIOCHE   (T)        3,08 €  11"
        };

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
        const results = await db.getGroupSelectList();

        let groups: string[] = [];

        //Default  empty entry for select
        groups.push("");

        for (let i in results) {
            groups.push(results[i].group);
        }

        res.status(200).json(groups);
    });

    // GET /GetProductPrices?id=1
    app.get('/GetProductPrices', async (req: Request, res: Response) => {
        if (req.query.id) {
            const results = await db.getGetProductPrices(req.query.id as string);
            res.status(200).json(results);
        } else {
            res.status(200).json("[]");
        }
    });

    // GET /ProductsNames?search=a&group=fruits
    app.get('/ProductsNames', async (req: Request, res: Response) => {
        const results = await db.getProductsNames(
            req.query.search as string | undefined,
            req.query.group as string | undefined
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

