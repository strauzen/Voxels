using UnityEngine;
using System.Collections;

public class Modify : MonoBehaviour {

	Vector2 rotation;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
		
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
			
				Terrain.SetBlock(hit, new BlockAir());
			}
		}

		rotation = new Vector2 (
			rotation.x + Input.GetAxis ("Mouse X") * 3,
			rotation.y + Input.GetAxis ("Mouse Y") * 3
		);

		transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(rotation.y, Vector3.left);

		transform.position += transform.forward * 3 * Input.GetAxis ("Vertical");
		transform.position += transform.right * 3 * Input.GetAxis ("Horizontal");
	}
}
