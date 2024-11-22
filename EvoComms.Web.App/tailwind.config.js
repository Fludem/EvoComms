/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    "./wwwroot/assets/js/**/*.js",
    "./Pages/**/*.{razor,cshtml}",
    "./Shared/**/*.{razor,cshtml}",
    "./Components/**/*.{razor,cshtml}",
    "./wwwroot/assets/vendor/preline/dist/*.js"
  ],
  theme: {
    fontFamily: {
      sans: ['Geist', 'Inter', 'sans-serif'],
      serif: ['Geist', 'Inter', 'serif'],
      'input': ['Inter', 'sans-serif'],
    },
    extend: {
      fontWeight: {
        thin: '100',
        extralight: '200',
        light: '300',
        normal: '400',
        medium: '500',
        semibold: '600',
        bold: '700',
        extrabold: '800',
        black: '900',
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
    require('tailwindcss-motion'),
    require('preline/plugin'),
  ],
}

