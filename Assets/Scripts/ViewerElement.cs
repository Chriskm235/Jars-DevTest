using UnityEngine;
using UnityEngine.EventSystems;

namespace Jars.DevTest
{
    public class ViewerElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] ViewerState viewerState;
        [SerializeField] Vector2 rotationSpeed = new Vector2(15, 15);

        bool mouseOver;

        public void OnPointerEnter(PointerEventData eventData) => mouseOver = true;
        public void OnPointerExit(PointerEventData eventData) => mouseOver = false;

        void Update()
        {
            if (mouseOver && Input.GetKey(KeyCode.Mouse0))
            {
                var delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                var inputRot = viewerState.inputRot;

                // Apply x delta
                inputRot += Vector2.right * delta.x * rotationSpeed.x;
                inputRot.x %= 360;

                // Apply y delta
                inputRot += Vector2.up * delta.y * rotationSpeed.y;
                inputRot.y = Mathf.Clamp(inputRot.y, -15f, 45f);

                viewerState.inputRot = inputRot;
            }
        }
    }
}