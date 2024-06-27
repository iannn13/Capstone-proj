using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class MamaMovement : MonoBehaviour
{
    public GameObject pointA;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*public void WalkAway()
    {
        // Move Mama to the right
        Vector2 movement = new Vector2(1f, 0f);  // Adjust direction here if needed
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }*/

    private void Update()
    {
        rb.velocity = new Vector2(speed, 0);
    }
}
