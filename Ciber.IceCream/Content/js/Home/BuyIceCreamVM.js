define(["Domain/currentUser", "knockout", "ordnung/ajax"], function(currentUser, ko, ajax) {

    function BuyIceCreamVM(onBought) {
        var self = this;

        this.selectedIceCream = ko.observable();
        this.buy = function () {
            var id = self.selectedIceCream().id;
            ajax("/api/buy", { iceCreamId: id, buyer: currentUser.id}, "POST", function(xhr) {
                onBought(id);
            });
        };
    }

    return BuyIceCreamVM;

});