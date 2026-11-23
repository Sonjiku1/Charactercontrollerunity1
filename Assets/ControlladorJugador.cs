using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controlladorjugador : MonoBehaviour
{



    [Header("Referencias")]
    [SerializeField] private Transform pivoteCamara;

    [SerializeField] private Camera camara;

    [SerializeField] private GameObject modelo ;

    [Header("Movimiento")]
    [SerializeField]float velocidad = 5f;

    private float velocidadvertical = 0f;
    private float alturaSalto = 1.5f;

    private float gravedad = -20f;

    private CharacterController controlador;

    private PlayerInput Input;

    private InputAction mover;

    private InputAction saltar;

    private InputAction mirar;

    private InputAction apuntar;

    private InputAction cambiarvista;
    private float inclinacion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlador = GetComponent<CharacterController>();
        Input = GetComponent<PlayerInput>();

        mover = Input.actions["Move"];
        saltar = Input.actions["Jump"];
        mirar = Input.actions["Look"];
        apuntar = Input.actions["Point"];
        cambiarvista = Input.actions["Change View"];

        //bloquear el cursor y lo hacemos invisible 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        GirarCamara();
        MoverPersonaje();
        ActualizarCamara();
    }

    void GirarCamara()
    {
        //1. Obtener el delta del ratón

        Vector2 delta = mirar.ReadValue<Vector2>();
        
        transform.Rotate(Vector3.up * delta.x );

        inclinacion -= delta.y;

        inclinacion = Mathf.Clamp(inclinacion, -90f, 90f);

        pivoteCamara.localRotation = Quaternion.Euler(inclinacion, 0f, 0f);
    }


    void MoverPersonaje()
    {
        //1. leer el input del jugador
        Vector2 entrada  = mover.ReadValue<Vector2>();

        //2. Calcular la dirección de movimiento en el mundo
        Vector3 direccion= transform.right * entrada.x + transform.forward * entrada.y;

        direccion= Vector3.ClampMagnitude(direccion, 1f);

        //  Gestionar la velocidad e implementar el salto 

        if (controlador.isGrounded && saltar.WasPressedThisFrame())
        {
            velocidadvertical = Mathf.Sqrt(alturaSalto * -2f * gravedad);

        }
       velocidad += gravedad * Time.deltaTime;

        //3. calcular vector desplazamiento final para utilizar con el Move
        Vector3 desplazamiento = direccion * velocidad;

        desplazamiento.y = velocidadvertical;

        //4. mover al personaje

        controlador.Move(desplazamiento * Time.deltaTime);
        
    }

    void ActualizarCamara()
    {
        //Todo
     }
}
