using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletHole;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessShot();
    }

    private void ProcessShot()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2( Screen.width/2, Screen.height/2));
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit, float.PositiveInfinity)) { 
                Instantiate(BulletHole,Hit.point + (Hit.normal*.01f),Quaternion.FromToRotation(Vector3.up, Hit.normal));
            }
            
        }
    }
}

