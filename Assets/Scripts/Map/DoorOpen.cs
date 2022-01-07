using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [field: SerializeField]
    private AutoDoor door; 
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<foot>() != null)
        {
            Debug.Log("a player enters door.");
            door.GetComponent<AutoDoor>().Open();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<foot>() != null)
        {
            Debug.Log("a player Exit door.");
            door.GetComponent<AutoDoor>().Close();
        }
    }
}
