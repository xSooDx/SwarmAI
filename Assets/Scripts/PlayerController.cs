using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5;
    float collisionRadius;

    CharacterController charController;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        collisionRadius = charController.radius;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveInput = moveInput.normalized * moveSpeed * Time.deltaTime;
        charController.Move(moveInput);
    }
}
