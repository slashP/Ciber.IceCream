define([], function() {

    var _popupStack = [];
    var _requestPopup = null;
    var _requestClose = null;

    function subscribe(onShowRequested, onCloseRequested) {
        _requestPopup = onShowRequested;
        _requestClose = onCloseRequested;
        return popupClosed;
    }
    
    function topOfStack() {
        return _popupStack[_popupStack.length - 1];
    }

    function createPopup(template, viewmodel, onClose) {
        console.log("create popup");
        var popup = {
            open: function () {
                console.log("open popup");
                return openPopup({ template: template, viewmodel: viewmodel, onClose: onClose, closeRequested: false});
            }
        };
        return popup;
    }
    function openPopup(popup){
        _popupStack.push(popup);
        _requestPopup(popup);

        return {
            close: function () {
                if (topOfStack() == popup) {
                    console.log("close popup");
                    _requestClose();
                } else {
                    popup.closeRequested = true;
                }
            }
        };
    }

    function popupClosed(result) {
        console.log("popupClosed");
        var popup = _popupStack.pop();
        if (popup.onClose)
            popup.onClose(result);
        if (_popupStack.length > 0) {
            var nextPopup = topOfStack();
            if (nextPopup.closeRequested) {
                nextPopup.close();
            }else{
                _requestPopup(nextPopup);
            }
        }
    }
    
    return {
        __subscribe__: subscribe,
        createPopup: createPopup
    };

});