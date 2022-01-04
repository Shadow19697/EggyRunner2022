using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformController : MonoBehaviour
{
    public GameObject[] Pisos;
    public GameObject Juego;

    private int Contador;
    private int Velocidad;
    private int Indice;

    private bool Vel1;
    private bool Vel2;
    private bool Vel3;
    private bool Vel4;


    // Start is called before the first frame update
    void Start(){
        Vel1 = true;
        Vel2 = true;
        Vel3 = true;
        Vel4 = true;
        Velocidad = 8;
        Indice = 0;
    }

    // Update is called once per frame
    void Update(){
        Contador = (int)Juego.GetComponent<GameScript>().Contador; // Copia del contadr del GameScript
        if (!Juego.GetComponent<GameScript>().Fin){ // Si comenzó el juego 

            Pisos[Indice].GetComponent<PlataformScript>().Velocidad = Velocidad;
            Pisos[Indice+1].GetComponent<PlataformScript>().Velocidad = Velocidad;

            if (Contador == 100 && Vel1){ // Primer cambio de velocidad
                Velocidad += 1;
                Vel1 = false;
            }
            if (Contador == 200 && Vel2){  // Segundo cambio de velocidad
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
        }
        else{ // Si pierdo, detengo los pisos
            Pisos[Indice].GetComponent<PlataformScript>().Velocidad = 0;
            Pisos[Indice + 1].GetComponent<PlataformScript>().Velocidad = 0;
        }
    }
}
