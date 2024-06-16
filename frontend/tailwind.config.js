/** @type {import('tailwindcss').Config} */
const plugin = require("tailwindcss/plugin");

export default {
  content: ["./index.html", "./src/**/*.{js,jsx}"],
  theme: {
    extend: {
      width: {
        "1/16": "6.25%",
      },
      height: {
        "6/7": "85.7412%",
      },
      flexBasis: {
        "1/16": "6.25%",
      },
      textShadow: {
        sm: "0 1px 2px var(--tw-shadow-color)",
        DEFAULT: "0 2px 4px var(--tw-shadow-color)",
        lg: "0 8px 16px var(--tw-shadow-color)",
      },
      colors: {
        google: {
          light: "#5591F5",
          DEFAULT: "#4285F4",
        },
        jet: "#353535",
        caribbean: "#3C6E71",
        platinum: {
          light: "#EBEBEB",
          DEFAULT: "#D9D9D9",
        },
        indigo: "#284B63",
      },
    },
  },
  plugins: [
    plugin(function ({ matchUtilities, theme }) {
      matchUtilities(
        {
          "text-shadow": (value) => ({
            textShadow: value,
          }),
        },
        { values: theme("textShadow") }
      );
    }),
  ],
};
