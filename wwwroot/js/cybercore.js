/* ==========================================================================
   CYBERCORE JS - Interactive Animations & UI Enhancements
   ========================================================================== */

(function () {
    'use strict';

    /* --------------------------------------------------------------------------
       DOM Ready Handler
       -------------------------------------------------------------------------- */
    document.addEventListener('DOMContentLoaded', function () {
        initNavbar();
        initNotifications();
        initAddToCart();
        initQuantityControls();
        initScrollAnimations();
        initParticles();
        initFormEnhancements();
    });

    /* --------------------------------------------------------------------------
       Navbar Scroll Effect
       -------------------------------------------------------------------------- */
    function initNavbar() {
        const navbar = document.querySelector('.cyber-navbar');
        if (!navbar) return;

        let lastScroll = 0;
        window.addEventListener('scroll', function () {
            const currentScroll = window.pageYOffset;

            if (currentScroll > 50) {
                navbar.classList.add('cyber-navbar-scrolled');
            } else {
                navbar.classList.remove('cyber-navbar-scrolled');
            }

            // Hide/show on scroll direction
            if (currentScroll > lastScroll && currentScroll > 200) {
                navbar.style.transform = 'translateY(-100%)';
            } else {
                navbar.style.transform = 'translateY(0)';
            }
            lastScroll = currentScroll;
        });
    }

    /* --------------------------------------------------------------------------
       Toast Notifications
       -------------------------------------------------------------------------- */
    function initNotifications() {
        window.showNotification = function (message, type = 'info', duration = 3000) {
            const container = getOrCreateNotificationContainer();
            const notification = document.createElement('div');
            notification.className = `cyber-notification cyber-notification-${type}`;

            const icons = {
                success: 'fa-check-circle',
                error: 'fa-times-circle',
                warning: 'fa-exclamation-triangle',
                info: 'fa-info-circle'
            };

            notification.innerHTML = `
                <i class="fas ${icons[type] || icons.info}"></i>
                <span>${message}</span>
                <button class="cyber-notification-close"><i class="fas fa-times"></i></button>
            `;

            container.appendChild(notification);

            // Animate in
            requestAnimationFrame(() => {
                notification.classList.add('cyber-notification-show');
            });

            // Close button
            notification.querySelector('.cyber-notification-close').addEventListener('click', function () {
                closeNotification(notification);
            });

            // Auto close
            if (duration > 0) {
                setTimeout(() => closeNotification(notification), duration);
            }

            return notification;
        };

        function getOrCreateNotificationContainer() {
            let container = document.getElementById('cyber-notifications');
            if (!container) {
                container = document.createElement('div');
                container.id = 'cyber-notifications';
                container.className = 'cyber-notification-container';
                document.body.appendChild(container);
            }
            return container;
        }

        function closeNotification(notification) {
            notification.classList.remove('cyber-notification-show');
            notification.classList.add('cyber-notification-hide');
            setTimeout(() => notification.remove(), 300);
        }
    }

    /* --------------------------------------------------------------------------
       Add to Cart AJAX
       -------------------------------------------------------------------------- */
    function initAddToCart() {
        document.addEventListener('submit', function (e) {
            const form = e.target;
            if (!form.classList.contains('add-to-cart-form')) return;

            e.preventDefault();

            const button = form.querySelector('button[type="submit"]');
            const originalContent = button.innerHTML;

            // Loading state
            button.disabled = true;
            button.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';

            const formData = new FormData(form);

            fetch(form.action, {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        showNotification(data.message || 'Added to cart!', 'success');
                        updateCartBadge(data.cartCount);
                        animateCartIcon();
                    } else {
                        showNotification(data.message || 'Failed to add to cart', 'error');
                    }
                })
                .catch(() => {
                    showNotification('Something went wrong', 'error');
                })
                .finally(() => {
                    button.disabled = false;
                    button.innerHTML = originalContent;
                });
        });
    }

    function updateCartBadge(count) {
        const badge = document.querySelector('.cyber-cart-badge');
        if (badge) {
            badge.textContent = count;
            badge.style.transform = 'scale(1.3)';
            setTimeout(() => badge.style.transform = 'scale(1)', 200);
        }
    }

    function animateCartIcon() {
        const cartIcon = document.querySelector('.cyber-cart-icon');
        if (cartIcon) {
            cartIcon.classList.add('cyber-cart-bounce');
            setTimeout(() => cartIcon.classList.remove('cyber-cart-bounce'), 500);
        }
    }

    /* --------------------------------------------------------------------------
       Quantity Controls
       -------------------------------------------------------------------------- */
    function initQuantityControls() {
        document.addEventListener('click', function (e) {
            const btn = e.target.closest('.cyber-qty-btn');
            if (!btn) return;

            const container = btn.closest('.cyber-quantity-input');
            const input = container.querySelector('.cyber-qty-input');
            const max = parseInt(input.max) || 999;
            const min = parseInt(input.min) || 1;
            let value = parseInt(input.value) || 1;

            if (btn.classList.contains('cyber-qty-minus')) {
                value = Math.max(min, value - 1);
            } else if (btn.classList.contains('cyber-qty-plus')) {
                value = Math.min(max, value + 1);
            }

            input.value = value;
            input.dispatchEvent(new Event('change', { bubbles: true }));
        });
    }

    /* --------------------------------------------------------------------------
       Scroll Animations (Intersection Observer)
       -------------------------------------------------------------------------- */
    function initScrollAnimations() {
        const animatedElements = document.querySelectorAll('.cyber-animate');
        if (!animatedElements.length) return;

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('cyber-animated');
                    observer.unobserve(entry.target);
                }
            });
        }, {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        });

        animatedElements.forEach(el => observer.observe(el));
    }

    /* --------------------------------------------------------------------------
       Particle Effects (Lightweight)
       -------------------------------------------------------------------------- */
    function initParticles() {
        const heroSection = document.querySelector('.cyber-hero');
        if (!heroSection) return;

        const particleContainer = document.createElement('div');
        particleContainer.className = 'cyber-particles';
        heroSection.appendChild(particleContainer);

        // Create floating particles
        for (let i = 0; i < 20; i++) {
            createParticle(particleContainer);
        }
    }

    function createParticle(container) {
        const particle = document.createElement('div');
        particle.className = 'cyber-particle';

        // Random properties
        const size = Math.random() * 4 + 2;
        const x = Math.random() * 100;
        const duration = Math.random() * 10 + 10;
        const delay = Math.random() * 5;

        particle.style.cssText = `
            width: ${size}px;
            height: ${size}px;
            left: ${x}%;
            animation-duration: ${duration}s;
            animation-delay: ${delay}s;
        `;

        container.appendChild(particle);
    }

    /* --------------------------------------------------------------------------
       Form Enhancements
       -------------------------------------------------------------------------- */
    function initFormEnhancements() {
        // Input focus effects
        document.querySelectorAll('.cyber-input, .cyber-select, .cyber-textarea').forEach(input => {
            input.addEventListener('focus', function () {
                this.closest('.cyber-form-group')?.classList.add('cyber-focused');
            });

            input.addEventListener('blur', function () {
                this.closest('.cyber-form-group')?.classList.remove('cyber-focused');
            });
        });

        // Password visibility toggle
        document.querySelectorAll('.toggle-password').forEach(btn => {
            btn.addEventListener('click', function () {
                const input = this.closest('.cyber-input-group').querySelector('input');
                const icon = this.querySelector('i');

                if (input.type === 'password') {
                    input.type = 'text';
                    icon.classList.replace('fa-eye', 'fa-eye-slash');
                } else {
                    input.type = 'password';
                    icon.classList.replace('fa-eye-slash', 'fa-eye');
                }
            });
        });
    }

    /* --------------------------------------------------------------------------
       Utility: Ripple Effect on Buttons
       -------------------------------------------------------------------------- */
    document.addEventListener('click', function (e) {
        const btn = e.target.closest('.cyber-btn');
        if (!btn) return;

        const ripple = document.createElement('span');
        ripple.className = 'cyber-ripple';

        const rect = btn.getBoundingClientRect();
        const size = Math.max(rect.width, rect.height);

        ripple.style.width = ripple.style.height = `${size}px`;
        ripple.style.left = `${e.clientX - rect.left - size / 2}px`;
        ripple.style.top = `${e.clientY - rect.top - size / 2}px`;

        btn.appendChild(ripple);
        setTimeout(() => ripple.remove(), 600);
    });

    /* --------------------------------------------------------------------------
       Loading Overlay
       -------------------------------------------------------------------------- */
    window.showLoading = function () {
        let overlay = document.getElementById('cyber-loading');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.id = 'cyber-loading';
            overlay.className = 'cyber-loading-overlay';
            overlay.innerHTML = `
                <div class="cyber-loader">
                    <div class="cyber-loader-ring"></div>
                    <div class="cyber-loader-ring"></div>
                    <div class="cyber-loader-ring"></div>
                </div>
            `;
            document.body.appendChild(overlay);
        }
        overlay.classList.add('active');
    };

    window.hideLoading = function () {
        const overlay = document.getElementById('cyber-loading');
        if (overlay) overlay.classList.remove('active');
    };

})();
