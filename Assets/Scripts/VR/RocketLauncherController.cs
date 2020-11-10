using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusModules.Grab;

public class RocketLauncherController : MonoBehaviour
{
    public Transform LaunchPoint;
    public GameObject TNTPrefab;
    public AudioClip LaunchSound;
    public float LaunchForce = 1.0f;
    public ParticleSystem LaunchParticleSystem;

    private Grabbable Grabbable;
    private AudioSource AudioSource;

    private void Awake()
    {
        Grabbable = GetComponent<Grabbable>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Grabbable.IsGrabbed && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Rigidbody rigidbody = Instantiate(TNTPrefab, LaunchPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 direction = LaunchPoint.transform.position - transform.position;
            direction.Normalize();
            rigidbody.AddForce(direction * LaunchForce, ForceMode.Impulse);
            // Sound
            AudioSource.PlayOneShot(LaunchSound, 2.0f);
            // Particle System
            LaunchParticleSystem.Play();
        }
    }
}
