// Decompiled with JetBrains decompiler
// Type: IllusionUtility.SetUtility.TransformCopy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace IllusionUtility.SetUtility
{
  public static class TransformCopy
  {
    public static void CopyPosRotScl(this Transform dst, Transform src)
    {
      dst.set_localPosition(src.get_localPosition());
      dst.set_localRotation(src.get_localRotation());
      dst.set_localScale(src.get_localScale());
      dst.set_position(src.get_position());
      dst.set_rotation(src.get_rotation());
    }

    public static void Identity(this Transform transform)
    {
      transform.set_localPosition(Vector3.get_zero());
      transform.set_localRotation(Quaternion.get_identity());
      transform.set_localScale(Vector3.get_one());
      transform.set_position(Vector3.get_zero());
      transform.set_rotation(Quaternion.get_identity());
    }
  }
}
