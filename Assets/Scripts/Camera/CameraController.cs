using UnityEngine;


namespace Jars.DevTest
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float cameraRange = 10;
        [SerializeField] float floorY = .5f;
        [SerializeField] ViewerState viewerState;

        Camera camera;

        Quaternion currentRot;
        Quaternion currentRotOffset;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Start()
        {
            ApplyPosition(false);
            ApplyRotation(false);
        }

        private void Update()
        {
            ApplyPosition();
            ApplyRotation();
        }

        void ApplyPosition(bool tween = true)
        {
            var goalRot = Quaternion.identity;

            // Calculate and apply the two axies of rotation
            var xRotation = Quaternion.AngleAxis(viewerState.inputRot.Value.x, Vector3.up);
            var yRotation = Quaternion.AngleAxis(viewerState.inputRot.Value.y, xRotation * Vector3.right);
            goalRot *= yRotation;
            goalRot *= xRotation;

            // If youre tweening, tween to goal, otherwise go to goal
            currentRot = tween ? Quaternion.Slerp(currentRot, goalRot, 5f * Time.deltaTime) : goalRot;

            // Set the pos to be offset from the bounds based on rotation and range
            var pos = target.position - currentRot * Vector3.forward * cameraRange;
            pos.y = Mathf.Clamp(pos.y, floorY, float.MaxValue);
            transform.position = pos;
        }

        void ApplyRotation(bool tween = true)
        {
            var targetDir = currentRot * Vector3.forward;

            // Find the center and aim array so we know how much to offset the rotation
            var aimRay = camera.ViewportPointToRay(new Vector2(.75f, .5f));
            var centerRay = camera.ViewportPointToRay(new Vector2(.5f, .5f));

            // Find how much to offset from the target's direction
            var goalRot = Quaternion.FromToRotation(aimRay.direction, centerRay.direction);

            // If youre tweening, tween to goal, otherwise go to goal
            currentRotOffset = tween ? Quaternion.Lerp(currentRotOffset, goalRot, 5f * Time.deltaTime) : goalRot;

            // Set the dir to target dir rotated by the offset
            transform.rotation = Quaternion.LookRotation((currentRotOffset * targetDir).normalized);
        }
    }
}