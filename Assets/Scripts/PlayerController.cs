using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;

    //This is redundant because equipped == null is handsFree, whats the correct way to do this
    public bool handsFree;
    public Tool equipped; //Really the type here should be holdable or something

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Vector2 totalInput = new Vector3(xInput, yInput).normalized;
        Vector2 newPosition = rb.position + speed * totalInput;
        rb.MovePosition(newPosition);
    }
}
