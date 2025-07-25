import { createApp } from "vue";
import App from "./App.vue";
import PrimeVue from "primevue/config";
import Lara from "@primevue/themes/lara";
import router from "./router";
import { definePreset } from "@primeuix/themes";
import "primeicons/primeicons.css";
import "./styles/style.css";

//PrimeVue Components
import Button from "primevue/button";
import Card from "primevue/card";
import DatePicker from "primevue/datepicker";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Tag from "primevue/tag";
import { createPinia } from "pinia";
import ToastService from "primevue/toastservice";
import Toast from "primevue/toast";



const app = createApp(App);

const MyPreset = definePreset(Lara, {
    semantic: {
        colorScheme: {
            primary: {
                0: "#e0e0e0",
                50: "{blue.50}",
                100: "{cyan.100}",
                200: "{cyan.200}",
                300: "{cyan.300}",
                400: "{cyan.400}",
                500: "{cyan.500}",
                600: "{cyan.600}",
                700: "{cyan.700}",
                800: "{cyan.800}",
                900: "{cyan.900}",
                950: "{cyan.950}",
            },
            secondary: {
                0: "#ffffff",
                50: "{stone.50}",
                100: "{stone.100}",
                200: "{stone.200}",
                300: "{stone.300}",
                400: "{stone.400}",
                500: "{stone.500}",
                600: "{stone.600}",
                700: "{stone.700}",
                800: "{stone.800}",
                900: "{stone.900}",
                950: "{stone.950}",
            },
        },
    },
    
});

app.use(PrimeVue, {
    theme: {
        preset: MyPreset,
        options: {
            darkModeSelector: ".my-app-dark",
        },
    },
});
app.use(ToastService);
app.component("Toast", Toast);

const pinia = createPinia();
app.use(pinia);
app.component("Button", Button);
app.component("Card", Card);
app.component("DatePicker", DatePicker);
app.component("DataTable", DataTable);
app.component("Column", Column);
app.component("Tag", Tag);

app.use(router);
app.mount("#app");
