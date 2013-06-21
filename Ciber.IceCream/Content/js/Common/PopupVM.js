define(["Service/popupService", "knockout"], function (popupService, ko) {

    function PopupVM() {

        var self = this;

        var onPopupClosed = null;
        
        this.isPopupVisible = ko.observable(false);
        this.templateName = ko.observable();
        this.viewmodel = ko.observable();

        this.template = function() {
            return self.templateName();
        };
        this.showPopup = function (popup) {
            self.isPopupVisible(false);
            self.viewmodel(popup.viewmodel);
            self.templateName(popup.template);
            self.isPopupVisible(true);
        };
        this.hidePopup = function () {
            self.isPopupVisible(false);
            onPopupClosed(false);
        };


        this.isPopupVisible.extend({
            throttledSubscribe: {
                throttle: 1,
                subscribe: function (value) {
                    document.body.style.overflow = value ? "hidden" : "auto";
                    document.body.parentNode.style.overflow = value ? "hidden" : "auto";
                }
            }
        });
        
        init: {
            onPopupClosed = popupService.__subscribe__(self.showPopup, self.hidePopup);
        }

    }


    return PopupVM;

});