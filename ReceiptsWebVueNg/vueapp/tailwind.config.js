/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./src/**/*.{html,vue,js}"],
    prefixZZZ: 'tw-',
    theme: {
        colors: {
            'silver': '#ecebff',
            'grey': '#eeeeee',
            'red': '#ff0000',
        },
        extend: {},
    },
    plugins: [],
}