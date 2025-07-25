/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./index.html",
        "./src/**/*.{vue,js,ts,jsx,tsx}",
        "./node_modules/primevue/**/*.{vue,js,ts}",
    ],
    theme: {
        extend: {
            fontFamily: {
                ubuntu: ["Ubuntu", "sans-serif"],
                opensans: ["Open Sans", "sans-serif"],
            },
        },
    },
    plugins: [],
};
