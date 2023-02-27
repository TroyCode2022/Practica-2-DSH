using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Movimiento : MonoBehaviour
{
    public Camera camara;
    public int velocidad;
    public GameObject prefabSuelo;
    public GameObject prefabBarrera;
    public Text texto;

    private Vector3 offset;
    private float ValX;
    private float ValZ;
    private Rigidbody rb;
    private Vector3 direccionActual;
    private int cont = 0;
    private float maximosSuelo = 6;
    private float puntos = 0;

    private List<GameObject> barreras = new List<GameObject>();
    private Queue<GameObject> suelos = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        offset = camara.transform.position;
        ValX = 0;
        ValZ = 0;
        //Necesitamos pillar el rigidbody para añadirle el movimiento, fuercas, velocidad...
        rb = GetComponent<Rigidbody>();

        SueloInicial();
    }

    // Update is called once per frame
    void Update()
    {
        camara.transform.position = this.transform.position + offset;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            //Si la direccion actual es igual a eso significa que estoy avanzando
            direccionActual = Vector3.left;
           
        }else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            direccionActual = Vector3.right;
        }else if(Input.GetKeyDown(KeyCode.Space))
            direccionActual = Vector3.forward;

        //delta time se corresponde con los frames
        float tiempo = velocidad * Time.deltaTime;
        rb.transform.Translate(direccionActual * tiempo);

        if(this.transform.position.y < -20)
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

    }

    void SueloInicial()
    {
        GameObject primersuelo = Instantiate(prefabSuelo, new Vector3(0, 0.0f, 0), Quaternion.identity) as GameObject;
        suelos.Enqueue(primersuelo);
        //Durante 3 veces añadimos un suelo mas 
        for (int n = 0; n < 3; n++)
        {
            ValZ += 6;
            //Instantiate es crear algo
            //Cuaternion es como estaba girado el objeto originalmente
            GameObject elsuelo = Instantiate(prefabSuelo, new Vector3(ValX, 0.0f, ValZ), Quaternion.identity) as GameObject;
            suelos.Enqueue(elsuelo);
        }
    }

    //Exixt porque hará eso una vez que salga del objeto, que deje de colisionar
    IEnumerator OnCollisionExit (Collision other)
    {
        if (other.gameObject.CompareTag("suelo"))
        {
            Coroutine currentCoroutine = StartCoroutine(CrearSuelo(other));
            yield return currentCoroutine;
        }
    }

    //El collider será el suelo en este caso
    IEnumerator CrearSuelo(Collision other)
    {
        float desp;
        cont++;
        
        

        if (suelos.Count >= maximosSuelo)
        {
            //Hacemos que caiga la plataforma
            //iskinematic hace que solo se mueva cuando tu le dices, no responde a otros objetos del entorno
            //suelos[0].GetComponent<Rigidbody>().isKinematic = false;
            //suelos[0].GetComponent<Rigidbody>().useGravity = true;

            //yield return new WaitForSeconds(0.5f);
            
            Debug.Log("Destruye" + cont);
            Destroy(suelos.Dequeue(), 0.2f);//Destruimos el objeto para que no caiga infinitamente.
            //suelos.RemoveAt(0);
        }

        yield return new WaitForSeconds(0.5f);
        //Lo movemos a la derecha o adelante aleatoriamente
        float ran = Random.Range(0f, 1f);
        if (ran < 0.5f)
            ValX += 6.0f;
        else
            ValZ += 6.0f;

        //Volvemos a crear el objeto segun los nuevos valores de posicion
        GameObject elsuelo = Instantiate(prefabSuelo, new Vector3(ValX, 0.0f, ValZ), Quaternion.identity) as GameObject;
        
        //Añadimos el suelo a la lista para poder eliminarlo para descargar la memoria
        suelos.Enqueue(elsuelo);
        
        //Creamos una barrera aleatoriamente en la nueva plataforma para ponerlo más dificil
        if (Random.Range(0f, 1f) > 0.5)
        {
            
            float ranBarrera = Random.Range(0f, 1f);
            if (ranBarrera < 0.5f)
                desp = 1.74f;
            else
                desp = -1.74f;

            //Generamos una barrera y destruimos la más antigua, permitimos 5 a la vez
            GameObject labarrera = Instantiate(prefabBarrera, new Vector3(ValX+desp, 0.86f, ValZ), Quaternion.identity) as GameObject;
            barreras.Add(labarrera);
            if (barreras.Count >= 6)
            {
                Destroy(barreras[0]);
                barreras.RemoveAt(0);
            }
        }

        if (cont == 7)
        {
            Debug.Log(maximosSuelo);
            velocidad += 1;
            maximosSuelo += 0.5f;
            cont = 0;
        }

        
        ActualizarPuntos();
    }

    void ActualizarPuntos()
    {
        Debug.Log(puntos);
        puntos += velocidad * 12;
        texto.text = "Puntuación: " + puntos;
    }
}
    

