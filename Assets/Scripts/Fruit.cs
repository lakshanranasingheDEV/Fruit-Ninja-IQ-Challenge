/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public GameObject _fruitSlicingPrefab;
    public float startForce = 15f;

    Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * startForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Blade")
        {

            Vector3 direction = (col.transform.position - transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject slicedFruit = Instantiate(_fruitSlicingPrefab, transform.position, rotation);
            Destroy(slicedFruit, 3f);
            Destroy(gameObject);
            
        }       
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject fruitSlicingPrefab; // Prefab for the sliced fruit effect
    public float startForce = 15f; // Force to apply when the fruit spawns

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * startForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Blade"))
        {
            // Calculate direction of the blade for proper slicing effect
            Vector3 direction = (col.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // Instantiate the sliced fruit prefab
            GameObject slicedFruit = Instantiate(fruitSlicingPrefab, transform.position, rotation);

            // Add scattering effect to sliced pieces
            Rigidbody2D[] slicedParts = slicedFruit.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D part in slicedParts)
            {
                Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
                part.AddForce(force * 5f, ForceMode2D.Impulse);
            }

            // Destroy the sliced fruit effect after 3 seconds
            Destroy(slicedFruit, 3f);

            // Destroy the original fruit
            Destroy(gameObject);
        }
    }
}
