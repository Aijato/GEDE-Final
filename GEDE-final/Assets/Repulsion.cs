using System.Collections.Generic;
using UnityEngine;

public class Repulsion : MonoBehaviour
{
	

	public float MFStrength = 0;
	public float magneticCharge;
	public bool charged = false;
	
	private const float MAGNETIC_STRENGHT = 1000;

	private Vector3 localMagneticForce;
	private List<Vector3> voxels;
	private int slicesPerAxis = 1;


	
	private void Start()
	{

		var originalRotation = transform.rotation;
		var originalPosition = transform.position;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;



		if (collider == null)
		{
			//object must have a collider
			gameObject.AddComponent<MeshCollider>();
		}

		var bounds = collider.bounds;
//

		if (rigidbody == null)
		{
			gameObject.AddComponent<Rigidbody>();
		}
		rigidbody.centerOfMass = new Vector3(0, -bounds.extents.y * 0f, 0) + transform.InverseTransformPoint(bounds.center);

		voxels = SliceIntoVoxels();

		transform.rotation = originalRotation;
		transform.position = originalPosition;

		float floatingMass = rigidbody.mass / 500;

		float magneticForceMagnitude = MAGNETIC_STRENGHT * Mathf.Abs(Physics.gravity.y) * floatingMass;
		localMagneticForce = new Vector3(0, magneticForceMagnitude, 0) / voxels.Count;

	}

	private List<Vector3> SliceIntoVoxels()
	{
		var points = new List<Vector3>(slicesPerAxis * slicesPerAxis * slicesPerAxis);

		var bounds = GetComponent<Collider>().bounds;
		for (int ix = 0; ix < slicesPerAxis; ix++)
		{
			for (int iy = 0; iy < slicesPerAxis; iy++)
			{
				for (int iz = 0; iz < slicesPerAxis; iz++)
				{						float x = bounds.min.x + bounds.size.x / slicesPerAxis * (0.5f + ix);
					float y = bounds.min.y + bounds.size.y / slicesPerAxis * (0.5f + iy);
					float z = bounds.min.z + bounds.size.z / slicesPerAxis * (0.5f + iz);
		
					var p = transform.InverseTransformPoint(new Vector3(x, y, z));
											points.Add(p);
				}
			}
		}

		return points;
	}

	private void OnTriggerStay (Collider other) {
		//get the MC of the ground
		MFStrength = other.GetComponent<Ground_MF>().MFLevel;
	}

	private float GetMagneticFieldLevel(float x, float z)
	{
		float totalMFStrenght = MFStrength + (magneticCharge*5);
		return totalMFStrenght;
	}

	// update physics
	private void FixedUpdate()
	{
		//get the colliders for the spheres
		SphereCollider sphCol = this.GetComponent<SphereCollider> ();
		// shut off/turn on the magnetic field
		if (Input.GetKeyDown (KeyCode.N)) {
			if (charged == false)
				charged = true;
			else 
				charged = false;
		}
		if (charged) {
			sphCol.radius = magneticCharge / 2;


			foreach (var point in voxels) {
				//fo for floating object
				var fo = transform.TransformPoint (point);
				float MFLevel = GetMagneticFieldLevel (fo.x, fo.z);

				if (fo.y  < MFLevel) {
					float k = (MFLevel - fo.y);
					if (k > 1) {
						k = 1f;
					} else if (k < 0) {
						k = 0f;
					}

					var force =  Mathf.Sqrt (k) * localMagneticForce;
					rigidbody.AddForceAtPosition (force, fo);

				}
			}
		} else
			//reset the radius of the colliders
			sphCol.radius = 0.5f;
	}


}