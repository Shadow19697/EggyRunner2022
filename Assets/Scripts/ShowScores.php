<?php
mb_internal_encoding("UTF-8");
$link = mysqli_connect("localhost", "id18768240_mc19697", "m19a06c97Sh@dow");
mysqli_select_db($link, "id18768240_eggyrunner");

if(mysqli_connect_errno()){
    printf("Conexión fallida: %s\n", mysqli_connect_errno());
    exit();
}

mysqli_set_charset($link, "utf8");

$query = "SELECT name, score, level FROM `RankingJugadores` ORDER BY `score` DESC";

if($result = mysqli_query($link, $query)){
    $newArray = array();
    while($row = mysqli_fetch_assoc($result)){
        $newArray[] = $row;
    }
    echo json_encode($newArray, JSON_NUMERIC_CHECK);
}
else{
    printf("Error");
}
?>