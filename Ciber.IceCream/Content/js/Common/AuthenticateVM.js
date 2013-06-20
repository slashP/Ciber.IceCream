define(["knockout", "ordnung/ajax", "when"], function (ko, ajax, when) {

    function AuthenticateVM() {

        var self = this;
        var _onAuthenticatedResolver = null;
        

        this.buyerId = ko.observable();

        this.onAuthenticated = function(resolver) {
            _onAuthenticatedResolver = resolver;
        };

        this.authenticate = function () {
            console.log("authenticate");
            if (true) {
                _onAuthenticatedResolver.resolve(self.buyerId());
            } else {
                _onAuthenticatedResolver.reject();
            }
        };
    }


    return AuthenticateVM;

});