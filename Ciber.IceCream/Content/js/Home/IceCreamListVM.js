define(["Domain/IceCream", "knockout", "ordnung/ajax"], function(IceCream, ko, ajax) {

    function IceCreamListVM() {

        var self = this;

        this.iceCreams = ko.observableArray();
        
        init:{
            ajax("api/IceCream", {}, "GET", function (xhr) {
                var iceCreams = JSON.parse(xhr.responseText).map(function (raw) { return new IceCream(raw); });
                console.log(iceCreams);
                self.iceCreams(iceCreams);
            });
        }

    }


    return IceCreamListVM;

});