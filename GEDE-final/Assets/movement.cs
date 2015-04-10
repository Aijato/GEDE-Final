using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
	public float movingSpeed = 3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.forward * movingSpeed* Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.back * movingSpeed* Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right * movingSpeed* Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.A)){
			transform.Translate(Vector3.left * movingSpeed* Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.Space)){
			transform.Translate(Vector3.up * 10f*3* Time.deltaTime);
		}
	
	}
}
