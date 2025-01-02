using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yunash.Game;

public class Fruit : MonoBehaviour
{
    public GameObject fruitSlicingPrefab; // Prefab for the sliced fruit effect
    public GameObject floatingTextPrefab; // Prefab for floating text
    public float startForce = 15f; // Force to apply when the fruit spawns
    public bool harmful; // Flag to indicate if this fruit is harmful

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
            // Spawn slicing effect
            Vector3 direction = (col.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject slicedFruit = Instantiate(fruitSlicingPrefab, transform.position, rotation);

            // Set the parent of the sliced fruit to the "Slices" object
            GameObject slicesParent = GameObject.Find("Slices");
            if (slicesParent != null)
            {
                slicedFruit.transform.SetParent(slicesParent.transform);
            }
            else
            {
                Debug.LogWarning("Slices parent object not found in the hierarchy!");
            }

            Rigidbody2D[] slicedParts = slicedFruit.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D part in slicedParts)
            {
                Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
                part.AddForce(force * 5f, ForceMode2D.Impulse);
            }

            // Spawn floating text
            if (floatingTextPrefab != null)
            {
                GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                TextMesh textMesh = floatingText.GetComponent<TextMesh>();
                if (textMesh != null)
                {
                    textMesh.text = harmful ? "-1" : "+1";
                    Destroy(floatingText, 2f);
                }
            }

            Destroy(slicedFruit, 3f);
            Destroy(gameObject);

            // Update score
            if (harmful)
            {
                NinjaManager.Instance.SubtractLife();
            }
            else
            {
                NinjaManager.Instance.AddScore(1); // Add points for normal fruits
            }

            // Notify TaskManager
            TaskManager.Instance.DecrementTaskCount(gameObject);
        }
    }
}
