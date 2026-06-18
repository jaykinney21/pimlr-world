using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCar01Track : MonoBehaviour
{

    public GameObject TheMarker;
    public List<GameObject> Markers = new List<GameObject>();

    public int MarkTracker = 0;

    void Update()
    {
        if (MarkTracker < Markers.Count)
        {
            TheMarker.transform.position = Markers[MarkTracker].transform.position;
        }
    }

    private IEnumerator OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "DreamCar01")
        {
            this.GetComponent<BoxCollider>().enabled = false;
            MarkTracker = (MarkTracker + 1) % Markers.Count;  // This will loop back to 0 if MarkTracker equals the number of Markers.
            yield return new WaitForSeconds(1);
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
