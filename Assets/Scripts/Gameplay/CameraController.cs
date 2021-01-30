using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void Initialize(PlayerController player)
    {
        this.player = player;
    }

    public void SetBounds(float maxLeft, float maxRight)
    {
        maxLeftPosition = maxLeft;
        maxRightPosition = maxRight;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(player.transform.position.x, maxLeftPosition, maxRightPosition);
        transform.position = position;
    }

    private PlayerController player;
    private float maxLeftPosition;
    private float maxRightPosition;
}
