// Decompiled with JetBrains decompiler
// Type: SceneGraphExtractor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SceneGraphExtractor
{
  public static List<string> MemCategories = new List<string>()
  {
    "Texture2D",
    "AnimationClip",
    "Mesh",
    "Font",
    "ParticleSystem",
    "Camera"
  };
  public List<int> GameObjectIDs = new List<int>();
  public Dictionary<string, List<int>> MemObjectIDs = new Dictionary<string, List<int>>();
  public Object m_root;
  public static AdditionalMemObjectExtractor UIWidgetExtractor;

  public SceneGraphExtractor(Object root)
  {
    this.m_root = root;
    foreach (string memCategory in SceneGraphExtractor.MemCategories)
      this.MemObjectIDs[memCategory] = new List<int>();
    GameObject root1 = this.m_root as GameObject;
    if (!Object.op_Inequality((Object) root1, (Object) null))
      return;
    this.ProcessRecursively(root1);
    this.ExtractComponentIDs<Camera>(root1);
    if (SceneGraphExtractor.UIWidgetExtractor != null)
    {
      using (List<Object>.Enumerator enumerator = SceneGraphExtractor.UIWidgetExtractor(root1).GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.CountMemObject(enumerator.Current);
      }
    }
    foreach (MeshFilter componentsInChild in root1.GetComponentsInChildren(typeof (MeshFilter), true))
      this.CountMemObject((Object) componentsInChild.get_sharedMesh());
    foreach (Renderer componentsInChild in root1.GetComponentsInChildren(typeof (Renderer), true))
    {
      if (Object.op_Inequality((Object) componentsInChild.get_sharedMaterial(), (Object) null))
      {
        this.CountMemObject((Object) componentsInChild.get_sharedMaterial());
        using (List<Texture2D>.Enumerator enumerator = ResourceTracker.Instance.GetTexture2DObjsFromMaterial(componentsInChild.get_sharedMaterial()).GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.CountMemObject((Object) enumerator.Current);
        }
      }
    }
    this.ExtractComponentIDs<Animator>(root1);
    this.ExtractComponentIDs<ParticleSystem>(root1);
  }

  private void CountMemObject(Object obj)
  {
    List<int> intList = (List<int>) null;
    if (!Object.op_Inequality(obj, (Object) null) || !this.MemObjectIDs.TryGetValue(((object) obj).GetType().Name, out intList) || (intList == null || intList.Contains(obj.GetInstanceID())))
      return;
    intList.Add(obj.GetInstanceID());
  }

  private void ExtractComponentIDs<T>(GameObject go) where T : Component
  {
    foreach (T componentsInChild in go.GetComponentsInChildren(typeof (T), true))
      this.CountMemObject((Object) (object) componentsInChild);
  }

  public void ProcessRecursively(GameObject obj)
  {
    if (!this.GameObjectIDs.Contains(((Object) obj).GetInstanceID()))
      this.GameObjectIDs.Add(((Object) obj).GetInstanceID());
    for (int index = 0; index < obj.get_transform().get_childCount(); ++index)
    {
      GameObject gameObject = ((Component) obj.get_transform().GetChild(index)).get_gameObject();
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        this.ProcessRecursively(gameObject);
    }
  }
}
