define(["ordnung/ajax", "when"], function(ajax, when) {

    return function (url, data, method) {

        var deferred = when.defer();

        ajax(url, data, method, function(xhr) {
            if (xhr.status == 200) {
                deferred.resolve(xhr.responseText);
            } else {
                xhr.reject(xhr);
            }
        });

        return deferred.promise;
    };
});