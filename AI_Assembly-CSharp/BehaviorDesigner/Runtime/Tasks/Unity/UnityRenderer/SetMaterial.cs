// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer.SetMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer
{
  [TaskCategory("Unity/Renderer")]
  [TaskDescription("Sets the material on the Renderer.")]
  public class SetMaterial : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The material to set")]
    public SharedMaterial material;
    private Renderer renderer;
    private GameObject prevGameObject;

    public SetMaterial()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.renderer = (Renderer) defaultGameObject.GetComponent<Renderer>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.renderer, (Object) null))
      {
        Debug.LogWarning((object) "Renderer is null");
        return (TaskStatus) 1;
      }
      this.renderer.set_material(this.material.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.material = (SharedMaterial) null;
    }
  }
}
