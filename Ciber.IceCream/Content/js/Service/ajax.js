define(["ordnung/ajax", "when"], function(ajax, when) {

    return function (url, data, method) {

        var deferred = when.defer();

        ajax(url, data, method, function(xhr) {
            if (xhr.status == 200) {
                try {
                    deferred.resolve(JSON.parse(xhr.responseText));
                } catch(e) {
                    deferred.reject(e);
                }
            } else {
                deferred.reject(xhr);
            }
        });

        return deferred.promise;
    };
});