<?php
mb_internal_encoding("UTF-8");
$enlace = mysqli_connect("localhost", "id18768240_mc19697", "m19a06c97Sh@dow");
mysqli_select_db($enlace, "id18768240_eggyrunner");

if(mysqli_connect_errno()){
    printf("Conexión fallida: %s\n", mysqli_connect_errno());
    exit();
}

mysqli_set_charset($enlace, "utf8");

$data = file_get_contents('php://input');
$row = json_decode($data, true);

$query = "INSERT INTO RankingJugadores VALUES (NULL, '" . $row['name'] . "','" . $row['level'] . "','" . $row['score'] . "')";

if((mysqli_query($enlace, $query)))
    echo "OK";
else
    echo "Error: " . mysqli_error($enlace);
?>