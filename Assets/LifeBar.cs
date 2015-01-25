using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

	public Image Left;
	public Image Right;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Right.fillAmount = Level.Instance.fractionOfFoodDeath;
		Left.fillAmount = Level.Instance.fractionOfMicrobeDeath;
	
	}
}
