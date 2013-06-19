define(["knockout"], function(ko) {
    ko.bindingHandlers.currency = {
        init: function(element, valueAccessor, allBindingsAccessor, context) {
            var value = valueAccessor();
            
            ko.bindingHandlers.text.update(element, function () { return value + ",-"; }, allBindingsAccessor, context);
        }
    }
});