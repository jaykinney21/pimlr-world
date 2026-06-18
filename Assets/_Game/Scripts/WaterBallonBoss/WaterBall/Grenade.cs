using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0, 1, 0);
    [SerializeField] private GameObject audioSourcePrefab;

    [Header("Explosion Setting")]
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionRadius = 5f;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip impactSound;



    private float countdown;
    private bool hasExploded = false;
    private AudioSource audioSource;

    private void Start()
    {
        countdown = explosionDelay;
    }

    private void Update()
    {
        if (!hasExploded)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f  && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    void Explode()
    {
        hasExploded = true;
        if (!GameExecutionManager.Instance.zone2Finish)
        {

            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffset, Quaternion.identity);

            Destroy(explosionEffect, 4f);

            //PlaySoundAtPosition(explosionSound);

            NearbyForceApply();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void PlaySoundAtPosition(AudioClip clip)
    {
        GameObject audioSourceObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource instantiatedAudioSource = audioSourceObject.GetComponent<AudioSource>();
        instantiatedAudioSource.clip = clip;
        instantiatedAudioSource.spatialBlend = 1;
        instantiatedAudioSource.Play();

        Destroy(audioSourcePrefab, instantiatedAudioSource.clip.length);
    }

    void NearbyForceApply()
    {
        Collider[] colliiders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliiders)
        {
            if (nearbyObject.tag == "Player")
            {

                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

            }

        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        audioSource.clip = impactSound;

        audioSource.spatialBlend= 1;

        audioSource.Play();
    }*/
}
