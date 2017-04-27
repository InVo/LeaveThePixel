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

        verticalText.text = vInput.ToString();
        horizontalText.text = hInput.ToString();
        if (vInput != 0 || hInput != 0) {
            animator.SetBool("moving", true);
            _rigidbody.MovePosition(transform.localPosition += new Vector3(hInput * speedX, vInput * speedY));
            //transform.localPosition += new Vector3(hInput * speedX, vInput * speedY);
        }
        else
        {

        }
	}

    public void OnTriggerEnter2D(Collider2D other) {
        float contactPosition = other.bounds.center.x - other.bounds.size.x / 2;
        float myWidth = _collider.bounds.size.x;
        float posX = contactPosition - myWidth / 2f;
    }
}
