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
                popup.onClose(deferred.resolver.reject);
                _authenticateVM.onAuthenticated(deferred.resolver);
                return deferred.promise.then(function(userId) {
                    localStorage.setItem("CiberIceUserId", userId);
                    localStorage.setItem("CiberIceUserIsAdmin", userId == 250);
                    popup.close();
                    return userId;
                });
            }
        );
    }
    
    function logout() {
        localStorage.setItem("CiberIceUserId", "");
        localStorage.setItem("CiberIceUserIsAdmin", "");
    }
    
    function isAdmin() {
        return localStorage.getItem("CiberIceUserIsAdmin") === "true";
    }
    
    function isLoggedIn() {
        return !!localStorage.getItem("CiberIceUserId");
    }

    return {
        authenticate: authenticate,
        logout: logout,
        isAdmin: isAdmin,
        isLoggedIn: isLoggedIn
    };

});