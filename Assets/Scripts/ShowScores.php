<?php
mb_internal_encoding("UTF-8");
$enlace = mysqli_connect("localhost", "id18768240_mc19697", "m19a06c97Sh@dow");
mysqli_select_db($enlace, "id18768240_eggyrunner");
if(mysqli_connect_errno()){
    printf("Conexión fallida: %s\n", mysqli_connect_errno());
    exit();
}

mysqli_set_charset($enlace, "utf8");

$consulta = "SELECT * FROM `RankingJugadores` ORDER BY `puntaje` DESC LIMIT 10";
//$resultado = mysqli_query($enlace, $consulta);
//$num = mysqli_num_rows($resultado);
//for($i = 0; $i < $num; $i++){
//    $row = mysqli_fetch_array($resultado, MYSQLI_ASSOC);
//    echo $row['jugador'] . ";" . $row['nivel'] . ";" . $row['puntaje'] . ";";
//}

if($resultado = mysqli_query($enlace, $consulta)){
    $nuevoArray = array();
    while($campoDB = mysqli_fetch_assoc($resultado)){
        $nuevoArray[] = $campoDB;
    }
    echo json_encode($nuevoArray);
}
else{
    printf("Error");
}

?>