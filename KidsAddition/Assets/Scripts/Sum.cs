using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sum : MonoBehaviour
{
	private int myValue;
	
	void Start ()
	{
		myValue = Random.Range(1, 10);
		GetComponent<Text>().text = myValue.ToString();
	}
	
	void Update ()
	{
		
	}

	public int GetMyValue()
	{
		return myValue;
	}

	public void TriggerSumReached()
	{
		GetComponent<Text>().color = Color.white;
	}
}
