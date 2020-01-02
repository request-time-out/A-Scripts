// Decompiled with JetBrains decompiler
// Type: Housing.Add.MaterialUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Housing.Add
{
  public class MaterialUI : MonoBehaviour
  {
    [SerializeField]
    private MaterialUI.MaterialInfo[] materialInfos;

    public MaterialUI()
    {
      base.\u002Ector();
    }

    public MaterialUI.MaterialInfo[] MaterialInfos
    {
      get
      {
        return this.materialInfos;
      }
    }

    public bool UpdateUI(Manager.Housing.LoadInfo _loadInfo)
    {
      bool flag = true;
      if (((IList<Manager.Housing.RequiredMaterial>) _loadInfo.requiredMaterials).IsNullOrEmpty<Manager.Housing.RequiredMaterial>())
      {
        foreach (MaterialUI.MaterialInfo materialInfo in this.materialInfos)
          materialInfo.Active = false;
        return true;
      }
      for (int index = 0; index < this.materialInfos.Length; ++index)
      {
        Manager.Housing.RequiredMaterial requiredMaterial = _loadInfo.requiredMaterials.SafeGet<Manager.Housing.RequiredMaterial>(index);
        if (requiredMaterial == null)
        {
          this.materialInfos[index].Active = false;
        }
        else
        {
          StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(requiredMaterial.category, requiredMaterial.no);
          if (stuffItemInfo == null)
          {
            this.materialInfos[index].Active = false;
          }
          else
          {
            int _num = 0;
            StuffItem stuffItem1 = new StuffItem(requiredMaterial.category, requiredMaterial.no, 0);
            StuffItem stuffItem2 = Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList.FindItem(stuffItem1);
            if (stuffItem2 != null)
              _num += stuffItem2.Count;
            StuffItem stuffItem3 = Singleton<Game>.Instance.Environment.ItemListInStorage.FindItem(stuffItem1);
            if (stuffItem3 != null)
              _num += stuffItem3.Count;
            flag &= this.materialInfos[index].Set(stuffItemInfo.Name, _num, requiredMaterial.num);
            this.materialInfos[index].Active = true;
          }
        }
      }
      return flag;
    }

    [Serializable]
    public class MaterialInfo
    {
      public GameObject gameObject;
      public Text textName;
      public Text textNum;
      public Text textNeed;

      public bool Active
      {
        set
        {
          this.gameObject.SetActiveIfDifferent(value);
        }
      }

      public bool Set(string _name, int _num, int _need)
      {
        bool flag = _num < _need;
        this.gameObject.SetActiveIfDifferent(true);
        this.textName.set_text(_name);
        this.textNum.set_text(!flag ? _num.ToString() : string.Format("<color=red>{0}</color>", (object) _num));
        this.textNeed.set_text(string.Format("/{0}", (object) _need));
        return !flag;
      }
    }
  }
}
