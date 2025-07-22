export function showToast(type, message) {
    const container = document.getElementById("toast-container");
    if (!container) return;

    const toast = document.createElement("div");
    toast.className = `
        px-4 py-3 rounded-lg shadow-lg text-white text-sm font-medium
        transition-all duration-300 transform opacity-0 -translate-y-4
    `;

    switch (type) {
        case "success":
        toast.classList.add("bg-green-600");
        break;
        case "error":
        toast.classList.add("bg-red-600");
        break;
        case "warning":
        toast.classList.add("bg-yellow-500", "text-black");
        break;
        default:
        toast.classList.add("bg-gray-700");
    }

    toast.textContent = message;
    container.appendChild(toast);

    // 🪄 Trigger the animation
    void toast.offsetHeight; // forces a DOM reflow
    toast.classList.remove("opacity-0", "-translate-y-4");
    toast.classList.add("opacity-100", "translate-y-0");

    // ⏱️ Auto-remove after 3 seconds
    setTimeout(() => {
        toast.classList.remove("opacity-100", "translate-y-0");
        toast.classList.add("opacity-0", "-translate-y-4");
        setTimeout(() => container.removeChild(toast), 300); // Wait for fade out
    }, 3000);
}
