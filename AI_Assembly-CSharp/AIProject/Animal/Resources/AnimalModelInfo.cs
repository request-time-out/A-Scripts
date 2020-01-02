// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Resources.AnimalModelInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEx;

namespace AIProject.Animal.Resources
{
  public struct AnimalModelInfo
  {
    public AssetBundleInfo AssetInfo;
    public AnimalShapeInfo EyesShapeInfo;
    public AnimalShapeInfo MouthShapeInfo;

    public AnimalModelInfo(AssetBundleInfo _assetInfo)
    {
      this.AssetInfo = _assetInfo;
      this.EyesShapeInfo = new AnimalShapeInfo();
      this.MouthShapeInfo = new AnimalShapeInfo();
    }

    public AnimalModelInfo(
      AssetBundleInfo _assetInfo,
      string _eyesName,
      int _eyesShapeIndex,
      string _mouthsName,
      int _mouthShapeIndex)
    {
      this.AssetInfo = _assetInfo;
      this.EyesShapeInfo = new AnimalShapeInfo(!_eyesName.IsNullOrEmpty(), _eyesName, _eyesShapeIndex);
      this.MouthShapeInfo = new AnimalShapeInfo(!_mouthsName.IsNullOrEmpty(), _mouthsName, _mouthShapeIndex);
    }

    public AnimalModelInfo(
      AssetBundleInfo _assetInfo,
      bool _eyesShapeEnabled,
      string _eyesName,
      int _eyesShapeIndex,
      bool _mouthShapeEnabled,
      string _mouthName,
      int _mouthShapeIndex)
    {
      this.AssetInfo = _assetInfo;
      this.EyesShapeInfo = new AnimalShapeInfo(_eyesShapeEnabled, _eyesName, _eyesShapeIndex);
      this.MouthShapeInfo = new AnimalShapeInfo(_mouthShapeEnabled, _mouthName, _mouthShapeIndex);
    }

    public AnimalModelInfo(
      AssetBundleInfo _assetInfo,
      AnimalShapeInfo _eyesShapeInfo,
      AnimalShapeInfo _mouthShapeInfo)
    {
      this.AssetInfo = _assetInfo;
      this.EyesShapeInfo = _eyesShapeInfo;
      this.MouthShapeInfo = _mouthShapeInfo;
    }

    public void ClearShapeState()
    {
      this.EyesShapeInfo.ClearState();
      this.MouthShapeInfo.ClearState();
    }

    public void SetShapeState(Transform _transform)
    {
      this.EyesShapeInfo.SetRenderer(_transform, (string) null);
      this.MouthShapeInfo.SetRenderer(_transform, (string) null);
    }
  }
}
