define(["Service/currentUser"], function (currentUser) {

    function NavBarVM() {

        var self = this;


        this.logout = function () {
            currentUser.logout();
        };

        init: {

        }

    }


    return NavBarVM;

});