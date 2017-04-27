using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
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
            transform.localPosition += new Vector3(hInput * speedX, vInput * speedY);
        }
        else
        {

        }
	}
}
