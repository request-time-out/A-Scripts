// Decompiled with JetBrains decompiler
// Type: FindAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class FindAssist
{
  public Dictionary<string, GameObject> dictObjName { get; private set; }

  public Dictionary<string, List<GameObject>> dictTagName { get; private set; }

  public void Initialize(Transform trf)
  {
    this.dictObjName = new Dictionary<string, GameObject>();
    this.dictTagName = new Dictionary<string, List<GameObject>>();
    this.FindAll(trf);
  }

  private void FindAll(Transform trf)
  {
    if (!this.dictObjName.ContainsKey(((Object) trf).get_name()))
      this.dictObjName[((Object) trf).get_name()] = ((Component) trf).get_gameObject();
    string tag = ((Component) trf).get_tag();
    if (string.Empty != tag)
    {
      List<GameObject> gameObjectList = (List<GameObject>) null;
      if (!this.dictTagName.TryGetValue(tag, out gameObjectList))
      {
        gameObjectList = new List<GameObject>();
        this.dictTagName[tag] = gameObjectList;
      }
      gameObjectList.Add(((Component) trf).get_gameObject());
    }
    for (int index = 0; index < trf.get_childCount(); ++index)
      this.FindAll(trf.GetChild(index));
  }

  public GameObject GetObjectFromName(string objName)
  {
    if (this.dictObjName == null)
      return (GameObject) null;
    GameObject gameObject = (GameObject) null;
    this.dictObjName.TryGetValue(objName, out gameObject);
    return gameObject;
  }

  public Transform GetTransformFromName(string objName)
  {
    if (this.dictObjName == null)
      return (Transform) null;
    GameObject gameObject = (GameObject) null;
    return this.dictObjName.TryGetValue(objName, out gameObject) ? gameObject.get_transform() : (Transform) null;
  }

  public List<GameObject> GetObjectFromTag(string tagName)
  {
    if (this.dictTagName == null)
      return (List<GameObject>) null;
    List<GameObject> gameObjectList = (List<GameObject>) null;
    this.dictTagName.TryGetValue(tagName, out gameObjectList);
    return gameObjectList;
  }
}
