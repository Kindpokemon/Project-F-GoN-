using UnityEngine;
using System.Collections;

public class LightRotate : MonoBehaviour {

	public GameObject lightObj;
	public float xRotate;
	public float yRotate;
	public float zRotate;

	// Use this for initialization
	void Start () {
		lightObj = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		lightObj.transform.Rotate (new Vector3 (xRotate,yRotate,zRotate));
	}
}
