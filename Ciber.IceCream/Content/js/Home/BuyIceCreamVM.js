define(["Service/currentUser", "knockout", "Service/ajax"], function(currentUser, ko, ajax) {

    function BuyIceCreamVM(selectedIceCream) {
        var self = this;


        this.selectedIceCream = ko.observable(selectedIceCream);
        this.isBuying = ko.observable(false);
        this.hasBought = ko.observable(false);
        this.errorMessage = ko.observable("");


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
            }).then(
                function () {
                    self.hasBought(true);
                },
                function (error) {
                    if (error.status === 409) {
                        self.errorMessage("Ikke flere igjen av denne isen.");
                    }
                }
            );
        };
    }

    return BuyIceCreamVM;

});