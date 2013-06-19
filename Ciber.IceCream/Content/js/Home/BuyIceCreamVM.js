define(["Service/currentUser", "knockout", "ordnung/ajax"], function(currentUser, ko, ajax) {

    function BuyIceCreamVM(selectedIceCream) {
        var self = this;

        var onBought;

        this.selectedIceCream = ko.observable(selectedIceCream);

        this.onBought = function(callback) {
            onBought = callback;
        };
        
        this.buy = function () {
            var iceCreamId = self.selectedIceCream().id;
            currentUser.authenticate(function (success, currentUserId) {
                if (success) {
                    ajax("api/buy", { iceCreamId: iceCreamId, buyer: currentUserId }, "POST", function (xhr) {
                        console.log("buy response:", xhr);
                        if (xhr.status == 200) {
                            onBought(iceCreamId);
                        }
                    });
                }
            });
        };
    }

    return BuyIceCreamVM;

});