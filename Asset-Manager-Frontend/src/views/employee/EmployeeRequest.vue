<script setup>
import productsData from "@/assets/data/requestDummy.json";
import Column from "primevue/column";
import DataTable from "primevue/datatable";
import IconField from "primevue/iconfield";
import InputIcon from "primevue/inputicon";
import InputText from "primevue/inputtext";
import Tag from "primevue/tag";
import { computed, onMounted, ref, watch } from "vue";
import { getEmployeeRentals } from "../../api/rentalApi";
import { useAuthStore } from "../../stores/apiStore";

// Refs
const allProducts = ref(productsData);
const selectedProducts = ref(null);
const search = ref("");
const statusFilter = ref("all");
const rents = ref([]);

const authStore = useAuthStore();
const employeeId = ref(authStore.user?.id || "");

const fetchRents = async (id) => {
    try {
        const response = await getEmployeeRentals(id);
        console.log("Fetched rents:", response.data);
        rents.value = response.data;
    } catch (error) {
        console.error("Error fetching rents:", error);
    }
};

// Filter options
const filterOptions = [
    { label: "All", value: "all" },
    { label: "Active Request", value: "active" },
    { label: "Past Request", value: "past" },
];

// Filtering Logic
const filteredProducts = computed(() => {
    let data = rents.value;

    // Filter by status
    if (statusFilter.value === "active") {
        data = data.filter((item) =>
            ["Pending", "Confirmed"].includes(item.rentalStatus)
        );
    } else if (statusFilter.value === "past") {
        data = data.filter((item) => item.rentalStatus === "Returned");
    }

    // Global search
    if (search.value.trim()) {
        const q = search.value.toLowerCase();
        data = data.filter((item) =>
            Object.values(item).some((val) =>
                String(val).toLowerCase().includes(q)
            )
        );
    }

    return data;
});

onMounted(async () => {
    if (employeeId.value) {
        fetchRents(employeeId.value);
    }
});

watch(
    () => authStore.user,
    (newUser) => {
        if (newUser && newUser.id) {
            employeeId.value = newUser.id;
            fetchRents(newUser.id);
        }
    },
    { immediate: false }
);

// Helpers
function getStatusLabel(status) {
    switch (status) {
        case "Returned":
            return "success";
        case "Confirmed":
            return "warning";
        case "Pending":
            return "danger";
        default:
            return null;
    }
}
function formatDate(date) {
    return new Date(date).toLocaleDateString();
}
function formatCurrency(value) {
    const num = Number(value);
    if (isNaN(num)) return "₱0.00";
    return `₱${num.toFixed(2)}`;
}
</script>

<template>
    <div class="px-16 py-4">
        <!-- Filters -->
        <div class="flex justify-between items-center mb-4">
            <div class="flex gap-2 items-center">
                <label for="statusSelect" class="text-sm">Filter Status:</label>
                <select
                    v-model="statusFilter"
                    id="statusSelect"
                    class="bg-cyan-950 border px-3 py-1 rounded text-sm hover:bg-cyan-800 transition"
                >
                    <option
                        v-for="opt in filterOptions"
                        :key="opt.value"
                        :value="opt.value"
                    >
                        {{ opt.label }}
                    </option>
                </select>
            </div>

            <IconField>
                <InputIcon>
                    <i class="pi pi-search" />
                </InputIcon>
                <InputText
                    v-model="search"
                    placeholder="Search..."
                    class="w-64"
                />
            </IconField>
        </div>

        <!-- Table -->
        <DataTable
            v-model:selection="selectedProducts"
            :value="filteredProducts"
            dataKey="id"
            paginator
            :rows="10"
            :rowsPerPageOptions="[5, 10, 25]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} requests"
        >
            <Column field="assetName" header="Name" sortable />
            <Column field="date" header="Date" sortable>
                <template #body="slotProps">
                    {{ formatDate(slotProps.data.borrowedStart) }}
                </template>
            </Column>
            <Column field="price" header="Price" sortable>
                <template #body="slotProps">
                    {{ formatCurrency(slotProps.data.amount) }}
                </template>
            </Column>
            <Column field="inventoryStatus" header="Status" sortable>
                <template #body="slotProps">
                    <Tag
                        :value="slotProps.data.rentalStatus"
                        :severity="
                            getStatusLabel(slotProps.data.rentalStatus)
                        "
                    />
                </template>
            </Column>
        </DataTable>
    </div>
</template>
