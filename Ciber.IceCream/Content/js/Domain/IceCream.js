define(["knockout"], function(ko) {
    function IceCream(raw) {
        this.title = raw.Title || "";
        this.imageURL = raw.ImageURL || "";
        this.price = raw.Price || 0;
    }

    return IceCream;
});