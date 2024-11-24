using UnityEngine;

public class KeyboardCursorController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard.Cursor _cursor;

    void Start()
    {
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        _cursor.MoveCursor(new Vector2(moveHorizontal, moveVertical));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cursor.PressDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _cursor.PressUp();
        }

    }
}
