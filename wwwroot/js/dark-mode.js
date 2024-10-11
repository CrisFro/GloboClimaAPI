const darkModeEnabled = localStorage.getItem('darkMode') === 'true';

if (darkModeEnabled) {
    document.body.classList.add('dark-mode');
    document.querySelector('.footer').classList.add('dark-mode'); 
}

const darkModeButton = document.querySelector('.btn-dark-mode');

darkModeButton.innerHTML = darkModeEnabled ? '<i class="fas fa-sun"></i> Light Mode' : '<i class="far fa-moon"></i> Dark Mode';

darkModeButton.addEventListener('click', () => {
    document.body.classList.toggle('dark-mode');
    document.querySelector('.footer').classList.toggle('dark-mode');

    const isDarkMode = document.body.classList.contains('dark-mode');
    localStorage.setItem('darkMode', isDarkMode);

    darkModeButton.innerHTML = isDarkMode ? '<i class="fas fa-sun"></i> Light Mode' : '<i class="far fa-moon"></i> Dark Mode';
});