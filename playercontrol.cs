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
        cam = Camera.main; // Pobranie g³ównej kamery
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Zablokowanie rotacji
    }
    void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Skok
        if (Input.GetButtonDown("Jump") && stayground) // "Jump" jest domyœlnie przypisane do klawisza Space
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        }
        // Sprawdzanie naciœniêcia lewego przycisku myszy
        if (Input.GetMouseButtonDown(0)) // 0 oznacza lewy przycisk myszy
        {
            OnLeftMouseButtonPressed(); // Wywo³anie metody
        }

        // Sprawdzanie naciœniêcia prawego przycisku myszy
        if (Input.GetMouseButtonDown(1)) // 1 oznacza prawy przycisk myszy
        {
            OnRightMouseButtonPressed(); // Wywo³anie metody
        }

    }
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            // Ruch wzglêdem kamery
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;

            // Ignorowanie osi Y kamery, aby ruch by³ p³aski (2D na ziemi)
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalizacja wektorów, aby zachowaæ proporcje
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Obliczanie kierunku ruchu na podstawie wejœcia i kamery
            Vector3 moveDirection = (cameraForward * input.z) + (cameraRight * input.x * -1);

            // Nadawanie prêdkoœci graczowi
            rb.AddForce(moveDirection * speed * Time.fixedDeltaTime, ForceMode.Acceleration);

            float currentSpeed = rb.velocity.magnitude;
            Debug.Log($"Aktualna prêdkoœæ: {currentSpeed}");
            // Sprawdzenie aktualnej prêdkoœci
            if (rb.velocity.magnitude > maxSpeed)
            {                
                // Ograniczenie prêdkoœci do maksymalnej
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
            stayground = false; // Gracz przestaje dotykaæ ziemi
        }
    }


    // Metody zdarzeñ
    void OnLeftMouseButtonPressed()
    {
        Debug.Log("Naciœniêto lewy przycisk myszy");
        // Logika dla lewego przycisku
        StartCoroutine(SpawnDamageBox());
    }

    private IEnumerator SpawnDamageBox()
    {
        // Stworzenie boxa obra¿eñ przed graczem
        Vector3 spawnPosition = transform.position + transform.forward; // Przed graczem
        GameObject damageBox = Instantiate(damageBoxPrefab, spawnPosition, Quaternion.identity);

        // Ustawienie collidera jako trigger
        BoxCollider boxCollider = damageBox.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }

        // Poczekaj pó³ sekundy
        yield return new WaitForSeconds(0.5f);

        // Zniszczenie boxa po pó³ sekundy
        Destroy(damageBox);
    }

    //dla pociskof
    public GameObject projectilePrefab;  // Prefab pocisku
    public float projectileSpeed = 20f;  // Prêdkoœæ pocisku
    public Camera playerCamera;  // Kamera, z której ma byæ rzutowana wi¹zka
    public Transform firePoint;  // Punkt wystrza³u pocisku (np. rêka lub broñ)


    void OnRightMouseButtonPressed()
    {
        Debug.Log("Naciœniêto prawy przycisk myszy");

        // Rzutowanie ray'a z kamery gracza na pozycjê kursora
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Pozycja, gdzie ray trafi³
            Vector3 targetPosition = hit.point;
            Debug.Log($"Raycast trafi³ w punkt: {targetPosition}");

            // Tworzenie pocisku w punkcie wystrza³u
            if (projectilePrefab != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Debug.Log("Pocisk zosta³ stworzony");

                // Obliczanie kierunku od punktu wystrza³u do miejsca trafienia kursora
                Vector3 direction = (targetPosition - firePoint.position).normalized;
                Debug.Log($"Kierunek pocisku: {direction}");

                // Nadawanie prêdkoœci pociskowi
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                    Debug.Log($"Nadano prêdkoœæ pociskowi: {rb.velocity}");
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono Rigidbody na prefabrykacie pocisku!");
                }

                // Zniszczenie pocisku po pewnym czasie, aby nie pozostawa³ w grze wiecznie
                Destroy(projectile, 3f);
            }
            else
            {
                Debug.LogWarning("Prefabrykat pocisku nie jest przypisany!");
            }
        }
        else
        {
            Debug.Log("Raycast nie trafi³ w ¿aden obiekt.");
        }
    }

}