using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beatrate.ReliableMecanimEvents
{
	public class MecanimEventReceiver : MonoBehaviour
	{
		private class EventListenerStorage
		{
			private List<Action<MecanimEventInstance>> listeners = new List<Action<MecanimEventInstance>>();

			public void AddListener(Action<MecanimEventInstance> listener)
			{
				listeners.Add(listener);
			}

			public void RemoveListener(Action<MecanimEventInstance> listener)
			{
				listeners.Remove(listener);
			}

			public void Dispatch(MecanimEventInstance e)
			{
				for(int i = listeners.Count - 1; i >= 0; --i)
				{
					var listener = listeners[i];
					listener?.Invoke(e);
				}
			}
		}

		private Dictionary<string, EventListenerStorage> listenerStorages = new Dictionary<string, EventListenerStorage>();

		public void Dispatch(MecanimEventInstance e)
		{
			if(listenerStorages.TryGetValue(e.MethodName, out EventListenerStorage storage))
			{
				storage.Dispatch(e);
			}
		}

		public void AddListener(string eventName, Action<MecanimEventInstance> listener)
		{
			if(!listenerStorages.TryGetValue(eventName, out EventListenerStorage storage))
			{
				storage = new EventListenerStorage();
				listenerStorages.Add(eventName, storage);
			}

			storage.AddListener(listener);
		}

		public void RemoveListener(string eventName, Action<MecanimEventInstance> listener)
		{
			if(!listenerStorages.TryGetValue(eventName, out EventListenerStorage storage))
			{
				return;
			}

			storage.RemoveListener(listener);
		}
	}
}

