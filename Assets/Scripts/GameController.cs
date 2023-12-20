using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class GameController : MonoBehaviour
{
    public GameObject SteeringWheel;
    public PrometeoCarController CarController;

    // Start is called before the first frame update
    void Start()
    {

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Input.compensateSensors = true;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float val = MyFn.GetDeviceXYRotation(5f);
        val = Mathf.Clamp(val, -40, +40);
        var rot = SteeringWheel.transform.eulerAngles;
        rot.z = -val;
        if (! CarController.useTouchControls)
        {
            rot.z = 0;
            SteeringWheel.SetActive(false);
        }
        else
        {
            SteeringWheel.SetActive(true);
        }
        SteeringWheel.transform.eulerAngles= rot;
    }
}



//public class CinemachineCoreGetInputTouchAxis : MonoBehaviour
//{

//    public float TouchSensitivity_x = 10f;
//    public float TouchSensitivity_y = 10f;

//    // Use this for initialization
//    void Start()
//    {
        
//        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
//    }

//    float HandleAxisInputDelegate(string axisName)
//    {
//        switch (axisName)
//        {

//            case "Mouse X":

//                if (Input.touchCount > 0)
//                {
//                    return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
//                }
//                else
//                {
//                    return Input.GetAxis(axisName);
//                }

//            case "Mouse Y":
//                if (Input.touchCount > 0)
//                {
//                    return Input.touches[0].deltaPosition.y / TouchSensitivity_y;
//                }
//                else
//                {
//                    return Input.GetAxis(axisName);
//                }

//            default:
//                Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
//                break;
//        }

//        return 0f;
//    }
//}
