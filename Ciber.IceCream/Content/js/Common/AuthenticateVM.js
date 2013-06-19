define(["knockout", "ordnung/ajax"], function (ko, ajax) {

    function AuthenticateVM() {

        var self = this;
        var _onAuthenticated = null;
        

        this.buyerId = ko.observable();

        this.onAuthenticated = function(callback) {
            _onAuthenticated = callback;
        };

        this.authenticate = function () {
            console.log("authenticate");
            _onAuthenticated(self.buyerId());
        };
    }


    return AuthenticateVM;

});