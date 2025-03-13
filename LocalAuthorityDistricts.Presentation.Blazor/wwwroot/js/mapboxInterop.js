window.mapboxInterop = {
    initializeMap: function (accessToken, containerId) {
        mapboxgl.accessToken = accessToken;
        var map = new mapboxgl.Map({
            container: containerId,
            style: 'mapbox://styles/mapbox/streets-v11',
            center: [-0.1276, 51.5074], // e.g., London
            zoom: 8
        });
        window._map = map;
    },

    addGeoJsonLayer: function (layerId, geoJsonData) {
        if (!window._map) return;

        if (window._map.getSource(layerId)) {
            window._map.getSource(layerId).setData(geoJsonData);
        } else {
            window._map.addSource(layerId, {
                type: 'geojson',
                data: geoJsonData
            });
            window._map.addLayer({
                id: layerId,
                type: 'fill',
                source: layerId,
                paint: {
                    'fill-color': '#888888',
                    'fill-opacity': 0.4
                }
            });

            // On click, show popup with district properties
            window._map.on('click', layerId, function (e) {
                if (e.features && e.features.length > 0) {
                    var props = e.features[0].properties;
                    new mapboxgl.Popup()
                        .setLngLat(e.lngLat)
                        .setHTML(
                            "<b>" + (props.Name || "Unknown") + "</b><br/>" +
                            "Code: " + (props.Code || "Unknown") + "<br/>" +
                            "Population: " + (props.Population || "N/A") + "<br/>" +
                            "Region: " + (props.Region || "Unknown")
                        )
                        .addTo(window._map);
                }
            });
        }
    },

    removeLayer: function (layerId) {
        if (!window._map) return;
        if (window._map.getLayer(layerId)) {
            window._map.removeLayer(layerId);
            window._map.removeSource(layerId);
        }
    }
};
