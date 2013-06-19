define(["knockout", "ordnung/ajax"], function (ko, ajax) {

    function AuthenticateVM(_authenticate) {

        var self = this;

        this.buyerId = ko.observable();

        this.authenticate = function() {
            _authenticate();
        };
    }


    return AuthenticateVM;

});