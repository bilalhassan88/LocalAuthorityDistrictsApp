// wwwroot/js/cityDropdown.js

// Define getSelectedCities globally
window.getSelectedCities = function (selector) {
    const $dropdown = $(selector);
    // Ensure Select2 is initialized
    if (!$dropdown.hasClass('select2-hidden-accessible')) {
        $dropdown.select2();
    }
    return $dropdown.val() || [];
};

window.initializeCityDropdown = (cities) => {
    const $dropdown = $("#cityDropdown");
    $dropdown.empty();

    // Add cities as options
    cities.forEach(city => {
        $dropdown.append(new Option(city.name, city.id, false, false));
    });

    // Initialize Select2
    $dropdown.select2({
        placeholder: "Select cities",
        allowClear: true,
        width: '100%'
    });
};