using UnityEngine;


namespace Jars.DevTest
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Renderer target;
        [SerializeField] float cameraRange = 10;
        [SerializeField] ViewerState viewerState;

        Camera camera;

        Quaternion offsetRot;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Start()
        {
            // NOTE: This behavior was being all jittery when I did it every update,
            //  so instead I cache it

            // Find the center and aim array so we know how much to offset the rotation
            var aimRay = camera.ViewportPointToRay(new Vector2(.75f, .5f));
            var centerRay = camera.ViewportPointToRay(new Vector2(.5f, .5f));

            // Find how much to offset from the target's direction
            offsetRot = Quaternion.FromToRotation(aimRay.direction, centerRay.direction);
        }

        private void Update()
        {
            ApplyPosition();
            ApplyRotation();
        }

        void ApplyPosition()
        {
            var quatRotation = Quaternion.identity;
            var xRotation = Quaternion.AngleAxis(viewerState.inputRot.x, Vector3.up);
            var yRotation = Quaternion.AngleAxis(viewerState.inputRot.y, xRotation * Vector3.right);
            quatRotation *= yRotation;
            quatRotation *= xRotation;

            // Set the pos to be offset from the bounds based on rotation and range
            transform.position = target.bounds.center - quatRotation * Vector3.forward * cameraRange;
        }

        void ApplyRotation()
        {
            var targetDir = (target.bounds.center - transform.position).normalized;

            // Set the dir to target dir rotated by the offset
            transform.rotation = Quaternion.LookRotation((offsetRot * targetDir).normalized);
        }
    }
}