// Decompiled with JetBrains decompiler
// Type: IllusionUtility.SetUtility.TransformPositionEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace IllusionUtility.SetUtility
{
  public static class TransformPositionEx
  {
    public static void SetPositionX(this Transform transform, float x)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, (float) transform.get_position().y, (float) transform.get_position().z);
      transform.set_position(vector3);
    }

    public static void SetPositionY(this Transform transform, float y)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) transform.get_position().x, y, (float) transform.get_position().z);
      transform.set_position(vector3);
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) transform.get_position().x, (float) transform.get_position().y, z);
      transform.set_position(vector3);
    }

    public static void SetPosition(this Transform transform, float x, float y, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, y, z);
      transform.set_position(vector3);
    }

    public static void SetLocalPositionX(this Transform transform, float x)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, (float) transform.get_localPosition().y, (float) transform.get_localPosition().z);
      transform.set_localPosition(vector3);
    }

    public static void SetLocalPositionY(this Transform transform, float y)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) transform.get_localPosition().x, y, (float) transform.get_localPosition().z);
      transform.set_localPosition(vector3);
    }

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) transform.get_localPosition().x, (float) transform.get_localPosition().y, z);
      transform.set_localPosition(vector3);
    }

    public static void SetLocalPosition(this Transform transform, float x, float y, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, y, z);
      transform.set_localPosition(vector3);
    }
  }
}
