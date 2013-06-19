define(["knockout", "ordnung/ajax"], function (ko, ajax) {
    function RoleAdminVM() {
        var self = this;
        this.userName = ko.observable('');
        this.result = ko.observable('');
        this.addUserAsAdmin = function () {
            ajax("RoleAdmin/AddUser", { name: self.userName() }, "POST", function (xhr) {
                self.result(xhr.responseText);
            });
        };
    }

    return RoleAdminVM;
});