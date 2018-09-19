using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    private Quaternion initialRotaion;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        initialRotaion = transform.rotation;
    }

    private void OnEnable()
    {
        transform.rotation = initialRotaion;
    }

	void Update () {
        transform.Rotate(Vector3.forward, rotationSpeed);
	}
}
