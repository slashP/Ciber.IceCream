define(["Service/currentUser", "knockout", "Service/ajax"], function (currentUser, ko, ajax) {

    function AdminIceCreamVM(selectedIceCream) {
        var self = this;
        
        this.selectedIceCream = ko.observable(selectedIceCream);
        this.quantity = ko.observable(0);
        this.price = ko.observable(0);

        this.isAdding = ko.observable(false);
        this.hasAdded = ko.observable(false);
        this.errorMessage = ko.observable("");


        this.showAddButton = ko.computed(function () {
            return self.isAdding() == false && self.hasAdded() == false;
        });
        this.showSpinner = ko.computed(function () {
            return self.isAdding() == true && self.hasAdded() == false;
        });

        this.add = function () {
            var iceCreamId = self.selectedIceCream().id;
            self.isAdding(true);
            currentUser.authenticate(
            ).then(function (currentUserId) {
                return ajax("/api/icecream", {
                    iceCreamId: iceCreamId,
                    quantity: self.quantity(),
                    price: self.price()
                }, "PUT");
            }).then(function (response) {
                self.hasAdded(true);
                self.selectedIceCream().quantityAvailable(response.quantity);
                self.selectedIceCream().price(response.price);
            }, function (reason) {
                if (reason.status === 409) {
                    self.errorMessage("Ikke flere igjen av denne isen.");
                }
                self.isAdding(false);
            });
        };
    }

    return AdminIceCreamVM;

});