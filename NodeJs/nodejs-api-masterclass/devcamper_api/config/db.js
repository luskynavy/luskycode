const mongoose = require('mongoose');

const connectDB = async () => {
    const conn = await mongoose.connect(process.env.MONGO_URI, {
        //useNewUrlParser: true,
        //useCreateIndex: true,
        //useFindAndModifiy: false,
        //useUnifiedTopology: true
    });

    console.log(`MongDB Connected: ${conn.connection.host}`.cyan.underline);
}

module.exports = connectDB;