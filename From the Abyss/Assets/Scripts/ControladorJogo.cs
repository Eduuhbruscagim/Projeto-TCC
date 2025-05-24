using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ControladorJogo : MonoBehaviour
{
    public static ControladorJogo Controlador;
    public TextMeshProUGUI TextoMoeda;

    public int Moedas;

    void Awake()
    {
        if (Controlador == null)
        {
            Controlador = this;
        }

        else if (Controlador != this)
        {
            Destroy(gameObject);
        }
    }

    
    void Update()
    {
        
    }
}
