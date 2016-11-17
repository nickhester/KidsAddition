using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddendStage : MonoBehaviour
{
	private int numInhabitants = 0;
	private Sum sum;
	[SerializeField] Text myValueDisplay;

	void Start()
	{
		sum = FindObjectOfType<Sum>();
		if (sum == null)
		{
			Debug.LogError("stage cannot find sum object");
		}

		myValueDisplay = GetComponentInChildren<Text>();
		myValueDisplay.text = numInhabitants.ToString();
	}

	public int GetNumInhabitants()
	{
		return numInhabitants;
	}

	public void UpdateNumInhabitants(int n)
	{
		numInhabitants += n;

		myValueDisplay.text = numInhabitants.ToString();
	}
}
