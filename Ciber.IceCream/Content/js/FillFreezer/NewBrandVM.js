define(["knockout", "ordnung/ajax"], function (ko, ajax) {

    function NewBrandVM() {

        var self = this;

        this.title = ko.observable("");
        this.image = ko.observable("");
        this.price = ko.observable(0);

        this.addNewBrand = function() {
            ajax("/api/icecream", {
                title: self.title(),
                image: self.image(),
                price: self.price()
                }, "POST", function (xhr) {
                    console.log("success");
                });
        };

        init: {

        }

    }


    return NewBrandVM;

});