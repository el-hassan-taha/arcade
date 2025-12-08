/**
 * ARCADE - Advanced Animation & Interaction System
 * Handles scroll animations, micro-interactions, and dynamic effects
 */

(function () {
    'use strict';

    // =================================================================
    // INTERSECTION OBSERVER - Reveal on Scroll
    // =================================================================
    const initScrollReveal = () => {
        const revealElements = document.querySelectorAll('.reveal, .cyber-panel, .product-card');

        if ('IntersectionObserver' in window) {
            const revealObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('active', 'animate-slide-in-up');
                        revealObserver.unobserve(entry.target);
                    }
                });
            }, {
                threshold: 0.1,
                rootMargin: '0px 0px -50px 0px'
            });

            revealElements.forEach(el => {
                el.style.opacity = '0';
                el.style.transform = 'translateY(30px)';
                el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
                revealObserver.observe(el);
            });
        }
    };

    // =================================================================
    // STAGGER ANIMATIONS - Sequential Item Reveal
    // =================================================================
    const initStaggerAnimations = () => {
        const containers = document.querySelectorAll('.stagger-container');

        containers.forEach(container => {
            const items = container.querySelectorAll('.stagger-item');
            items.forEach((item, index) => {
                item.style.animationDelay = `${index * 0.1}s`;
            });
        });
    };

    // =================================================================
    // RIPPLE EFFECT - Material Design Click Animation
    // =================================================================
    const initRippleEffect = () => {
        const rippleElements = document.querySelectorAll('.cyber-btn, .btn, .action-btn, .cyber-nav-link');

        rippleElements.forEach(element => {
            element.addEventListener('click', function (e) {
                const ripple = document.createElement('span');
                const rect = this.getBoundingClientRect();
                const size = Math.max(rect.width, rect.height);
                const x = e.clientX - rect.left - size / 2;
                const y = e.clientY - rect.top - size / 2;

                ripple.style.width = ripple.style.height = size + 'px';
                ripple.style.left = x + 'px';
                ripple.style.top = y + 'px';
                ripple.classList.add('ripple');

                this.appendChild(ripple);

                setTimeout(() => ripple.remove(), 600);
            });
        });
    };

    // =================================================================
    // NAVBAR SCROLL BEHAVIOR
    // =================================================================
    const initNavbarScroll = () => {
        const navbar = document.querySelector('.cyber-navbar');
        let lastScroll = 0;

        window.addEventListener('scroll', () => {
            const currentScroll = window.pageYOffset;

            if (currentScroll > 50) {
                navbar?.classList.add('scrolled');
            } else {
                navbar?.classList.remove('scrolled');
            }

            // Hide/show navbar on scroll
            if (currentScroll > lastScroll && currentScroll > 100) {
                navbar?.style.transform = 'translateY(-100%)';
            } else {
                navbar?.style.transform = 'translateY(0)';
            }

            lastScroll = currentScroll;
        });
    };

    // =================================================================
    // ENHANCED SEARCH - Real-time Autocomplete
    // =================================================================
    const initEnhancedSearch = () => {
        const searchInput = document.getElementById('searchInput');
        const searchDropdown = document.getElementById('searchDropdown');
        let searchTimeout;

        if (!searchInput || !searchDropdown) return;

        searchInput.addEventListener('input', function () {
            clearTimeout(searchTimeout);
            const query = this.value.trim();

            if (query.length < 2) {
                searchDropdown.classList.remove('active');
                return;
            }

            searchTimeout = setTimeout(() => {
                // Simulate search (replace with actual API call)
                fetchSearchResults(query);
            }, 300);
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', (e) => {
            if (!e.target.closest('.cyber-search-container')) {
                searchDropdown.classList.remove('active');
            }
        });

        // Keyboard navigation
        searchInput.addEventListener('keydown', function (e) {
            const items = searchDropdown.querySelectorAll('.cyber-search-dropdown-item');
            const activeItem = searchDropdown.querySelector('.cyber-search-dropdown-item.selected');
            let index = Array.from(items).indexOf(activeItem);

            if (e.key === 'ArrowDown') {
                e.preventDefault();
                index = Math.min(index + 1, items.length - 1);
                items.forEach((item, i) => {
                    item.classList.toggle('selected', i === index);
                });
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                index = Math.max(index - 1, 0);
                items.forEach((item, i) => {
                    item.classList.toggle('selected', i === index);
                });
            } else if (e.key === 'Enter' && activeItem) {
                e.preventDefault();
                activeItem.click();
            }
        });
    };

    const fetchSearchResults = (query) => {
        // This would be replaced with actual AJAX call
        // For now, just show/hide the dropdown
        const searchDropdown = document.getElementById('searchDropdown');
        searchDropdown.classList.add('active');
    };

    // =================================================================
    // SMOOTH SCROLL TO TOP
    // =================================================================
    const initScrollToTop = () => {
        // Create scroll-to-top button
        const scrollBtn = document.createElement('button');
        scrollBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
        scrollBtn.className = 'scroll-to-top';
        scrollBtn.style.cssText = `
            position: fixed;
            bottom: 30px;
            right: 30px;
            width: 50px;
            height: 50px;
            border-radius: 50%;
            background: linear-gradient(135deg, #7c3aed 0%, #06b6d4 100%);
            color: white;
            border: none;
            cursor: pointer;
            display: none;
            align-items: center;
            justify-content: center;
            font-size: 1.2rem;
            box-shadow: 0 4px 20px rgba(124, 58, 237, 0.5);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            z-index: 1000;
        `;

        document.body.appendChild(scrollBtn);

        window.addEventListener('scroll', () => {
            if (window.pageYOffset > 300) {
                scrollBtn.style.display = 'flex';
                scrollBtn.style.animation = 'bounceIn 0.5s ease';
            } else {
                scrollBtn.style.display = 'none';
            }
        });

        scrollBtn.addEventListener('click', () => {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });

        scrollBtn.addEventListener('mouseenter', () => {
            scrollBtn.style.transform = 'translateY(-5px) scale(1.1)';
            scrollBtn.style.boxShadow = '0 6px 30px rgba(124, 58, 237, 0.7)';
        });

        scrollBtn.addEventListener('mouseleave', () => {
            scrollBtn.style.transform = 'translateY(0) scale(1)';
            scrollBtn.style.boxShadow = '0 4px 20px rgba(124, 58, 237, 0.5)';
        });
    };

    // =================================================================
    // ANIMATED COUNTERS
    // =================================================================
    const initAnimatedCounters = () => {
        const counters = document.querySelectorAll('[data-count]');

        const animateCounter = (counter) => {
            const target = parseInt(counter.dataset.count);
            const duration = 2000;
            const increment = target / (duration / 16);
            let current = 0;

            const updateCounter = () => {
                current += increment;
                if (current < target) {
                    counter.textContent = Math.floor(current);
                    requestAnimationFrame(updateCounter);
                } else {
                    counter.textContent = target;
                }
            };

            updateCounter();
        };

        if ('IntersectionObserver' in window) {
            const counterObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting && !entry.target.dataset.animated) {
                        animateCounter(entry.target);
                        entry.target.dataset.animated = 'true';
                    }
                });
            });

            counters.forEach(counter => counterObserver.observe(counter));
        }
    };

    // =================================================================
    // PARALLAX EFFECT
    // =================================================================
    const initParallax = () => {
        const parallaxElements = document.querySelectorAll('[data-parallax]');

        window.addEventListener('scroll', () => {
            const scrolled = window.pageYOffset;

            parallaxElements.forEach(element => {
                const speed = element.dataset.parallax || 0.5;
                const yPos = -(scrolled * speed);
                element.style.transform = `translateY(${yPos}px)`;
            });
        });
    };

    // =================================================================
    // CARD HOVER 3D EFFECT
    // =================================================================
    const init3DCardEffect = () => {
        const cards = document.querySelectorAll('.product-card, .cyber-panel');

        cards.forEach(card => {
            card.addEventListener('mousemove', function (e) {
                const rect = this.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;

                const centerX = rect.width / 2;
                const centerY = rect.height / 2;

                const rotateX = (y - centerY) / 10;
                const rotateY = (centerX - x) / 10;

                this.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.02)`;
            });

            card.addEventListener('mouseleave', function () {
                this.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) scale(1)';
            });
        });
    };

    // =================================================================
    // TOAST NOTIFICATIONS
    // =================================================================
    window.showToast = (message, type = 'info') => {
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.innerHTML = `
            <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-circle' : 'info-circle'}"></i>
            <span>${message}</span>
        `;
        toast.style.cssText = `
            position: fixed;
            top: 100px;
            right: 30px;
            padding: 1rem 1.5rem;
            background: var(--arcade-surface);
            border: 1px solid var(--arcade-border);
            border-radius: 12px;
            color: var(--arcade-text);
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
            display: flex;
            align-items: center;
            gap: 0.75rem;
            animation: slideInRight 0.4s ease;
            z-index: 10000;
            min-width: 300px;
        `;

        document.body.appendChild(toast);

        setTimeout(() => {
            toast.style.animation = 'slideOutRight 0.4s ease';
            setTimeout(() => toast.remove(), 400);
        }, 3000);
    };

    // =================================================================
    // FORM VALIDATION ANIMATIONS
    // =================================================================
    const initFormAnimations = () => {
        const inputs = document.querySelectorAll('.form-control, .form-select');

        inputs.forEach(input => {
            // Floating label effect
            input.addEventListener('focus', function () {
                this.parentElement?.querySelector('label')?.classList.add('active');
            });

            input.addEventListener('blur', function () {
                if (!this.value) {
                    this.parentElement?.querySelector('label')?.classList.remove('active');
                }
            });

            // Error shake animation
            input.addEventListener('invalid', function () {
                this.style.animation = 'shake 0.5s ease';
                setTimeout(() => {
                    this.style.animation = '';
                }, 500);
            });
        });
    };

    // =================================================================
    // LOADING OVERLAY
    // =================================================================
    window.showLoading = () => {
        const overlay = document.createElement('div');
        overlay.id = 'loadingOverlay';
        overlay.innerHTML = `
            <div style="text-align: center;">
                <div class="spinner-large" style="margin: 0 auto 1rem;"></div>
                <p style="color: var(--arcade-text); font-size: 1.1rem;">Loading...</p>
            </div>
        `;
        overlay.style.cssText = `
            position: fixed;
            inset: 0;
            background: rgba(12, 15, 26, 0.8);
            backdrop-filter: blur(10px);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 99999;
            animation: fadeIn 0.3s ease;
        `;
        document.body.appendChild(overlay);
    };

    window.hideLoading = () => {
        const overlay = document.getElementById('loadingOverlay');
        if (overlay) {
            overlay.style.animation = 'fadeOut 0.3s ease';
            setTimeout(() => overlay.remove(), 300);
        }
    };

    // =================================================================
    // IMAGE LAZY LOADING WITH ANIMATION
    // =================================================================
    const initLazyLoading = () => {
        const images = document.querySelectorAll('img[data-src]');

        if ('IntersectionObserver' in window) {
            const imageObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const img = entry.target;
                        img.src = img.dataset.src;
                        img.classList.add('loaded');
                        img.style.animation = 'fadeIn 0.5s ease';
                        imageObserver.unobserve(img);
                    }
                });
            });

            images.forEach(img => imageObserver.observe(img));
        }
    };

    // =================================================================
    // INITIALIZE ALL
    // =================================================================
    const init = () => {
        // Wait for DOM to be ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', init);
            return;
        }

        // Initialize all features
        initScrollReveal();
        initStaggerAnimations();
        initRippleEffect();
        initNavbarScroll();
        initEnhancedSearch();
        initScrollToTop();
        initAnimatedCounters();
        initParallax();
        init3DCardEffect();
        initFormAnimations();
        initLazyLoading();

        // Add page transition class
        document.body.classList.add('page-transition');
    };

    // Auto-initialize
    init();

    // =================================================================
    // DARK MODE TOGGLE ENHANCEMENT
    // =================================================================
    const darkModeToggle = document.querySelector('.dark-mode-toggle');
    if (darkModeToggle) {
        darkModeToggle.addEventListener('click', function () {
            this.style.animation = 'rotateIn 0.5s ease';
            setTimeout(() => {
                this.style.animation = '';
            }, 500);
        });
    }

    // =================================================================
    // DYNAMIC BACKGROUND PARTICLES
    // =================================================================
    const initDynamicParticles = () => {
        const particlesContainer = document.querySelector('.cyber-particles');
        if (!particlesContainer) return;

        const particleCount = 15;
        const colors = [
            'rgba(124, 58, 237, 0.4)',
            'rgba(6, 182, 212, 0.4)',
            'rgba(236, 72, 153, 0.3)'
        ];

        for (let i = 0; i < particleCount; i++) {
            const particle = document.createElement('div');
            particle.className = 'cyber-particle';

            const size = Math.random() * 3 + 2;
            const startX = Math.random() * 100;
            const startY = Math.random() * 100;
            const duration = Math.random() * 15 + 10;
            const delay = Math.random() * 5;
            const color = colors[Math.floor(Math.random() * colors.length)];

            particle.style.cssText = `
                position: absolute;
                width: ${size}px;
                height: ${size}px;
                background: ${color};
                border-radius: 50%;
                top: ${startY}%;
                left: ${startX}%;
                animation: particleFloat ${duration}s infinite ease-in-out ${delay}s;
                box-shadow: 0 0 ${size * 3}px ${color};
            `;

            particlesContainer.appendChild(particle);
        }
    };

    // Add particle float keyframes dynamically
    const addParticleAnimation = () => {
        const style = document.createElement('style');
        style.textContent = `
            @keyframes particleFloat {
                0%, 100% {
                    transform: translate(0, 0) scale(1);
                    opacity: 0.3;
                }
                25% {
                    transform: translate(50px, -50px) scale(1.2);
                    opacity: 0.6;
                }
                50% {
                    transform: translate(-30px, -100px) scale(0.8);
                    opacity: 0.4;
                }
                75% {
                    transform: translate(-80px, -50px) scale(1.1);
                    opacity: 0.5;
                }
            }
        `;
        document.head.appendChild(style);
    };

    // =================================================================
    // MOUSE MOVE PARALLAX EFFECT
    // =================================================================
    const initParallaxBackground = () => {
        const orbs = document.querySelectorAll('.cyber-orb');
        if (orbs.length === 0) return;

        let mouseX = 0;
        let mouseY = 0;
        let currentX = 0;
        let currentY = 0;

        document.addEventListener('mousemove', (e) => {
            mouseX = (e.clientX / window.innerWidth) - 0.5;
            mouseY = (e.clientY / window.innerHeight) - 0.5;
        });

        const animate = () => {
            currentX += (mouseX - currentX) * 0.05;
            currentY += (mouseY - currentY) * 0.05;

            orbs.forEach((orb, index) => {
                const speed = (index + 1) * 20;
                const x = currentX * speed;
                const y = currentY * speed;
                orb.style.transform = `translate(${x}px, ${y}px)`;
            });

            requestAnimationFrame(animate);
        };

        animate();
    };

    // =================================================================
    // INITIALIZE ALL BACKGROUND ANIMATIONS
    // =================================================================
    const initBackgroundAnimations = () => {
        addParticleAnimation();
        initDynamicParticles();
        initParallaxBackground();
    };

    // Initialize on load
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initBackgroundAnimations);
    } else {
        initBackgroundAnimations();
    }

})();
