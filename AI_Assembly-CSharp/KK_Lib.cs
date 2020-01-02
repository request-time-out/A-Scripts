// Decompiled with JetBrains decompiler
// Type: KK_Lib
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class KK_Lib
{
  public static AssetBundleLoadAssetOperation LoadFile<Type>(
    string _assetBundleName,
    string _assetName,
    string _manifest = "")
  {
    if (!AssetBundleCheck.IsFile(_assetBundleName, _assetName))
    {
      Debug.LogError((object) ("読み込みエラー\r\nassetBundleName：" + _assetBundleName + "\tassetName：" + _assetName));
      return (AssetBundleLoadAssetOperation) null;
    }
    AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(_assetBundleName, _assetName, typeof (Type), !_manifest.IsNullOrEmpty() ? _manifest : (string) null);
    if (loadAssetOperation != null)
      return loadAssetOperation;
    Debug.LogError((object) string.Format("読み込みエラー\r\nassetName：{0}", (object) _assetName));
    return (AssetBundleLoadAssetOperation) null;
  }

  public static bool Range(int _value, int _min, int _max)
  {
    return _min <= _value && _value <= _max;
  }

  public static bool Range(float _value, float _min, float _max)
  {
    return (double) _min <= (double) _value && (double) _value <= (double) _max;
  }

  public static float GCD(float _a, float _b)
  {
    if ((double) _a == 0.0 || (double) _b == 0.0)
      return 0.0f;
    while ((double) _a != (double) _b)
    {
      if ((double) _a > (double) _b)
        _a -= _b;
      else
        _b -= _a;
    }
    return _a;
  }

  public static float LCM(float _a, float _b)
  {
    return (double) _a == 0.0 || (double) _b == 0.0 ? 0.0f : _a / KK_Lib.GCD(_a, _b) * _b;
  }

  public static void Ratio(ref float _outA, ref float _outB, float _a, float _b)
  {
    float num = KK_Lib.GCD(_a, _b);
    _outA = _a / num;
    _outB = _b / num;
  }

  public static int Search(int _value)
  {
    if (_value < 2)
      return 0;
    int num1 = 1;
    for (int index1 = 2; index1 < _value; ++index1)
    {
      int num2 = 0;
      for (int index2 = 2; index2 <= index1; ++index2)
      {
        if (index1 % index2 <= 0)
          ++num2;
      }
      if (num2 == 1)
        ++num1;
    }
    return num1;
  }
}
