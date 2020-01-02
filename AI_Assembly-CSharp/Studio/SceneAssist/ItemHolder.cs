// Decompiled with JetBrains decompiler
// Type: Studio.SceneAssist.ItemHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio.SceneAssist
{
  public class ItemHolder : MonoBehaviour
  {
    public List<GameObject> listItem;

    public ItemHolder()
    {
      base.\u002Ector();
    }

    public ChaControl CharFemale { get; private set; }

    public AnimatorStateInfo NowState
    {
      get
      {
        return this.CharFemale.getAnimatorStateInfo(0);
      }
    }

    public void PlayAnime(string _name, int _layer = 0)
    {
    }

    public bool LoadItem(string _asset, string _file, string _parent)
    {
      return true;
    }

    public void ReleaseItem(string _name)
    {
      int index = this.listItem.FindIndex((Predicate<GameObject>) (o => ((Object) o).get_name() == _name));
      if (index < 0)
        return;
      Object.Destroy((Object) this.listItem[index]);
      this.listItem.RemoveAt(index);
    }

    public void ReleaseAllItem()
    {
      if (this.listItem == null)
        return;
      for (int index = 0; index < this.listItem.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.listItem[index], (Object) null))
          Object.Destroy((Object) this.listItem[index]);
      }
      this.listItem.Clear();
    }

    public void SetVisible(bool _visible)
    {
      for (int index = 0; index < this.listItem.Count; ++index)
      {
        if (!Object.op_Equality((Object) this.listItem[index], (Object) null) && this.listItem[index].get_activeSelf() != _visible)
          this.listItem[index].SetActive(_visible);
      }
    }

    private void Awake()
    {
      this.CharFemale = (ChaControl) ((Component) this).GetComponent<ChaControl>();
    }

    private void OnDestroy()
    {
      this.ReleaseAllItem();
    }
  }
}
