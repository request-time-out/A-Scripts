// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource.GetMute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
  [TaskCategory("Unity/AudioSource")]
  [TaskDescription("Stores the mute value of the AudioSource. Returns Success.")]
  public class GetMute : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The mute value of the AudioSource")]
    [RequiredField]
    public SharedBool storeValue;
    private AudioSource audioSource;
    private GameObject prevGameObject;

    public GetMute()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.audioSource = (AudioSource) defaultGameObject.GetComponent<AudioSource>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.audioSource, (Object) null))
      {
        Debug.LogWarning((object) "AudioSource is null");
        return (TaskStatus) 1;
      }
      this.storeValue.set_Value(this.audioSource.get_mute());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.storeValue = (SharedBool) false;
    }
  }
}
