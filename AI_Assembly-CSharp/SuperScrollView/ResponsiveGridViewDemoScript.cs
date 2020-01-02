// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ResponsiveGridViewDemoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ResponsiveGridViewDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;
    private int mItemCountPerRow;
    private int mListItemTotalCount;
    public DragChangSizeScript mDragChangSizeScript;

    public ResponsiveGridViewDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
      int itemTotalCount = this.mListItemTotalCount / this.mItemCountPerRow;
      if (this.mListItemTotalCount % this.mItemCountPerRow > 0)
        ++itemTotalCount;
      this.mLoopListView.InitListView(itemTotalCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mSetCountButton = (Button) GameObject.Find("ButtonPanel/buttonGroup1/SetCountButton").GetComponent<Button>();
      this.mScrollToButton = (Button) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
      this.mAddItemButton = (Button) GameObject.Find("ButtonPanel/buttonGroup3/AddItemButton").GetComponent<Button>();
      this.mSetCountInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup1/SetCountInputField").GetComponent<InputField>();
      this.mScrollToInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
      this.mAddItemInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup3/AddItemInputField").GetComponent<InputField>();
      // ISSUE: method pointer
      ((UnityEvent) this.mScrollToButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnJumpBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mAddItemButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnAddItemBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mSetCountButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnSetItemCountBtnClicked)));
      this.mBackButton = (Button) GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
      this.mDragChangSizeScript.mOnDragEndAction = new Action(this.OnViewPortSizeChanged);
      this.OnViewPortSizeChanged();
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private void SetListItemTotalCount(int count)
    {
      this.mListItemTotalCount = count;
      if (this.mListItemTotalCount < 0)
        this.mListItemTotalCount = 0;
      if (this.mListItemTotalCount > DataSourceMgr.Get.TotalItemCount)
        this.mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
      int itemCount = this.mListItemTotalCount / this.mItemCountPerRow;
      if (this.mListItemTotalCount % this.mItemCountPerRow > 0)
        ++itemCount;
      this.mLoopListView.SetListItemCount(itemCount, false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void UpdateItemPrefab()
    {
      GameObject mItemPrefab = this.mLoopListView.GetItemPrefabConfData("ItemPrefab1").mItemPrefab;
      RectTransform component1 = (RectTransform) mItemPrefab.GetComponent<RectTransform>();
      ListItem6 component2 = (ListItem6) mItemPrefab.GetComponent<ListItem6>();
      float viewPortWidth = this.mLoopListView.ViewPortWidth;
      int count = component2.mItemList.Count;
      GameObject gameObject1 = ((Component) component2.mItemList[0]).get_gameObject();
      Rect rect = ((RectTransform) gameObject1.GetComponent<RectTransform>()).get_rect();
      float width = ((Rect) ref rect).get_width();
      int num1 = Mathf.FloorToInt(viewPortWidth / width);
      if (num1 == 0)
        num1 = 1;
      this.mItemCountPerRow = num1;
      float num2 = (viewPortWidth - width * (float) num1) / (float) (num1 + 1);
      if ((double) num2 < 0.0)
        num2 = 0.0f;
      component1.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, viewPortWidth);
      if (num1 > count)
      {
        int num3 = num1 - count;
        for (int index = 0; index < num3; ++index)
        {
          GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, Vector3.get_zero(), Quaternion.get_identity(), (Transform) component1);
          RectTransform component3 = (RectTransform) gameObject2.GetComponent<RectTransform>();
          ((Transform) component3).set_localScale(Vector3.get_one());
          component3.set_anchoredPosition3D(Vector3.get_zero());
          ((Transform) component3).set_rotation(Quaternion.get_identity());
          ListItem5 component4 = (ListItem5) gameObject2.GetComponent<ListItem5>();
          component2.mItemList.Add(component4);
        }
      }
      else if (num1 < count)
      {
        int num3 = count - num1;
        for (int index = 0; index < num3; ++index)
        {
          ListItem5 mItem = component2.mItemList[component2.mItemList.Count - 1];
          component2.mItemList.RemoveAt(component2.mItemList.Count - 1);
          Object.DestroyImmediate((Object) ((Component) mItem).get_gameObject());
        }
      }
      float num4 = num2;
      for (int index = 0; index < component2.mItemList.Count; ++index)
      {
        ((RectTransform) ((Component) component2.mItemList[index]).get_gameObject().GetComponent<RectTransform>()).set_anchoredPosition3D(new Vector3(num4, 0.0f, 0.0f));
        num4 = num4 + width + num2;
      }
      this.mLoopListView.OnItemPrefabChanged("ItemPrefab1");
    }

    private void OnViewPortSizeChanged()
    {
      this.UpdateItemPrefab();
      this.SetListItemTotalCount(this.mListItemTotalCount);
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem6 component = (ListItem6) ((Component) loopListViewItem2).GetComponent<ListItem6>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      for (int index1 = 0; index1 < this.mItemCountPerRow; ++index1)
      {
        int num = index * this.mItemCountPerRow + index1;
        if (num >= this.mListItemTotalCount)
        {
          ((Component) component.mItemList[index1]).get_gameObject().SetActive(false);
        }
        else
        {
          ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num);
          if (itemDataByIndex != null)
          {
            ((Component) component.mItemList[index1]).get_gameObject().SetActive(true);
            component.mItemList[index1].SetItemData(itemDataByIndex, num);
          }
          else
            ((Component) component.mItemList[index1]).get_gameObject().SetActive(false);
        }
      }
      return loopListViewItem2;
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      if (result < 0)
        result = 0;
      int num = result + 1;
      int itemIndex = num / this.mItemCountPerRow;
      if (num % this.mItemCountPerRow > 0)
        ++itemIndex;
      if (itemIndex > 0)
        --itemIndex;
      this.mLoopListView.MovePanelToItemIndex(itemIndex, 0.0f);
    }

    private void OnAddItemBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mAddItemInput.get_text(), out result))
        return;
      this.SetListItemTotalCount(this.mListItemTotalCount + result);
    }

    private void OnSetItemCountBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mSetCountInput.get_text(), out result))
        return;
      this.SetListItemTotalCount(result);
    }
  }
}
