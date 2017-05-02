using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
    private const float FLOAT_EPSILON = 0.001f;

    private Dictionary<int, string> _animationHashes = new Dictionary<int, string>();

    private Vector3 _currentMovement = new Vector3(0, 0);

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _animationHashes = new Dictionary<int, string>();
        foreach (var clip in animator.runtimeAnimatorController.animationClips) {
            _animationHashes.Add(Animator.StringToHash(clip.name), clip.name);
        }
    }

    // Use this for initialization
    void Start() {
        Debug.LogFormat("Current animation: {0}", _animationHashes[animator.GetCurrentAnimatorStateInfo(0).shortNameHash]);
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

        Vector3 movement = Vector3.zero;
        if (vInput != 0 || hInput != 0) {
            //animator.SetBool("moving", true);
            var currentSpeedX = hInput != 0 ? speedX * Mathf.Sign(hInput) : 0;
            var currentSpeedY = vInput != 0 ? speedY * Mathf.Sign(vInput) : 0;

            //Check horizontal colliding
            if (currentSpeedX != 0) {
                if (Mathf.Abs(_currentMovement.y) < FLOAT_EPSILON) {
                    // X-axis move is prioritized
                    currentSpeedY = 0;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(currentSpeedX, 0), _collider.size.x / 2);
                    if (hit.collider != null) {
                        currentSpeedX = 0f;
                    }
                }
                else {
                    currentSpeedX = 0;
                }
            }

            //Check vertical colliding
            if (currentSpeedY != 0) {
                if (Mathf.Abs(_currentMovement.x) < FLOAT_EPSILON) {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, currentSpeedY), _collider.size.y / 2);
                    if (hit.collider != null) {
                        currentSpeedY = 0f;
                    }
                }
                else {
                    currentSpeedY = 0;
                }
            }

            movement = new Vector2(currentSpeedX, currentSpeedY);
        }

        if ((movement - _currentMovement).magnitude > FLOAT_EPSILON) {
            _currentMovement = movement;
            TriggerAnimations();
        }
    }

    private void TriggerAnimations() {
        _rigidbody.velocity = _currentMovement;
        if (_currentMovement == Vector3.zero) {
            Debug.Log("STOP");
            animator.SetTrigger("stop");
        }
        else
        if (_currentMovement.x > 0) {
            Debug.Log("MOVE RIGHT");
            animator.SetTrigger("move_right");
        }
        else
        if (_currentMovement.x < 0) {
            Debug.Log("MOVE LEFT");
            animator.SetTrigger("move_left");
        }
        else
        if (_currentMovement.y > 0) {
            Debug.Log("MOVE UP");
            animator.SetTrigger("move_up");
        }
        else
        if (_currentMovement.y < 0) {
            Debug.Log("MOVE DOWN");
            animator.SetTrigger("move_down");
        }

        StartCoroutine(PrintAnimationLog());
    }

    private IEnumerator PrintAnimationLog() {
        yield return new WaitForSeconds(0.1f);
        Debug.LogFormat("Current animation: {0}", _animationHashes[animator.GetCurrentAnimatorStateInfo(0).shortNameHash]);
    }
}
