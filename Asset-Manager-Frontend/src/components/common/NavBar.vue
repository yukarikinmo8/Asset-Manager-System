<script setup>
import { ref } from "vue";
import { Menu } from "lucide-vue-next";
import Sidebar from "@/components/common/Sidebar.vue";

const isSidebarOpen = ref(false);
</script>

<template>
    <div>
        <!-- Sidebar with animation -->
        <transition name="slide">
            <Sidebar v-if="isSidebarOpen" />
        </transition>

        <!-- Top Nav that shifts with sidebar -->
        <nav
            :class="[
                'shadow transition-all duration-300',
                isSidebarOpen ? 'ml-64' : 'ml-0',
            ]"
        >
            <div class="px-4 sm:px-6 lg:px-8">
                <div class="flex items-center h-16">
                    <Menu
                        class="w-6 h-6 text-brand-primary cursor-pointer"
                        @click="isSidebarOpen = !isSidebarOpen"
                    />
                </div>
            </div>
        </nav>
    </div>
</template>

<style scoped>
.slide-enter-active,
.slide-leave-active {
    transition: transform 0.3s ease;
}
.slide-enter-from,
.slide-leave-to {
    transform: translateX(-100%);
}
.slide-enter-to,
.slide-leave-from {
    transform: translateX(0);
}
</style>
