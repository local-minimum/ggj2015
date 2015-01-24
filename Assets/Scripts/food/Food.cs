using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Food : MonoBehaviour {

	public List<FoodComponent> foodComponents = new List<FoodComponent>();
	public List<Transform> foodComponentPositions = new List<Transform>();

	// Use this for initialization
	void Start () {
		VerifyIntegrity();
		InstantiateFoodComponents();
	}

	void VerifyIntegrity() {
		
		if (foodComponents.Count != foodComponentPositions.Count)
			Debug.LogError(string.Format("{0} missmatch in number of food components and positions", gameObject));

	}

	void InstantiateFoodComponents() {
		for (int i=0, l=foodComponents.Count; i<l; i++) {
			FoodComponent instance = ((GameObject) Instantiate(foodComponents[i].gameObject)).GetComponent<FoodComponent>();
			instance.transform.parent = foodComponentPositions[i];
			instance.transform.localPosition = Vector3.zero;
			foodComponents[i] = instance;
		}

	}

	private FoodComponent getFirstNonDepletedComponent(Swarmer.SwarmerTypes foodType) {
		return foodComponents.Where(fc => !fc.depleted && fc.foodType == foodType).FirstOrDefault();
	}

	public bool isEdible(Swarmer swarmer) {
		return getFirstNonDepletedComponent(swarmer.SwarmerType) != null;
	}

	public void Eat(Swarmer.SwarmerTypes foodType, float biteSize) {
		FoodComponent foodComponent = getFirstNonDepletedComponent(foodType);
		if (foodComponent)
			foodComponent.Eat(biteSize);
	}

	void OnTriggerStay2D(Collider2D other) {
		other.BroadcastMessage("EatFood", this, SendMessageOptions.DontRequireReceiver);
	}

}
