using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSegue : MonoBehaviour
{
    public Transform Personagem;
    public float minX, maxX;

    private void FixedUpdate()
    {
        Vector3 newPosition = Personagem.position + new Vector3(0, 0, -10);
        newPosition.y = 0.01f;
        transform.position = newPosition;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
    }
}
