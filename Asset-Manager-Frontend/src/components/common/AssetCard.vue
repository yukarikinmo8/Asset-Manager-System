<script setup>
defineProps({
    name: String,
    description: String,
    image: String,
});
</script>

<template>
    <div class="relative bg-yellow-50 rounded-lg shadow-sm min-h-[200px] overflow-hidden cursor-pointer hover:shadow-md transition">
        <img :src="image" alt="Asset" class="w-full h-[200px] object-cover" />
        <div class="absolute bottom-0 w-full bg-black/20 text-white text-sm font-semibold px-2 py-1 truncate">
            {{ name }}
        </div>
    </div>
</template>
