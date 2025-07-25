<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import EmployeeSignUpModal from "@/components/employee/EmployeeSignUpModal.vue";
import {registerEmployee} from "../../api/userApi";
import { useAppToast } from "../../composables/useAppToast";

const router = useRouter();
const { success, error } = useAppToast();

function goToManager() {
    router.push("/manager");
}

function onClickLogin() {
    router.push("/");
}

async function handleRegister(formData) {
    try {
        const result = await registerEmployee(formData.values);
        success("Registered successfully!");
        router.push("/");
    } catch (err) {
        const errors = err?.response?.data?.message
        if (errors) {
            const message = Object.values(error).flat();
            message.forEach((msg) => error(msg));
        } else {
            error("Something went wrong");

        }
        console.error("Registration error:", err);
    }
}

</script>

<template>
    <div class="flex items-center justify-center min-h-screen">
        <div
            @click="goToManager"
            class="absolute top-8 right-8 text-sm font-bold text-black cursor-pointer"
        >
            Manager Sign Up
        </div>

        <div
            class="flex flex-col items-center gap-6 rounded-2xl shadow-2xl p-10"
        >
            <employee-sign-up-modal @register="handleRegister"/>
            <div class="text-center">
                Already have an account?
                <span
                    class="text-violet-500 cursor-pointer"
                    @click="onClickLogin"
                    >Login</span
                >
            </div>
        </div>
    </div>
</template>
