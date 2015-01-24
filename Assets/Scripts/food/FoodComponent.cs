using UnityEngine;
using System.Collections;

public class FoodComponent : MonoBehaviour {

	public Swarmer.SwarmerTypes foodType;

	public float startingEnergy = 100f;

	private float _currentEnergy;

	private SpriteRenderer spriteRenderer;
	private Color color;

	void Start() {
		_currentEnergy = startingEnergy;
		spriteRenderer = GetComponent<SpriteRenderer>();
		color = spriteRenderer.color;
	}

	public float energyFractionRemaining {
		get {
			return _currentEnergy / startingEnergy;
		}
	}

	public bool depleted {
		get {
			return Mathf.Sign(_currentEnergy) != 1;
		}
	}

	public void Eat(float biteSize) {
		_currentEnergy = Mathf.Clamp(_currentEnergy - biteSize, 0f, startingEnergy);
		SetSpriteEffect();
		SetEatingEffect();
	}

	void SetSpriteEffect() {
		color.a = energyFractionRemaining;
		spriteRenderer.color = color;
	}

	void SetEatingEffect() {
		particleSystem.Play();
	}
}
