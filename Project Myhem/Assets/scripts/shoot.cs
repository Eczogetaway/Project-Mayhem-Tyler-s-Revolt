using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        
    }


    void Shoot()
    {
        _audioSource.PlayOneShot(shotSFX);
        muzzleFlashe.Play();

        //Instantiate(muzzleFlashe, bulletSpawn.position, bulletSpawn.rotation);

        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit,range))
        {
            

            GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);


            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
            }
        }
    }


}
