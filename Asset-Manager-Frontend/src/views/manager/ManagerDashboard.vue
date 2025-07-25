<script setup>
import AssetModal from "@/components/manager/AssetModal.vue";
import Button from "primevue/button";
import DataView from "primevue/dataview";
import Dropdown from "primevue/dropdown";
import SelectButton from "primevue/selectbutton";
import Tag from "primevue/tag";
import { computed, onMounted, ref, watch } from "vue";
import { getAllAssets } from "../../api/assetApi";
import categories from "../../assets/data/categoryDummy.json";

// State
const selected = ref("All");
const selectedCategory = ref(null);
const statusFilter = ref("All");
const first = ref(0);
const itemsPerPage = 10;
const searchQuery = ref("");
const showModal = ref(false);
const selectedAsset = ref(null);
const assets = ref([]);


watch(statusFilter, async (newVal) => {
    await fetchAssets();
});

onMounted(async () => {
    await fetchAssets();
});

// Options
const categoryOptions = [
    { name: "All" },
    ...categories.map((c) => ({ name: c.name })),
];

const statusOptions = [
    { label: "All", value: "All" },
    { label: "Available", value: "Available" },
    { label: "Borrowed", value: "Borrowed" },
];

// Computed Filters
const filteredAssets = computed(() => {
    const query = searchQuery.value.toLowerCase();

    return assets.value.filter((asset) => {
        const matchesCategory = selectedCategory.value
            ? asset.category === selectedCategory.value
            : true;

        const matchesSearch = asset.name.toLowerCase().includes(query);

        // Status filtering is now handled by the backend
        return matchesCategory && matchesSearch;
    });
});

const fetchAssets = async () => {
    const res = await getAllAssets(statusFilter.value);
    assets.value = res.data;
};

const paginatedAssets = computed(() =>
    filteredAssets.value.slice(first.value, first.value + itemsPerPage)
);

const totalPages = computed(() =>
    Math.ceil(filteredAssets.value.length / itemsPerPage)
);

const currentPage = computed(() => Math.floor(first.value / itemsPerPage) + 1);

// Pagination
function goToPage(page) {
    first.value = (page - 1) * itemsPerPage;
}
function prevPage() {
    if (currentPage.value > 1) goToPage(currentPage.value - 1);
}
function nextPage() {
    if (currentPage.value < totalPages.value) goToPage(currentPage.value + 1);
}

// Category
function selectCategory(categoryObj) {
    selected.value = categoryObj;
    selectedCategory.value =
        categoryObj.name === "All" ? null : categoryObj.name;
    first.value = 0;
}

// Modal
function openModal(asset) {
    selectedAsset.value = asset;
    showModal.value = true;
}
</script>

<template>
    <div class="px-16 py-4">
        <!-- Filter Controls -->
        <div class="mt-6">
            <h2 class="text-lg font-semibold mb-2">Category</h2>
            <div
                class="flex flex-wrap sm:flex-nowrap justify-between items-center gap-4"
            >
                <!-- Category SelectButtons -->
                <SelectButton
                    v-model="selected"
                    :options="categoryOptions"
                    optionLabel="name"
                    @change="selectCategory(selected)"
                    :unstyled="true"
                    class="flex flex-wrap gap-2"
                >
                    <template #option="slotProps">
                        <button
                            class="px-4 py-2 rounded-full border text-sm transition"
                            :class="[
                                selected.name === slotProps.option.name
                                    ? 'bg-cyan-950 border-cyan-500 text-cyan-500'
                                    : 'bg-cyan-50 border-cyan-50 text-black hover:bg-cyan-100',
                            ]"
                        >
                            {{ slotProps.option.name }}
                        </button>
                    </template>
                </SelectButton>

                <!-- Search Input -->
                <input
                    v-model="searchQuery"
                    type="text"
                    placeholder="Search assets..."
                    class="bg-transparent border border-cyan-900 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-1 focus:ring-cyan-500 w-full sm:w-64"
                />
            </div>
        </div>

        <!-- Status Filter Dropdown -->
        <div class="flex gap-4 mt-4 items-center">
            <Dropdown
                v-model="statusFilter"
                :options="statusOptions"
                optionLabel="label"
                optionValue="value"
                placeholder="Select Status"
                class="w-48"
            />
        </div>

        <!-- Asset List -->
        <div class="mt-8">
            <DataView :value="paginatedAssets" :paginator="false">
                <template #list="slotProps">
                    <div class="flex flex-col gap-4">
                        <div
                            v-for="(item, index) in slotProps.items"
                            :key="index"
                            class="flex flex-col sm:flex-row items-center gap-4 p-4 border rounded-lg"
                        >
                            <!-- Image -->
                            <div class="w-full sm:w-40">
                                <img
                                    :src="item.imageUrl"
                                    :alt="item.name"
                                    class="rounded w-full object-cover h-32"
                                />
                            </div>

                            <!-- Info -->
                            <div class="flex-1 flex flex-col gap-1">
                                <Tag
                                    :value="item.category"
                                    severity="info"
                                    class="w-fit text-xs"
                                />
                                <h3 class="text-lg font-semibold text-black">
                                    {{ item.name }}
                                </h3>
                                <p class="text-sm text-gray-700 line-clamp-2">
                                    {{ item.description }}
                                </p>
                            </div>

                            <!-- View Button -->
                            <div>
                                <Button
                                    icon="pi pi-eye"
                                    label="View"
                                    @click="openModal(item)"
                                    class="p-button-outlined"
                                    severity="primary"
                                />
                            </div>
                        </div>
                    </div>
                </template>
            </DataView>
        </div>

        <!-- Pagination -->
        <div class="mt-6 flex justify-center">
            <div
                class="flex items-center gap-4 border border-cyan-500 rounded-full px-4 py-2 bg-transparent"
            >
                <button
                    @click="prevPage"
                    :disabled="currentPage === 1"
                    class="text-sm px-2 py-1 hover:text-cyan-500 disabled:opacity-50"
                >
                    Prev
                </button>
                <span class="text-sm text-cyan-500">
                    Page {{ currentPage }} of {{ totalPages }}
                </span>
                <button
                    @click="nextPage"
                    :disabled="currentPage === totalPages"
                    class="text-sm px-2 py-1 hover:text-cyan-500 disabled:opacity-50"
                >
                    Next
                </button>
            </div>
        </div>

        <!-- Asset Modal -->
        <AssetModal v-model="showModal" :asset="selectedAsset" />
    </div>
</template>

<style scoped>
.p-dataview .p-dataview-list .p-dataview-item {
    background-color: transparent !important;
    box-shadow: none !important;
}
</style>
