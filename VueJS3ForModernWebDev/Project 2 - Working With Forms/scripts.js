members = [
    {
        fname: 'John',
        lname: 'Lennon',
        instrument: 'Acoustic Guitar'
    },
    {
        fname: 'George',
        lname: 'Harrison',
        instrument: 'Electric Guitar'
    }
]

const handlingForms = {

    data() {
        return {
            members: window.members,
            newMember: {
                fname: "",
                lname: "",
                instrument: ""
            }
        }
    },
    methods: {
        addMember() {
            this.members.push(this.newMember)
            this.newMember = {
                fname: "",
                lname: "",
                instrument: ""
            }
        }
    }
};

Vue.createApp(handlingForms).mount('#app');
