App.service('user_management_SERVICE',['$http','util_SERVICE',function($http,US){
											
								var url= US.url;
								
								
								
								
								
								this.updateUserRol= function(s,userid)
								 {							
									var rdata = [];									
									var data = {
												"requestType": "authorisation",
												"subRequestType": "updateRoleId",
												"systemId": US.systemId,
												"sessionId": US.gsid(),
												"authKey" : US.authKey,
												"userId": US.userId,
												"roleName":US.roleName,
												"requestId":US.grequestId(),
												"updateRoleCategory": {
													"updatedUserId": parseInt(userid),
													"userRoleid": s,													
													"purcategoryId": [1,3],
													"defaultPurCategory": 3
												}
											};

					
									var promise = $http.get(url+JSON.stringify(data)).success(function(response){
											if (US.eh(response)) {									
												return response;
												//alert(response.status);
											} else {
												//alert('Not Connecting to server');
												return false;
											}
										});
									return promise; 
    							 };
								 
								 
							 	 this.getUser= function()
								 {							
									var rdata = [];
									var data = {
													"requestType": "authorisation",
													"sessionId":US.gsid(),
													"subRequestType": "getUserData",
													"systemId": US.systemId,
													"authKey" : US.authKey,
													"userId": "senthil@gmail.com"													
												};						
									var promise = $http.get(url+JSON.stringify(data)).success(function(response){
											if (US.eh(response)) {									
												return response;
												//alert(response.status);
											} else {
												//alert('Not Connecting to server');
												return false;
											}
										});
									return promise; 
    							 };
								 this.getindentno = function(d)
								 {
									 var rdata = [];
									 for(var i=0;i<d.length;i++)
									 {
										 rdata.push(d[i].id);
									 }
									 return rdata;
								 }
								 
								 this.getItemDetails = function(s)
								 {							
									var rdata = [];
									var data = {
													"requestType": "deliveryNote",
													"sessionId":US.gsid(),
													"subRequestType": "getDNItemDetails",
													"systemId": US.systemId,
													"authKey" : US.authKey,
													"userId": US.userId,"roleName":US.roleName,"requestId":US.grequestId(),
													 "parameter": {
																		   "compDeptid": s
																  }
												};						
									var promise = $http.get(url+JSON.stringify(data)).success(function(response){
											if (US.eh(response)) {									
												return response;
												//alert(response.status);
											} else {
												//alert('Not Connecting to server');
												return false;
											}
										});
									return promise; 
    							 
								 }
								 
								 
								 this.saveIndentDept = function(s)
								 {}
								 
								  this.getSupplierItem = function(id)
								 {};
								 
								 this.GetIndentDept = function()
								 {}
								 
								 
								  this.approved = function(scope,supid)
								 {};
								 
							 }]);

