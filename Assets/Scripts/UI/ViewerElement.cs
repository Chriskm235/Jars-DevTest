using UnityEngine;
using UnityEngine.EventSystems;

namespace Jars.DevTest
{
    public class ViewerElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] ViewerState viewerState;
        [SerializeField] Vector2 rotationSpeed = new Vector2(15, 15);
        [SerializeField] Vector2 startRot;

        bool mouseOver;
        bool startedMouseOver;

        public void OnPointerEnter(PointerEventData eventData) => mouseOver = true;
        public void OnPointerExit(PointerEventData eventData) => mouseOver = false;

        private void Awake()
        {
            viewerState.inputRot.Value = startRot;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                startedMouseOver = mouseOver;

            if (startedMouseOver && Input.GetKey(KeyCode.Mouse0))
            {
                var delta = new Vector3(Input.GetAxis("Mouse X")/Screen.width, Input.GetAxis("Mouse Y")/Screen.height);
                var inputRot = viewerState.inputRot.Value;

                // Apply x delta
                inputRot += Vector2.right * delta.x * rotationSpeed.x;
                inputRot.x %= 360;

                // Apply y delta
                inputRot += Vector2.up * delta.y * rotationSpeed.y;
                inputRot.y = Mathf.Clamp(inputRot.y, -15f, 45f);

                viewerState.inputRot.Value = inputRot;
            }
        }
    }
}