using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;

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
