

require.config({
    baseUrl: "/content/js",
    paths: {
        "ordnung": "ordnung",
        "knockout": "libs/knockout-2.1.0"
    },
    packages: [
        { name: 'when', location: 'libs/components/when/', main: 'when' },
        // ... other packages ...
    ]
});

require(["ordnung/loader", "customBindings"], function(load, customBindings) {
    load();
});