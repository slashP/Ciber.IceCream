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
    


    /**
usage:

this.observable.extend({
    throttledSubscribe: {
        throttle: 500, //milliseconds
        subscribe: function(value){
            doAjax(value);
        }
    }
});


this.onload = function(){
    //does not cause throttledSubscribe to get called
    this.observable.updateWithoutNotifyingSubscriber(newValue);
};

this.setManually = function(newValue){
    //does not cause throttledSubscribe to get called the next time the observable is set
    this.observable.disregardNextUpdate();
    this.observable(newValue);
    //from now on the observable will call
}
*/

    ko.extenders.throttledSubscribe = function (target, params) {
        var throttleTime = params.throttle;
        var subscriber = params.subscribe;

        var updating = false;
        target.disregardNextUpdate = function () {
            updating = true;
        };
        target.updateWithoutNotifyingSubscriber = function (value) {
            updating = true;
            target(value);
        };

        var throttledComputed = ko.computed(function () {
            return target();
        }).extend({ throttle: throttleTime });

        throttledComputed.subscribe(function (value) {
            if (updating == false) {
                subscriber(value);
            } else {
                updating = false;
            }
        });
    };
});