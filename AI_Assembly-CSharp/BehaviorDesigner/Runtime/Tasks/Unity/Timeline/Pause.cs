// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Timeline.Pause
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Playables;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Timeline
{
  [TaskCategory("Unity/Timeline")]
  [TaskDescription("Pauses playback of the currently running playable.")]
  public class Pause : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    private PlayableDirector playableDirector;
    private GameObject prevGameObject;

    public Pause()
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
      this.playableDirector.Pause();
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
    }
  }
}
