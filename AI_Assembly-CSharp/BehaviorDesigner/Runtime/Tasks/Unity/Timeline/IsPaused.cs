// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Timeline.IsPaused
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Playables;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Timeline
{
  [TaskCategory("Unity/Timeline")]
  [TaskDescription("Is the timeline currently paused?")]
  public class IsPaused : Conditional
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    private PlayableDirector playableDirector;
    private GameObject prevGameObject;

    public IsPaused()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.playableDirector = (PlayableDirector) defaultGameObject.GetComponent<PlayableDirector>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.playableDirector, (Object) null))
      {
        Debug.LogWarning((object) "PlayableDirector is null");
        return (TaskStatus) 1;
      }
      return this.playableDirector.get_state() == null ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
    }
  }
}
