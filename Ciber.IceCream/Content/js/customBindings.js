define(["knockout"], function(ko) {
    ko.bindingHandlers.currency = {
        init: function(element, valueAccessor, allBindingsAccessor, context) {
            var value = valueAccessor();

            ko.bindingHandlers.text.update(element, function() { return value + ",-"; }, allBindingsAccessor, context);
        }
    };
    





    /**
    Sets the html inside an element and binds in to a viewmodel
    Usage: 
    <div data-bind="bindHTML: '<img data-bind=\"attr:{src:src}\" />', viewModel: {src:'path/to/image.gif'}"></div>
    */

    ko.bindingHandlers.bindHtml = {
        init: function () {
            return { controlsDescendantBindings: true };
        },
        update: function (element, valueAccessor, allBindingsAccessor) {
            var html = ko.utils.unwrapObservable(valueAccessor()),
                viewModel = ko.utils.unwrapObservable(allBindingsAccessor().viewModel);

            if (html) {
                $(element).html(html);
            }
            if (viewModel != null) {
                ko.applyBindingsToDescendants(viewModel, element);
            }

        }
    };
});