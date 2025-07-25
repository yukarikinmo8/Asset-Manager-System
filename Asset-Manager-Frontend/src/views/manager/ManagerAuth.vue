<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import ManagerSignUpModal from "../../components/manager/ManagerSignUpModal.vue";
import {registerManager} from "../../api/userApi";
import { useAppToast } from "../../composables/useAppToast";

const router = useRouter();
const { success, error } = useAppToast();

function goToEmployee() {
    router.push("/employee");
}

function onClickLogin() {
    router.push("/");
}

async function handleRegister(formData) {
    try {
        const result = await registerManager(formData.values);
        success("Registered successfully!");
        router.push("/");
    } catch (err) {
        const errors = err?.response?.data?.message
        if (errors) {
            const message = Object.values(error).flat();
            message.forEach((msg) => error(msg));
        } else {
            error("Something went wrong oh no");
        }
        console.error("Registration error:", err);
    }
}

</script>

<template>
    <div class="flex items-center justify-center min-h-screen">
        <div
            @click="goToEmployee"
            class="absolute top-8 right-8 text-sm font-ubuntu font-bold text-black cursor-pointer"
        >
            Employee Sign Up
        </div>

        <div
            class="flex flex-col items-center gap-6 rounded-2xl shadow-2xl p-10"
        >
            <manager-sign-up-modal @register="handleRegister"/>
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
