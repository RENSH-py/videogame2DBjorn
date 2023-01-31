using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //
using UnityEngine.UI; //
using System; //    

public class ValidarUsuario : MonoBehaviour
{
    public Text CajaNombre, CajaEdad; 

    string nombre;
    byte edad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Validar(){
        nombre = CajaNombre.text;
        try{
            edad = byte.Parse(CajaEdad.text);
            if(edad>=18){
                SceneManager.LoadScene("Nivel0"); 
            }
            else{
                Debug.Log($"Usted no tiene la edad mínima para jugar, su edad es {edad}");
            }
        }
        catch(Exception ex){
            Debug.Log($"Error en formato de edad {ex.Message}");
        }
        
        Debug.Log(nombre);
            
    }
}
