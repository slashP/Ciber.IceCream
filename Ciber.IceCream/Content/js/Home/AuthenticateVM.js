define(["Domain/currentUser", "knockout", "ordnung/ajax"], function (currentUser, ko, ajax) {

    function AuthenticateVM() {

        var self = this;
        var whenAuthenticated = null;

        this.buyerId = ko.observable();
        this.isPopupVisible = ko.observable(false);

        this.showPopup = function (whenDone) {
            self.isPopupVisible(true);
            whenAuthenticated = whenDone;
        };
        this.hidePopup = function () {
            self.isPopupVisible(false);
        };

        this.authenticate = function() {
            whenAuthenticated(self.buyerId());
        };

        init: {
            currentUser.showLoginMethod(self.showPopup);
        }

    }


    return AuthenticateVM;

});