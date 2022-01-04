using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public int Velocidad;
    public int PosX;
    public int PosY;
    public bool Disponible;
    public bool Molino;

    // Start is called before the first frame update
    void Start(){
        this.transform.position = new Vector3(PosX, PosY, 0);
        this.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Disponible = true;
        Molino = false;
    }

    // Update is called once per frame
    void Update(){
        if (Velocidad != 0){ // Si el controlador le asignó una velocidad
            this.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-Velocidad,
                    this.GetComponent<Rigidbody2D>().velocity.y);
            Disponible = false;
        }
        
        if ((int)this.transform.position.x < Random.Range(-2, 10)){ // A partir de esta posición ya puede spawnearse otro obstáculo si hubiera disponible
            Molino = true;
        }

        if ((int)this.transform.position.x == -4){ // Cuando sale de pantalla
            this.transform.position = new Vector3(PosX,
                PosY,
                this.transform.position.z); // Vuelve a la posición inicial a la derecha de la camara
            Velocidad = 0; // Lo detengo pues el controlador deja de indicarle su velocidad
            this.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(
                -Velocidad,
                this.GetComponent<Rigidbody2D>().velocity.y);
            Disponible = true;
            Molino = false;
        }
    }
}
