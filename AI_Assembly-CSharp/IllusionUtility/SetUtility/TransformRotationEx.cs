// Decompiled with JetBrains decompiler
// Type: IllusionUtility.SetUtility.TransformRotationEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace IllusionUtility.SetUtility
{
  public static class TransformRotationEx
  {
    public static void SetRotationX(this Transform transform, float x)
    {
      Vector3 vector3;
      ref Vector3 local = ref vector3;
      double num = (double) x;
      Quaternion rotation1 = transform.get_rotation();
      // ISSUE: variable of the null type
      __Null y = ((Quaternion) ref rotation1).get_eulerAngles().y;
      Quaternion rotation2 = transform.get_rotation();
      // ISSUE: variable of the null type
      __Null z = ((Quaternion) ref rotation2).get_eulerAngles().z;
      ((Vector3) ref local).\u002Ector((float) num, (float) y, (float) z);
      transform.set_rotation(Quaternion.Euler(vector3));
    }

    public static void SetRotationY(this Transform transform, float y)
    {
      Vector3 vector3;
      ref Vector3 local = ref vector3;
      Quaternion rotation1 = transform.get_rotation();
      // ISSUE: variable of the null type
      __Null x = ((Quaternion) ref rotation1).get_eulerAngles().x;
      double num = (double) y;
      Quaternion rotation2 = transform.get_rotation();
      // ISSUE: variable of the null type
      __Null z = ((Quaternion) ref rotation2).get_eulerAngles().z;
      ((Vector3) ref local).\u002Ector((float) x, (float) num, (float) z);
      transform.set_rotation(Quaternion.Euler(vector3));
    }

    public static void SetRotationZ(this Transform transform, float z)
    {
      Vector3 vector3;
      ref Vector3 local = ref vector3;
      Quaternion rotation1 = transform.get_rotation();
      // ISSUE: variable of the null type
      __Null x = ((Quaternion) ref rotation1).get_eulerAngles().x;
      Quaternion rotation2 = transform.get_rotation();
      // ISSUE: variable of the null type
      __Null y = ((Quaternion) ref rotation2).get_eulerAngles().y;
      double num = (double) z;
      ((Vector3) ref local).\u002Ector((float) x, (float) y, (float) num);
      transform.set_rotation(Quaternion.Euler(vector3));
    }

    public static void SetRotation(this Transform transform, float x, float y, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, y, z);
      transform.set_rotation(Quaternion.Euler(vector3));
    }

    public static void SetLocalRotationX(this Transform transform, float x)
    {
      Vector3 vector3;
      ref Vector3 local = ref vector3;
      double num = (double) x;
      Quaternion localRotation1 = transform.get_localRotation();
      // ISSUE: variable of the null type
      __Null y = ((Quaternion) ref localRotation1).get_eulerAngles().y;
      Quaternion localRotation2 = transform.get_localRotation();
      // ISSUE: variable of the null type
      __Null z = ((Quaternion) ref localRotation2).get_eulerAngles().z;
      ((Vector3) ref local).\u002Ector((float) num, (float) y, (float) z);
      transform.set_localRotation(Quaternion.Euler(vector3));
    }

    public static void SetLocalRotationY(this Transform transform, float y)
    {
      Vector3 vector3;
      ref Vector3 local = ref vector3;
      Quaternion localRotation1 = transform.get_localRotation();
      // ISSUE: variable of the null type
      __Null x = ((Quaternion) ref localRotation1).get_eulerAngles().x;
      double num = (double) y;
      Quaternion localRotation2 = transform.get_localRotation();
      // ISSUE: variable of the null type
      __Null z = ((Quaternion) ref localRotation2).get_eulerAngles().z;
      ((Vector3) ref local).\u002Ector((float) x, (float) num, (float) z);
      transform.set_localRotation(Quaternion.Euler(vector3));
    }

    public static void SetLocalRotationZ(this Transform transform, float z)
    {
      Vector3 vector3;
      ref Vector3 local = ref vector3;
      Quaternion localRotation1 = transform.get_localRotation();
      // ISSUE: variable of the null type
      __Null x = ((Quaternion) ref localRotation1).get_eulerAngles().x;
      Quaternion localRotation2 = transform.get_localRotation();
      // ISSUE: variable of the null type
      __Null y = ((Quaternion) ref localRotation2).get_eulerAngles().y;
      double num = (double) z;
      ((Vector3) ref local).\u002Ector((float) x, (float) y, (float) num);
      transform.set_localRotation(Quaternion.Euler(vector3));
    }

    public static void SetLocalRotation(this Transform transform, float x, float y, float z)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(x, y, z);
      transform.set_localRotation(Quaternion.Euler(vector3));
    }
  }
}
