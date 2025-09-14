import { createApp } from 'vue';
import App from './App.vue';
import {createRouter, createWebHistory} from 'vue-router';
import ListaPaises from './components/CountriesList.vue';
import FormularioPais from './components/CountryForm.vue';



const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: "Home", component: ListaPaises },
    { path: '/country', name: "Country", component: FormularioPais }
  ]
});

createApp(App).use(router).mount('#app');
