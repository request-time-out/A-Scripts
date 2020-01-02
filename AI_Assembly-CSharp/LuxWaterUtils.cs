// Decompiled with JetBrains decompiler
// Type: LuxWaterUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public static class LuxWaterUtils
{
  public static void GetGersterWavesDescription(
    ref LuxWaterUtils.GersterWavesDescription Description,
    Material WaterMaterial)
  {
    Description.intensity = Vector4.op_Implicit(WaterMaterial.GetVector("_GerstnerVertexIntensity"));
    Description.steepness = WaterMaterial.GetVector("_GSteepness");
    Description.amp = WaterMaterial.GetVector("_GAmplitude");
    Description.freq = WaterMaterial.GetVector("_GFinalFrequency");
    Description.speed = WaterMaterial.GetVector("_GFinalSpeed");
    Description.dirAB = WaterMaterial.GetVector("_GDirectionAB");
    Description.dirCD = WaterMaterial.GetVector("_GDirectionCD");
    Description.secondaryWaveParams = WaterMaterial.GetVector("_GerstnerSecondaryWaves");
  }

  public static Vector3 InternalGetGestnerDisplacement(
    Vector2 xzVtx,
    Vector4 intensity,
    Vector4 steepness,
    Vector4 amp,
    Vector4 freq,
    Vector4 speed,
    Vector4 dirAB,
    Vector4 dirCD,
    float TimeOffset)
  {
    Vector4 vector4_1;
    vector4_1.x = steepness.x * amp.x * dirAB.x;
    vector4_1.y = steepness.x * amp.x * dirAB.y;
    vector4_1.z = steepness.y * amp.y * dirAB.z;
    vector4_1.w = steepness.y * amp.y * dirAB.w;
    Vector4 vector4_2;
    vector4_2.x = steepness.z * amp.z * dirCD.x;
    vector4_2.y = steepness.z * amp.z * dirCD.y;
    vector4_2.z = steepness.w * amp.w * dirCD.z;
    vector4_2.w = steepness.w * amp.w * dirCD.w;
    Vector4 vector4_3;
    vector4_3.x = freq.x * (dirAB.x * xzVtx.x + dirAB.y * xzVtx.y);
    vector4_3.y = freq.y * (dirAB.z * xzVtx.x + dirAB.w * xzVtx.y);
    vector4_3.z = freq.z * (dirCD.x * xzVtx.x + dirCD.y * xzVtx.y);
    vector4_3.w = freq.w * (dirCD.z * xzVtx.x + dirCD.w * xzVtx.y);
    float num = Time.get_timeSinceLevelLoad() + TimeOffset;
    Vector4 vector4_4;
    vector4_4.x = (__Null) ((double) num * speed.x);
    vector4_4.y = (__Null) ((double) num * speed.y);
    vector4_4.z = (__Null) ((double) num * speed.z);
    vector4_4.w = (__Null) ((double) num * speed.w);
    ref Vector4 local1 = ref vector4_3;
    local1.x = local1.x + vector4_4.x;
    ref Vector4 local2 = ref vector4_3;
    local2.y = local2.y + vector4_4.y;
    ref Vector4 local3 = ref vector4_3;
    local3.z = local3.z + vector4_4.z;
    ref Vector4 local4 = ref vector4_3;
    local4.w = local4.w + vector4_4.w;
    Vector4 vector4_5;
    vector4_5.x = (__Null) Math.Cos((double) vector4_3.x);
    vector4_5.y = (__Null) Math.Cos((double) vector4_3.y);
    vector4_5.z = (__Null) Math.Cos((double) vector4_3.z);
    vector4_5.w = (__Null) Math.Cos((double) vector4_3.w);
    Vector4 vector4_6;
    vector4_6.x = (__Null) Math.Sin((double) vector4_3.x);
    vector4_6.y = (__Null) Math.Sin((double) vector4_3.y);
    vector4_6.z = (__Null) Math.Sin((double) vector4_3.z);
    vector4_6.w = (__Null) Math.Sin((double) vector4_3.w);
    Vector3 vector3;
    vector3.x = (vector4_5.x * vector4_1.x + vector4_5.y * vector4_1.z + vector4_5.z * vector4_2.x + vector4_5.w * vector4_2.z) * intensity.x;
    vector3.z = (vector4_5.x * vector4_1.y + vector4_5.y * vector4_1.w + vector4_5.z * vector4_2.y + vector4_5.w * vector4_2.w) * intensity.z;
    vector3.y = (vector4_6.x * amp.x + vector4_6.y * amp.y + vector4_6.z * amp.z + vector4_6.w * amp.w) * intensity.y;
    return vector3;
  }

  public static Vector3 GetGestnerDisplacement(
    Vector3 WorldPosition,
    LuxWaterUtils.GersterWavesDescription Description,
    float TimeOffset)
  {
    Vector2 xzVtx;
    xzVtx.x = WorldPosition.x;
    xzVtx.y = WorldPosition.z;
    Vector3 vector3 = LuxWaterUtils.InternalGetGestnerDisplacement(xzVtx, Vector4.op_Implicit(Description.intensity), Description.steepness, Description.amp, Description.freq, Description.speed, Description.dirAB, Description.dirCD, TimeOffset);
    if (Description.secondaryWaveParams.x > 0.0)
    {
      ref Vector2 local1 = ref xzVtx;
      local1.x = local1.x + vector3.x;
      ref Vector2 local2 = ref xzVtx;
      local2.y = local2.y + vector3.z;
      vector3 = Vector3.op_Addition(vector3, LuxWaterUtils.InternalGetGestnerDisplacement(xzVtx, Vector4.op_Implicit(Description.intensity), Vector4.op_Multiply(Description.steepness, (float) Description.secondaryWaveParams.z), Vector4.op_Multiply(Description.amp, (float) Description.secondaryWaveParams.x), Vector4.op_Multiply(Description.freq, (float) Description.secondaryWaveParams.y), Vector4.op_Multiply(Description.speed, (float) Description.secondaryWaveParams.w), new Vector4((float) Description.dirAB.z, (float) Description.dirAB.w, (float) Description.dirAB.x, (float) Description.dirAB.y), new Vector4((float) Description.dirCD.z, (float) Description.dirCD.w, (float) Description.dirCD.x, (float) Description.dirCD.y), TimeOffset));
    }
    return vector3;
  }

  public static Vector3 GetGestnerDisplacementSingle(
    Vector3 WorldPosition,
    LuxWaterUtils.GersterWavesDescription Description,
    float TimeOffset)
  {
    Vector2 vector2;
    vector2.x = WorldPosition.x;
    vector2.y = WorldPosition.z;
    Vector4 vector4_1;
    vector4_1.x = Description.steepness.x * Description.amp.x * Description.dirAB.x;
    vector4_1.y = Description.steepness.x * Description.amp.x * Description.dirAB.y;
    vector4_1.z = Description.steepness.y * Description.amp.y * Description.dirAB.z;
    vector4_1.w = Description.steepness.y * Description.amp.y * Description.dirAB.w;
    Vector4 vector4_2;
    vector4_2.x = Description.steepness.z * Description.amp.z * Description.dirCD.x;
    vector4_2.y = Description.steepness.z * Description.amp.z * Description.dirCD.y;
    vector4_2.z = Description.steepness.w * Description.amp.w * Description.dirCD.z;
    vector4_2.w = Description.steepness.w * Description.amp.w * Description.dirCD.w;
    Vector4 vector4_3;
    vector4_3.x = Description.freq.x * (Description.dirAB.x * vector2.x + Description.dirAB.y * vector2.y);
    vector4_3.y = Description.freq.y * (Description.dirAB.z * vector2.x + Description.dirAB.w * vector2.y);
    vector4_3.z = Description.freq.z * (Description.dirCD.x * vector2.x + Description.dirCD.y * vector2.y);
    vector4_3.w = Description.freq.w * (Description.dirCD.z * vector2.x + Description.dirCD.w * vector2.y);
    float num = Time.get_timeSinceLevelLoad() + TimeOffset;
    Vector4 vector4_4;
    vector4_4.x = (__Null) ((double) num * Description.speed.x);
    vector4_4.y = (__Null) ((double) num * Description.speed.y);
    vector4_4.z = (__Null) ((double) num * Description.speed.z);
    vector4_4.w = (__Null) ((double) num * Description.speed.w);
    ref Vector4 local1 = ref vector4_3;
    local1.x = local1.x + vector4_4.x;
    ref Vector4 local2 = ref vector4_3;
    local2.y = local2.y + vector4_4.y;
    ref Vector4 local3 = ref vector4_3;
    local3.z = local3.z + vector4_4.z;
    ref Vector4 local4 = ref vector4_3;
    local4.w = local4.w + vector4_4.w;
    Vector4 vector4_5;
    vector4_5.x = (__Null) Math.Cos((double) vector4_3.x);
    vector4_5.y = (__Null) Math.Cos((double) vector4_3.y);
    vector4_5.z = (__Null) Math.Cos((double) vector4_3.z);
    vector4_5.w = (__Null) Math.Cos((double) vector4_3.w);
    Vector4 vector4_6;
    vector4_6.x = (__Null) Math.Sin((double) vector4_3.x);
    vector4_6.y = (__Null) Math.Sin((double) vector4_3.y);
    vector4_6.z = (__Null) Math.Sin((double) vector4_3.z);
    vector4_6.w = (__Null) Math.Sin((double) vector4_3.w);
    Vector3 vector3;
    vector3.x = (vector4_5.x * vector4_1.x + vector4_5.y * vector4_1.z + vector4_5.z * vector4_2.x + vector4_5.w * vector4_2.z) * Description.intensity.x;
    vector3.z = (vector4_5.x * vector4_1.y + vector4_5.y * vector4_1.w + vector4_5.z * vector4_2.y + vector4_5.w * vector4_2.w) * Description.intensity.z;
    vector3.y = (vector4_6.x * Description.amp.x + vector4_6.y * Description.amp.y + vector4_6.z * Description.amp.z + vector4_6.w * Description.amp.w) * Description.intensity.y;
    return vector3;
  }

  public struct GersterWavesDescription
  {
    public Vector3 intensity;
    public Vector4 steepness;
    public Vector4 amp;
    public Vector4 freq;
    public Vector4 speed;
    public Vector4 dirAB;
    public Vector4 dirCD;
    public Vector4 secondaryWaveParams;
  }
}
