using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [field: SerializeField]
    private AutoDoor door; 
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        Debug.Log("something enters door.");
        door.GetComponent<AutoDoor>().Open();
    }
    private void OnTriggerExit(Collider other) {
        Debug.Log("something Exit door.");
        door.GetComponent<AutoDoor>().Close();
    }
}
