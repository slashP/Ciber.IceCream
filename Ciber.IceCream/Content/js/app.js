App = Ember.Application.create();

App.Router.map(function() {
  this.resource('buyIceCream', { path: '/' }, function(){
  	this.resource('now', {path: '/buyNow/:id' });
  });
  this.resource('fillFreezer', function(){
  	this.route('addBrand');
  });
});


App.BuyIceCreamNowController = Ember.ObjectController.extend({

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

App.BuyIceCreamRoute = Ember.Route.extend({
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