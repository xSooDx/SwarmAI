using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5;
    public float health = 10;
    float collisionRadiusSq;

    CharacterController charController;
    SpacialPartitionGrid<SwarmEntity> partitionGrid = null;



    Vector3Int[] checkVec = { Vector3Int.zero };

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        collisionRadiusSq = charController.radius;
        collisionRadiusSq *= collisionRadiusSq;
    }


    private void Start()
    {
        partitionGrid = FindAnyObjectByType<SwarmManager>()?.m_PartitionGrid;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        if (health > 0)
        {
            partitionGrid.RunOpperationOnCells(checkVec, transform.position, (LinkedList<SwarmEntity> list, int neighbourIndex, Vector3Int cellIndex) =>
            {
                foreach (var entity in list)
                {
                    if (Vector3.SqrMagnitude(entity.transform.position - transform.position) < collisionRadiusSq)
                    {
                        Destroy(entity.gameObject);
                        health--;
                        if (health <= 0)
                        {
                            Dead();
                            break;
                        }
                    }
                }
            });
        }
    }

    private void Dead()
    {
        SwarmUIControls.Instance.ResetSim();
    }

    void HandleInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.forward = moveInput;
        moveInput.Normalize();
        moveInput = moveInput * moveSpeed * Time.deltaTime;
        charController.Move(moveInput);

    }
}
