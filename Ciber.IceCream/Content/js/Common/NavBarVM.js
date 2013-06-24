define(["Service/currentUser", "knockout"], function (currentUser, ko) {


    function NavBarVM() {

        var self = this;
        var setIsLoggedIn = function() {
            self.isLoggedIn(!!localStorage.getItem("CiberIceUserId"));
        };
        this.isLoggedIn = ko.observable(false);

        this.logout = function () {
            currentUser.logout();
            setIsLoggedIn();
        };

        init: {
            setIsLoggedIn();
        }

    }


    return NavBarVM;

});