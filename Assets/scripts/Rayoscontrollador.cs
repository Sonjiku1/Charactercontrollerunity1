
using UnityEngine;
using UnityEngine.InputSystem;

public class Rayoscontrollador : MonoBehaviour
{
    PlayerInput input;

    [Header("Referencias")]
    [SerializeField] private Transform pivotecamara;
    [SerializeField] private Transform puntoDisparo;

    [Header("Interaccion")]
    [SerializeField] private float alcanceinteraccion = 3f;

    [Header("Proyectiles")]
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private float shootforce = 30f;
    [SerializeField] private float alcancedisparo = 100f;

    InputAction shoot;
    InputAction interactuar;

    void Awake()
    {
        input = GetComponent<PlayerInput>();

        shoot = input.actions.FindAction("Shoot");
        interactuar = input.actions.FindAction("Interact");
    }

    void OnEnable()
    {
        if (shoot != null)
            shoot.performed += Disparar;

        if (interactuar != null)
            interactuar.performed += MatarPaco;
    }

    void OnDisable()
    {
        if (shoot != null)
            shoot.performed -= Disparar;

        if (interactuar != null)
            interactuar.performed -= MatarPaco;
    }

    void Disparar(InputAction.CallbackContext context)
    {
        if (bulletprefab == null || puntoDisparo == null || pivotecamara == null)
            return;

        Vector3 direccion = pivotecamara.forward;

        // Raycast para calcular donde estamos apuntando
        if (Physics.Raycast(
            pivotecamara.position,
            pivotecamara.forward,
            out RaycastHit hit,
            alcancedisparo))
        {
            direccion = (hit.point - puntoDisparo.position).normalized;
        }

        // Crear proyectil en la pistola
        GameObject bala = Instantiate(
            bulletprefab,
            puntoDisparo.position,
            Quaternion.LookRotation(direccion)
        );

        // Lanzar proyectil fisicamente
        Rigidbody rb = bala.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(direccion * shootforce, ForceMode.Impulse);
        }
    }

    void MatarPaco(InputAction.CallbackContext context)
    {
        if (pivotecamara == null)
            return;

        if (Physics.Raycast(
            pivotecamara.position,
            pivotecamara.forward,
            out RaycastHit hit,
            alcanceinteraccion))
        {
            if (hit.collider.CompareTag("paco"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
