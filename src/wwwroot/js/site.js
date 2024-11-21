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

// Advanced Wheel Spinning Animation
function createWheelSpinner() {
    const canvas = document.getElementById('fortuneWheel');
    const ctx = canvas.getContext('2d');
    const centerX = canvas.width / 2;
    const centerY = canvas.height / 2;
    const radius = canvas.width / 2 - 10;

    const categories = ['Sports', 'Geography', 'History', 'Entertainment'];
    const colors = ['#FF6B6B', '#4ECDC4', '#45B7D1', '#FDCB6E'];

    let rotation = 0;
    let spinning = false;
    let spinSpeed = 0;
    let finalRotation = 0;

    function drawWheel(currentRotation) {
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        categories.forEach((category, index) => {
            const startAngle = currentRotation + (index * Math.PI * 2) / categories.length;
            const endAngle = currentRotation + ((index + 1) * Math.PI * 2) / categories.length;

            ctx.beginPath();
            ctx.moveTo(centerX, centerY);
            ctx.arc(centerX, centerY, radius, startAngle, endAngle);
            ctx.closePath();

            ctx.fillStyle = colors[index];
            ctx.fill();

            // Add category text
            ctx.save();
            ctx.translate(centerX, centerY);
            ctx.rotate(startAngle + (endAngle - startAngle) / 2);
            ctx.textAlign = 'right';
            ctx.fillStyle = 'white';
            ctx.font = 'bold 16px Arial';
            ctx.fillText(category, radius - 20, 0);
            ctx.restore();
        });

        // Decorative pointer
        ctx.beginPath();
        ctx.moveTo(centerX, 10);
        ctx.lineTo(centerX - 10, 0);
        ctx.lineTo(centerX + 10, 0);
        ctx.closePath();
        ctx.fillStyle = 'red';
        ctx.fill();
    }

    function spin() {
        if (spinning) return;

        spinning = true;
        spinSpeed = Math.random() * 0.5 + 0.5; // Random spin speed

        // Determine final rotation (multiple full rotations + random final position)
        const fullRotations = Math.floor(Math.random() * 5) + 3; // 3-7 full rotations
        const selectedSectorAngle = Math.random() * (Math.PI * 2 / categories.length);
        finalRotation = fullRotations * Math.PI * 2 + selectedSectorAngle;

        function animate() {
            // Deceleration effect
            spinSpeed *= 0.99;

            rotation += spinSpeed;

            // Stop condition
            if (spinSpeed < 0.01) {
                spinning = false;

                // Determine selected category
                const normalizedRotation = rotation % (Math.PI * 2);
                const categoryIndex = Math.floor((categories.length - normalizedRotation / (Math.PI * 2 / categories.length) + categories.length) % categories.length);

                // Trigger trivia logic
                const selectedCategory = categories[categoryIndex];
                triggerTriviaForCategory(selectedCategory);

                return;
            }

            drawWheel(rotation);
            requestAnimationFrame(animate);
        }

        animate();
    }

    function triggerTriviaForCategory(category) {
        const categoryDisplay = document.getElementById('categoryDisplay');
        const questionDisplay = document.getElementById('questionDisplay');
        const answerOptions = document.getElementById('answerOptions');

        // Your existing trivia logic here
        const questions = triviaCategories[category];
        const selectedQuestion = questions[Math.floor(Math.random() * questions.length)];

        categoryDisplay.innerHTML = `Category: ${category}`;
        questionDisplay.innerHTML = selectedQuestion.question;

        answerOptions.innerHTML = selectedQuestion.options
            .sort(() => Math.random() - 0.5)
            .map(option => `
                <button class="answer-btn" data-correct="${option === selectedQuestion.answer}">
                    ${option}
                </button>
            `).join('');

        // Add click event to answer buttons (same as previous implementation)
        document.querySelectorAll('.answer-btn').forEach(btn => {
            btn.addEventListener('click', function () {
                const isCorrect = this.dataset.correct === 'true';
                this.classList.add(isCorrect ? 'correct' : 'incorrect');

                document.querySelectorAll('.answer-btn').forEach(b => b.disabled = true);

                answerOptions.innerHTML += `
                    <div class="feedback ${isCorrect ? 'success' : 'error'}">
                        ${isCorrect ? '✅ Correct!' : '❌ Wrong Answer'}
                    </div>
                `;
            });
        });
    }

    // Initial wheel draw
    drawWheel(0);

    // Attach spin to button
    const spinButton = document.getElementById('spinButton');
    spinButton.addEventListener('click', spin);

    return {
        spin
    };
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    const wheelSpinner = createWheelSpinner();
});
