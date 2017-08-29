<?php
//echo $_SERVER['HOME'];
//upload_max_filesize = 25M;


date_default_timezone_set('Singapore');
//printf($_FILES);
$WebURL ="C:\\inetpub\\wwwroot\\ICSB_Live\\TempFiles\\";
$SAPURL = "";
$eMSG = "";

$path_parts = pathinfo($_FILES["file"]["name"]);
$filePath = $path_parts['filename'].'_'.time().'.'.$path_parts['extension'];
		$fileName = $_FILES["file"]["name"];	
		$id = $_POST['name'];				
		$target_file = $WebURL . basename($filePath);
		
if($_FILES["file"]['size'] > 100000000)
{
	$eMSG = "File size should be less than 100mb";
	$status = 'false';
}
else
{
	if(move_uploaded_file($_FILES["file"]["tmp_name"], $target_file ))
	{
		$eMSG = "";
		$status = 'true';
	}
	else
	{
		$eMSG = "something went wrong.";
		$status = 'false';
	}
		
}

	$rarr = array('FileName'=>$filePath,'FilePath'=>$WebURL.$filePath,'id'=>$id,'Date'=>date('d-m-Y'),'Status'=>$status,'errMSG'=>$eMSG);
		echo json_encode($rarr);
			
//print_r($arr);
?>
