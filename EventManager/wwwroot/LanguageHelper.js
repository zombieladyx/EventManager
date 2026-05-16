window.languageHelper = {
    // odczytuje zapisany język z localStorage przeglądarki
    get: function () {
        return localStorage.getItem('selectedLanguage');
    },
    // zapisuje wybrany język do localStorage przeglądarki
    set: function (lang) {
        localStorage.setItem('selectedLanguage', lang);
    }
};