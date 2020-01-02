// Decompiled with JetBrains decompiler
// Type: IllusionUtility.SetUtility.TransformScaleEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace IllusionUtility.SetUtility
{
  public static class TransformScaleEx
  {
    public static void SetLocalScaleX(this Transform transform, float x)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, (float) transform.get_localScale().y, (float) transform.get_localScale().z);
      transform.set_localScale(vector3);
    }

    public static void SetLocalScaleY(this Transform transform, float y)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) transform.get_localScale().x, y, (float) transform.get_localScale().z);
      transform.set_localScale(vector3);
    }

    public static void SetLocalScaleZ(this Transform transform, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) transform.get_localScale().x, (float) transform.get_localScale().y, z);
      transform.set_localScale(vector3);
    }

    public static void SetLocalScale(this Transform transform, float x, float y, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, y, z);
      transform.set_localScale(vector3);
    }
  }
}
