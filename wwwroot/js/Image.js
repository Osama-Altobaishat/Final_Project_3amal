    let selectedFiles = [];
    document
        .getElementById("serviceImage")
        .addEventListener("change", function (event) {
            const files = event.target.files;
            if (files && files[0]) {
                // Log the files for debugging
                const reader = new FileReader();
                console.log(selectedFiles);
                reader.onload = function (e) {
                    const carouselInner = document.querySelector(
                        "#imageCarousel .carousel-inner"
                    );

                    // Create new carousel item
                    const newCarouselItem = document.createElement("div");
                    newCarouselItem.classList.add("carousel-item");
                    if (carouselInner.children.length === 0) {
                        newCarouselItem.classList.add("active");
                    }

                    // Create image element
                    const img = document.createElement("img");
                    img.src = e.target.result;
                    img.classList.add("d-block", "w-100");
                    img.alt = "Service Image";

                    // Create delete button
                    const deleteBtn = document.createElement("button");
                    deleteBtn.classList.add("btn", "btn-danger", "delete-btn");
                    deleteBtn.textContent = "Delete";
                    deleteBtn.onclick = function () {
                        deleteImage(newCarouselItem);
                    };

                    // Append the image and delete button to the carousel item
                    newCarouselItem.appendChild(img);
                    newCarouselItem.appendChild(deleteBtn);

                    carouselInner.appendChild(newCarouselItem);
                };

                reader.readAsDataURL(files[0]); // Read the file as a Data URL
            }
        });

    // Function to delete an image from the carousel
    function deleteImage(carouselItem) {
        const carouselInner = document.querySelector(
            "#imageCarousel .carousel-inner"
        );
        carouselItem.remove();

        // If the deleted item was the active one, make the first remaining item active
        const items = carouselInner.querySelectorAll(".carousel-item");
        if (
            items.length > 0 &&
            !carouselInner.querySelector(".carousel-item.active")
        ) {
            items[0].classList.add("active");
        }
    }