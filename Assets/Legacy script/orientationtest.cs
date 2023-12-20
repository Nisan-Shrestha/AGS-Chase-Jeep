using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class orientationtest : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       
        
       
        ////Debug.Log(Input.compensateSensors);
        Debug.Log(Input.deviceOrientation);
        Debug.Log(Input.acceleration);
        //Debug.Log(Input.gyro.rotationRate);
    }

    

}

