using UnityEngine;
using System.Collections;

public class InsideScript : MonoBehaviour {

	public Material lightMaterial;
	public Material darkMaterial;
	public Material wrongMaterial;
	public Material correctMaterial;
	int _id = -1;
	SceneManager sm;

	void Start () {
		sm = (SceneManager) GameObject.Find("SceneManager").GetComponent(typeof(SceneManager));
	}

	void OnMouseDown() {
		sm.WindowClicked(_id);
	}

	public void SetStatus(string status) {
		switch (status) {
			case "on": this.renderer.material = lightMaterial; break;
			case "off": this.renderer.material = darkMaterial; break;
			case "wrong": this.renderer.material = wrongMaterial; break;
			case "correct": this.renderer.material = correctMaterial; break;
		}

	}

	public void SetId(int id) {
		this._id = id;
	}
}
