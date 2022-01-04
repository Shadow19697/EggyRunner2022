using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameScript : MonoBehaviour
{
    public GameObject Player;

    public Text GameText;
    public Text Puntaje;
    public Text HighScore;
    public Text FechaHora;

    public AudioSource Music;
    public ParticleSystem Fondo;
    
    public bool Fin;
    public bool Comienzo;
    public float Contador;

    private string path;

    void CreateText(){
        path = Application.dataPath + "/Records.txt";
        if (!File.Exists(path)){
            File.WriteAllText(path, "----------------Records----------------\n");
        }
    }

    // Start is called before the first frame update
    void Start(){
        CreateText();
        Fin = false;
        Comienzo = true;
        Fondo.Play();
        Music.Pause();
        HighScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    // Update is called once per frame
    void Update() {
        if (Player.GetComponent<SpriteRenderer>().enabled != false){ // Webo visible
            if (Comienzo){ // "Interfaz" de inicio
                GameText.text = "Press Space to start\nPress Esc to exit";
                if (Input.GetKeyDown("space")){
                    Comienzo = false;
                    GameText.text = "";
                    Music.Play();
                }
                if (Input.GetKeyDown(KeyCode.Escape)){
                    Application.Quit();
                }
                //if (Input.GetKeyDown("q"))             //Lo dejo comentado para reiniciar el High Score cada vez que haga el build
                //{
                //    PlayerPrefs.DeleteAll();
                //}
            } else{ // Comienza el juego
                Contador += Time.deltaTime * 8;
                Puntaje.text = "Score: " + (int)Contador;
                if ((int)Contador > PlayerPrefs.GetInt("HighScore",0)){ // Si supero el Highscore guardado
                    PlayerPrefs.SetInt("HighScore", (int)Contador);
                    HighScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
                }
            }
        } else{ // Webo no visible -> Chocó con un obstaculo
            if (!Fin){ // Solo entra una sola vez aqui para mostrar el mensaje y guardar el puntaje
                Music.Stop();
                Fin = true;
                GameText.text = "Game Over\nPress R to restart\nPress Escape to exit";
                Fondo.Stop();
                File.AppendAllText(path, "High Score: " + (int)Contador + "\t- " + FechaHora.text + "\n");
            }
            if (Fin){ // Espera la opción del jugador
                if (Input.GetKeyDown("r")){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (Input.GetKeyDown(KeyCode.Escape)){
                    Application.Quit();
                }
            }
        }
    }
}
