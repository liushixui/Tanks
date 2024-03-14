using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTankShooting : MonoBehaviour
{
    public int PlayerNumber = 1;
    public GameObject FireTransForm;
    public GameObject Shell;
    public Slider AimSlider;
    public AudioSource ShootingAudio;
    public AudioClip FireClip;
    public AudioClip ChargingClip;
    public float MinLaunchForce = 15f;
    public float MaxLaunchForce = 30f;
    public float ChargeTime = 0.75f;
    private string FireButton;
    private float CurrentLaunchForce;
    private float ChargeSpeed;
    private bool Fired;
    // Start is called before the first frame update
    void Start()
    {
        FireButton = "Fire" + PlayerNumber;
        ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / ChargeTime;
        AimSlider.value = MinLaunchForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(FireButton))
        {
            CurrentLaunchForce = MinLaunchForce;
            ShootingAudio.clip = ChargingClip;
            ShootingAudio.Play();
            Fired = false;
        }
        else if (Input.GetButton(FireButton) && !Fired)
        {
            CurrentLaunchForce += ChargeSpeed * Time.deltaTime;

            AimSlider.value = CurrentLaunchForce;
         
            if (CurrentLaunchForce >= MaxLaunchForce )
            {
                CurrentLaunchForce = MaxLaunchForce;

                Fire();
            }

        }
        else if (Input.GetButtonUp(FireButton) && !Fired) 
        {
            Fire();
        }
    }

    void Fire() 
    {
        AimSlider.value = MinLaunchForce;
        Fired = true;
        GameObject gameObjectInstance = Instantiate(Shell,FireTransForm.transform.position,FireTransForm.transform.rotation);
        Rigidbody rigidbody = gameObjectInstance.GetComponent<Rigidbody>();
        rigidbody.velocity = FireTransForm.transform.forward*CurrentLaunchForce;

        CurrentLaunchForce = MinLaunchForce;
        ShootingAudio.clip = FireClip;
        ShootingAudio.Play();
    }
}
