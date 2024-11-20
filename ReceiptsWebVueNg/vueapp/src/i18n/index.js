import { createI18n } from 'vue-i18n'

import messages from './messages'

// Set and expose the default locale
export const defaultLocale = 'fr'

// Private instance of VueI18n object
let _i18n

// Initializer
function setup(options = { locale: defaultLocale }) {
    _i18n = createI18n({
        locale: options.locale,
        fallbackLocale: defaultLocale,
        messages,
        //runtimeOnly: false,
        /*numberFormats,
        datetimeFormats,
        pluralizationRules: {
            'ar-EG': arabicPluralRules,
        },*/
    })

    setLocale(options.locale)

    return _i18n
}

// Sets the active locale.
function setLocale(newLocale) {
    _i18n.global.locale = newLocale
}

// Public interface
export default {
    // Expose the VueI18n instance via a getter
    get vueI18n() {
        return _i18n
    },
    setup,
    setLocale,
}

// Using a { localeCode: localeData } structure
// allows us to add metadata, like a name, to each
// locale as our needs grow.
export const supportedLocales = {
    'en': { name: 'English' },
    'fr': { name: 'Français' },
}