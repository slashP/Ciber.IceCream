define(["knockout", "ordnung/ajax"], function (ko, ajax) {
    function ReportVM() {
        var self = this;
        this.lastNonPaid = ko.observable('');
        this.dateFrom = ko.observable('');
        this.dateTo = ko.observable('');
        this.seeReport = function () {
            window.location = "/Report/SeeReport?dateFrom=" + self.dateFrom() + "&dateTo=" + self.dateTo();
        };
        this.effectuateReport = function () {
            window.location = "/Report/EffectuateReport?dateFrom=" + self.dateFrom() + "&dateTo=" + self.dateTo();
        };
        (function() {
            ajax("/Report/LastNotPaid", {}, "GET", function (xhr) {
                console.log("last not paid is " + xhr.responseText);
                self.dateFrom(xhr.responseText);
                var isoString = new Date().toISOString().substr(0, 10);
                self.dateTo(isoString);
                self.lastNonPaid(xhr.responseText);
            });
        })();
    }

    return ReportVM;
});