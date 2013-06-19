define(["Service/popupService", "Common/AuthenticateVM"], function(popupService, AuthenticateVM) {

    var _loggedIn = false;
    var _userId = null;


    var _authenticateVM = new AuthenticateVM();
    var _popup = popupService.createPopup("AuthenticationPopup", _authenticateVM);
    
    function authenticate(whenDone) {
        if (_loggedIn) {
            whenDone(true, _userId);
        } else {
            var popup = _popup.open();
            _authenticateVM.onAuthenticated(function (userId) {
                _userId = userId;
                _loggedIn = true;
                whenDone(true, userId);
                popup.close();
            });
        }
    }

    return {
        authenticate: authenticate
    };

});