// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask.SetLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask
{
  [TaskCategory("Unity/LayerMask")]
  [TaskDescription("Sets the layer of a GameObject.")]
  public class SetLayer : Action
  {
    [Tooltip("The GameObject to set the layer of")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the layer to set")]
    public SharedString layerName;

    public SetLayer()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).set_layer(LayerMask.NameToLayer(this.layerName.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.layerName = (SharedString) "Default";
    }
  }
}
