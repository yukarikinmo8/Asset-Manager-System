<script setup>
import { ref, provide, computed } from "vue";
import { useRoute } from "vue-router";
import Sidebar from "@/components/common/Sidebar.vue";
import { Menu } from "lucide-vue-next";

const isSidebarOpen = ref(true);
provide("isSidebarOpen", isSidebarOpen); // Optional: if used by children

const route = useRoute();

const currentPage = computed(() => {
    return route.meta.title || route.name || route.path;
});
</script>

<template>
    <div class="flex">
        <!-- Sidebar -->
        <Sidebar
            v-if="isSidebarOpen"
            class="w-64 fixed top-0 left-0 h-full shadow z-50 bg-[#051e2e]"
        />

        <!-- Main Content -->
        <div
            :class="[
                isSidebarOpen ? 'ml-64' : 'ml-0',
                'transition-all duration-300 flex-1',
            ]"
        >
            <!-- Top Nav -->
            <nav class="h-16 px-6 flex items-center shadow-lg bg-[#051e2e]">
                <Menu
                    @click="isSidebarOpen = !isSidebarOpen"
                    class="w-6 h-6 text-brand-primary cursor-pointer"
                />
                <span class="ml-4 text-lg text-cyan-50 font-medium capitalize">
                    {{ currentPage }}
                </span>
            </nav>

            <!-- Page Content -->
            <div class="p-4">
                <RouterView />
            </div>
        </div>
    </div>
</template>
