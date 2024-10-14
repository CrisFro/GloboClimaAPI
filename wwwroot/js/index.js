document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem('authToken');
    const sidebar = document.getElementById('favoritesSidebar');
    const toggleSidebarBtn = document.getElementById('toggleSidebar');
    const sidebarIcon = document.getElementById('sidebarIcon');
    const mainContent = document.getElementById('mainContent');
    const searchForm = document.querySelector('form');
    const countryInput = document.getElementById('country');
    const cityInput = document.getElementById('city');

    if (token) {
        toggleSidebarBtn.style.display = 'block';
        loadFavorites();
    } else {
        sidebar.classList.remove('active');
        toggleSidebarBtn.style.display = 'none';
    }

    toggleSidebarBtn.addEventListener('click', function () {
        sidebar.classList.toggle('active');

        if (sidebar.classList.contains('active')) {
            mainContent.style.marginLeft = "250px";
        } else {
            mainContent.style.marginLeft = "auto";
            mainContent.style.marginRight = "auto";
        }

        if (sidebar.classList.contains('active')) {
            sidebarIcon.classList.remove('fa-star');
            sidebarIcon.classList.add('fa-times');
        } else {
            sidebarIcon.classList.remove('fa-times');
            sidebarIcon.classList.add('fa-star');
        }
    });

    async function loadFavorites() {
        try {
            const response = await fetch('/api/favorites/list', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (response.ok) {
                const favorites = await response.json();
                displayFavorites(favorites);
                updateStarIcons(favorites);
            } else {
                console.error('Erro ao carregar favoritos.');
            }
        } catch (error) {
            console.error('Erro na solicitação:', error);
        }
    }

    function displayFavorites(favorites) {
        const container = document.getElementById('favoriteCardsContainer');
        if (!container) return;

        console.log('Received favorites:', favorites); 
        container.innerHTML = '';

        favorites.forEach(favorite => {
            const card = document.createElement('div');
            card.classList.add('favorite-card');

            if (favorite.city) {
                card.innerHTML = `
            <div class="favorite-content">
                <span>Cidade: <strong>${favorite.city}</strong></span>
                <button class="remove-favorite" data-id="${favorite.id}" style="cursor: pointer;">&times;</button>
            </div>`;
            } else if (favorite.country) {
                card.innerHTML = `
            <div class="favorite-content">
                <span>Pa&#237;s: <strong>${favorite.country}</strong></span>
                <button class="remove-favorite" data-id="${favorite.id}" style="cursor: pointer;">&times;</button>
            </div>`;
            }

            card.addEventListener('dblclick', function () {
                searchFavorite(favorite);
            });

            const removeButton = card.querySelector('.remove-favorite');
            removeButton.addEventListener('click', function (event) {
                event.stopPropagation();
                removeFavorite(favorite.id);
            });

            container.appendChild(card);
        });
    }

    async function removeFavorite(favoriteId) {
        const token = localStorage.getItem('authToken');

        try {
            const response = await fetch('/api/favorites/remove', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ id: favoriteId })
            });

            if (response.ok) {
                loadFavorites();
            } else {
                console.error('Erro ao remover favorito.');
            }
        } catch (error) {
            console.error('Erro na solicitação:', error);
        }
    }

    function updateStarIcons(favorites) {
        const favoriteCountries = favorites.map(f => f.country).filter(Boolean);
        const favoriteCities = favorites.map(f => f.city).filter(Boolean);

        document.querySelectorAll('.favorite-icon').forEach(icon => {
            const country = icon.dataset.country;
            const city = icon.dataset.city;

            if ((country && favoriteCountries.includes(country)) || (city && favoriteCities.includes(city))) {
                icon.setAttribute('data-favorited', 'true');
                icon.classList.remove('far', 'fa-star');
                icon.classList.add('fas', 'fa-star');
                icon.style.pointerEvents = 'none';
            } else {
                icon.setAttribute('data-favorited', 'false');
                icon.classList.remove('fas', 'fa-star');
                icon.classList.add('far', 'fa-star');
                icon.style.pointerEvents = 'auto';
            }
        });
    }

    function searchFavorite(favorite) {
        if (favorite.city) {
            cityInput.value = favorite.city;
            countryInput.value = '';
        } else if (favorite.country) {
            countryInput.value = favorite.country;
            cityInput.value = '';
        }

        searchForm.submit();
    }

    document.querySelectorAll('.favorite-icon').forEach(icon => {
        icon.addEventListener('click', function (event) {
            const isFavorited = this.getAttribute('data-favorited') === 'true';
            const itemId = this.dataset.city || this.dataset.country;

            const country = this.dataset.country;
            const city = this.dataset.city;

            if (isFavorited) {
                removeFavorite(itemId);
                this.setAttribute('data-favorited', 'false');
                this.classList.remove('fas', 'fa-star');
                this.classList.add('far', 'fa-star');
            } else {
                addFavorite({ country: country || null, city: city || null });
                this.setAttribute('data-favorited', 'true');
                this.classList.remove('far', 'fa-star');
                this.classList.add('fas', 'fa-star');
            }

            event.stopPropagation();
        });
    });

    async function addFavorite(favoriteData) {
        const token = localStorage.getItem('authToken');

        if (!token) {
            alert("Você precisa estar logado para adicionar favoritos.");
            toggleSidebarBtn.style.display = 'none';
            sidebar.classList.remove('active');
            return;
        }

        try {
            const response = await fetch('/api/favorites/add', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(favoriteData)
            });

            if (response.ok) {
                alert("Favorito adicionado com sucesso!");
                loadFavorites();
            } else {
                console.error('Erro ao adicionar favorito.');
            }
        } catch (error) {
            console.error('Erro na solicitação:', error);
        }
    }
});
