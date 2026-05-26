document.addEventListener('DOMContentLoaded', function () {
    const navbar = document.getElementById('mainNavbar');
    const hasHero = document.querySelector('.carousel, .hero-section, .contact-hero-img, .contact-img');

    window.addEventListener('scroll', function () {
        if (!hasHero) {
            // No hero — always keep red
            navbar.classList.add('scrolled');
        } else {
            // Has hero — transparent at top, red on scroll
            if (window.scrollY > 50) {
                navbar.classList.add('scrolled');
            } else {
                navbar.classList.remove('scrolled');
            }
        }
    });

    // Also check on page load
    if (!hasHero) {
        navbar.classList.add('scrolled');
    }
});