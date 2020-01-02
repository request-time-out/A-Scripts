// Decompiled with JetBrains decompiler
// Type: BoneSwayCtrl.FakeTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BoneSwayCtrl
{
  public class FakeTransform
  {
    public Vector3 Pos;
    public Quaternion Rot;
    public Vector3 Scale;

    public FakeTransform()
    {
      this.Pos = Vector3.get_zero();
      this.Rot = Quaternion.get_identity();
      this.Scale = Vector3.get_one();
    }

    public FakeTransform(FakeTransform _In)
    {
      this.Pos = _In.Pos;
      this.Rot = _In.Rot;
      this.Scale = _In.Scale;
    }
  }
}
