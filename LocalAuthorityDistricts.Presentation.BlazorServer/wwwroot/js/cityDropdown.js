// wwwroot/js/cityDropdown.js

window.initializeCityDropdown = (cities) => {
    // Ensure jQuery is available
    const $dropdown = $("#cityDropdown");
    // Clear any existing options
    $dropdown.empty();

    // Add an empty option if desired (for placeholder)
    $dropdown.append(new Option("", "", false, false));

    // Add the cities as options (assume each city object has 'id' and 'name' properties)
    cities.forEach(city => {
        const option = new Option(city.name, city.id, false, false);
        $dropdown.append(option);
    });

    // Initialize Select2 on the dropdown with search enabled
    $dropdown.select2({
        placeholder: "Select cities",
        allowClear: true,
        width: '100%'
    });
};
