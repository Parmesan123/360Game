using System;
using UnityEngine;


public abstract class Trigger : MonoBehaviour
{
	public event Action OnTriggerEvent;
	
	private void OnTriggerEnter(Collider other)
	{
		if (!other.TryGetComponent(out Player _))
			return;
		
		TriggerOnPlayer();
	}

	protected void InvokeEvent()
	{
		OnTriggerEvent?.Invoke();
	}
	
	protected abstract void TriggerOnPlayer();
}
