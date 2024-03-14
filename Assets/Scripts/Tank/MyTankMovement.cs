using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MyTankMovement : MonoBehaviour
{
    public int PlayerNumber = 1;
    public float Speed = 12f;
    public float TurnSpeed = 180;
    public AudioSource MovementAudio;
    public AudioClip EngineIdling;
    public AudioClip EngineDriving;
    public AudioClip EmptySound;
    public float PitchRange = 0.2f;
    public float AddSpeed = 2f;

    private Rigidbody rb;
    private string MovementAxisName;
    private string TurnAxisName;
    private float MovementInputValue;
    private float TurnInputValue;
    private float OriginalPitch;
    private string AddButton;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        AddButton = "Acceleration" + PlayerNumber;
        MovementAxisName = "Vertical"+ PlayerNumber;
        TurnAxisName = "Horizontal"+ PlayerNumber;

        OriginalPitch = MovementAudio.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        MovementInputValue = Input.GetAxis(MovementAxisName);
        TurnInputValue = Input.GetAxis(TurnAxisName);

       
    }
    private void FixedUpdate()
    {

        Move();
        Turn();
        AddSeed();
        EngineAudio();


    }
    void Move() 
    {
        Vector3 movementV3 = transform.forward * MovementInputValue * Speed * Time.deltaTime;
        rb.MovePosition(rb.position + movementV3);

    }
    void Turn()
    {
        float turn =TurnInputValue * TurnSpeed * Time.deltaTime;
        Quaternion quaternion = Quaternion.Euler(0, turn, 0);
        rb.MoveRotation(rb.rotation * quaternion);
    }

    void AddSeed() 
    {
        if (Input.GetButton(AddButton))
        {
            Vector3 movementV3 = transform.forward * MovementInputValue * (Speed/AddSpeed) * Time.deltaTime;
            rb.MovePosition(rb.position + movementV3);

        }

    }

    void EngineAudio()
    {

        if (Mathf.Abs(MovementInputValue) < 0.1f && Mathf.Abs(TurnInputValue) < 0.1f)
        {
            if (MovementAudio.clip == EngineDriving | MovementAudio.clip == EngineIdling)
            {
                MovementAudio.clip = EmptySound;
                MovementAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            }

        }

        else if (Input.GetButton(AddButton) && Mathf.Abs(MovementInputValue) > 0.1f)
        {
            if (MovementAudio.clip == EngineIdling)
            {
                MovementAudio.clip = EngineDriving;
                MovementAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();

            }
        }

        else
        {
            if (MovementAudio.clip == EngineDriving | MovementAudio.clip == EmptySound)
            {
                MovementAudio.clip = EngineIdling;
                MovementAudio.Play();
            }
        }

    }
}
