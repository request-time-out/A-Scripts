// Decompiled with JetBrains decompiler
// Type: Studio.AnimeCategoryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class AnimeCategoryList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private AnimeList animeList;
    private AnimeCategoryList.ListNodePool listNodePool;
    private bool isInit;
    private AnimeGroupList.SEX sex;
    private int group;
    private int select;
    private Dictionary<int, Studio.Anime.ListNode> dicNode;

    public AnimeCategoryList()
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
        if (((Component) this).get_gameObject().get_activeSelf())
          return;
        this.animeList.active = false;
      }
    }

    public void InitList(AnimeGroupList.SEX _sex, int _group)
    {
      this.Init();
      this.listNodePool.Return();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode.Clear();
      foreach (KeyValuePair<int, Info.CategoryInfo> keyValuePair in (IEnumerable<KeyValuePair<int, Info.CategoryInfo>>) Singleton<Info>.Instance.dicAGroupCategory[_group].dicCategory.OrderBy<KeyValuePair<int, Info.CategoryInfo>, int>((Func<KeyValuePair<int, Info.CategoryInfo>, int>) (v => v.Value.sort)))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: method pointer
        Studio.Anime.ListNode listNode = this.listNodePool.Rent(keyValuePair.Value.name, new UnityAction((object) new AnimeCategoryList.\u003CInitList\u003Ec__AnonStorey0()
        {
          \u0024this = this,
          no = keyValuePair.Key
        }, __methodptr(\u003C\u003Em__0)));
        this.dicNode.Add(keyValuePair.Key, listNode);
      }
      this.select = -1;
      this.group = _group;
      this.sex = _sex;
      this.active = true;
      this.animeList.active = false;
    }

    private bool CheckCategory(
      int _group,
      int _category,
      Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>> _dic)
    {
      Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>> dictionary = (Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>) null;
      return _dic.TryGetValue(_group, out dictionary) && dictionary.ContainsKey(_category);
    }

    private void OnSelect(int _no)
    {
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      this.animeList.InitList(this.sex, this.group, _no);
      Studio.Anime.ListNode listNode1 = (Studio.Anime.ListNode) null;
      if (this.dicNode.TryGetValue(select, out listNode1) && Object.op_Inequality((Object) listNode1, (Object) null))
        listNode1.Select = false;
      Studio.Anime.ListNode listNode2 = (Studio.Anime.ListNode) null;
      if (!this.dicNode.TryGetValue(this.select, out listNode2) || !Object.op_Inequality((Object) listNode2, (Object) null))
        return;
      listNode2.Select = true;
    }

    private void Init()
    {
      if (this.isInit)
        return;
      this.listNodePool = new AnimeCategoryList.ListNodePool(this.transformRoot, (Studio.Anime.ListNode) this.objectPrefab.GetComponent<Studio.Anime.ListNode>());
      this.dicNode = new Dictionary<int, Studio.Anime.ListNode>();
      this.isInit = true;
    }

    private class ListNodePool : ObjectPool<Studio.Anime.ListNode>
    {
      private readonly Transform parent;
      private readonly Studio.Anime.ListNode prefab;
      private List<Studio.Anime.ListNode> nodes;

      public ListNodePool(Transform _parent, Studio.Anime.ListNode _prefab)
      {
        this.\u002Ector();
        this.parent = _parent;
        this.prefab = _prefab;
        this.nodes = new List<Studio.Anime.ListNode>();
      }

      protected virtual Studio.Anime.ListNode CreateInstance()
      {
        return (Studio.Anime.ListNode) Object.Instantiate<Studio.Anime.ListNode>((M0) this.prefab, this.parent);
      }

      public Studio.Anime.ListNode Rent(string _text, UnityAction _action)
      {
        Studio.Anime.ListNode listNode = this.Rent();
        ((Component) listNode).get_transform().SetAsLastSibling();
        listNode.Select = false;
        this.nodes.Add(listNode);
        listNode.Text = _text;
        listNode.SetButtonAction(_action);
        return listNode;
      }

      public void Return()
      {
        foreach (Studio.Anime.ListNode node in this.nodes)
          this.Return(node);
        this.nodes.Clear();
      }
    }
  }
}
