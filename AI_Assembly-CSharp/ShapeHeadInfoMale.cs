// Decompiled with JetBrains decompiler
// Type: ShapeHeadInfoMale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ShapeHeadInfoMale : ShapeHeadInfoFemale
{
  public override void InitShapeInfo(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string anmKeyInfoPath,
    string cateInfoPath,
    Transform trfObj)
  {
    base.InitShapeInfo(manifest, assetBundleAnmKey, assetBundleCategory, anmKeyInfoPath, cateInfoPath, trfObj);
  }

  public override void ForceUpdate()
  {
    base.ForceUpdate();
  }

  public override void Update()
  {
    base.Update();
  }

  public override void UpdateAlways()
  {
    base.UpdateAlways();
  }
}
