define(["Home/BuyIceCreamVM", "Domain/IceCream", "Service/popupService", "knockout", "ordnung/ajax"], function(BuyIceCreamVM, IceCream, popupService, ko, ajax) {

    function IceCreamListVM() {

        var self = this;

        this.iceCreams = ko.observableArray();

        this.showBuyPopup = function (iceCream) {
            var buyIceCream = new BuyIceCreamVM(iceCream);
            var popup = popupService.createPopup("BuyIceCreamPopup", buyIceCream).open();
            buyIceCream.onBought(popup.close);
        };
        
        init: {


            ajax("/api/IceCream", {includeAll: true}, "GET", function (xhr) {
                var iceCreams = JSON.parse(xhr.responseText).map(function (raw) { return new IceCream(raw); });
                self.iceCreams(iceCreams);
            });
        }

    }


    return IceCreamListVM;

});