using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject Juego;
    public GameObject[] Obstaculos;

    private int Contador;
    private int Velocidad;
    private int Indice;
    private bool Arranca;

    private bool Vel1;
    private bool Vel2;
    private bool Vel3;
    private bool Vel4;
    
    // Start is called before the first frame update
    void Start()
    {
        Vel1 = true;
        Vel2 = true;
        Vel3 = true;
        Vel4 = true;
        Velocidad = 0;
        Arranca = true; // Indica que es el primer spawn de obstaculos
        Indice = 0;
    }

    // Update is called once per frame
    void Update(){
        Contador = (int)Juego.GetComponent<GameScript>().Contador; // Copia del contadr del GameScript
        if (!Juego.GetComponent<GameScript>().Comienzo){ // Si comenzó el juego
            if (Obstaculos[Indice].GetComponent<ObstacleScript>().Molino || Arranca){ 
                do{
                    Indice = Random.Range(0, 3);
                    Arranca = false;
                } while (!Obstaculos[Indice].GetComponent<ObstacleScript>().Disponible); // Selecciono un obstáculo del arreglo siempre y cuando esté disponible
            }

            Obstaculos[Indice].GetComponent<ObstacleScript>().Velocidad = 0; // Le asigno velocidad 0

            if (Contador % Random.Range(8, 50) == 0){ // De esta manera hago que se lancen los obstaculos de manera aleatoria
                Obstaculos[Indice].GetComponent<ObstacleScript>().Velocidad = Velocidad;
            }

            if (Contador == 0){ // Si recién comienza
                Velocidad = 11;
            }
            if (Contador == 100 && Vel1){ // Primer cambio de velocidad
                Velocidad += 1;
                Vel1 = false;
            }
            if (Contador == 200 && Vel2){ // Segundo cambio de velocidad
                Velocidad += 1;
                Vel2 = false;
            }
            if (Contador == 300 && Vel3){ // Tercer cambio de velocidad
                Velocidad += 2;
                Vel3 = false;
            }
            if (Contador == 400 && Vel4){ // Cuarto cambio de velocidad
                Velocidad += 1;
                Vel4 = false;
            }
            
            if ((Contador % 75 == 0) && Contador != 0  && (Obstaculos[Obstaculos.Length-1].GetComponent<ObstacleScript>().Disponible)){ // Spawn del último obstaculo a destiempo
                Obstaculos[Obstaculos.Length-1].GetComponent<ObstacleScript>().Velocidad = (Velocidad * 2) - 4;
            }

        }
        if (Juego.GetComponent<GameScript>().Fin){ // En caso que el juego haya acabado
            Velocidad = 0;
            Obstaculos[Indice].GetComponent<ObstacleScript>().Velocidad = Velocidad;
        }
    }
}
