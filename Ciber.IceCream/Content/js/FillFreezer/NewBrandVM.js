define(["knockout", "ordnung/ajax"], function (ko, ajax) {

    function NewBrandVM() {

        var self = this;

        this.title = ko.observable("");
        this.image = ko.observable("");
        this.price = ko.observable(0);
        this.saveResult = ko.observable("");

        this.addNewBrand = function() {
            ajax("/api/icecream", {
                title: self.title(),
                image: self.image(),
                price: self.price()
            }, "POST", function (xhr) {
                self.title("").image("").price("").saveResult("Saved new ice.");
            }, function(reason) {
                self.saveResult("Something failed");
                console.log(reason);
            });
        };

        init: {

        }

    }


    return NewBrandVM;

});