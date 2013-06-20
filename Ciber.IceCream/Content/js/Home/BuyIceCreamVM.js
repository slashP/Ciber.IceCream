define(["Service/currentUser", "knockout", "Service/ajax"], function(currentUser, ko, ajax) {

    function BuyIceCreamVM(selectedIceCream) {
        var self = this;


        this.selectedIceCream = ko.observable(selectedIceCream);
        this.isBuying = ko.observable(false);
        this.hasBought = ko.observable(false);


        this.showBuyButton = ko.computed(function () {
            return self.isBuying() == false && self.hasBought() == false;
        });
        this.showSpinner = ko.computed(function () {
            return self.isBuying() == true && self.hasBought() == false;
        });

        this.buy = function () {
            var iceCreamId = self.selectedIceCream().id;
            self.isBuying(true);
            currentUser.authenticate(
            ).then(function (currentUserId) {
                return ajax("/api/buy", { iceCreamId: iceCreamId, buyer: currentUserId }, "POST");
            }).then(function(response) {
                self.hasBought(true);
            });
        };
    }

    return BuyIceCreamVM;

});