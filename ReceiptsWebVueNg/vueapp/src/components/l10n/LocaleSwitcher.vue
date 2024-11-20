<script>
    import { supportedLocales } from '../../i18n'
    export default {
        /*methods: {
            // Called when the user selects a new locale
            // from the dropdown
            onLocaleChange(event_) {
                const newLocale = event_.target.value
                // If the selected locale is the same as the
                // active one, do nothing
                if (newLocale === this.$i18n.locale) {
                    return
                }
                // Navigate to the localized root route for
                // the chosen locale
                this.$router.push(`/${newLocale}`)
            },
        },*/
        computed: {
            // Transfrom our supportedLocales object to
            // an array of [{ code: 'en-US', name: 'English' }, ...]
            locales() {
                return Object.keys(supportedLocales).map((code) => ({
                    code,
                    name: supportedLocales[code].name,
                }))
            },
        },
    }
</script>

<template>
    <!-- Using Vue's v-model for two-way binding -->
    <select id="localeSwitcher" v-model="$i18n.locale">
        <option v-for="locale in locales"
                :key="locale.code"
                :value="locale.code">
            {{ locale.name }}
        </option>
    </select>
</template>