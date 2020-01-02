// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.HasReceivedEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Returns success as soon as the event specified by eventName has been received.")]
  [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
  public class HasReceivedEvent : Conditional
  {
    [Tooltip("The name of the event to receive")]
    public SharedString eventName;
    [Tooltip("Optionally store the first sent argument")]
    [SharedRequired]
    public SharedVariable storedValue1;
    [Tooltip("Optionally store the second sent argument")]
    [SharedRequired]
    public SharedVariable storedValue2;
    [Tooltip("Optionally store the third sent argument")]
    [SharedRequired]
    public SharedVariable storedValue3;
    private bool eventReceived;
    private bool registered;

    public HasReceivedEvent()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      if (this.registered)
        return;
      ((Task) this).get_Owner().RegisterEvent(this.eventName.get_Value(), new Action(this.ReceivedEvent));
      ((Task) this).get_Owner().RegisterEvent<object>(this.eventName.get_Value(), (Action<M0>) new Action<object>(this.ReceivedEvent));
      ((Task) this).get_Owner().RegisterEvent<object, object>(this.eventName.get_Value(), (Action<M0, M1>) new Action<object, object>(this.ReceivedEvent));
      ((Task) this).get_Owner().RegisterEvent<object, object, object>(this.eventName.get_Value(), (Action<M0, M1, M2>) new Action<object, object, object>(this.ReceivedEvent));
      this.registered = true;
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.eventReceived ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnEnd()
    {
      if (this.eventReceived)
      {
        ((Task) this).get_Owner().UnregisterEvent(this.eventName.get_Value(), new Action(this.ReceivedEvent));
        ((Task) this).get_Owner().UnregisterEvent<object>(this.eventName.get_Value(), (Action<M0>) new Action<object>(this.ReceivedEvent));
        ((Task) this).get_Owner().UnregisterEvent<object, object>(this.eventName.get_Value(), (Action<M0, M1>) new Action<object, object>(this.ReceivedEvent));
        ((Task) this).get_Owner().UnregisterEvent<object, object, object>(this.eventName.get_Value(), (Action<M0, M1, M2>) new Action<object, object, object>(this.ReceivedEvent));
        this.registered = false;
      }
      this.eventReceived = false;
    }

    private void ReceivedEvent()
    {
      this.eventReceived = true;
    }

    private void ReceivedEvent(object arg1)
    {
      this.ReceivedEvent();
      if (this.storedValue1 == null || this.storedValue1.get_IsNone())
        return;
      this.storedValue1.SetValue(arg1);
    }

    private void ReceivedEvent(object arg1, object arg2)
    {
      this.ReceivedEvent();
      if (this.storedValue1 != null && !this.storedValue1.get_IsNone())
        this.storedValue1.SetValue(arg1);
      if (this.storedValue2 == null || this.storedValue2.get_IsNone())
        return;
      this.storedValue2.SetValue(arg2);
    }

    private void ReceivedEvent(object arg1, object arg2, object arg3)
    {
      this.ReceivedEvent();
      if (this.storedValue1 != null && !this.storedValue1.get_IsNone())
        this.storedValue1.SetValue(arg1);
      if (this.storedValue2 != null && !this.storedValue2.get_IsNone())
        this.storedValue2.SetValue(arg2);
      if (this.storedValue3 == null || this.storedValue3.get_IsNone())
        return;
      this.storedValue3.SetValue(arg3);
    }

    public virtual void OnBehaviorComplete()
    {
      ((Task) this).get_Owner().UnregisterEvent(this.eventName.get_Value(), new Action(this.ReceivedEvent));
      ((Task) this).get_Owner().UnregisterEvent<object>(this.eventName.get_Value(), (Action<M0>) new Action<object>(this.ReceivedEvent));
      ((Task) this).get_Owner().UnregisterEvent<object, object>(this.eventName.get_Value(), (Action<M0, M1>) new Action<object, object>(this.ReceivedEvent));
      ((Task) this).get_Owner().UnregisterEvent<object, object, object>(this.eventName.get_Value(), (Action<M0, M1, M2>) new Action<object, object, object>(this.ReceivedEvent));
      this.eventReceived = false;
      this.registered = false;
    }

    public virtual void OnReset()
    {
      this.eventName = (SharedString) string.Empty;
    }
  }
}
