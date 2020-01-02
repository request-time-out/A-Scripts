// Decompiled with JetBrains decompiler
// Type: Studio.AnimeGroupList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class AnimeGroupList : MonoBehaviour
  {
    public AnimeGroupList.SEX sex;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private AnimeCategoryList animeCategoryList;
    [SerializeField]
    private AnimeList animeList;
    private int select;
    private Dictionary<int, Image> dicNode;
    private bool isInit;

    public AnimeGroupList()
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
        this.animeCategoryList.active = false;
      }
    }

    public void InitList(AnimeGroupList.SEX _sex)
    {
      if (this.isInit)
        return;
      int childCount = this.transformRoot.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode = new Dictionary<int, Image>();
      this.sex = _sex;
      foreach (KeyValuePair<int, Info.GroupInfo> keyValuePair in (IEnumerable<KeyValuePair<int, Info.GroupInfo>>) Singleton<Info>.Instance.dicAGroupCategory.OrderBy<KeyValuePair<int, Info.GroupInfo>, int>((Func<KeyValuePair<int, Info.GroupInfo>, int>) (_v => _v.Value.sort)))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimeGroupList.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new AnimeGroupList.\u003CInitList\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectPrefab);
        if (!gameObject.get_activeSelf())
          gameObject.SetActive(true);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        ListNode component = (ListNode) gameObject.GetComponent<ListNode>();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.no = keyValuePair.Key;
        // ISSUE: method pointer
        component.AddActionToButton(new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        component.text = keyValuePair.Value.name;
        this.dicNode.Add(keyValuePair.Key, component.image);
      }
      this.select = -1;
      if (!((Component) this).get_gameObject().get_activeSelf())
        ((Component) this).get_gameObject().SetActive(true);
      this.animeCategoryList.active = false;
      this.animeList.active = false;
      this.isInit = true;
    }

    private void OnSelect(int _no)
    {
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      this.animeCategoryList.InitList(this.sex, _no);
      Image image1 = (Image) null;
      if (this.dicNode.TryGetValue(select, out image1) && Object.op_Inequality((Object) image1, (Object) null))
        ((Graphic) image1).set_color(Color.get_white());
      Image image2 = (Image) null;
      if (!this.dicNode.TryGetValue(this.select, out image2) || !Object.op_Inequality((Object) image2, (Object) null))
        return;
      ((Graphic) image2).set_color(Color.get_green());
    }

    public enum SEX
    {
      Male,
      Female,
      Unknown,
    }
  }
}
