using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beatrate.ReliableMecanimEvents
{
	public enum MecanimEventParameter
	{
		None,
		Int,
		Float,
		String,
		Object,
		Event
	}

	public enum MecanimEventDispatchMode
	{
		SendMessage,
		EventReceiver
	}

	public enum MecanimAnimatorParameterType
	{
		None = 0,
		Float = 1,
		Int = 3,
		Bool = 4,
		Trigger = 9
	}

	public enum MecanimTriggerAction
	{
		Set,
		Reset
	}

	[Serializable]
	public class MecanimAnimatorAction
	{
		public string VariableName
		{
			get => variableName;
			set => variableName = value;
		}
		[SerializeField]
		private string variableName = "";

		public MecanimAnimatorParameterType VariableType
		{
			get => variableType;
			set => variableType = value;
		}
		[SerializeField]
		private MecanimAnimatorParameterType variableType = MecanimAnimatorParameterType.None;

		public float FloatValue
		{
			get => floatValue;
			set => floatValue = value;
		}
		[SerializeField]
		private float floatValue = 0.0f;

		public int IntValue
		{
			get => intValue;
			set => intValue = value;
		}
		[SerializeField]
		private int intValue = 0;

		public bool BoolValue
		{
			get => boolValue;
			set => boolValue = value;
		}
		[SerializeField]
		private bool boolValue = false;

		public MecanimTriggerAction TriggerValue
		{
			get => triggerValue;
			set => triggerValue = value;
		}
		[SerializeField]
		private MecanimTriggerAction triggerValue = MecanimTriggerAction.Set;

		private int variableHash = -1;

		public void Invoke(Animator animator)
		{
			if(VariableName.Length == 0 || VariableType == MecanimAnimatorParameterType.None)
			{
				return;
			}

			if(variableHash == -1)
			{
				variableHash = Animator.StringToHash(VariableName);
			}

			switch(VariableType)
			{
				case MecanimAnimatorParameterType.Float:
					animator.SetFloat(variableHash, FloatValue);
					break;
				case MecanimAnimatorParameterType.Int:
					animator.SetInteger(variableHash, IntValue);
					break;
				case MecanimAnimatorParameterType.Bool:
					animator.SetBool(variableHash, BoolValue);
					break;
				case MecanimAnimatorParameterType.Trigger:
					switch(TriggerValue)
					{
						case MecanimTriggerAction.Set:
							animator.SetTrigger(variableHash);
							break;
						case MecanimTriggerAction.Reset:
							animator.ResetTrigger(variableHash);
							break;
					}
					break;
				default:
					break;
			}
		}
	}

	[Serializable]
	public class MecanimEventInstance
	{
		public MecanimEventDispatchMode DispatchMode
		{
			get => dispatchMode;
			set => dispatchMode = value;
		}
		[SerializeField]
		private MecanimEventDispatchMode dispatchMode = MecanimEventDispatchMode.SendMessage;

		public string MethodName
		{
			get => methodName;
			set => methodName = value;
		}
		[SerializeField]
		private string methodName = "";

		public float NormalizedTime
		{
			get => normalizedTime;
			set => normalizedTime = Mathf.Clamp01(value);
		}
		[SerializeField]
		[Range(0.0f, 1.0f)]
		private float normalizedTime = 0.0f;

		public bool AlwaysTrigger
		{
			get => alwaysTrigger;
			set => alwaysTrigger = value;
		}
		[SerializeField]
		private bool alwaysTrigger = false;

		public bool RepeatOnLoop
		{
			get => repeatOnLoop;
			set => repeatOnLoop = value;
		}
		[SerializeField]
		private bool repeatOnLoop = false;

		public MecanimEventParameter Parameter
		{
			get => parameter;
			set => parameter = value;
		}
		[SerializeField]
		private MecanimEventParameter parameter = MecanimEventParameter.None;

		public int IntParameter
		{
			get => intParameter;
			set => intParameter = value;
		}
		[SerializeField]
		private int intParameter = default;

		public float FloatParameter
		{
			get => floatParameter;
			set => floatParameter = value;
		}
		[SerializeField]
		private float floatParameter = default;

		public string StringParameter
		{
			get => stringParameter;
			set => stringParameter = value;
		}
		[SerializeField]
		private string stringParameter = default;

		public UnityEngine.Object ObjectParameter
		{
			get => objectParameter;
			set => objectParameter = value;
		}
		[SerializeField]
		private UnityEngine.Object objectParameter = default;

		public List<MecanimAnimatorAction> AnimatorActions => animatorActions;
		[SerializeField]
		private List<MecanimAnimatorAction> animatorActions = new List<MecanimAnimatorAction>();

		public bool DebugBreak => debugBreak;
		[SerializeField]
		private bool debugBreak = false;

		public bool DebugLog => debugLog;
		[SerializeField]
		private bool debugLog = false;

		public string DebugString => debugString;
		[SerializeField]
		private string debugString = "";

		public void Invoke(Animator animator, MecanimEventReceiver eventReceiver = null)
		{
			DispatchAnimatorActions(animator);

			if(ValidateMethodName(MethodName))
			{
				switch(DispatchMode)
				{
					case MecanimEventDispatchMode.EventReceiver:
						DispatchEventReceiver(animator, eventReceiver);
						break;
					default:
						DispatchSendMessage(animator);
						break;
				}
			}


			if(Application.isEditor && DebugLog)
			{
				string debugString;
				if(!string.IsNullOrEmpty(DebugString))
				{
					debugString = DebugString;
				}
				else if(!string.IsNullOrEmpty(MethodName))
				{
					debugString = MethodName;
				}
				else
				{
					debugString = "Unnamed animation event";
				}

				Debug.Log(debugString);
			}

			if(Application.isEditor && DebugBreak)
			{
				Debug.Break();
			}
		}

		private void DispatchAnimatorActions(Animator animator)
		{
			for(int actionIndex = 0; actionIndex < AnimatorActions.Count; ++actionIndex)
			{
				MecanimAnimatorAction action = AnimatorActions[actionIndex];
				if(action != null)
				{
					action.Invoke(animator);
				}
			}
		}

		private bool ValidateMethodName(string methodName)
		{
			return !string.IsNullOrEmpty(methodName);
		}

		private void DispatchEventReceiver(Animator animator, MecanimEventReceiver eventReceiver)
		{
			if(eventReceiver != null)
			{
				eventReceiver.Dispatch(this);
			}
		}

		private void DispatchSendMessage(Animator animator)
		{
			switch(Parameter)
			{
				case MecanimEventParameter.Int:
					animator.gameObject.SendMessage(MethodName, IntParameter, SendMessageOptions.DontRequireReceiver);
					break;
				case MecanimEventParameter.Float:
					animator.gameObject.SendMessage(MethodName, FloatParameter, SendMessageOptions.DontRequireReceiver);
					break;
				case MecanimEventParameter.String:
					animator.gameObject.SendMessage(MethodName, StringParameter, SendMessageOptions.DontRequireReceiver);
					break;
				case MecanimEventParameter.Object:
					animator.gameObject.SendMessage(MethodName, ObjectParameter, SendMessageOptions.DontRequireReceiver);
					break;
				case MecanimEventParameter.Event:
					animator.gameObject.SendMessage(MethodName, this, SendMessageOptions.DontRequireReceiver);
					break;
				default:
					animator.gameObject.SendMessage(MethodName, SendMessageOptions.DontRequireReceiver);
					break;
			}
		}
	}

	public class MecanimEvent : StateMachineBehaviour
	{
		private struct MecanimEventTimestamp
		{
			public int Cycle;
			public float NormalizedTime;
		}

		private struct MecanimEventTrackedInfo
		{
			public bool DispatchedOnce;
			public int? LastDispatchCycle;
		}

		public MecanimEventInstance OnEnterEvent => onEnterEvent;
		[SerializeField]
		private MecanimEventInstance onEnterEvent = null;

		public MecanimEventInstance OnExitEvent => onExitEvent;
		[SerializeField]
		private MecanimEventInstance onExitEvent = null;

		public List<MecanimEventInstance> Events => events;
		[SerializeField]
		private List<MecanimEventInstance> events = new List<MecanimEventInstance>();

		private MecanimEventReceiver eventReceiver = null;
		private float? lastNormalizedTime = null;
		private List<MecanimEventTrackedInfo> trackedInfos = new List<MecanimEventTrackedInfo>();

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			lastNormalizedTime = null;

			trackedInfos.Clear();
			for(int i = 0; i < events.Count; ++i)
			{
				trackedInfos.Add(new MecanimEventTrackedInfo
				{
					DispatchedOnce = false,
					LastDispatchCycle = null
				});
			}

			Call(animator, onEnterEvent);

		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float normalizedTime = stateInfo.normalizedTime;
			int cycle = Mathf.FloorToInt(normalizedTime);
			normalizedTime = normalizedTime - cycle;

			for(int i = 0; i < events.Count; ++i)
			{
				if(ShouldCallEventOnExit(i, stateInfo))
				{
					Call(animator, events[i]);
					RecordLastCall(i, stateInfo);
				}
			}

			Call(animator, onExitEvent);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(lastNormalizedTime.HasValue)
			{
				float normalizedTime = stateInfo.normalizedTime;
				int cycle = Mathf.FloorToInt(normalizedTime);
				int lastCycle = Mathf.FloorToInt(lastNormalizedTime.Value);

				if(cycle != lastCycle)
				{
					for(int i = 0; i < trackedInfos.Count; ++i)
					{
						var trackedInfo = trackedInfos[i];
						trackedInfo.LastDispatchCycle = null;
						trackedInfos[i] = trackedInfo;
					}
				}
			}

			for(int i = 0; i < events.Count; ++i)
			{
				if(ShouldCallEventOnUpdate(i, stateInfo))
				{
					Call(animator, events[i]);
					RecordLastCall(i, stateInfo);
				}
			}

			lastNormalizedTime = stateInfo.normalizedTime;
		}

		private bool ShouldCallEventOnExit(int eventIndex, AnimatorStateInfo stateInfo)
		{
			MecanimEventInstance e = events[eventIndex];
			float normalizedTime = stateInfo.normalizedTime;
			int cycle = Mathf.FloorToInt(normalizedTime);
			normalizedTime -= cycle;
			var trackedInfo = trackedInfos[eventIndex];

			if(!e.AlwaysTrigger)
			{
				return false;
			}

			if(!trackedInfo.DispatchedOnce)
			{
				return true;
			}

			if(!e.RepeatOnLoop)
			{
				return false;
			}

			if(trackedInfo.LastDispatchCycle.HasValue && trackedInfo.LastDispatchCycle.Value == cycle)
			{
				return false;
			}

			return true;
		}

		private bool ShouldCallEventOnUpdate(int eventIndex, AnimatorStateInfo stateInfo)
		{
			MecanimEventInstance e = events[eventIndex];

			float normalizedTime = stateInfo.normalizedTime;
			int cycle = Mathf.FloorToInt(normalizedTime);
			normalizedTime -= cycle;
			float timeInSeconds = normalizedTime * stateInfo.length;
			var trackedInfo = trackedInfos[eventIndex];

			if(!e.RepeatOnLoop && trackedInfo.DispatchedOnce)
			{
				return false;
			}

			if(!lastNormalizedTime.HasValue)
			{
				return normalizedTime == e.NormalizedTime;
			}

			int lastCycle = Mathf.FloorToInt(lastNormalizedTime.Value);

			float normalizedEventTimeInLastCycle = lastCycle + e.NormalizedTime;
			float normalizedEventTimeInThisCycle = cycle + e.NormalizedTime;

			if(TimeRangeContains(normalizedEventTimeInLastCycle, lastNormalizedTime.Value, stateInfo.normalizedTime) || (lastCycle != cycle && TimeRangeContains(normalizedEventTimeInThisCycle, lastNormalizedTime.Value, stateInfo.normalizedTime)))
			{
				return true;
			}

			return false;
		}

		private bool TimeRangeContains(float value, float start, float end)
		{
			if(end >= start)
			{
				return value > start && value <= end;
			}
			else
			{
				return value > end && value <= start;
			}
		}

		private void Call(Animator animator, MecanimEventInstance e)
		{
			e.Invoke(animator, GetOrCacheEventReceiver(animator));
		}

		private void RecordLastCall(int eventIndex, AnimatorStateInfo stateInfo)
		{
			float normalizedTime = stateInfo.normalizedTime;
			int cycle = Mathf.FloorToInt(normalizedTime);

			var trackedInfo = trackedInfos[eventIndex];

			trackedInfo.DispatchedOnce = true;
			trackedInfo.LastDispatchCycle = cycle;

			trackedInfos[eventIndex] = trackedInfo;
		}

		

		private MecanimEventReceiver GetOrCacheEventReceiver(Animator animator)
		{
			if(eventReceiver == null)
			{
				eventReceiver = animator.GetComponent<MecanimEventReceiver>();
			}

			return eventReceiver;
		}
	}
}

