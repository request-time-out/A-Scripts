// Decompiled with JetBrains decompiler
// Type: AssignedAnotherWeights
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedAnotherWeights
{
  public AssignedAnotherWeights()
  {
    this.dictBone = new Dictionary<string, GameObject>();
  }

  public Dictionary<string, GameObject> dictBone { get; private set; }

  public void Release()
  {
    this.dictBone.Clear();
  }

  public void CreateBoneList(GameObject obj, string name)
  {
    this.dictBone.Clear();
    this.CreateBoneListLoop(obj, name);
  }

  public void CreateBoneListMultiple(GameObject obj, params string[] names)
  {
    this.dictBone.Clear();
    foreach (string name in names)
      this.CreateBoneListLoop(obj, name);
  }

  public void CreateBoneListLoop(GameObject obj, string name)
  {
    if ((string.Compare(((Object) obj).get_name(), 0, name, 0, name.Length) == 0 || string.Empty == name) && !this.dictBone.ContainsKey(((Object) obj).get_name()))
      this.dictBone[((Object) obj).get_name()] = obj;
    for (int index = 0; index < obj.get_transform().get_childCount(); ++index)
      this.CreateBoneListLoop(((Component) obj.get_transform().GetChild(index)).get_gameObject(), name);
  }

  public void CreateBoneList(GameObject obj, string assetBundleName, string assetName)
  {
    this.dictBone.Clear();
    if (!AssetBundleCheck.IsFile(assetBundleName, assetName))
    {
      Debug.LogWarning((object) ("読み込みエラー\r\nassetBundleName：" + assetBundleName + "\tassetName：" + assetName));
    }
    else
    {
      AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (TextAsset), (string) null);
      if (loadAssetOperation.IsEmpty())
      {
        Debug.LogError((object) ("読み込みエラー\r\nassetName：" + assetName));
      }
      else
      {
        string[,] data;
        YS_Assist.GetListString(loadAssetOperation.GetAsset<TextAsset>().get_text(), out data);
        int length1 = data.GetLength(0);
        int length2 = data.GetLength(1);
        if (length1 != 0 && length2 != 0)
        {
          for (int index = 0; index < length1; ++index)
          {
            GameObject loop = obj.get_transform().FindLoop(data[index, 0]);
            if (Object.op_Implicit((Object) loop))
              this.dictBone[data[index, 0]] = loop;
          }
        }
        AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
      }
    }
  }

  public void CreateBoneList(GameObject obj, string[] bonename)
  {
    this.dictBone.Clear();
    for (int index = 0; index < bonename.Length; ++index)
    {
      GameObject loop = obj.get_transform().FindLoop(bonename[index]);
      if (Object.op_Implicit((Object) loop))
        this.dictBone[bonename[index]] = loop;
    }
  }

  public void AssignedWeights(GameObject obj, string delTopName, Transform rootBone = null)
  {
    if (this.dictBone == null || this.dictBone.Count == 0 || Object.op_Equality((Object) null, (Object) obj))
      return;
    this.AssignedWeightsLoop(obj.get_transform(), rootBone);
    GameObject loop = obj.get_transform().FindLoop(delTopName);
    if (!Object.op_Implicit((Object) loop))
      return;
    loop.get_transform().SetParent((Transform) null);
    Object.Destroy((Object) loop);
  }

  private void AssignedWeightsLoop(Transform t, Transform rootBone = null)
  {
    SkinnedMeshRenderer component = (SkinnedMeshRenderer) ((Component) t).GetComponent<SkinnedMeshRenderer>();
    if (Object.op_Implicit((Object) component))
    {
      int length = component.get_bones().Length;
      Transform[] transformArray = new Transform[length];
      GameObject gameObject = (GameObject) null;
      for (int index = 0; index < length; ++index)
      {
        if (this.dictBone.TryGetValue(((Object) component.get_bones()[index]).get_name(), out gameObject))
          transformArray[index] = gameObject.get_transform();
      }
      component.set_bones(transformArray);
      if (Object.op_Implicit((Object) rootBone))
        component.set_rootBone(rootBone);
      else if (Object.op_Implicit((Object) component.get_rootBone()) && this.dictBone.TryGetValue(((Object) component.get_rootBone()).get_name(), out gameObject))
        component.set_rootBone(gameObject.get_transform());
    }
    IEnumerator enumerator = ((Component) t).get_gameObject().get_transform().GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        this.AssignedWeightsLoop((Transform) enumerator.Current, rootBone);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void AssignedWeightsAndSetBounds(
    GameObject obj,
    string delTopName,
    Bounds bounds,
    Transform rootBone = null)
  {
    if (this.dictBone == null || this.dictBone.Count == 0 || Object.op_Equality((Object) null, (Object) obj))
      return;
    this.AssignedWeightsAndSetBoundsLoop(obj.get_transform(), bounds, rootBone);
    GameObject loop = obj.get_transform().FindLoop(delTopName);
    if (!Object.op_Implicit((Object) loop))
      return;
    loop.get_transform().SetParent((Transform) null);
    Object.Destroy((Object) loop);
  }

  private void AssignedWeightsAndSetBoundsLoop(Transform t, Bounds bounds, Transform rootBone = null)
  {
    SkinnedMeshRenderer component1 = (SkinnedMeshRenderer) ((Component) t).GetComponent<SkinnedMeshRenderer>();
    if (Object.op_Implicit((Object) component1))
    {
      int length = component1.get_bones().Length;
      Transform[] transformArray = new Transform[length];
      GameObject gameObject = (GameObject) null;
      for (int index = 0; index < length; ++index)
      {
        if (this.dictBone.TryGetValue(((Object) component1.get_bones()[index]).get_name(), out gameObject))
          transformArray[index] = gameObject.get_transform();
      }
      component1.set_bones(transformArray);
      component1.set_localBounds(bounds);
      Cloth component2 = (Cloth) ((Component) component1).get_gameObject().GetComponent<Cloth>();
      if (Object.op_Implicit((Object) rootBone) && Object.op_Equality((Object) null, (Object) component2))
        component1.set_rootBone(rootBone);
      else if (Object.op_Implicit((Object) component1.get_rootBone()) && this.dictBone.TryGetValue(((Object) component1.get_rootBone()).get_name(), out gameObject))
        component1.set_rootBone(gameObject.get_transform());
    }
    IEnumerator enumerator = ((Component) t).get_gameObject().get_transform().GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        this.AssignedWeightsAndSetBoundsLoop((Transform) enumerator.Current, bounds, rootBone);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }
}
