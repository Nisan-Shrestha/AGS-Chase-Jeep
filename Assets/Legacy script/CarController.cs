using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private Rigidbody playerRB;
    public WheelColliders wheelColliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;
    public float gasInput;
    public float brakeInput;
    public float steeringInput;
    public GameObject smokePrefab;
    public float motorPower;
    public float brakePower;
    public float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;
    public Text forwardSlipText;
    public Text sideSlipText;
    /*public Mybutton gasPedal;
    public Mybutton brakePedal;
    public Mybutton leftButton;
    public Mybutton rightButton;*/

    // Start is called before the first frame update
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerRB.centerOfMass += new Vector3(0, -.3f, 0);
        InstantiateSmoke();
    }

    void InstantiateSmoke()
    {
        wheelParticles.FRWheel = Instantiate(smokePrefab, wheelColliders.FRWheel.transform.position - Vector3.up * wheelColliders.FRWheel.radius, Quaternion.identity, wheelColliders.FRWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.FLWheel = Instantiate(smokePrefab, wheelColliders.FLWheel.transform.position - Vector3.up * wheelColliders.FRWheel.radius, Quaternion.identity, wheelColliders.FLWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.RRWheel = Instantiate(smokePrefab, wheelColliders.RRWheel.transform.position - Vector3.up * wheelColliders.FRWheel.radius, Quaternion.identity, wheelColliders.RRWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.RLWheel = Instantiate(smokePrefab, wheelColliders.RLWheel.transform.position - Vector3.up * wheelColliders.FRWheel.radius, Quaternion.identity, wheelColliders.RLWheel.transform)
            .GetComponent<ParticleSystem>();
    }
    // Update is called once per frame

    void FixedUpdate()
    {
        speed = playerRB.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        CheckParticles();
        ApplyWheelPositions();
        PrintSlip();
    }

    private void PrintSlip()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        wheelColliders.FRWheel.GetGroundHit(out wheelHits[0]);
        wheelColliders.FLWheel.GetGroundHit(out wheelHits[1]);

        wheelColliders.RRWheel.GetGroundHit(out wheelHits[2]);
        wheelColliders.RLWheel.GetGroundHit(out wheelHits[3]);
        string forwardslip = "forward slip: ";
        for (int i = 0; i < 4; i++)
        {
            forwardslip += Mathf.Abs(wheelHits[0].forwardSlip).ToString("0.00");
            forwardslip += " ";
        }                 
        string sideslip = "side    slip: ";
        for (int i = 0; i < 4; i++)
        {
            sideslip += Mathf.Abs(wheelHits[0].sidewaysSlip).ToString("0.00");
            sideslip += " ";
        }
        string normal= "";
        for (int i = 0; i < 4; i++)
        {
            normal += Mathf.Abs(wheelHits[0].force).ToString();
            normal += " ";
        }
        forwardSlipText.text= forwardslip;
        sideSlipText.text= sideslip;
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        /*if (gasPedal.isPressed)
        {
            gasInput += gasPedal.dampenPress;
        }
        if (brakePedal.isPressed)
        {
            gasInput -= brakePedal.dampenPress;
        }
        */
        steeringInput = Input.GetAxis("Horizontal");
        /*if (rightButton.isPressed)
        {
            steeringInput += rightButton.dampenPress;
        }
        if (leftButton.isPressed)
        {
            steeringInput -= leftButton.dampenPress;
        }*/
        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

        //fixed code to brake even after going on reverse by Andrew Alex 
        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);
        if (movingDirection < -0.5f && gasInput > 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else
        {
            brakeInput = 0;
        }
        
        //old tutorial code
        //if (slipAngle < 120f) {
        //    if (gasInput < 0)
        //    {
        //        brakeInput = Mathf.Abs( gasInput);
        //        gasInput = 0;
        //    }
        //    else
        //    {
        //        brakeInput = 0;
        //    }
        //}
        //else
        //{
        //    brakeInput = 0;
        //}

    }
    void ApplyBrake()
    {
        wheelColliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        wheelColliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

        wheelColliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        wheelColliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;

        if (Input.GetKey("space"))
        {
            wheelColliders.RRWheel.brakeTorque = brakePower ;
            wheelColliders.RLWheel.brakeTorque =  brakePower;

        }
    }
    void ApplyMotor()
    {

        wheelColliders.RRWheel.motorTorque = motorPower * gasInput ;
        wheelColliders.RLWheel.motorTorque = motorPower * gasInput ;

    }
    void ApplySteering()
    {

        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        //if (slipAngle < 120f)
        //{
        //    steeringAngle += Vector3.SignedAngle(transform.forward, playerRB.velocity + transform.forward, Vector3.up) * Time.deltaTime;
        //}
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        Debug.Log(speed);
        wheelColliders.FRWheel.steerAngle = steeringAngle;
        wheelColliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPositions()
    {   
        UpdateWheel(wheelColliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(wheelColliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(wheelColliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(wheelColliders.RLWheel, wheelMeshes.RLWheel);
    }
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        wheelColliders.FRWheel.GetGroundHit(out wheelHits[0]);
        wheelColliders.FLWheel.GetGroundHit(out wheelHits[1]);

        wheelColliders.RRWheel.GetGroundHit(out wheelHits[2]);
        wheelColliders.RLWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.8f;
        if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > slipAllowance))
        {
            wheelParticles.FRWheel.Play();
        }
        else
        {
            wheelParticles.FRWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > slipAllowance))
        {
            wheelParticles.FLWheel.Play();
        }
        else
        {
            wheelParticles.FLWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > slipAllowance))
        {
            wheelParticles.RRWheel.Play();
        }
        else
        {
            wheelParticles.RRWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > slipAllowance))
        {
            wheelParticles.RLWheel.Play();
        }
        else
        {
            wheelParticles.RLWheel.Stop();
        }


    }

}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}
[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;

}