define(["knockout", "ordnung/ajax"], function (ko, ajax) {
    function RoleAdminVM() {
        var self = this;
        this.roleName = ko.observable('');
        this.result = ko.observable('');
        this.addRole = function () {
            ajax("/RoleAdmin/AddRole", { name: self.roleName() }, "POST", function (xhr) {
                self.result(xhr.responseText);
            });
        };
    }

    return RoleAdminVM;
});