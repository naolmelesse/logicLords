// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

        // Show or hide the button based on scroll position
    window.onscroll = function () {
            const scrollTopBtn = document.getElementById("scrollTopBtn");
            if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
        scrollTopBtn.style.display = "block";
            } else {
        scrollTopBtn.style.display = "none";
            }
        };

    // Smooth scroll to top function
    function scrollToTop() {
        window.scrollTo({ top: 0, behavior: "smooth" });
        }
