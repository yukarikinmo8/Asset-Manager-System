<script setup>
import productsData from "@/assets/data/requestDummy.json"; // renamed to avoid conflict
import Column from "primevue/column";
import DataTable from "primevue/datatable";
import SelectButton from "primevue/selectbutton";
import { computed, onMounted, ref } from "vue";
import { getEmployeeRentals, putRentalStatus } from "../../api/rentalApi";

const buttons = [
    "All",
    "Confirmed Request",
    "Pending Request",
    "Returned Assets",
];
const selected = ref("All");
const rentals = ref([]);

// Fetch rentals from API
const fetchRentals = async () => {
    try {
        const response = await getEmployeeRentals();
        rentals.value = response.data;
    } catch (error) {
        console.error("Error fetching rentals:", error);
    }
};

// Update rental status
const updateStatus = async (item, newStatus) => {
    try {
        await putRentalStatus(item.id, newStatus);
        await fetchRentals(); // Refresh after update
    } catch (error) {
        console.error("Failed to update status", error);
    }
};

// Dynamic filtering based on selected button
const products = computed(() => {
    if (selected.value === "All") return rentals.value;
    if (selected.value === "Confirmed Request")
        return rentals.value.filter((p) => p.rentalStatus === "Confirmed");
    if (selected.value === "Pending Request")
        return rentals.value.filter((p) => p.rentalStatus === "Pending");
    if (selected.value === "Returned Assets")
        return rentals.value.filter((p) => p.rentalStatus === "Returned");
    return [];
});

// Severity for PrimeVue Tag
const getSeverity = (product) => {
    switch (product.rentalStatus) {
        case "Returned":
            return "success";
        case "Confirmed":
            return "warning";
        case "Pending":
            return "danger";
        default:
            return null;
    }
};

function formatDate(date) {
    return new Date(date).toLocaleDateString();
}


onMounted(() => {
    fetchRentals();
});
</script>

<template>
    <div class="px-16 py-4">
        <h1 class="text-2xl font-semibold">Asset Requests</h1>
        <div class="flex gap-2 mt-4">
            <SelectButton
                v-model="selected"
                :options="buttons"
                :unstyled="true"
                class="flex flex-wrap gap-2"
            >
                <template #option="slotProps">
                    <button
                        class="px-4 py-2 rounded-full border text-sm transition"
                        :class="[
                            selected === slotProps.option
                                ? 'bg-cyan-950 text-cyan-500 border-cyan-500'
                                : 'bg-cyan-50 border-cyan-50 text-black hover:bg-cyan-100',
                        ]"
                    >
                        {{ slotProps.option }}
                    </button>
                </template>
            </SelectButton>
        </div>

        <div class="mt-6">
            <DataTable :value="products" class="no-style-table">
                <Column field="employeeName" header="Employee Name" />
                <Column field="assetName" header="Item Name" />

                <Column field="startDate" header="Start Date">
                    <template #body="slotProps">
                        {{ formatDate(slotProps.data.borrowedStart) }}
                    </template>
                </Column>

                <Column field="date" header="End Date">
                    <template #body="slotProps">
                        {{ formatDate(slotProps.data.borrowedEnd) }}
                    </template>
                </Column>

                <Column header="Status">
                    <template #body="slotProps">
                        <span
                            :class="{
                                'text-blue-600 font-medium':
                                    slotProps.data.rentalStatus ===
                                    'Pending',
                                'text-red-600 font-medium':
                                    slotProps.data.rentalStatus ===
                                        'Confirmed' ||
                                    slotProps.data.rentalStatus === 'Return',
                                'text-green-600 font-medium':
                                    slotProps.data.rentalStatus ===
                                        'Returned' ||
                                    slotProps.data.rentalStatus ===
                                        'Completed',
                            }"
                        >
                            {{ slotProps.data.rentalStatus }}
                        </span>
                    </template>
                </Column>

                <Column header="Action">
                    <template #body="slotProps">
                        <div class="flex gap-2">
                            <button
                                v-if="
                                    slotProps.data.rentalStatus === 'Pending'
                                "
                                class="px-2 py-1 text-md font-medium text-blue-600 hover:text-blue-900"
                                @click="
                                    updateStatus(slotProps.data, 'Approved')
                                "
                            >
                                Approve Request
                            </button>

                            <button
                                v-else-if="
                                    slotProps.data.rentalStatus ===
                                    'Confirmed'
                                "
                                class="px-2 py-1 text-md font-medium text-red-600 hover:text-red-800"
                                @click="
                                    updateStatus(slotProps.data, 'Returned')
                                "
                            >
                                Request Return
                            </button>

                            <button
                                v-else-if="
                                    slotProps.data.rentalStatus ===
                                    'Returned'
                                "
                                class="px-2 py-1 text-md font-medium text-green-600"
                                disabled
                            >
                                Completed
                            </button>
                        </div>
                    </template>
                </Column>

                <template #footer>
                    In total there are
                    {{ products ? products.length : 0 }} products.
                </template>
            </DataTable>
        </div>
    </div>
</template>
