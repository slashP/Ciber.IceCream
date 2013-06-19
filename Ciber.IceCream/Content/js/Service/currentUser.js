define(["Service/popupService", "Common/AuthenticateVM"], function(popupService, AuthenticateVM) {

    var loggedIn = false;
    var userId = null;

    function authenticate(whenDone) {
        if (loggedIn) {
            whenDone(true, userId);
        } else {
            var authenticateVM = new AuthenticateVM();
            var popup = popupService.createPopup("AuthenticationPopup", authenticateVM, function (success) {
                if (success) {
                    userId = authenticateVM.buyerId();
                    loggedIn = true;
                    whenDone(true, userId);
                } else {
                    whenDone(false);
                }
            }).open();
        }
    }

    return {
        authenticate: authenticate
    };

});