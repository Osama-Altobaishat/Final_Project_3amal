function myFunction() {
  var x = document.getElementById("myTopnav");
  if (x.className === "topnav") {
    x.className += " responsive";
  } else {
    x.className = "topnav";
  }
}
window.onscroll = function () {
  const navbar = document.getElementById("navbar");
  const sticky = navbar.offsetTop; // Get the initial offset of the navbar

  if (window.pageYOffset > sticky) {
    // Use pageYOffset for scrolling
    navbar.classList.add("fixed"); // Add fixed class when scrolling
  } else {
    navbar.classList.remove("fixed"); // Remove fixed class when not scrolling
  }
};
const notification = document.getElementById("fixedButton");

notification.addEventListener("click", function () {
  window.scrollTo({ top: 0, behavior: "smooth" });
});

window.addEventListener("scroll", function () {
  if (window.scrollY > 0) {
    // Show button after scrolling down 100px
    notification.classList.remove("hidden");
    notification.style.opacity = 1; // Show the button
  } else {
    notification.style.opacity = 0; // Hide the button
    setTimeout(() => {
      notification.classList.add("hidden"); // Add hidden class after opacity transition
    }, 300); // Match the duration of the CSS transition
  }
});
