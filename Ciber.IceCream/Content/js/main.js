

require.config({
    baseUrl: "/content/js",
    paths: {
        "ordnung": "ordnung",
        "knockout": "libs/knockout-2.1.0"
    }
});

require(["ordnung/loader", "customBindings"], function(load, customBindings) {
    load();
});