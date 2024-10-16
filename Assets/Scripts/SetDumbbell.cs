using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;



public class SetDumbell : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    private Vector3 leftHandPosition;
    private Quaternion leftHandRotation;
    private Vector3 rightHandPosition;
    private Quaternion rightHandRotation;

    [SerializeField] public GameObject dumbbell;

    void Start()
    {
        // Try to get the XRHandSubsystem
        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
        if (handSubsystem == null)
        {
            Debug.LogError("XRHandSubsystem not found. Ensure XR Hands is set up correctly.");
        }
    }

    void Update()
    {
        if (handSubsystem != null)
        {
            // Get the left hand
            XRHand leftHand = handSubsystem.leftHand;
            if (leftHand.isTracked)
            {
                if (leftHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose leftHandPose))
                {
                    leftHandPosition = leftHandPose.position;
                    leftHandRotation = leftHandPose.rotation;
                }
            }

            // Get the right hand
            XRHand rightHand = handSubsystem.rightHand;
            if (rightHand.isTracked)
            {
                if (rightHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose rightHandPose))
                {
                    rightHandPosition = rightHandPose.position;
                    rightHandRotation = rightHandPose.rotation;
                }
            }
        }
    }
    public void setPosition(){
        XRHand leftHand = handSubsystem.leftHand;
        XRHand rightHand = handSubsystem.rightHand;

        GameObject.FindGameObjectWithTag(dumbbell.tag).SetActive(true);
        
        if (leftHand.isTracked)
            {
                transform.position = leftHandPosition;
                transform.rotation = leftHandRotation;
            }
        if (rightHand.isTracked)
            {
                transform.position = rightHandPosition;
                transform.rotation = rightHandRotation;
            }
        
       
    }
}

        