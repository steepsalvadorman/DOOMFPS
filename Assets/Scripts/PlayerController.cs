using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float shootDistance = 4f;
    [SerializeField]
    private ParticleSystem[] shootPS;
    [SerializeField]
    public float health;

    [Header("Armas")]
    [SerializeField] GameObject[] weapons; // Para las armas
    private int currentWeaponIndex = 0; // �ndice del arma actual


    private Rigidbody mRb;
    private Vector2 mDirection;
    private Vector2 mDeltaLook;
    private Transform cameraMain;
    private GameObject debugImpactSphere;
    private GameObject bloodObjectParticles;
    private GameObject otherObjectParticles;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera");

        debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
        bloodObjectParticles = Resources.Load<GameObject>("BloodSplat_FX Variant");
        otherObjectParticles = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        mRb.velocity = mDirection.y * speed * transform.forward 
            + mDirection.x * speed * transform.right;

        transform.Rotate(
            Vector3.up,
            turnSpeed * Time.deltaTime * mDeltaLook.x
        );
        cameraMain.GetComponent<CameraMovement>().RotateUpDown(
            -turnSpeed * Time.deltaTime * mDeltaLook.y
        );

    }

    private void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        mDeltaLook = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            Shoot();
        }
    }

    private void Update()
    {
        SwitchWeapon();
    }
    private void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Incrementa el �ndice del arma actual
            currentWeaponIndex++;

            // Si se excede el �ndice m�ximo, regresa al primer arma
            if (currentWeaponIndex >= weapons.Length)
            {
                currentWeaponIndex = 0;
            }

            // Cambia el arma activa
            Switch();
        }
    }

    private void Switch()
    {
        // Desactiva todas las armas
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        // Activa el arma seleccionada
        weapons[currentWeaponIndex].SetActive(true);
    }

    private void Shoot()
    {

        shootPS[currentWeaponIndex].Play();

        RaycastHit hit;
        if (Physics.Raycast(
            cameraMain.position,
            cameraMain.forward,
            out hit,
            shootDistance
        ))
        {
            if (hit.collider.CompareTag("Enemigos"))
            {
                var bloodPS = Instantiate(bloodObjectParticles, hit.point, Quaternion.identity);
                Destroy(bloodPS, 3f);
                var enemyController = hit.collider.GetComponent<EnemyController>();
                enemyController.TakeDamage(1f);
            }else
            {
                var otherPS = Instantiate(otherObjectParticles, hit.point, Quaternion.identity);
                otherPS.GetComponent<ParticleSystem>().Play();
                Destroy(otherPS, 3f);
            }
            
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            // Fin del juego
            Debug.Log("Fin del juego");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemigo-Attack"))
        {
            Debug.Log("Player recibio danho");
            TakeDamage(1f);
        }
        
    }

}
