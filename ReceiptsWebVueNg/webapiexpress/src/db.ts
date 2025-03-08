import mysql, { RowDataPacket } from 'mysql2/promise';

interface IGroup extends RowDataPacket {
    group: string
}

interface IName extends RowDataPacket {
    name: string
}

interface IPriceDateReceipt extends RowDataPacket {
    price: Number,
    dateReceipt: string
}

export class Db {
    private connection!: mysql.Connection;

    async connect() {
        this.connection = await mysql.createConnection({
            host: process.env.MYSQL_HOST,
            user: process.env.MYSQL_USER,
            database: process.env.MYSQL_DB,
            //timezone: '+01:00'
        });
    }

    async getGroupSelectList() {
        const [results]: [IGroup[], any] = await this.connection.query<IGroup[]>(
            "SELECT DISTINCT `group` FROM products ORDER BY `group`"
        );

        return results;
    }

    async getProductsNames(search: string | undefined, group: string | undefined) {
        let nbParams = 0;
        let paramList = [];
        let query = "SELECT DISTINCT name FROM products";

        //optional param search using LIKE
        if (search) {
            nbParams++;
            paramList.push("%" + search + "%");
            query += " WHERE name LIKE ?";
        }

        //optional param group using equal
        if (group) {
            paramList.push(group);
            if (nbParams == 0) {
                query += " WHERE `GROUP`=?";
            } else {
                query += " AND `GROUP`=?";
            }
        }

        query += " ORDER BY name";
        const [results]: [IName[], any] = await this.connection.query<IName[]>(
            query, paramList
        );

        return results;
    }

    async getGetProductPrices(id: string | undefined) {
        if (id) {
            const idNumber: number = parseInt(id, 10);

            const [product]: [IName[], any] = await this.connection.query<IName[]>(
                "SELECT name FROM products WHERE id = ?", [idNumber]
            );

            if (product.length > 0) {
                const name = product[0].name;

                const [products]: [IPriceDateReceipt[], any] = await this.connection.query<IPriceDateReceipt[]>(
                    "SELECT price, DATE_FORMAT(DateReceipt, '%Y-%m-%dT00:00:00') as dateReceipt FROM products WHERE name = ? ORDER BY DateReceipt", [name]
                );

                return products;
            } else {
                return [];
            }
        } else {
            return [];
        }
    }
}