// Decompiled with JetBrains decompiler
// Type: ShapeInfoBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShapeInfoBase
{
  private AnimationKeyInfo anmKeyInfo = new AnimationKeyInfo();
  private Dictionary<int, List<ShapeInfoBase.CategoryInfo>> dictCategory;
  protected Dictionary<int, ShapeInfoBase.BoneInfo> dictDst;
  protected Dictionary<int, ShapeInfoBase.BoneInfo> dictSrc;

  public ShapeInfoBase()
  {
    this.InitEnd = false;
    this.dictCategory = new Dictionary<int, List<ShapeInfoBase.CategoryInfo>>();
    this.dictDst = new Dictionary<int, ShapeInfoBase.BoneInfo>();
    this.dictSrc = new Dictionary<int, ShapeInfoBase.BoneInfo>();
  }

  public bool InitEnd { get; protected set; }

  public int GetKeyCount()
  {
    return this.anmKeyInfo == null ? 0 : this.anmKeyInfo.GetKeyCount();
  }

  public abstract void InitShapeInfo(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string anmKeyInfoName,
    string cateInfoName,
    Transform trfObj);

  protected void InitShapeInfoBase(
    string manifest,
    string assetBundleAnmKey,
    string assetBundleCategory,
    string anmKeyInfoName,
    string cateInfoName,
    Transform trfObj,
    Dictionary<string, int> dictEnumDst,
    Dictionary<string, int> dictEnumSrc,
    Action<string, string> funcAssetBundleEntry = null)
  {
    this.anmKeyInfo.LoadInfo(manifest, assetBundleAnmKey, anmKeyInfoName, funcAssetBundleEntry);
    this.LoadCategoryInfoList(assetBundleCategory, cateInfoName, dictEnumSrc);
    this.GetDstBoneInfo(trfObj, dictEnumDst);
    this.GetSrcBoneInfo();
  }

  public void ReleaseShapeInfo()
  {
    this.InitEnd = false;
    this.dictCategory.Clear();
    this.dictDst.Clear();
    this.dictSrc.Clear();
  }

  private bool LoadCategoryInfoList(
    string assetBundleName,
    string assetName,
    Dictionary<string, int> dictEnumSrc)
  {
    if (!AssetBundleCheck.IsFile(assetBundleName, assetName))
    {
      Debug.LogError((object) ("読み込みエラー\r\nassetBundleName：" + assetBundleName + "\tassetName：" + assetName));
      return false;
    }
    AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (TextAsset), (string) null);
    if (loadAssetOperation == null)
    {
      Debug.LogError((object) ("読み込みエラー\r\nassetName：" + assetName));
      return false;
    }
    TextAsset asset = loadAssetOperation.GetAsset<TextAsset>();
    if (Object.op_Equality((Object) null, (Object) asset))
    {
      Debug.LogError((object) "ありえない");
      return false;
    }
    string[,] data;
    YS_Assist.GetListString(asset.get_text(), out data);
    int length1 = data.GetLength(0);
    int length2 = data.GetLength(1);
    this.dictCategory.Clear();
    if (length1 != 0 && length2 != 0)
    {
      for (int index = 0; index < length1; ++index)
      {
        ShapeInfoBase.CategoryInfo categoryInfo = new ShapeInfoBase.CategoryInfo();
        categoryInfo.Initialize();
        int key = int.Parse(data[index, 0]);
        categoryInfo.name = data[index, 1];
        int num = 0;
        if (!dictEnumSrc.TryGetValue(categoryInfo.name, out num))
        {
          Debug.LogWarning((object) ("SrcBone【" + categoryInfo.name + "】のIDが見つかりません"));
        }
        else
        {
          categoryInfo.id = num;
          categoryInfo.use[0][0] = !(data[index, 2] == "0");
          categoryInfo.use[0][1] = !(data[index, 3] == "0");
          categoryInfo.use[0][2] = !(data[index, 4] == "0");
          if (categoryInfo.use[0][0] || categoryInfo.use[0][1] || categoryInfo.use[0][2])
            categoryInfo.getflag[0] = true;
          categoryInfo.use[1][0] = !(data[index, 5] == "0");
          categoryInfo.use[1][1] = !(data[index, 6] == "0");
          categoryInfo.use[1][2] = !(data[index, 7] == "0");
          if (categoryInfo.use[1][0] || categoryInfo.use[1][1] || categoryInfo.use[1][2])
            categoryInfo.getflag[1] = true;
          categoryInfo.use[2][0] = !(data[index, 8] == "0");
          categoryInfo.use[2][1] = !(data[index, 9] == "0");
          categoryInfo.use[2][2] = !(data[index, 10] == "0");
          if (categoryInfo.use[2][0] || categoryInfo.use[2][1] || categoryInfo.use[2][2])
            categoryInfo.getflag[2] = true;
          List<ShapeInfoBase.CategoryInfo> categoryInfoList = (List<ShapeInfoBase.CategoryInfo>) null;
          if (!this.dictCategory.TryGetValue(key, out categoryInfoList))
          {
            categoryInfoList = new List<ShapeInfoBase.CategoryInfo>();
            this.dictCategory[key] = categoryInfoList;
          }
          categoryInfoList.Add(categoryInfo);
        }
      }
    }
    AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
    return true;
  }

  private bool GetDstBoneInfo(Transform trfBone, Dictionary<string, int> dictEnumDst)
  {
    this.dictDst.Clear();
    foreach (KeyValuePair<string, int> keyValuePair in dictEnumDst)
    {
      GameObject loop = trfBone.FindLoop(keyValuePair.Key);
      if (Object.op_Inequality((Object) null, (Object) loop))
        this.dictDst[keyValuePair.Value] = new ShapeInfoBase.BoneInfo()
        {
          trfBone = loop.get_transform()
        };
    }
    return true;
  }

  private void GetSrcBoneInfo()
  {
    this.dictSrc.Clear();
    foreach (KeyValuePair<int, List<ShapeInfoBase.CategoryInfo>> keyValuePair in this.dictCategory)
    {
      int count = keyValuePair.Value.Count;
      for (int index = 0; index < count; ++index)
      {
        ShapeInfoBase.BoneInfo boneInfo1 = (ShapeInfoBase.BoneInfo) null;
        if (!this.dictSrc.TryGetValue(keyValuePair.Value[index].id, out boneInfo1))
        {
          ShapeInfoBase.BoneInfo boneInfo2 = new ShapeInfoBase.BoneInfo();
          this.dictSrc[keyValuePair.Value[index].id] = boneInfo2;
        }
      }
    }
  }

  public bool ChangeValue(int category, float value)
  {
    List<ShapeInfoBase.CategoryInfo> categoryInfoList;
    if (this.anmKeyInfo == null || !this.dictCategory.TryGetValue(category, out categoryInfoList))
      return false;
    int count = categoryInfoList.Count;
    string empty = string.Empty;
    for (int index1 = 0; index1 < count; ++index1)
    {
      ShapeInfoBase.BoneInfo boneInfo = (ShapeInfoBase.BoneInfo) null;
      int id = categoryInfoList[index1].id;
      string name = categoryInfoList[index1].name;
      if (this.dictSrc.TryGetValue(id, out boneInfo))
      {
        Vector3[] vector3Array = new Vector3[3];
        for (int index2 = 0; index2 < 3; ++index2)
          vector3Array[index2] = Vector3.get_zero();
        bool[] flag = new bool[3];
        for (int index2 = 0; index2 < 3; ++index2)
          flag[index2] = categoryInfoList[index1].getflag[index2];
        this.anmKeyInfo.GetInfo(name, value, ref vector3Array, flag);
        if (categoryInfoList[index1].use[0][0])
          boneInfo.vctPos.x = vector3Array[0].x;
        if (categoryInfoList[index1].use[0][1])
          boneInfo.vctPos.y = vector3Array[0].y;
        if (categoryInfoList[index1].use[0][2])
          boneInfo.vctPos.z = vector3Array[0].z;
        if (categoryInfoList[index1].use[1][0])
          boneInfo.vctRot.x = vector3Array[1].x;
        if (categoryInfoList[index1].use[1][1])
          boneInfo.vctRot.y = vector3Array[1].y;
        if (categoryInfoList[index1].use[1][2])
          boneInfo.vctRot.z = vector3Array[1].z;
        if (categoryInfoList[index1].use[2][0])
          boneInfo.vctScl.x = vector3Array[2].x;
        if (categoryInfoList[index1].use[2][1])
          boneInfo.vctScl.y = vector3Array[2].y;
        if (categoryInfoList[index1].use[2][2])
          boneInfo.vctScl.z = vector3Array[2].z;
      }
    }
    return true;
  }

  public bool ChangeValue(int category, int key01, int key02, float blend)
  {
    List<ShapeInfoBase.CategoryInfo> categoryInfoList;
    if (this.anmKeyInfo == null || !this.dictCategory.TryGetValue(category, out categoryInfoList))
      return false;
    int count = categoryInfoList.Count;
    string empty = string.Empty;
    for (int index1 = 0; index1 < count; ++index1)
    {
      ShapeInfoBase.BoneInfo boneInfo = (ShapeInfoBase.BoneInfo) null;
      int id = categoryInfoList[index1].id;
      string name = categoryInfoList[index1].name;
      if (this.dictSrc.TryGetValue(id, out boneInfo))
      {
        Vector3[] vector3Array1 = new Vector3[3];
        for (int index2 = 0; index2 < 3; ++index2)
          vector3Array1[index2] = Vector3.get_zero();
        Vector3[] vector3Array2 = new Vector3[3];
        for (int index2 = 0; index2 < 3; ++index2)
          vector3Array2[index2] = Vector3.get_zero();
        bool[] flag = new bool[3];
        for (int index2 = 0; index2 < 3; ++index2)
          flag[index2] = categoryInfoList[index1].getflag[index2];
        if (!this.anmKeyInfo.GetInfo(name, key01, ref vector3Array1, flag) || !this.anmKeyInfo.GetInfo(name, key02, ref vector3Array2, flag))
          return false;
        Vector3 vector3 = Vector3.Lerp(vector3Array1[0], vector3Array2[0], blend);
        if (categoryInfoList[index1].use[0][0])
          boneInfo.vctPos.x = vector3.x;
        if (categoryInfoList[index1].use[0][1])
          boneInfo.vctPos.y = vector3.y;
        if (categoryInfoList[index1].use[0][2])
          boneInfo.vctPos.z = vector3.z;
        vector3.x = (__Null) (double) Mathf.LerpAngle((float) vector3Array1[1].x, (float) vector3Array2[1].x, blend);
        vector3.y = (__Null) (double) Mathf.LerpAngle((float) vector3Array1[1].y, (float) vector3Array2[1].y, blend);
        vector3.z = (__Null) (double) Mathf.LerpAngle((float) vector3Array1[1].z, (float) vector3Array2[1].z, blend);
        if (categoryInfoList[index1].use[1][0])
          boneInfo.vctRot.x = vector3.x;
        if (categoryInfoList[index1].use[1][1])
          boneInfo.vctRot.y = vector3.y;
        if (categoryInfoList[index1].use[1][2])
          boneInfo.vctRot.z = vector3.z;
        vector3 = Vector3.Lerp(vector3Array1[2], vector3Array2[2], blend);
        if (categoryInfoList[index1].use[2][0])
          boneInfo.vctScl.x = vector3.x;
        if (categoryInfoList[index1].use[2][1])
          boneInfo.vctScl.y = vector3.y;
        if (categoryInfoList[index1].use[2][2])
          boneInfo.vctScl.z = vector3.z;
      }
    }
    return true;
  }

  public abstract void ForceUpdate();

  public abstract void Update();

  public abstract void UpdateAlways();

  public class CategoryInfo
  {
    public string name = string.Empty;
    public bool[][] use = new bool[3][];
    public bool[] getflag = new bool[3];
    public int id;

    public void Initialize()
    {
      for (int index = 0; index < 3; ++index)
      {
        this.use[index] = new bool[3];
        this.getflag[index] = false;
      }
    }
  }

  public class BoneInfo
  {
    public Vector3 vctPos = Vector3.get_zero();
    public Vector3 vctRot = Vector3.get_zero();
    public Vector3 vctScl = Vector3.get_one();
    public Transform trfBone;
  }
}
