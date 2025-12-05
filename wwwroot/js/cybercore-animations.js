/* ==========================================================================
   ARCADE THEME - Animations & Interactions
   ========================================================================== */

document.addEventListener('DOMContentLoaded', function () {

    // =========================================================================
    // DARK MODE TOGGLE
    // =========================================================================
    const darkModeToggle = document.getElementById('darkModeToggle');
    const darkModeIcon = document.getElementById('darkModeIcon');
    const html = document.documentElement;

    // Check for saved theme preference or default to dark
    const savedTheme = localStorage.getItem('arcade-theme');
    if (savedTheme === 'light') {
        html.setAttribute('data-theme', 'light');
        if (darkModeIcon) {
            darkModeIcon.classList.remove('fa-moon');
            darkModeIcon.classList.add('fa-sun');
        }
    }

    if (darkModeToggle) {
        darkModeToggle.addEventListener('click', function () {
            const currentTheme = html.getAttribute('data-theme');

            if (currentTheme === 'light') {
                html.removeAttribute('data-theme');
                localStorage.setItem('arcade-theme', 'dark');
                if (darkModeIcon) {
                    darkModeIcon.classList.remove('fa-sun');
                    darkModeIcon.classList.add('fa-moon');
                }
            } else {
                html.setAttribute('data-theme', 'light');
                localStorage.setItem('arcade-theme', 'light');
                if (darkModeIcon) {
                    darkModeIcon.classList.remove('fa-moon');
                    darkModeIcon.classList.add('fa-sun');
                }
            }
        });
    }

    // =========================================================================
    // NAVBAR SCROLL EFFECT
    // =========================================================================
    const navbar = document.getElementById('cyberNav');
    let lastScroll = 0;

    window.addEventListener('scroll', () => {
        const currentScroll = window.pageYOffset;

        // Add scrolled class for background change
        if (currentScroll > 50) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }

        lastScroll = currentScroll;
    });

    // =========================================================================
    // SCROLL TO TOP BUTTON
    // =========================================================================
    const scrollTopBtn = document.getElementById('scrollTopBtn');

    if (scrollTopBtn) {
        window.addEventListener('scroll', () => {
            if (window.pageYOffset > 300) {
                scrollTopBtn.classList.add('visible');
            } else {
                scrollTopBtn.classList.remove('visible');
            }
        });

        scrollTopBtn.addEventListener('click', () => {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    // =========================================================================
    // BUTTON RIPPLE EFFECT
    // =========================================================================
    document.querySelectorAll('.cyber-btn').forEach(button => {
        button.addEventListener('click', function (e) {
            const rect = this.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            const ripple = document.createElement('span');
            ripple.className = 'cyber-ripple';
            ripple.style.cssText = `
                position: absolute;
                width: 0;
                height: 0;
                border-radius: 50%;
                background: rgba(255, 255, 255, 0.4);
                transform: translate(-50%, -50%);
                left: ${x}px;
                top: ${y}px;
                animation: rippleEffect 0.6s ease-out forwards;
                pointer-events: none;
            `;

            this.style.position = 'relative';
            this.style.overflow = 'hidden';
            this.appendChild(ripple);

            setTimeout(() => ripple.remove(), 600);
        });
    });

    // Add ripple keyframes
    if (!document.getElementById('cyber-ripple-styles')) {
        const style = document.createElement('style');
        style.id = 'cyber-ripple-styles';
        style.textContent = `
            @keyframes rippleEffect {
                to {
                    width: 300px;
                    height: 300px;
                    opacity: 0;
                }
            }
        `;
        document.head.appendChild(style);
    }

    // =========================================================================
    // PRODUCT CARD HOVER EFFECT
    // =========================================================================
    document.querySelectorAll('.cyber-product-card').forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-4px)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.transform = '';
        });
    });

    // =========================================================================
    // INTERSECTION OBSERVER - FADE IN ON SCROLL
    // =========================================================================
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const fadeObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('cyber-visible');
                fadeObserver.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Add fade-in animation styles
    if (!document.getElementById('cyber-fade-styles')) {
        const style = document.createElement('style');
        style.id = 'cyber-fade-styles';
        style.textContent = `
            .cyber-fade-in {
                opacity: 0;
                transform: translateY(20px);
                transition: opacity 0.5s ease, transform 0.5s ease;
            }
            .cyber-fade-in.cyber-visible {
                opacity: 1;
                transform: translateY(0);
            }
            .cyber-fade-in:nth-child(1) { transition-delay: 0.1s; }
            .cyber-fade-in:nth-child(2) { transition-delay: 0.15s; }
            .cyber-fade-in:nth-child(3) { transition-delay: 0.2s; }
            .cyber-fade-in:nth-child(4) { transition-delay: 0.25s; }
        `;
        document.head.appendChild(style);
    }

    // Observe elements
    document.querySelectorAll('.cyber-product-card, .cyber-stat-card, .cyber-panel').forEach(el => {
        el.classList.add('cyber-fade-in');
        fadeObserver.observe(el);
    });

    // =========================================================================
    // AUTO-DISMISS ALERTS
    // =========================================================================
    document.querySelectorAll('.cyber-alert').forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'alertSlideOut 0.4s ease forwards';
            setTimeout(() => alert.remove(), 400);
        }, 5000);
    });

    if (!document.getElementById('alert-out-styles')) {
        const style = document.createElement('style');
        style.id = 'alert-out-styles';
        style.textContent = `
            @keyframes alertSlideOut {
                to { transform: translateX(100%); opacity: 0; }
            }
        `;
        document.head.appendChild(style);
    }

    // =========================================================================
    // COUNTER ANIMATION
    // =========================================================================
    const animateCounter = (element) => {
        const target = parseInt(element.getAttribute('data-count'));
        const duration = 2000;
        const step = target / (duration / 16);
        let current = 0;

        const updateCounter = () => {
            current += step;
            if (current < target) {
                element.textContent = Math.floor(current).toLocaleString();
                requestAnimationFrame(updateCounter);
            } else {
                element.textContent = target.toLocaleString();
            }
        };
        updateCounter();
    };

    const counterObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                animateCounter(entry.target);
                counterObserver.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    document.querySelectorAll('[data-count]').forEach(el => {
        counterObserver.observe(el);
    });

    // =========================================================================
    // MOBILE MENU ANIMATION
    // =========================================================================
    const menuToggle = document.querySelector('.cyber-menu-toggle');
    if (menuToggle) {
        menuToggle.addEventListener('click', function () {
            this.classList.toggle('active');
        });

        if (!document.getElementById('menu-toggle-styles')) {
            const style = document.createElement('style');
            style.id = 'menu-toggle-styles';
            style.textContent = `
                .cyber-menu-toggle.active span:nth-child(1) {
                    transform: rotate(45deg) translate(5px, 5px);
                }
                .cyber-menu-toggle.active span:nth-child(2) {
                    opacity: 0;
                }
                .cyber-menu-toggle.active span:nth-child(3) {
                    transform: rotate(-45deg) translate(7px, -6px);
                }
            `;
            document.head.appendChild(style);
        }
    }

    // =========================================================================
    // SEARCH INPUT FOCUS EFFECT
    // =========================================================================
    const searchInput = document.querySelector('.cyber-search-input');
    if (searchInput) {
        searchInput.addEventListener('focus', function () {
            this.parentElement.style.borderColor = 'var(--arcade-primary)';
            this.parentElement.style.boxShadow = '0 0 0 3px var(--arcade-primary-light)';
        });

        searchInput.addEventListener('blur', function () {
            this.parentElement.style.borderColor = '';
            this.parentElement.style.boxShadow = '';
        });
    }

    // =========================================================================
    // SEARCH AUTOCOMPLETE DROPDOWN
    // =========================================================================
    const searchInputEl = document.getElementById('searchInput');
    const searchDropdown = document.getElementById('searchDropdown');
    let selectedIndex = -1;
    let searchTimeout = null;

    console.log('🔍 Search elements:', { searchInputEl, searchDropdown });

    function highlightMatch(text, term) {
        if (!text || !term) return text || '';
        const regex = new RegExp(`(${term.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')})`, 'gi');
        return text.replace(regex, '<span class="search-highlight">$1</span>');
    }

    function renderDropdown(products, term) {
        if (!products || products.length === 0) {
            searchDropdown.innerHTML = '<div class="cyber-search-dropdown-empty"><i class="fas fa-search"></i> No products found</div>';
            searchDropdown.classList.add('active');
            return;
        }

        let html = '';
        products.forEach((p, index) => {
            html += `
                <a href="/Product/Details/${p.id}" class="cyber-search-dropdown-item" data-index="${index}">
                    <img src="${p.imageUrl || '/images/placeholder.svg'}" alt="${p.name}" class="cyber-search-dropdown-img" onerror="this.src='/images/placeholder.svg'">
                    <div class="cyber-search-dropdown-info">
                        <span class="cyber-search-dropdown-name">${highlightMatch(p.name, term)}</span>
                        <span class="cyber-search-dropdown-desc">${p.category || ''}</span>
                    </div>
                    <span class="cyber-search-dropdown-price">EGP ${p.price.toLocaleString()}</span>
                </a>
            `;
        });

        html += `<div class="cyber-search-dropdown-footer"><a href="/Product?searchTerm=${encodeURIComponent(term)}">View all results <i class="fas fa-arrow-right"></i></a></div>`;

        searchDropdown.innerHTML = html;
        searchDropdown.classList.add('active');
        selectedIndex = -1;
    }

    function closeDropdown() {
        if (searchDropdown) {
            searchDropdown.classList.remove('active');
            selectedIndex = -1;
        }
    }

    if (searchInputEl && searchDropdown) {
        console.log('✅ Search autocomplete initialized');
        searchInputEl.addEventListener('input', function () {
            const term = this.value.trim();
            console.log('🔎 Searching for:', term);
            if (searchTimeout) clearTimeout(searchTimeout);
            
            if (term.length < 2) {
                closeDropdown();
                return;
            }

            searchTimeout = setTimeout(() => {
                console.log('📡 Fetching:', `/Product/QuickSearch?term=${encodeURIComponent(term)}`);
                fetch(`/Product/QuickSearch?term=${encodeURIComponent(term)}`)
                    .then(res => res.json())
                    .then(data => {
                        console.log('📦 Results:', data);
                        renderDropdown(data, term);
                    })
                    .catch(err => {
                        console.error('❌ Fetch error:', err);
                        closeDropdown();
                    });
            }, 200);
        });

        searchInputEl.addEventListener('keydown', function (e) {
            const items = searchDropdown.querySelectorAll('.cyber-search-dropdown-item');
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                if (selectedIndex < items.length - 1) selectedIndex++;
                items.forEach((item, i) => item.classList.toggle('selected', i === selectedIndex));
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                if (selectedIndex > 0) selectedIndex--;
                items.forEach((item, i) => item.classList.toggle('selected', i === selectedIndex));
            } else if (e.key === 'Enter' && selectedIndex >= 0) {
                e.preventDefault();
                items[selectedIndex]?.click();
            } else if (e.key === 'Escape') {
                closeDropdown();
            }
        });

        document.addEventListener('click', function (e) {
            if (!searchInputEl.contains(e.target) && !searchDropdown.contains(e.target)) {
                closeDropdown();
            }
        });
    }

    // =========================================================================
    // CATEGORIES DROPDOWN
    // =========================================================================
    const catDropdownBtn = document.querySelector('.cyber-cat-dropdown-btn');
    const catDropdown = document.querySelector('.cyber-cat-dropdown');

    if (catDropdownBtn && catDropdown) {
        catDropdownBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            catDropdown.classList.toggle('open');
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', function (e) {
            if (!catDropdown.contains(e.target)) {
                catDropdown.classList.remove('open');
            }
        });
    }

    // Add dropdown open styles
    if (!document.getElementById('cat-dropdown-styles')) {
        const style = document.createElement('style');
        style.id = 'cat-dropdown-styles';
        style.textContent = `
            .cyber-cat-dropdown.open .cyber-cat-dropdown-menu {
                opacity: 1;
                visibility: visible;
                transform: translateY(0);
            }
            .cyber-cat-dropdown.open .cyber-cat-arrow {
                transform: rotate(180deg);
            }
        `;
        document.head.appendChild(style);
    }

    console.log('✨ Arcade Theme Initialized');
});
