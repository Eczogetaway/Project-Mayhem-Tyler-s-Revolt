using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shoot : MonoBehaviour
{
    public float damage = 21;
    public float fireRate = 5;
    public float range = 15;
    public float force = 155;
    public GameObject hitEffect;
    public ParticleSystem muzzleFlashe;
    public AudioClip shotSFX;
    public AudioSource _audioSource;
    public Transform bulletSpawn;
    public Camera _cam;

    public int maxAmmo = 7;
    private int currentAmmo;
    public int totalAmmo = 120;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Text ammoText;
    public Slider reloadSlider; // Полоска перезарядки

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        ammoText.text = currentAmmo + " / " + totalAmmo;

        if (isReloading)
            return;

        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        if (totalAmmo <= 0) // Если у игрока нет патронов, то выходим из функции перезарядки
        {
            Debug.Log("No ammo left");
            yield break;
        }

        isReloading = true;
        Debug.Log("Reloading...");

        reloadSlider.gameObject.SetActive(true); // Показываем полоску перезарядки
        float reloadProgress = 0;
        while (reloadProgress < reloadTime)
        {
            reloadProgress += Time.deltaTime;
            reloadSlider.value = reloadProgress / reloadTime;
            yield return null;
        }

        int ammoToReload = Mathf.Min(maxAmmo - currentAmmo, totalAmmo);
        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        reloadSlider.gameObject.SetActive(false); // Скрываем полоску перезарядки
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;

        _audioSource.PlayOneShot(shotSFX);
        muzzleFlashe.Play();

        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, range))
        {
            Pedestrian pedestrian = hit.transform.GetComponent<Pedestrian>();
            GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);

            if (pedestrian != null) // Если у объекта есть компонент Pedestrian
            {
                pedestrian.TakeDamage(damage); // Наносим урон цели
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
            }
        }
    }
}
