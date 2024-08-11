/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./TrustSeal/Pages/**/*.cshtml","./node_modules/flowbite/**/*.js"],
  theme: {
    extend: {},
  },
  plugins: [
    require('flowbite/plugin')
  ],
}

