define(["Home/BuyIceCreamVM", "Domain/IceCream", "Service/popupService", "knockout", "Service/ajax", "when", "Service/currentUser"], function (BuyIceCreamVM, IceCream, popupService, ko, ajax, when, currentUser) {



    function mapResult(method) {
        return function (array) {
            return array.map(method);
        };
    }

    function filterResult(method) {
        return function (array) {
            return array.filter(method);
        };
    }
    

    function IceCreamListVM() {

        var self = this;

        this.iceCreams = ko.observableArray();

        this.showBuyPopup = function (iceCream) {
            var buyIceCream = new BuyIceCreamVM(iceCream);
            popupService.createPopup("BuyIceCreamPopup", buyIceCream).open();
        };

        init: {
                var updateIceCreams = function() {
                    ajax("/api/IceCream", {
                        includeAll: true
                    }, "GET").then(mapResult(function (raw) {
                        return new IceCream(raw);
                    })).then(filterResult(function (iceCream) {
                        if (currentUser.isAdmin())
                            return true;
                        else
                            return iceCream.quantityAvailable() > 0;
                    })).then(self.iceCreams);
                };
                  setInterval(function() {
                      updateIceCreams();
                  }, 30000);
                  updateIceCreams();
              }

    }


    return IceCreamListVM;

});