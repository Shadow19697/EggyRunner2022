using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformScript : MonoBehaviour
{
    public int Velocidad;
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        this.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(
            -Velocidad,
            this.GetComponent<Rigidbody2D>().velocity.y);
        if ((int)this.transform.position.x == -25){ // Cuando haya salido de pantalla que se vuelva a posicionar a la derecha del anterior
            this.transform.position = new Vector3 (46,
                this.transform.position.y,
                this.transform.position.z);
        }
    }
}
