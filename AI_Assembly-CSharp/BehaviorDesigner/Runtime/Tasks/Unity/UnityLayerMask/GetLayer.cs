// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask.GetLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask
{
  [TaskCategory("Unity/LayerMask")]
  [TaskDescription("Gets the layer of a GameObject.")]
  public class GetLayer : Action
  {
    [Tooltip("The GameObject to set the layer of")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the layer to get")]
    [RequiredField]
    public SharedString storeResult;

    public GetLayer()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(LayerMask.LayerToName(((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).get_layer()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.storeResult = (SharedString) string.Empty;
    }
  }
}
