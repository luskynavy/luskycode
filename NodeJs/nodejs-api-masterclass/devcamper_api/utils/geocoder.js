/*
const NodeGeocoder = require('node-geocoder');

const options = {
  provider: process.env.GEOCODER_PROVIDER,
  httpAdapter: 'https',
  apiKey: process.env.GEOCODER_API_KEY,
  formatter: null
};

const geocoder = NodeGeocoder(options);
*/

class fakeNodeGeocoder {
    async geocode(address) {
        const loc = [];
        loc[0] = {
            latitude: 12.34,
            longitude: 56.78,
            formattedAddress: '21 Baker street',
            streetName: 'baker street',
            city: 'London',
            stateCode: '123',
            country: 'UK'
        };
        return loc;
    }
}


const geocoder = new fakeNodeGeocoder();


module.exports = geocoder;
