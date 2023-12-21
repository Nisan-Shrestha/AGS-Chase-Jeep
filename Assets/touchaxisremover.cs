using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchaxisremover : MonoBehaviour
{
    public CinemachineFreeLook Cinemachine; // set in the editor
    public BoxCollider2D bx;
    bool badpos = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        badpos= false;
        if (Input.touchCount >= 1)
        {
            
            foreach (var touch in Input.touches)
            {
               
                if(touch.position.y< Screen.height/2)
                //Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
                //if (bx.OverlapPoint(wp))
                {
                    badpos = true;
                    Cinemachine.m_XAxis.m_InputAxisName = " ";
                    Cinemachine.m_YAxis.m_InputAxisName = " ";
                    break;
                }

            }
            
        }
        if (!badpos)
        {
            Cinemachine.m_XAxis.m_InputAxisName = "Mouse X";
            Cinemachine.m_YAxis.m_InputAxisName = "Mouse Y";

        }
    }
}
