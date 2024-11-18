function setStarRating(rating, serviceId) {
    const starContainer = document.getElementById('star_' + serviceId);
    starContainer.innerHTML = ''; // Clear existing stars

    for (let i = 1; i <= 5; i++) {
        const star = document.createElement('i');
        if (i <= rating) {
            star.className = 'bi bi-star-fill'; // Full star
        } else if (i === Math.ceil(rating) && rating % 1 !== 0) {
            star.className = 'bi bi-star-half'; // Half star
        } else {
            star.className = 'bi bi-star'; // Empty star
        }
        starContainer.appendChild(star);
    }
}
