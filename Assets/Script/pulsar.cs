using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class pulsar : MonoBehaviour
{
    public Button btn;
    public Text texto;
    public Image img;
    public Sprite[] spNumero;
    private bool contar;
    private int numero;
    // Start is called before the first frame update
    void Start()
    {
        //btn = gameObject.findAny
        //La siguiente linea es para no tener que arrastrar el objeto en la interfaz
        btn = GameObject.FindWithTag("btnstart").GetComponent<Button>();
        //Llama a la funcion una vez que se pulse el boton
        btn.onClick.AddListener(Pulsado);
        contar = false;
        numero = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (contar)
        {

            switch (numero)
            {
                case 0:
                    SceneManager.LoadScene("2", LoadSceneMode.Single);
                    break;
                case 1:
                    img.sprite = spNumero[0];
                    texto.text = "1";
                    break;
                case 2:
                    img.sprite = spNumero[1];
                    texto.text = "2"; 
                    break;
                    
                case 3:
                    img.sprite = spNumero[2];
                    texto.text = "3"; 
                    break;
            }
            //Lanzo la corutina y no vuelve a entrar en el bucle
            StartCoroutine(esperar());
            contar = false;
            numero--;

        }
        
    }

    void Pulsado()
    {
        Debug.Log("Pulsacion");
        //Una vez pulsamos el boton activamos la imagen para que se vea
        //activamos la imagen que hay en el sprite, es decir, la del vector
        img.gameObject.SetActive(true);

        //Desactivamos el bot�n para que no se vea
        btn.gameObject.SetActive(false);
        contar = true;
    }

    IEnumerator esperar()
    {
        yield return new WaitForSeconds(1);
        contar = true;
    }
}
