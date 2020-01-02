// Decompiled with JetBrains decompiler
// Type: Studio.ItemList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class ItemList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Material[] materialsTextMesh;
    private Studio.Item.ListNodePool listNodePool;
    private bool isInit;
    private int group;
    private int category;

    public ItemList()
    {
      base.\u002Ector();
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
        if (value)
          return;
        this.category = -1;
      }
    }

    public void InitList(int _group, int _category)
    {
      this.Init();
      if (this.group == _group && this.category == _category)
        return;
      this.listNodePool.Return();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      foreach (KeyValuePair<int, Info.ItemLoadInfo> keyValuePair in Singleton<Info>.Instance.dicItemLoadInfo[_group][_category])
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: method pointer
        Studio.Anime.ListNode listNode = this.listNodePool.Rent(keyValuePair.Value.name, new UnityAction((object) new ItemList.\u003CInitList\u003Ec__AnonStorey0()
        {
          \u0024this = this,
          no = keyValuePair.Key
        }, __methodptr(\u003C\u003Em__0)), true);
        ItemColorData.ColorData itemColorData = Singleton<Info>.Instance.SafeGetItemColorData(_group, _category, keyValuePair.Key);
        switch (itemColorData == null ? 0 : itemColorData.Count)
        {
          case 1:
            listNode.TextColor = Color.get_red();
            continue;
          case 2:
            listNode.TextColor = Color.get_cyan();
            continue;
          case 3:
            listNode.TextColor = Color.get_green();
            continue;
          case 4:
            listNode.TextColor = Color.get_yellow();
            continue;
          default:
            listNode.TextColor = Color.get_white();
            continue;
        }
      }
      ((Component) this).get_gameObject().SetActiveIfDifferent(true);
      this.group = _group;
      this.category = _category;
    }

    private void OnSelect(int _no)
    {
      Singleton<Studio.Studio>.Instance.AddItem(this.group, this.category, _no);
    }

    private void Init()
    {
      if (this.isInit)
        return;
      this.listNodePool = new Studio.Item.ListNodePool(this.transformRoot, (Studio.Anime.ListNode) this.objectNode.GetComponent<Studio.Anime.ListNode>());
      this.isInit = true;
      this.group = -1;
      this.category = -1;
    }
  }
}
