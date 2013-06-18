App = Ember.Application.create();

App.Router.map(function() {
  this.resource('iceCreams', { path: '/' }, function(){
  	this.resource('buy', {path: '/buy/:id' });
  });
  this.resource('fillFreezer', function(){
  	this.route('addBrand');
  });
});


App.IceCreamsController = Ember.ArrayController.extend({

});


App.IceCreamsBuyController = Ember.ObjectController.extend({

	buy: function(){
		//post to server
		//go back to front page
	}
});


App.FillFreezerAddBrandController = Ember.ObjectController.extend({

	addBrand: function(){
		//post to server
		//hide the addBrand view
	}
});

App.IceCreamsRoute = Ember.Route.extend({
  model: function() {
    return [
		{
			id: 1,
			title: 'Krone-is Jordbær',
			image: "images/is_1.jpg",
			price: 8
		},
		{
			id: 2,
			title: 'Lollipop',
			image: "images/is_2.jpg",
			price: 3
		},
		{
			id: 3,
			title: 'Snickers is',
			image: "images/is_3.jpg",
			price: 9
		}
    ];
  }
});