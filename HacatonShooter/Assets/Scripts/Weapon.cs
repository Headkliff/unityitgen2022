using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 21f;
    public float fireRate = 1f;
    public float force = 155f;
    public float range = 15f;
    public ParticleSystem muzzleFlash;
    public Transform bulletSpawn;
    public AudioClip shotSFX;
    public AudioSource _audioSource;
    public GameObject hitEffect;

    public Camera _cam;
    private float nextFire = 0f;


    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, _cam.transform.position + _cam.transform.right * 0.25f - _cam.transform.up * 0.25f + _cam.transform.forward, Time.deltaTime * 10);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _cam.transform.rotation, Time.deltaTime * 10);
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        _audioSource.PlayOneShot(shotSFX);
        muzzleFlash.transform.position = bulletSpawn.position;
        muzzleFlash.Emit(1);

        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, range, 3, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Вы попали в объект! " + hit.collider);

            GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
            }
        }
    }
}