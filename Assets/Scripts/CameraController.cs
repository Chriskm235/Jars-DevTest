using UnityEngine;


namespace Jars.DevTest
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Renderer target;
        [SerializeField] Camera camera;
        [SerializeField] float cameraRange;

        Vector3 goalDir;
        float dirVel;
        Vector3 goalPos;
        Vector3 posVel;

        private void Update()
        {
            var aimRay = camera.ViewportPointToRay(new Vector2(.75f, .5f));
            var centerRay = camera.ViewportPointToRay(new Vector2(.5f, .5f));

            var offsetRot = Quaternion.FromToRotation(aimRay.direction, centerRay.direction);
            var targetDir = (target.bounds.center - transform.position).normalized;

            goalDir = (offsetRot * targetDir).normalized;
            goalPos = target.bounds.center - targetDir * cameraRange;

            transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, goalDir, Mathf.SmoothDamp(0, 1, ref dirVel, .1f)));
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref posVel, .1f);
        }
    }
}