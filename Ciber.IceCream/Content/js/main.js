

require.config({
    baseUrl: "/content/js",
    urlArgs: "bust=" + document.getElementsByTagName("meta")[0].getAttribute("content"),
    paths: {
        "ordnung": "ordnung",
        "knockout": "libs/knockout-2.1.0",
        "fastclick": "libs/fastclick"
    },
    packages: [
        { name: 'when', location: 'libs/components/when/', main: 'when' },
        // ... other packages ...
    ]
});

require(["ordnung/loader", "fastclick", "customBindings"], function (load, FastClick, customBindings) {
    FastClick.attach(document.body);
    document.title = (document.URL.indexOf("ciberice-dev") !== -1 ? "DEV - " : "") + document.title;
    load();
});