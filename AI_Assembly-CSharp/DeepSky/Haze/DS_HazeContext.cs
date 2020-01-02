// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeContext
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeepSky.Haze
{
  [AddComponentMenu("")]
  [Serializable]
  public class DS_HazeContext
  {
    [SerializeField]
    private int m_SoloItem = -1;
    [SerializeField]
    public List<DS_HazeContextItem> m_ContextItems;

    public DS_HazeContext()
    {
      this.m_ContextItems = new List<DS_HazeContextItem>();
      this.m_ContextItems.Add(new DS_HazeContextItem()
      {
        m_Name = "Default"
      });
    }

    public int Solo
    {
      get
      {
        return this.m_SoloItem;
      }
    }

    public void DuplicateContextItem(int index)
    {
      if (index < 0 || index >= this.m_ContextItems.Count)
        return;
      DS_HazeContextItem dsHazeContextItem = new DS_HazeContextItem();
      dsHazeContextItem.CopyFrom(this.m_ContextItems[index]);
      dsHazeContextItem.m_Name += "_Copy";
      this.m_ContextItems.Add(dsHazeContextItem);
    }

    public void RemoveContextItem(int index)
    {
      if (index < 0 || index >= this.m_ContextItems.Count || this.m_ContextItems.Count == 1)
        return;
      this.m_ContextItems.RemoveAt(index);
      if (this.m_SoloItem == -1 || this.m_SoloItem != index)
        return;
      this.m_SoloItem = -1;
    }

    public void MoveContextItemUp(int index)
    {
      if (index < 1 || index >= this.m_ContextItems.Count)
        return;
      DS_HazeContextItem contextItem = this.m_ContextItems[index];
      this.m_ContextItems.RemoveAt(index);
      this.m_ContextItems.Insert(index - 1, contextItem);
      if (this.m_SoloItem == -1)
        return;
      if (this.m_SoloItem == index)
      {
        --this.m_SoloItem;
      }
      else
      {
        if (this.m_SoloItem != index - 1)
          return;
        ++this.m_SoloItem;
      }
    }

    public void MoveContextItemDown(int index)
    {
      if (index < 0 || index >= this.m_ContextItems.Count - 1)
        return;
      DS_HazeContextItem contextItem = this.m_ContextItems[index];
      this.m_ContextItems.RemoveAt(index);
      this.m_ContextItems.Insert(index + 1, contextItem);
      if (this.m_SoloItem == -1)
        return;
      if (this.m_SoloItem == index)
      {
        ++this.m_SoloItem;
      }
      else
      {
        if (this.m_SoloItem != index + 1)
          return;
        --this.m_SoloItem;
      }
    }

    public DS_HazeContextItem GetContextItemBlended(float time = -1f)
    {
      DS_HazeContextItem dsHazeContextItem = new DS_HazeContextItem();
      dsHazeContextItem.CopyFrom(this.m_ContextItems[0]);
      if (this.m_ContextItems.Count == 1)
        return dsHazeContextItem;
      time = Mathf.Clamp01(time);
      for (int index = 1; index < this.m_ContextItems.Count; ++index)
      {
        float dt = this.m_ContextItems[index].m_Weight.Evaluate(time);
        dsHazeContextItem.Lerp(this.m_ContextItems[index], dt);
      }
      return dsHazeContextItem;
    }

    public DS_HazeContextItem GetItemAtIndex(int index)
    {
      return index < 0 || index >= this.m_ContextItems.Count ? (DS_HazeContextItem) null : this.m_ContextItems[index];
    }

    public void CopyFrom(DS_HazeContext other)
    {
      if (this.m_ContextItems.Count > 0)
        this.m_ContextItems.Clear();
      for (int index = 0; index < other.m_ContextItems.Count; ++index)
      {
        DS_HazeContextItem dsHazeContextItem = new DS_HazeContextItem();
        dsHazeContextItem.CopyFrom(other.m_ContextItems[index]);
        this.m_ContextItems.Add(dsHazeContextItem);
      }
    }

    public DS_HazeContextAsset GetContextAsset()
    {
      DS_HazeContextAsset instance = (DS_HazeContextAsset) ScriptableObject.CreateInstance<DS_HazeContextAsset>();
      instance.Context.CopyFrom(this);
      instance.Context.m_SoloItem = -1;
      return instance;
    }

    public string[] GetItemNames()
    {
      string[] strArray = new string[this.m_ContextItems.Count];
      for (int index = 0; index < this.m_ContextItems.Count; ++index)
        strArray[index] = this.m_ContextItems[index].m_Name;
      return strArray;
    }
  }
}
