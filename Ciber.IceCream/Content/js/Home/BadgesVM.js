define(["Service/currentUser", "knockout", "Service/ajax"], function(currentUser, ko, ajax) {
    function BadgesVM() {
        var self = this;
        self.badges = ko.observableArray([]);
        init: {
                  currentUser.authenticate(
                  ).then(function(currentUserId) {
                      return ajax("/api/badge", { buyer: currentUserId }, "GET");
                  }).then(function (response) {
                      for (var i = 0; i < response.BadgesForUser.length; i++) {
                          self.badges.push({ badge: response.BadgesForUser[i] });
                      }
                  });
              }
    }
    return BadgesVM;
});

