import EmployeeLayout from "@/layouts/EmployeeLayout.vue";
import ManagerLayout from "@/layouts/ManagerLayout.vue";
import EmployeeAuth from "@/views/employee/EmployeeAuth.vue";
import EmployeeDashboard from "@/views/employee/EmployeeDashboard.vue";
import EmployeeRequest from "@/views/employee/EmployeeRequest.vue";
import ManagerAuth from "@/views/manager/ManagerAuth.vue";
import ManagerDashboard from "@/views/manager/ManagerDashboard.vue";
import ManagerRequest from "@/views/manager/ManagerRequest.vue";
import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "../stores/apiStore";
import index from "../views/index.vue";

const routes = [
    { path: "/", component: index },
    { path: "/employee", component: EmployeeAuth },
    { path: "/manager", component: ManagerAuth },
    {
        path: "/employee",
        component: EmployeeLayout,
        children: [
            {
                path: "dashboard",
                component: EmployeeDashboard,
                meta: {
                    title: "Assets",
                    requiresAuth: true,
                    roles: ["employee", "assetmanager"],
                },
            },
            {
                path: "requests",
                component: EmployeeRequest,
                meta: {
                    title: "Requests History",
                    requiresAuth: true,
                    roles: ["employee", "assetmanager"],
                },
            },
        ],
    },
    {
        path: "/manager",
        component: ManagerLayout,
        children: [
            {
                path: "dashboard",
                component: ManagerDashboard,
                meta: {
                    title: "Assets",
                    requiresAuth: true,
                    roles: ["assetmanager"],
                },
            },
            {
                path: "requests",
                component: ManagerRequest,
                meta: {
                    title: "Requests History",
                    requiresAuth: true,
                    roles: ["assetmanager"],
                },
            },
        ],
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach(async (to, _from, next) => {
    const auth = useAuthStore();

    // If store is empty on refresh, fetch user to restore session
    if (!auth.isAuthenticated) {
        await auth.fetchUser();
    }

    if (to.meta.requiresAuth && !auth.isAuthenticated) {
        next("/"); // Redirect to login
    } else {
        next(); // Continue as normal
    }
});

export default router;