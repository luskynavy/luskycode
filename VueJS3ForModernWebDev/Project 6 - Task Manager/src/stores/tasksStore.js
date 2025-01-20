import { defineStore } from 'pinia'
import { reactive, ref, computed } from 'vue';

export const useTasksStore = defineStore('tasks', () => {
    // reactive for arrays and objects
    let tasks= reactive(JSON.parse(localStorage.getItem('tasks')) || []);

    let modalIsActive = ref(false);

    let filterBy = ref('');

    function setFilter(value) {
        filterBy.value = value
    }

    const filteredTasks = computed(() => {
        switch (filterBy.value) {
          case 'todo':
            return tasks.filter(task => !task.completed)
          case 'done':
            return tasks.filter(task => task.completed)
          default:
            return tasks;
        }
    })

    function addTask(newTask) {
        if (newTask.name && newTask.description) {
            newTask.id = tasks.length ? Math.max(...tasks.map(task => task.id)) + 1 : 1;
            tasks.push(newTask);
            newTask = { completed: false };

            closeModal();
        } else {
            alert('Please enter a name and a desscription.')
        }
    };

    function toggleCompleted(id) {
        tasks.forEach(task => {
          if (task.id == id) {
            task.completed = !task.completed;
          }
        });
    }

    function openModal() {
        modalIsActive.value = true;
    }

    function closeModal() {
        modalIsActive.value = false;
    }

    return { tasks, filterBy, setFilter, filteredTasks, addTask, toggleCompleted, modalIsActive, openModal, closeModal }
})