define(["Home/BuyIceCreamVM", "Domain/IceCream", "knockout", "ordnung/ajax"], function(BuyIceCreamVM, IceCream, ko, ajax) {

    function IceCreamListVM() {

        var self = this;

        this.iceCreams = ko.observableArray();
        this.showPopup = ko.observable(false);

        this.showBuyPopup = function (iceCream) {
            self.buyIceCream.selectedIceCream(iceCream);
            self.showPopup(true);
        };
        this.hideBuyPopup = function (iceCream, event) {
            self.showPopup(false);
        };
        
        init: {

            this.buyIceCream = new BuyIceCreamVM(function() {
                self.showPopup(false);
            });

            ajax("/api/IceCream", {includeAll: true}, "GET", function (xhr) {
                var iceCreams = JSON.parse(xhr.responseText).map(function (raw) { return new IceCream(raw); });
                self.iceCreams(iceCreams);
            });
        }

    }


    return IceCreamListVM;

});