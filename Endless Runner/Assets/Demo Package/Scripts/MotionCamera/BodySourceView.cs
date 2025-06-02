using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using System.Buffers.Text;

public class BodySourceView : MonoBehaviour
{
    //[SerializeField] RectTransform cursor;
    [SerializeField] CharacterInputController characterInputController;
    public BodySourceManager mBodySourceManager;
    public GameObject mJointObject;

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();

    private bool isJumping = false;
    private float jumpThreshold = 0.9f; // Adjust based on your needs
    private float groundedThreshold = 0.05f;
    private float baselineY; // baseline Y when standing

    private bool isCrouching = false;
    private float crouchThreshold = 3.6f;


    private float lastJumpTime = -10f;
    private float lastCrouchTime = -10f;

    private float crouchCooldownAfterJump = 1.6f;
    private float jumpCooldownAfterCrouch = 1.6f;

    private List<JointType> _joints = new List<JointType>()
    {
            JointType.HandLeft,
            JointType.HandRight,
            JointType.Head,
            JointType.SpineBase,
    };
    
    void Update () 
    {

        #region Get Kinect data


        Body[] data = mBodySourceManager.GetData();


        if (data == null) return;


        Body firstTrackedBody = null;


        foreach (var body in data)
        {
            if (body != null && body.IsTracked)
            {
                float distance = body.Joints[JointType.SpineBase].Position.Z;
                if (distance <= 3f) // 3 meters from the sensor
                {
                    firstTrackedBody = body;
                    break;
                }
            }
        }

        if (firstTrackedBody == null) return;

        List<ulong> trackedIds = new List<ulong> { firstTrackedBody.TrackingId };

        #endregion

        #region Delete Kinect Data

        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(mBodies[trackingId]);

                mBodies.Remove(trackingId);
            }
        }

        #endregion

        #region Create Kinect Bodies
        if (!mBodies.ContainsKey(firstTrackedBody.TrackingId))
        {
            mBodies[firstTrackedBody.TrackingId] = CreateBodyObject(firstTrackedBody.TrackingId);
        }

        UpdateBodyObject(firstTrackedBody, mBodies[firstTrackedBody.TrackingId]);
        #endregion
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        foreach (JointType joint in _joints)
        {
            GameObject newJoint = Instantiate(mJointObject);
            newJoint.name = joint.ToString();

            newJoint.transform.parent = body.transform;
        }
        
        return body;
    }

    int currentLane = 1;

    private void UpdateLane(int lane)
    {
        //if (lane == currentLane) return;

        //currentLane = lane;
        characterInputController.SetLane(lane);
    }

    float laneWidth = 2.5f;


    public Vector3 cursorPosition = Vector3.zero;

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {


        Vector3 rightHandPos = GetVector3FromJoint(body.Joints[JointType.HandRight]);
        Vector3 leftHandPos = GetVector3FromJoint(body.Joints[JointType.HandLeft]);
        Vector3 selectedHand;
        Vector3 handAdjustedPosition;

        if (rightHandPos.z < leftHandPos.z)
        {
            selectedHand = rightHandPos;
        }
        else
        {
            selectedHand = leftHandPos;
        }

        handAdjustedPosition = selectedHand;

        handAdjustedPosition.z = 0;
        handAdjustedPosition.y -= 4;
        handAdjustedPosition *= 90;

        print("hand pos " + handAdjustedPosition);
        cursorPosition = handAdjustedPosition;


        foreach (JointType _joint in _joints)
        {
            Joint sourceJoint = body.Joints[_joint];

            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
                jointObject.position = targetPosition;


            if(jointObject.name == "SpineBase")
            {
                Transform spine = jointObject;

                if (spine.position.x > laneWidth)
                {
                    UpdateLane(2);
                    print("RIGHT");
                }
                else if (spine.position.x < -laneWidth)
                {
                    UpdateLane(0);
                    print("LEFT");
                }
                else
                {
                    UpdateLane(1);
                    print("CENTRE");
                }

            }


            if (jointObject.name == "Head")
            {
                Transform head = jointObject;

               

                bool inCrouchCooldown = Time.time - lastJumpTime < crouchCooldownAfterJump;
                bool inJumpCooldown = Time.time - lastCrouchTime < jumpCooldownAfterCrouch;


                float currentY = head.position.y;

                // Establish baseline when first detected
                if (baselineY == 0f) baselineY = currentY;

                float diff = currentY - baselineY;

                // Jump detection
                if (!inJumpCooldown && !isCrouching && !isJumping && diff > jumpThreshold)
                {
                    isJumping = true;
                    characterInputController.Jump();
                    Debug.LogWarning("Jump detected!");
                }
                else if (isJumping && diff < jumpThreshold * 0.5f)
                {
                    lastJumpTime = Time.time;
                    isJumping = false;
                    //Debug.Log("Landed.");
                }


                // Crouch detection
                if (!inCrouchCooldown && !isJumping && !isCrouching && diff < -crouchThreshold)
                {
                    isCrouching = true;
                    characterInputController.Slide();
                    Debug.Log("Crouch detected!");
                }
                else if (isCrouching && diff > -crouchThreshold * 0.5f)
                {
                    //characterInputController.StopSliding();
                    lastCrouchTime = Time.time;
                    isCrouching = false;
                    //Debug.Log("Standing up.");
                }

            }


        }
    }
    
    
    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
