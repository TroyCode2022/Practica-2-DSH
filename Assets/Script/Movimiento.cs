using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public Camera camara;
    public int velocidad;
    public GameObject prefabSuelo;

    private Vector3 offset;
    private float ValX;
    private float ValZ;
    private Rigidbody rb;
    private Vector3 direccionActual;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Si la direccion actual es igual a eso significa que estoy avanzando
            if (direccionActual == Vector3.forward)
                direccionActual = Vector3.right;
            else
                direccionActual = Vector3.forward;
        }
        //delta time se corresponde con los frames
        float tiempo = velocidad * Time.deltaTime;
        rb.transform.Translate(direccionActual * tiempo);
    }

    void SueloInicial()
    {
        //Durante 3 veces añadimos un suelo mas 
        for (int n = 0; n < 3; n++)
        {
            ValZ += 6;
            //Instantiate es crear algo
            //Cuaternion es como estaba girado el objeto originalmente
            GameObject elsuelo = Instantiate(prefabSuelo, new Vector3(ValX, 0.0f, ValZ), Quaternion.identity) as GameObject;
        }
    }

    //Exixt porque hará eso una vez que salga del objeto, que deje de colisionar
    void OnCollisionExit (Collision other)
    {
        if (other.gameObject.CompareTag("suelo"))
        {
            StartCoroutine(CrearSuelo(other));
        }
    }

    //El collider será el suelo en este caso
    IEnumerator CrearSuelo(Collision other)
    {
        Debug.Log("crea suelo");
        //Hacemos que caiga
        yield return new WaitForSeconds(0.2f);
        other.rigidbody.isKinematic = false;
        other.rigidbody.useGravity = true;
        yield return new WaitForSeconds(1.5f);
        Destroy(other.gameObject);//Destruimos el objeto para que no caiga infinitamente.

        //Lo movemos a la derecha o adelante aleatoriamente
        float ran = Random.Range(0f, 1f);
        if (ran < 0.5f)
            ValX += 6.0f;
        else
            ValZ += 6.0f;

        //Volvemos a crear el objeto segun los nuevos valores de posicion
        GameObject elsuelo = Instantiate(prefabSuelo, new Vector3(ValX, 0.0f, ValZ), Quaternion.identity) as GameObject;

    }
}
    

