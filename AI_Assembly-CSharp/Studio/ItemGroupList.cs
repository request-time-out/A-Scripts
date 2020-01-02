// Decompiled with JetBrains decompiler
// Type: Studio.ItemGroupList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class ItemGroupList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private ItemCategoryList itemCategoryList;
    [SerializeField]
    private ItemList itemList;
    private int select;
    private Dictionary<int, Image> dicNode;

    public ItemGroupList()
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
        this.itemCategoryList.active = false;
      }
    }

    public void InitList()
    {
      int childCount = this.transformRoot.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode = new Dictionary<int, Image>();
      foreach (KeyValuePair<int, Info.GroupInfo> keyValuePair in Singleton<Info>.Instance.dicItemGroupCategory)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ItemGroupList.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new ItemGroupList.\u003CInitList\u003Ec__AnonStorey0();
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
        this.dicNode.Add(keyValuePair.Key, (Image) gameObject.GetComponent<Image>());
      }
      this.select = -1;
      if (!((Component) this).get_gameObject().get_activeSelf())
        ((Component) this).get_gameObject().SetActive(true);
      this.itemCategoryList.active = false;
      this.itemList.active = false;
    }

    private void OnSelect(int _no)
    {
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      this.itemCategoryList.InitList(_no);
      Image image1 = (Image) null;
      if (this.dicNode.TryGetValue(select, out image1) && Object.op_Inequality((Object) image1, (Object) null))
        ((Graphic) image1).set_color(Color.get_white());
      Image image2 = (Image) null;
      if (!this.dicNode.TryGetValue(this.select, out image2) || !Object.op_Inequality((Object) image2, (Object) null))
        return;
      ((Graphic) image2).set_color(Color.get_green());
    }

    private void Start()
    {
      this.InitList();
    }
  }
}
