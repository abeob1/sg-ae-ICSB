App.service('SQ_SERVICE', ['$http', 'util_SERVICE', function ($http,US) {
											
							
								
								
								
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
								 
								 
							 }]);

