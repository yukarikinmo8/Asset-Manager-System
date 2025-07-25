<script setup>
import { format } from "date-fns";
import { X } from "lucide-vue-next";
import { ref } from "vue";

const props = defineProps({
    modelValue: Boolean,
    asset: Object,
});
const emit = defineEmits(["update:modelValue"]);

const startDate = ref(null);
const endDate = ref(null);

const closeModal = () => {
    emit("update:modelValue", false);
};

const onClickRent = () => {
    const formattedStart = startDate.value
        ? format(startDate.value, "yyyy-MM-dd")
        : null;
    const formattedEnd = endDate.value
        ? format(endDate.value, "yyyy-MM-dd")
        : null;
    closeModal();
};
</script>

<template>
    <transition name="fade">
        <div
            v-if="modelValue"
            class="fixed inset-0 bg-black/50 flex items-center justify-center z-50"
        >
            <div class="bg-white rounded-lg p-6 shadow-lg max-w-[90vw] max-h-[90vh] overflow-auto relative">
                <button
                    @click="closeModal"
                    class="absolute top-4 right-4 text-gray-500 hover:text-black"
                >
                    <X class="w-6 h-6" />
                </button>
                <div class="flex gap-6">
                    <img
                        :src="asset.imageUrl"
                        alt="Asset Image"
                        class="w-[300px] h-[300px] object-cover rounded-lg"
                    />
                    <div class="flex flex-col justify-center">
                        <h1 class="text-cyan-950 text-xl font-semibold mb-4">
                            {{ asset.name }}
                        </h1>
                        <p class="text-gray-700 max-w-md">
                            {{ asset.description }}
                        </p>
                        <h1 class="text-cyan-950 text-xl font-semibold mt-4">
                            € {{ asset.amount.toFixed(2) }}
                        </h1>
                    </div>
                </div>
            </div>
        </div>
    </transition>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
