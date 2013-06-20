define(["Service/currentUser", "knockout", "Service/ajax"], function(currentUser, ko, ajax) {

    function BuyIceCreamVM(selectedIceCream) {
        var self = this;

        var onBought;

        this.selectedIceCream = ko.observable(selectedIceCream);

        this.onBought = function(callback) {
            onBought = callback;
        };
        

        this.buy = function () {
            var iceCreamId = self.selectedIceCream().id;

            currentUser.authenticate(
            ).then(function (currentUserId) {
                return ajax("/api/buy", { iceCreamId: iceCreamId, buyer: currentUserId }, "POST");
            }).then(function(response) {
                onBought(iceCreamId);
            });
        };
    }

    return BuyIceCreamVM;

});