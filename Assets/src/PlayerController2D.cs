using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour
{

    public Animator animator;

    [Header("Axis names")]
    public string axisXName;
    public string axisYName;

    [Header("Axis speed")]
    public float speedX;
    public float speedY;

    public Text verticalText;
    public Text horizontalText;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    private const float INPUT_CLAMPING = 0.99f;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float vInput = Input.GetAxis(axisYName);
        float hInput = Input.GetAxis(axisXName);

        //Makes axis inputs discrete and makes input zero right after releasing axis buttons
        vInput = Mathf.Clamp01(Mathf.Abs(vInput) - INPUT_CLAMPING) * Mathf.Sign(vInput);
        hInput = Mathf.Clamp01(Mathf.Abs(hInput) - INPUT_CLAMPING) * Mathf.Sign(hInput);

        verticalText.text = vInput.ToString();
        horizontalText.text = hInput.ToString();

        if (vInput != 0 || hInput != 0) {
            //animator.SetBool("moving", true);
            var currentSpeedX = hInput != 0 ? speedX * Mathf.Sign(hInput) : 0;
            var currentSpeedY = vInput != 0 ? speedY * Mathf.Sign(vInput) : 0;

            //Check vertical colliding
            if (currentSpeedY != 0) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, currentSpeedY), _collider.size.y / 2);
                if (hit.collider != null) {
                    currentSpeedY = 0f;
                }
            }

            //Check horizontal colliding
            if (currentSpeedX != 0) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(currentSpeedX, 0), _collider.size.x / 2);
                if (hit.collider != null) {
                    currentSpeedX = 0f;
                }
            }

            _rigidbody.velocity = new Vector2(currentSpeedX, currentSpeedY);
        }
        else {
            _rigidbody.velocity = new Vector2(0, 0);
        }
    }
}
