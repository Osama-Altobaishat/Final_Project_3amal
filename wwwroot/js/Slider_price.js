const slider = document.getElementById("myRange");
const tooltip = document.getElementById("tooltip");

// Function to update the background of the slider
function updateSliderBackground() {
  const value = slider.value;
  const min = slider.min;
  const max = slider.max;
  const percentage = ((value - min) / (max - min)) * 100;

  // Update background with a gradient based on the slider value
  slider.style.background = `linear-gradient(to right, #ff6600 ${percentage}%, #ddd ${percentage}%)`;

  // Update tooltip position and value
  const thumbWidth = 25; // Width of the thumb
  const sliderWidth = slider.clientWidth; // Width of the slider
  const tooltipPosition = (percentage / 100) * sliderWidth; // Calculate tooltip position
  tooltip.style.left = `${tooltipPosition}px`; // Set tooltip position
  tooltip.textContent = value; // Set tooltip text
}

// Function to show tooltip on hover
function showTooltip() {
  tooltip.style.display = "block"; // Show tooltip
  console.log("hover");
}

// Function to hide tooltip
function hideTooltip() {
  tooltip.style.display = "none"; // Hide tooltip
}

// Initial call to set the background
updateSliderBackground();

// Update background on input change
slider.addEventListener("input", updateSliderBackground);

// Show tooltip on mouse enter
slider.addEventListener("mouseenter", showTooltip);

// Hide tooltip on mouse leave
slider.addEventListener("mouseleave", hideTooltip);

// Update tooltip position when the mouse moves over the slider
slider.addEventListener("mousemove", updateSliderBackground);
slider.addEventListener("mousedown", showTooltip);
slider.addEventListener("mouseup", hideTooltip);
