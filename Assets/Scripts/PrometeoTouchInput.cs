using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrometeoTouchInput : MonoBehaviour
{
    public CinemachineFreeLook Cinemachine ; // set in the editor

    public bool changeScaleOnPressed = false;
    [HideInInspector]
    public bool buttonPressed = false;
    RectTransform rectTransform;
    Vector3 initialScale;
    float scaleDownMultiplier = 0.85f;

    private void Awake()
    {

        Cinemachine = FindObjectOfType<CinemachineFreeLook>();
    }

    void Start(){
        rectTransform = GetComponent<RectTransform>();
      initialScale = rectTransform.localScale;
    }

    public void ButtonDown(){
        Cinemachine.m_XAxis.m_InputAxisName = "";
        Cinemachine.m_YAxis.m_InputAxisName = "";
        buttonPressed = true;
      if(changeScaleOnPressed){
        rectTransform.localScale = initialScale * scaleDownMultiplier;
      }
    }

    public void ButtonUp(){
      buttonPressed = false;
      if(changeScaleOnPressed){
        rectTransform.localScale = initialScale;
      }

        Cinemachine.m_XAxis.m_InputAxisName = "Mouse X";
        Cinemachine.m_YAxis.m_InputAxisName = "Mouse Y";
        Debug.Log("but Down");
    }

}
