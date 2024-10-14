document.addEventListener("DOMContentLoaded", function () {
    const darkModeEnabled = localStorage.getItem('darkMode') === 'enabled';

    const darkModeButton = document.querySelector('.btn-dark-mode');

    if (darkModeEnabled) {
        document.body.classList.add('dark-mode');
        document.querySelector('.footer').classList.add('dark-mode');
        document.querySelector('#favoritesSidebar').classList.add('dark-mode');
    }

    if (darkModeButton) {
        darkModeButton.innerHTML = darkModeEnabled ? '<i class="fas fa-sun"></i>' : '<i class="far fa-moon"></i>';

        darkModeButton.addEventListener('click', function () {
            document.body.classList.toggle('dark-mode');
            document.querySelector('.footer').classList.toggle('dark-mode');
            document.querySelector('#favoritesSidebar').classList.toggle('dark-mode');

            const isDarkMode = document.body.classList.contains('dark-mode');
            localStorage.setItem('darkMode', isDarkMode ? 'enabled' : 'disabled');

            darkModeButton.innerHTML = isDarkMode ? '<i class="fas fa-sun"></i>' : '<i class="far fa-moon"></i>';
        });
    }
});
