using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] int maxSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject damageBoxPrefab;

    Rigidbody rb;
    Vector3 input;
    Camera cam;
    bool stayground;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main; // Pobranie g��wnej kamery
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Zablokowanie rotacji
    }
    void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Skok
        if (Input.GetButtonDown("Jump") && stayground) // "Jump" jest domy�lnie przypisane do klawisza Space
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        }
        // Sprawdzanie naci�ni�cia lewego przycisku myszy
        if (Input.GetMouseButtonDown(0)) // 0 oznacza lewy przycisk myszy
        {
            OnLeftMouseButtonPressed(); // Wywo�anie metody
        }

        // Sprawdzanie naci�ni�cia prawego przycisku myszy
        if (Input.GetMouseButtonDown(1)) // 1 oznacza prawy przycisk myszy
        {
            OnRightMouseButtonPressed(); // Wywo�anie metody
        }

    }
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            // Ruch wzgl�dem kamery
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;

            // Ignorowanie osi Y kamery, aby ruch by� p�aski (2D na ziemi)
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalizacja wektor�w, aby zachowa� proporcje
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Obliczanie kierunku ruchu na podstawie wej�cia i kamery
            Vector3 moveDirection = (cameraForward * input.z) + (cameraRight * input.x * -1);

            // Nadawanie pr�dko�ci graczowi
            rb.AddForce(moveDirection * speed * Time.fixedDeltaTime, ForceMode.Acceleration);

            float currentSpeed = rb.velocity.magnitude;
            Debug.Log($"Aktualna pr�dko��: {currentSpeed}");
            // Sprawdzenie aktualnej pr�dko�ci
            if (rb.velocity.magnitude > maxSpeed)
            {                
                // Ograniczenie pr�dko�ci do maksymalnej
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            // Obracanie postaci w kierunku kamery
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        }
    }

    // Sprawdzanie, czy gracz jest na ziemi
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            stayground = true; // Gracz dotyka ziemi
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            stayground = false; // Gracz przestaje dotyka� ziemi
        }
    }


    // Metody zdarze�
    void OnLeftMouseButtonPressed()
    {
        Debug.Log("Naci�ni�to lewy przycisk myszy");
        // Logika dla lewego przycisku
        StartCoroutine(SpawnDamageBox());
    }

    private IEnumerator SpawnDamageBox()
    {
        // Stworzenie boxa obra�e� przed graczem
        Vector3 spawnPosition = transform.position + transform.forward; // Przed graczem
        GameObject damageBox = Instantiate(damageBoxPrefab, spawnPosition, Quaternion.identity);

        // Ustawienie collidera jako trigger
        BoxCollider boxCollider = damageBox.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }

        // Poczekaj p� sekundy
        yield return new WaitForSeconds(0.5f);

        // Zniszczenie boxa po p� sekundy
        Destroy(damageBox);
    }

    //dla pociskof
    public GameObject projectilePrefab;  // Prefab pocisku
    public float projectileSpeed = 20f;  // Pr�dko�� pocisku
    public Camera playerCamera;  // Kamera, z kt�rej ma by� rzutowana wi�zka
    public Transform firePoint;  // Punkt wystrza�u pocisku (np. r�ka lub bro�)


    void OnRightMouseButtonPressed()
    {
        Debug.Log("Naci�ni�to prawy przycisk myszy");

        // Rzutowanie ray'a z kamery gracza na pozycj� kursora
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Pozycja, gdzie ray trafi�
            Vector3 targetPosition = hit.point;
            Debug.Log($"Raycast trafi� w punkt: {targetPosition}");

            // Tworzenie pocisku w punkcie wystrza�u
            if (projectilePrefab != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Debug.Log("Pocisk zosta� stworzony");

                // Obliczanie kierunku od punktu wystrza�u do miejsca trafienia kursora
                Vector3 direction = (targetPosition - firePoint.position).normalized;
                Debug.Log($"Kierunek pocisku: {direction}");

                // Nadawanie pr�dko�ci pociskowi
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                    Debug.Log($"Nadano pr�dko�� pociskowi: {rb.velocity}");
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono Rigidbody na prefabrykacie pocisku!");
                }

                // Zniszczenie pocisku po pewnym czasie, aby nie pozostawa� w grze wiecznie
                Destroy(projectile, 3f);
            }
            else
            {
                Debug.LogWarning("Prefabrykat pocisku nie jest przypisany!");
            }
        }
        else
        {
            Debug.Log("Raycast nie trafi� w �aden obiekt.");
        }
    }

}