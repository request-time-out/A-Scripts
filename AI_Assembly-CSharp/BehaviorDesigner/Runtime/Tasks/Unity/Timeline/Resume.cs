// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Timeline.Resume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Playables;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Timeline
{
  [TaskCategory("Unity/Timeline")]
  [TaskDescription("Resume playing a paused playable.")]
  public class Resume : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("Should the task be stopped when the timeline has stopped playing?")]
    public SharedBool stopWhenComplete;
    private PlayableDirector playableDirector;
    private GameObject prevGameObject;
    private bool playbackStarted;

    public Resume()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
      {
        this.playableDirector = (PlayableDirector) defaultGameObject.GetComponent<PlayableDirector>();
        this.prevGameObject = defaultGameObject;
      }
      this.playbackStarted = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.playableDirector, (Object) null))
      {
        Debug.LogWarning((object) "PlayableDirector is null");
        return (TaskStatus) 1;
      }
      if (this.playbackStarted)
        return this.stopWhenComplete.get_Value() && this.playableDirector.get_state() == 1 ? (TaskStatus) 3 : (TaskStatus) 2;
      this.playableDirector.Resume();
      this.playbackStarted = true;
      return this.stopWhenComplete.get_Value() ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.stopWhenComplete = (SharedBool) false;
    }
  }
}
