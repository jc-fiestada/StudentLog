

// Sidebar animation - hide/unhide

const sidebar = document.getElementById('sidebar');
const toggleBtn = document.getElementById('toggleSidebar');
const textLabels = document.querySelectorAll('.sidebar-text');
const logout = document.getElementById('logoutLink');

let isCollapsed = false;

toggleBtn.addEventListener('click', () => {
    isCollapsed = !isCollapsed;

    if (isCollapsed) {
        sidebar.classList.remove('w-64');
        sidebar.classList.add('w-16');

        textLabels.forEach(el => el.classList.add('hidden'));
        logout.classList.add('hidden');
    } else {
        sidebar.classList.remove('w-16');
        sidebar.classList.add('w-64');

        textLabels.forEach(el => el.classList.remove('hidden'));
        logout.classList.remove('hidden');
    }
});




