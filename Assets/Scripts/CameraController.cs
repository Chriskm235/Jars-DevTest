using UnityEngine;


namespace Jars.DevTest
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float cameraRange = 10;
        [SerializeField] ViewerState viewerState;

        Camera camera;

        Quaternion goalRot;
        Quaternion currentRot;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            ApplyPosition();
            ApplyRotation();
        }

        void ApplyPosition()
        {
            goalRot = Quaternion.identity;
            var xRotation = Quaternion.AngleAxis(viewerState.inputRot.x, Vector3.up);
            var yRotation = Quaternion.AngleAxis(viewerState.inputRot.y, xRotation * Vector3.right);
            goalRot *= yRotation;
            goalRot *= xRotation;

            currentRot = Quaternion.Slerp(currentRot, goalRot, 5f * Time.deltaTime);

            // Set the pos to be offset from the bounds based on rotation and range
            transform.position = target.position - currentRot * Vector3.forward * cameraRange;
        }

        void ApplyRotation()
        {
            var targetDir = (target.position - transform.position).normalized;

            // Find the center and aim array so we know how much to offset the rotation
            var aimRay = camera.ViewportPointToRay(new Vector2(.75f, .5f));
            var centerRay = camera.ViewportPointToRay(new Vector2(.5f, .5f));

            // Find how much to offset from the target's direction
            var offsetRot = Quaternion.FromToRotation(aimRay.direction, centerRay.direction);

            // Set the dir to target dir rotated by the offset
            transform.rotation = Quaternion.LookRotation((offsetRot * targetDir).normalized);
        }
    }
}