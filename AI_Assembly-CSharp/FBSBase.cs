// Decompiled with JetBrains decompiler
// Type: FBSBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using FBSAssist;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FBSBase
{
  protected Dictionary<int, float> dictBackFace = new Dictionary<int, float>();
  protected Dictionary<int, float> dictNowFace = new Dictionary<int, float>();
  [Range(0.0f, 1f)]
  public float OpenMax = 1f;
  [Range(-0.1f, 1f)]
  public float FixedRate = -0.1f;
  private float correctOpenMax = -1f;
  public FBSTargetInfo[] FBSTarget;
  protected float openRate;
  [Range(0.0f, 1f)]
  public float OpenMin;
  protected TimeProgressCtrl blendTimeCtrl;

  public bool Init()
  {
    this.blendTimeCtrl = new TimeProgressCtrl(0.15f);
    this.blendTimeCtrl.End();
    for (int index = 0; index < this.FBSTarget.Length; ++index)
      this.FBSTarget[index].SetSkinnedMeshRenderer();
    this.dictBackFace.Clear();
    this.dictBackFace[0] = 1f;
    this.dictNowFace.Clear();
    this.dictNowFace[0] = 1f;
    return true;
  }

  public void SetOpenRateForce(float rate)
  {
    this.openRate = rate;
  }

  public int GetMaxPtn()
  {
    return this.FBSTarget.Length == 0 ? 0 : this.FBSTarget[0].PtnSet.Length;
  }

  public void ChangePtn(int ptn, bool blend)
  {
    if (this.GetMaxPtn() <= ptn)
    {
      Debug.LogError((object) "パターンが範囲外");
    }
    else
    {
      if (this.dictNowFace.Count == 1 && this.dictNowFace.ContainsKey(ptn) && (double) this.dictNowFace[ptn] == 1.0)
        return;
      this.ChangeFace(new Dictionary<int, float>()
      {
        [ptn] = 1f
      }, blend);
    }
  }

  public void ChangeFace(Dictionary<int, float> dictFace, bool blend)
  {
    bool flag = false;
    byte num1 = 0;
    float num2 = 0.0f;
    foreach (FBSTargetInfo fbsTargetInfo in this.FBSTarget)
    {
      SkinnedMeshRenderer skinnedMeshRenderer = fbsTargetInfo.GetSkinnedMeshRenderer();
      foreach (int key in dictFace.Keys)
      {
        if (skinnedMeshRenderer.get_sharedMesh().get_blendShapeCount() <= fbsTargetInfo.PtnSet[key].Close)
        {
          num1 = (byte) 1;
          break;
        }
        if (skinnedMeshRenderer.get_sharedMesh().get_blendShapeCount() <= fbsTargetInfo.PtnSet[key].Open)
        {
          num1 = (byte) 1;
          break;
        }
        num2 += dictFace[key];
      }
      if (num1 == (byte) 0)
      {
        if (!flag && (double) num2 > 1.0)
        {
          num1 = (byte) 2;
          break;
        }
        flag = true;
      }
      else
        break;
    }
    if (num1 == (byte) 1)
      Debug.LogError((object) "ブレンドシェイプ番号が範囲外");
    else if (num1 == (byte) 2)
    {
      Debug.LogError((object) "合成の割合が１００％を超えています");
    }
    else
    {
      this.dictBackFace.Clear();
      foreach (int key in this.dictNowFace.Keys)
        this.dictBackFace[key] = this.dictNowFace[key];
      this.dictNowFace.Clear();
      foreach (int key in dictFace.Keys)
        this.dictNowFace[key] = dictFace[key];
      if (!blend)
        this.blendTimeCtrl.End();
      else
        this.blendTimeCtrl.Start();
    }
  }

  public void SetFixedRate(float value)
  {
    this.FixedRate = value;
  }

  public void SetCorrectOpenMax(float value)
  {
    this.correctOpenMax = value;
  }

  public void CalculateBlendShape()
  {
    if (this.FBSTarget.Length == 0)
      return;
    float num1 = Mathf.Lerp(this.OpenMin, (double) this.correctOpenMax >= 0.0 ? this.correctOpenMax : this.OpenMax, this.openRate);
    if (0.0 <= (double) this.FixedRate)
      num1 = this.FixedRate;
    float num2 = 0.0f;
    if (this.blendTimeCtrl != null)
      num2 = this.blendTimeCtrl.Calculate();
    foreach (FBSTargetInfo fbsTargetInfo in this.FBSTarget)
    {
      SkinnedMeshRenderer skinnedMeshRenderer = fbsTargetInfo.GetSkinnedMeshRenderer();
      Dictionary<int, float> toRelease = DictionaryPool<int, float>.Get();
      for (int index = 0; index < fbsTargetInfo.PtnSet.Length; ++index)
      {
        toRelease[fbsTargetInfo.PtnSet[index].Close] = 0.0f;
        toRelease[fbsTargetInfo.PtnSet[index].Open] = 0.0f;
      }
      int num3 = (int) Mathf.Clamp(num1 * 100f, 0.0f, 100f);
      if ((double) num2 != 1.0)
      {
        foreach (int key in this.dictBackFace.Keys)
        {
          toRelease[fbsTargetInfo.PtnSet[key].Close] = toRelease[fbsTargetInfo.PtnSet[key].Close] + (float) ((double) this.dictBackFace[key] * (double) (100 - num3) * (1.0 - (double) num2));
          toRelease[fbsTargetInfo.PtnSet[key].Open] = toRelease[fbsTargetInfo.PtnSet[key].Open] + (float) ((double) this.dictBackFace[key] * (double) num3 * (1.0 - (double) num2));
        }
      }
      foreach (int key in this.dictNowFace.Keys)
      {
        toRelease[fbsTargetInfo.PtnSet[key].Close] = toRelease[fbsTargetInfo.PtnSet[key].Close] + this.dictNowFace[key] * (float) (100 - num3) * num2;
        toRelease[fbsTargetInfo.PtnSet[key].Open] = toRelease[fbsTargetInfo.PtnSet[key].Open] + this.dictNowFace[key] * (float) num3 * num2;
      }
      foreach (KeyValuePair<int, float> keyValuePair in toRelease)
      {
        if (keyValuePair.Key == -1)
          Debug.LogError((object) (((Object) skinnedMeshRenderer.get_sharedMesh()).get_name() + ": 多分、名前が間違ったデータがある"));
        else
          skinnedMeshRenderer.SetBlendShapeWeight(keyValuePair.Key, keyValuePair.Value);
      }
      DictionaryPool<int, float>.Release(toRelease);
    }
  }
}
