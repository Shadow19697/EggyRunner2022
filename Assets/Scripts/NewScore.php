<?php
mb_internal_encoding("UTF-8");
$enlace = mysqli_connect("localhost", "mc19697", "m19a06c97Sh@dow");

if(mysqli_connect_errno()){
    printf("Conexión fallida: %s\n", mysqli_connect_errno());
    exit();
}

mysqli_set_charset($enlace, "utf8");

$nuevoJugador = mysqli_real_escape_string($enlace, $_GET['jugador']);
$nuevoNivel = mysqli_real_escape_string($enlace, $_GET['nivel']);
$nuevoPuntaje = mysqli_real_escape_string($enlace, $_GET['puntaje']);
$hash = $_GET['hash'];

$Jugadores = "SELECT * FROM `RankingJugadores`";
$resultadoJugadores = mysqli_query($enlace, $Jugadores);

$claveSecreta = "dcbkx12fjea59";

$real_hash = md5($nuevoJugador . $nuevoNivel .  $nuevoPuntaje . $claveSecreta);

$JugadorMinimo = "SELECT * FROM `RankingJugadores` WHERE `puntaje` = (SELECT MIN (puntaje) FROM `RankingJugadores`)";

if($real_hash == $hash){
    if($resultadoJugadorMinimo = mysqli_query($enlace, $JugadorMinimo)){
        $consulta = "INSERT INTO RankingJugadores VALUES (NULL, '$nuevoJugador', '$nuevoNivel', '$nuevoPuntaje');";
        if((mysqli_query($enlace, $consulta)))
            echo "Se cargo el jugador";
        else
            echo "Error: " . mysqli_error($enlace);
    }
}

?>