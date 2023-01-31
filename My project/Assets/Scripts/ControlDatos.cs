using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDatos : MonoBehaviour
{

    public Reloj refReloj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        refReloj = FindObjectOfType<Reloj>();
        Debug.Log(refReloj.tiempoEnSegundos);
    }
    
        
    
}
