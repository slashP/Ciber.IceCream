define(["knockout"], function(ko) {
    function IceCream(raw) {
        this.id = raw.Id;
        this.title = raw.Title || "";
        this.imageURL = raw.Image || "";
        this.price = raw.Price || 0;
        this.quantityAvailable = ko.observable(raw.Quantity || 0);
    }

    return IceCream;
});