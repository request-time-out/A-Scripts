// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace EpicToonFX
{
  public class ETFXRotation : MonoBehaviour
  {
    [Header("Rotate axises by degrees per second")]
    public Vector3 rotateVector;
    public ETFXRotation.spaceEnum rotateSpace;

    public ETFXRotation()
    {
      base.\u002Ector();
    }

    private void Start()
    {
    }

    private void Update()
    {
      if (this.rotateSpace == ETFXRotation.spaceEnum.Local)
        ((Component) this).get_transform().Rotate(Vector3.op_Multiply(this.rotateVector, Time.get_deltaTime()));
      if (this.rotateSpace != ETFXRotation.spaceEnum.World)
        return;
      ((Component) this).get_transform().Rotate(Vector3.op_Multiply(this.rotateVector, Time.get_deltaTime()), (Space) 0);
    }

    public enum spaceEnum
    {
      Local,
      World,
    }
  }
}
