// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation.Blend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
  [TaskCategory("Unity/Animation")]
  [TaskDescription("Blends the animation. Returns Success.")]
  public class Blend : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the animation")]
    public SharedString animationName;
    [Tooltip("The weight the animation should blend to")]
    public float targetWeight;
    [Tooltip("The amount of time it takes to blend")]
    public float fadeLength;
    private Animation animation;
    private GameObject prevGameObject;

    public Blend()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.animation = (Animation) defaultGameObject.GetComponent<Animation>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.animation, (Object) null))
      {
        Debug.LogWarning((object) "Animation is null");
        return (TaskStatus) 1;
      }
      this.animation.Blend(this.animationName.get_Value(), this.targetWeight, this.fadeLength);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.animationName = (SharedString) string.Empty;
      this.targetWeight = 1f;
      this.fadeLength = 0.3f;
    }
  }
}
