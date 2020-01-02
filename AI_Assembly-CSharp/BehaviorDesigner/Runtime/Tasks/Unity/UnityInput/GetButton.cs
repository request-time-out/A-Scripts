// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityInput.GetButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
  [TaskCategory("Unity/Input")]
  [TaskDescription("Stores the state of the specified button.")]
  public class GetButton : Action
  {
    [Tooltip("The name of the button")]
    public SharedString buttonName;
    [RequiredField]
    [Tooltip("The stored result")]
    public SharedBool storeResult;

    public GetButton()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Input.GetButton(this.buttonName.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.buttonName = (SharedString) "Fire1";
      this.storeResult = (SharedBool) false;
    }
  }
}
