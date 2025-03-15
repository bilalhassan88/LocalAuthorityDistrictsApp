window.getSelectedCities = function (selector) {
    const $dropdown = $(selector);
    if (!$dropdown.hasClass('select2-hidden-accessible')) {
        $dropdown.select2();
    }
    return $dropdown.val() || [];
};

window.initializeCityDropdown = (cities) => {
    const $dropdown = $("#cityDropdown");
    $dropdown.empty();

    cities.forEach(city => {
        $dropdown.append(new Option(city.name, city.id, false, false));
    });

    $dropdown.select2({
        placeholder: "Select cities",
        allowClear: true,
        width: '100%'
    });
};