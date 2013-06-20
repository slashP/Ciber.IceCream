define(["Service/popupService", "Common/AuthenticateVM", "when"], function(popupService, AuthenticateVM, when) {

    var _loggedIn = false;
    var _userId = null;


    var _authenticateVM = new AuthenticateVM();
    var _popup = popupService.createPopup("AuthenticationPopup", _authenticateVM);
    
    function authenticate() {
        if (_loggedIn) {
            return when.defer().resolve(_userId);
        } else {
            var popup = _popup.open();
            var deferred = when.defer();
            _authenticateVM.onAuthenticated(deferred.resolver);
            return deferred.promise.then(function(userId) {
                _userId = userId;
                _loggedIn = true;
                popup.close();
                return _userId;
            });
        }
    }

    return {
        authenticate: authenticate
    };

});