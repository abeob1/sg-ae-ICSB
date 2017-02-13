<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="WebApplication1.login1" %>

<!--
Author: W3layouts
Author URL: http://w3layouts.com
License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE HTML>
<html>
<head>
<title>ICSB</title>
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="ICSB" />
<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
<!-- Bootstrap Core CSS -->
<link href="asset/css/bootstrap.css" rel='stylesheet' type='text/css' />
<!-- Custom CSS -->
<link href="asset/css/style.css" rel='stylesheet' type='text/css' />
<!-- font CSS -->
<!-- font-awesome icons -->
<link href="asset/css/font-awesome.css" rel="stylesheet"> 
<!-- //font-awesome icons -->
 <!-- js-->
  <!-- Loading Angular libs-->
    <script src="asset/js/jquery-1.11.1.min.js"></script>
    <script src="asset/js/libs/angular.js"></script>
      <script src="asset/js/libs/angular-route.js"></script>
     <script src="asset/js/libs/angular-ui-router.js"></script>
    <script src="asset/js/libs/ocLazyLoad.js"></script>
    <script src="asset/js/libs/ui-bootstrap.js"></script>
    <script src="asset/js/libs/ui-bootstrap-tpls-0.9.0.js"></script>
    <script src="asset/js/libs/angular-cookies.js"></script>

    <!-- angular js short cut keys-->
    <script src="asset/js/libs/hotkeys.js"></script> 



<!-- angular js -->

<script src="asset/js/modernizr.custom.js"></script>
    <script src="asset/js/config/app.js"></script>
    <script src="asset/js/services/util_factory.js" ></script>
	<script src="asset/js/ctrl/signin_ctrl.js" ></script>
<!--webfonts-->
<link href='//fonts.googleapis.com/css?family=Roboto+Condensed:400,300,300italic,400italic,700,700italic' rel='stylesheet' type='text/css'>
<!--//webfonts--> 
<!--animate-->
<link href="asset/css/animate.css" rel="stylesheet" type="text/css" media="all">
<script src="asset/js/wow.min.js"></script>
	<script>
	    new WOW().init();
	</script>
<!--//end-animate-->
<!-- Metis Menu -->
<script src="asset/js/metisMenu.min.js"></script>
<script src="asset/js/custom.js"></script>
<link href="asset/css/custom.css" rel="stylesheet">
<!--//Metis Menu -->
</head> 
<body class="cbp-spmenu-push" ng-app="myApp" ng-controller="signin">
	<div class="main-content" >
		<!--left-fixed -navigation-->
		
		<!--left-fixed -navigation-->
		<!-- header-starts -->
		
		<!-- //header-ends -->
		<!-- main content start-->
		<div id="page-wrapper" style="margin:0px;">
			<div class="main-page login-page ">
				<h3 class="title1">Sign In</h3>
				<div class="widget-shadow">
					<div class="login-top">
					<img src="asset/images/logo.png" />
					</div>
					<div class="login-body">
						<form action="leadmaster.aspx">
							<input type="text" class="user" name="email" placeholder="Username" ng-model="userId" required="">
							<input type="password" name="password" class="lock" placeholder="password" ng-model="password" ng-Enter="checklogin();">
							<input type="button" name="Sign In" value="Sign In" ng-click="checklogin();">
							<div class="forgot-grid">
								<label class="checkbox"><input type="checkbox" name="checkbox" ><i></i>Remember me</label>
								<div class="forgot">
									<a href="#">forgot password?</a>
								</div>
								<div class="clearfix"> </div>
							</div>
						</form>
					</div>
				</div>
				
				
			</div>
		</div>
		<!--footer-->
		<div class="footer">
		   <p>&copy; 2016 ICSB. All Rights Reserved </p>
		</div>
        <!--//footer-->
	</div>
	<!-- Classie -->
		<script src="js/classie.js"></script>
		
	<!--scrolling js-->
	<script src="js/jquery.nicescroll.js"></script>
	<script src="js/scripts.js"></script>
	<!--//scrolling js-->
	<!-- Bootstrap Core JavaScript -->
   <script src="js/bootstrap.js"> </script>
</body>
</html>