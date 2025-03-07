## Project creation

Created with :

```sh
npm init
npm i -D typescript ts-node nodemon
npm i express @types/express dotenv mysql2
tsc --init
```

## Certificate creation (10 years, created with cygwin)

```sh
openssl req -x509 -nodes -days 3650 -newkey rsa:2048 -keyout ./selfsigned.key -out selfsigned.crt
```
