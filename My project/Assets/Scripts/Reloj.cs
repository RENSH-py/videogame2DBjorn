using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reloj : MonoBehaviour
{
    
    public int tiempoInicial;
    //[Range(-10.0, 10.0f)]
    public float velocidadTiempo = 1;

    private Text myText;
    private float tiempoDelFrame = 0f;
    public float tiempoEnSegundos= 0f;
    private float tiempoPausa, escalaDeTiempoIni;

    public void Awake()
    {
        LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {   //Se deja la escala de tiempo en 1 (escala original)
        escalaDeTiempoIni = velocidadTiempo;

        myText = GetComponent<Text>();
        //tiempoEnSegundos = tiempoInicial;
        //Se inicializa variable para acumular el tiempo con el tiempo inicial
        
        Scene scene = SceneManager.GetActiveScene();
        
        if (scene.name == "Nivel0"){
            tiempoEnSegundos = tiempoInicial;
            ActualizarReloj(tiempoInicial);
            }
        else
        {
            ActualizarReloj(tiempoEnSegundos);
        }
            //se utiliza la función para actualizar el tiempo y se recibe el dato en float por decimales que ocupa
        
    }
    
    // Update is called once per frame
    void Update()
    {
    tiempoDelFrame = Time.deltaTime * velocidadTiempo;
    tiempoEnSegundos += tiempoDelFrame;
    ActualizarReloj(tiempoEnSegundos);
    }
    public void ActualizarReloj(float tiempoEnSegundos)
    {
        int minutos = 0;
        int segundos = 0;
        string textoDelReloj;

        //se comprueba si el tiempo no es negativo, se transforma en caso de ser así
        if (tiempoEnSegundos < 0) tiempoEnSegundos = 0;
        //calculos
        minutos = (int)tiempoEnSegundos / 60;
        segundos = (int)tiempoEnSegundos % 60;

        textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00");

        myText.text = textoDelReloj;
    }
    
    public void OnDestroy()
    {
        SaveData();
        
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat("timePrefs", tiempoEnSegundos); //se utiliza a clase PlayerPrefs para guardado
    }
    private void LoadData()
    {
        tiempoEnSegundos= PlayerPrefs.GetFloat("timePrefs");
    }
    
}
