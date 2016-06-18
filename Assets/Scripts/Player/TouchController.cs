using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	Player player;

	void Start () {
		player = FindObjectOfType<Player>();
//		Debug.Log("Player" + player.name);
	}
	
	public void RightButton() {
		player.MoveControl(1f);
//		Debug.Log("Right");
	}

	public void LeftButton() {
		player.MoveControl(-1f);
//		Debug.Log("Left");
	}

	public void ReleaseButton() {
		player.MoveControl(0f);
//		Debug.Log("None");
	}

	public void JumpButton() {
		player.jumpButtonPressed = true;
		player.jumpButtonDown = true;
		player.jumpButtonUp = false;
//		Debug.Log("Jump");
	}


	public void JumpReleaseButton() {
		player.jumpButtonUp = true;
		player.jumpButtonDown = false;
//		Debug.Log("JNone");
	}
}
