const totoListApp = {
    data() {
        return {
            todoList: [],
            currentTodo: {
                text: '',
                done: false
            }
        }
    },
    methods: {
        addTodo() {
            if (this.currentTodo.text != '') {
                this.todoList.push(this.currentTodo);
                this.currentTodo = {
                    text: '',
                    done: false
                }
                this.saveTodos();
            }
        },
        clearAll() {
            this.todoList = [];
            this.currentTodo = {
                text: '',
                done: false
            };
            this.saveTodos();
        },
        toggleTodo(todo) {
            todo.done = ! todo.done;
            this.saveTodos();
        },
        saveTodos() {
            localStorage.setItem('todoList', JSON.stringify(this.todoList))
        }
    },
    created() {
        obj = localStorage.getItem('todoList')
        if (obj) {
            this.todoList =  JSON.parse(obj)
        }
    }
};

Vue.createApp(totoListApp).mount('#app');
