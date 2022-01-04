using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorScript : MonoBehaviour
{
    public int PotSalto;
    public AudioClip Salto;
    public ParticleSystem Explosion;
    public ParticleSystem Webito;

    private bool SaltoDoble = false;
    private bool EnPiso = false;
    private AudioSource Sonido;

    // Start is called before the first frame update
    void Start(){
        Sonido = GetComponent<AudioSource>();
        SaltoDoble = false;
        Explosion.Pause();
        Webito.Pause();
        this.GetComponent<JugadorScript>().Webito.transform.position = new Vector3(
            this.GetComponent<JugadorScript>().Webito.transform.position.x,
            this.GetComponent<JugadorScript>().Webito.transform.position.y,
            -13); // Por cuestiones que desconozco, probando el juego no aparece esta animación en el "Game", pero en el Build si, por lo que lo saco de camara
    }

    // Update is called once per frame
    void Update(){
        if (this.GetComponent<SpriteRenderer>().enabled != false){ // Si el Webo es visible
            if (Input.GetKeyDown("space") && SaltoDoble){ // Salto doble
                Sonido.PlayOneShot(Salto, 1f);
                SaltoDoble = false;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, PotSalto / 4));
            }
            if (Input.GetKeyDown("space") && EnPiso){ // Salto simple
                Sonido.PlayOneShot(Salto, 1f);
                EnPiso = false;
                SaltoDoble = true; // Para permitir hacer el doble salto
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, PotSalto));
            }
            if (Input.GetKeyDown("down") && !EnPiso){ // Bajar rápido
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -PotSalto));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D C1){
        EnPiso = true;
        SaltoDoble = false;
        if (C1.collider.gameObject.CompareTag("Obstacle") && (this.GetComponent<SpriteRenderer>().enabled != false)){ // En caso de chocar un obstáculo
            this.GetComponent<JugadorScript>().Webito.transform.position = new Vector3( 
                this.GetComponent<JugadorScript>().Webito.transform.position.x,
                this.GetComponent<JugadorScript>().Webito.transform.position.y,
                -8); // Vuelvo a poner en posición la animación
            Explosion.Play();
            Webito.Play();
            this.GetComponent<SpriteRenderer>().enabled = false; // Desactivo la visibilidad del Webo para no destruirlo
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnTriggerStay2D(Collider2D collision){ // Se encuentra en el piso
        EnPiso = true;
        SaltoDoble = false;
    }

}
