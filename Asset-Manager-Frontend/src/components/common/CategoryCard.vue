<script setup>
defineProps({
    name: String,
    image: String,
});
defineEmits(["click"]);
</script>

<template>
    <div
        class="w-full aspect-[4/3] rounded-xl overflow-hidden shadow-md cursor-pointer hover:scale-105 transition relative"
        @click="$emit('click')"
    >
        <img
            :src="image"
            alt="category image"
            class="w-full h-full object-cover"
        />
        <div
            class="absolute bottom-0 w-full bg-black/50 text-white text-center text-sm font-semibold px-2 py-1 truncate"
        >
            {{ name }}
        </div>
    </div>
</template>
