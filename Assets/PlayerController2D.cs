using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour {

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
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

            var moveDirection = new Vector3(currentSpeedX, currentSpeedY);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, _collider.size.x / 2);
            if (hit != null) {
                _rigidbody.velocity = new Vector2(0f, 0f);
            }
            _rigidbody.velocity = new Vector2(currentSpeedX, currentSpeedY);
            
        }
        else
        {
            _rigidbody.velocity = new Vector2(0, 0);
        }
	}

    public void OnTriggerEnter2D(Collider2D other) {
        float contactPosition = other.bounds.center.x - other.bounds.size.x / 2;
        float myWidth = _collider.bounds.size.x;
        float posX = contactPosition + myWidth / 2f;
        //_rigidbody.MovePosition(new Vector3(posX, transform.localPosition.y, transform.localPosition.z));
    }
}
