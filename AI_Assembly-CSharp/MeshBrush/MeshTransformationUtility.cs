// Decompiled with JetBrains decompiler
// Type: MeshBrush.MeshTransformationUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace MeshBrush
{
  public static class MeshTransformationUtility
  {
    public static void ApplyRandomScale(Transform targetTransform, Vector2 range)
    {
      float num = Mathf.Abs(Random.Range((float) range.x, (float) range.y));
      targetTransform.set_localScale(new Vector3(num, num, num));
    }

    public static void ApplyRandomScale(Transform targetTransform, Vector4 scaleRanges)
    {
      float num1 = Random.Range((float) scaleRanges.x, (float) scaleRanges.y);
      float num2 = Random.Range((float) scaleRanges.z, (float) scaleRanges.w);
      Transform transform = targetTransform;
      Vector3 vector3_1 = (Vector3) null;
      vector3_1.x = (__Null) (double) Mathf.Abs(num1);
      vector3_1.y = (__Null) (double) Mathf.Abs(num2);
      vector3_1.z = (__Null) (double) Mathf.Abs(num1);
      Vector3 vector3_2 = vector3_1;
      transform.set_localScale(vector3_2);
    }

    public static void ApplyRandomScale(
      Transform targetTransform,
      Vector2 rangeX,
      Vector2 rangeY,
      Vector2 rangeZ)
    {
      Transform transform = targetTransform;
      Vector3 vector3_1 = (Vector3) null;
      vector3_1.x = (__Null) (double) Mathf.Abs(Random.Range((float) rangeX.x, (float) rangeX.y));
      vector3_1.y = (__Null) (double) Mathf.Abs(Random.Range((float) rangeY.x, (float) rangeY.y));
      vector3_1.z = (__Null) (double) Mathf.Abs(Random.Range((float) rangeZ.x, (float) rangeZ.y));
      Vector3 vector3_2 = vector3_1;
      transform.set_localScale(vector3_2);
    }

    public static void AddConstantScale(Transform targetTransform, Vector2 range)
    {
      float num = Random.Range((float) range.x, (float) range.y);
      Vector3 vector3 = Vector3.op_Addition(targetTransform.get_localScale(), new Vector3(num, num, num));
      vector3.x = (__Null) (double) Mathf.Abs((float) vector3.x);
      vector3.y = (__Null) (double) Mathf.Abs((float) vector3.y);
      vector3.z = (__Null) (double) Mathf.Abs((float) vector3.z);
      targetTransform.set_localScale(vector3);
    }

    public static void AddConstantScale(Transform targetTransform, float x, float y, float z)
    {
      Vector3 localScale = targetTransform.get_localScale();
      Vector3 vector3_1 = (Vector3) null;
      vector3_1.x = (__Null) (double) Mathf.Abs(x);
      vector3_1.y = (__Null) (double) Mathf.Abs(y);
      vector3_1.z = (__Null) (double) Mathf.Abs(z);
      Vector3 vector3_2 = vector3_1;
      Vector3 vector3_3 = Vector3.op_Addition(localScale, vector3_2);
      targetTransform.set_localScale(vector3_3);
    }

    public static void ApplyRandomRotation(
      Transform targetTransform,
      float randomRotationIntensityPercentage)
    {
      float num = Random.Range(0.0f, 3.6f * randomRotationIntensityPercentage);
      targetTransform.Rotate(new Vector3(0.0f, num, 0.0f));
    }

    public static void ApplyMeshOffset(Transform targetTransform, float offset, Vector3 direction)
    {
      targetTransform.Translate(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref direction).get_normalized(), offset), 0.01f), (Space) 0);
    }
  }
}
