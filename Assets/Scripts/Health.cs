using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

	Text text;
	Graphic parent;
	Player player;
	float previoushealth;

	void Start () {
		player = FindObjectOfType<Player>();
		text = GetComponent<Text>();
		text.text = player.currentHealth.ToString();
		parent = transform.parent.gameObject.GetComponent<Graphic>();
	}


	void Update () {
		float currentHealth = player.currentHealth;
		text.text = currentHealth.ToString();
		if(currentHealth != previoushealth) {
			Fade();		//Call fade in/out if any change in health
		}
		previoushealth = currentHealth;
	}
		
	void Fade() {
		// Show Health
		parent.CrossFadeAlpha(1f, 0.01f, false);
		for(int i = 0; i<parent.gameObject.transform.childCount; i++) {
			GameObject child = parent.transform.GetChild(i).gameObject;
			Graphic childs = child.GetComponent<Graphic>();
			childs.CrossFadeAlpha(1f, 0.01f, false);
		}

		// Slowly fade out health
		parent.CrossFadeAlpha(0f, 4f, false);
		for(int i = 0; i<parent.gameObject.transform.childCount; i++) {
			GameObject child = parent.transform.GetChild(i).gameObject;
			Graphic childs = child.GetComponent<Graphic>();
			childs.CrossFadeAlpha(0f, 4f, false);
		}
	}
}
