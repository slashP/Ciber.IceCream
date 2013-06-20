define(["Service/popupService", "Common/AuthenticateVM", "when"], function(popupService, AuthenticateVM, when) {
    
    var _authenticateVM = new AuthenticateVM();
    var _popup = popupService.createPopup("AuthenticationPopup", _authenticateVM);
    
    function ifExists(value, truthy, falsy) {
        if (value) {
            return truthy(value);
        } else {
            return falsy();
        }
    }

    
    function authenticate() {
        return ifExists(localStorage.getItem("CiberIceUserId"),
            function(userId) {
                return when.defer().resolve(userId);
            }, function() {
                var popup = _popup.open();
                var deferred = when.defer();
                _authenticateVM.onAuthenticated(deferred.resolver);
                return deferred.promise.then(function(userId) {
                    localStorage.setItem("CiberIceUserId", userId);
                    popup.close();
                    return userId;
                });
            }
        );
    }
    
    function logout() {
        
    }

    return {
        authenticate: authenticate,
        logout: logout
    };

});