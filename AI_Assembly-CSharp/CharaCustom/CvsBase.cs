// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsBase : MonoBehaviour
  {
    [Button("ReacquireTab", "タブ再取得", new object[] {})]
    public int reacquireTab;
    [SerializeField]
    private UI_ToggleEx[] tglTab;
    public Text titleText;
    public CvsBase.ItemInfo[] items;

    public CvsBase()
    {
      base.\u002Ector();
    }

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaListControl lstCtrl
    {
      get
      {
        return Singleton<Character>.Instance.chaListCtrl;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    protected ChaFileFace face
    {
      get
      {
        return this.chaCtrl.fileFace;
      }
    }

    protected ChaFileBody body
    {
      get
      {
        return this.chaCtrl.fileBody;
      }
    }

    protected ChaFileHair hair
    {
      get
      {
        return this.chaCtrl.fileHair;
      }
    }

    protected ChaFileFace.MakeupInfo makeup
    {
      get
      {
        return this.chaCtrl.fileFace.makeup;
      }
    }

    protected ChaFileClothes orgClothes
    {
      get
      {
        return this.chaCtrl.chaFile.coordinate.clothes;
      }
    }

    protected ChaFileClothes nowClothes
    {
      get
      {
        return this.chaCtrl.nowCoordinate.clothes;
      }
    }

    protected ChaFileAccessory orgAcs
    {
      get
      {
        return this.chaCtrl.chaFile.coordinate.accessory;
      }
    }

    protected ChaFileAccessory nowAcs
    {
      get
      {
        return this.chaCtrl.nowCoordinate.accessory;
      }
    }

    protected ChaFileParameter parameter
    {
      get
      {
        return this.chaCtrl.chaFile.parameter;
      }
    }

    protected ChaFileGameInfo gameinfo
    {
      get
      {
        return this.chaCtrl.chaFile.gameinfo;
      }
    }

    protected ChaFileControl defChaCtrl
    {
      get
      {
        return this.customBase.defChaCtrl;
      }
    }

    public int SNo { get; set; }

    public void ReacquireTab()
    {
      this.tglTab = (UI_ToggleEx[]) null;
      List<UI_ToggleEx> uiToggleExList = new List<UI_ToggleEx>();
      GameObject loop = ((Component) this).get_transform().FindLoop("SelectMenu");
      if (!Object.op_Implicit((Object) loop))
        return;
      for (int index = 0; index < 5; ++index)
      {
        Transform transform = loop.get_transform().Find(string.Format("tgl{0:00}", (object) (index + 1)));
        if (Object.op_Implicit((Object) transform))
        {
          UI_ToggleEx component = (UI_ToggleEx) ((Component) transform).GetComponent<UI_ToggleEx>();
          if (Object.op_Implicit((Object) component))
            uiToggleExList.Add(component);
        }
      }
      if (uiToggleExList.Count == 0)
        return;
      this.tglTab = uiToggleExList.ToArray();
    }

    public void ShowOrHideTab(bool show, params int[] no)
    {
      if (this.tglTab.Length == 0)
        return;
      bool flag = false;
      for (int index = 0; index < no.Length; ++index)
      {
        if (this.tglTab.Length > no[index])
        {
          if (!show)
            flag |= this.tglTab[no[index]].get_isOn();
          ((Component) this.tglTab[no[index]]).get_gameObject().SetActiveIfDifferent(show);
        }
      }
      if (show)
        return;
      if (flag)
        this.tglTab[0].set_isOn(true);
      for (int index = 0; index < no.Length; ++index)
        this.tglTab[no[index]].SetIsOnWithoutCallback(false);
    }

    public virtual void UpdateCustomUI()
    {
    }

    public virtual void ChangeMenuFunc()
    {
    }

    public static List<CustomSelectInfo> CreateSelectList(
      ChaListDefine.CategoryNo cateNo,
      ChaListDefine.KeyType limitKey = ChaListDefine.KeyType.Unknown)
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      Dictionary<int, ListInfoBase> categoryInfo = chaListCtrl.GetCategoryInfo(cateNo);
      int[] array = categoryInfo.Keys.ToArray<int>();
      List<CustomSelectInfo> customSelectInfoList = new List<CustomSelectInfo>();
      for (int index = 0; index < categoryInfo.Count; ++index)
      {
        if (categoryInfo[array[index]].GetInfoInt(ChaListDefine.KeyType.Possess) != 99)
        {
          bool flag = false;
          if (chaListCtrl.CheckItemID(categoryInfo[array[index]].Category, categoryInfo[array[index]].Id) == (byte) 1)
            flag = true;
          customSelectInfoList.Add(new CustomSelectInfo()
          {
            category = categoryInfo[array[index]].Category,
            id = categoryInfo[array[index]].Id,
            limitIndex = limitKey != ChaListDefine.KeyType.Unknown ? categoryInfo[array[index]].GetInfoInt(limitKey) : -1,
            name = categoryInfo[array[index]].Name,
            assetBundle = categoryInfo[array[index]].GetInfo(ChaListDefine.KeyType.ThumbAB),
            assetName = categoryInfo[array[index]].GetInfo(ChaListDefine.KeyType.ThumbTex),
            newItem = flag
          });
        }
      }
      return customSelectInfoList;
    }

    public int GetSelectTab()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      \u003C\u003E__AnonType5<CvsBase.ItemInfo, int> anonType5 = ((IEnumerable<CvsBase.ItemInfo>) this.items).Select<CvsBase.ItemInfo, \u003C\u003E__AnonType5<CvsBase.ItemInfo, int>>((Func<CvsBase.ItemInfo, int, \u003C\u003E__AnonType5<CvsBase.ItemInfo, int>>) ((v, i) => new \u003C\u003E__AnonType5<CvsBase.ItemInfo, int>(v, i))).FirstOrDefault<\u003C\u003E__AnonType5<CvsBase.ItemInfo, int>>((Func<\u003C\u003E__AnonType5<CvsBase.ItemInfo, int>, bool>) (x => x.v.tglItem.get_isOn()));
      return anonType5 != null ? anonType5.i : -1;
    }

    public static List<CustomPushInfo> CreatePushList(
      ChaListDefine.CategoryNo cateNo)
    {
      Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(cateNo);
      int[] array = categoryInfo.Keys.ToArray<int>();
      List<CustomPushInfo> customPushInfoList = new List<CustomPushInfo>();
      for (int index = 0; index < categoryInfo.Count; ++index)
        customPushInfoList.Add(new CustomPushInfo()
        {
          category = categoryInfo[array[index]].Category,
          id = categoryInfo[array[index]].Id,
          name = categoryInfo[array[index]].Name,
          assetBundle = categoryInfo[array[index]].GetInfo(ChaListDefine.KeyType.ThumbAB),
          assetName = categoryInfo[array[index]].GetInfo(ChaListDefine.KeyType.ThumbTex)
        });
      return customPushInfoList;
    }

    protected virtual void Reset()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      GameObject objectFromName1 = findAssist.GetObjectFromName("textWinTitle");
      if (Object.op_Implicit((Object) objectFromName1))
        this.titleText = (Text) objectFromName1.GetComponent<Text>();
      List<CvsBase.ItemInfo> source = new List<CvsBase.ItemInfo>();
      for (int index = 0; index < 5; ++index)
      {
        GameObject objectFromName2 = findAssist.GetObjectFromName(string.Format("tgl{0:00}", (object) (index + 1)));
        if (Object.op_Implicit((Object) objectFromName2))
        {
          GameObject objectFromName3 = findAssist.GetObjectFromName(string.Format("Setting{0:00}", (object) (index + 1)));
          if (Object.op_Implicit((Object) objectFromName3))
          {
            UI_ToggleEx component1 = (UI_ToggleEx) objectFromName2.GetComponent<UI_ToggleEx>();
            CanvasGroup component2 = (CanvasGroup) objectFromName3.GetComponent<CanvasGroup>();
            source.Add(new CvsBase.ItemInfo()
            {
              tglItem = component1,
              cgItem = component2
            });
          }
        }
      }
      if (1 >= source.Count<CvsBase.ItemInfo>())
        return;
      this.items = source.ToArray();
    }

    protected virtual void Start()
    {
      if (!((IEnumerable<CvsBase.ItemInfo>) this.items).Any<CvsBase.ItemInfo>())
        return;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<CvsBase.ItemInfo>) this.items).Select<CvsBase.ItemInfo, \u003C\u003E__AnonType15<CvsBase.ItemInfo, int>>((Func<CvsBase.ItemInfo, int, \u003C\u003E__AnonType15<CvsBase.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CvsBase.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<CvsBase.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<CvsBase.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.tglItem, (Object) null))).ToList<\u003C\u003E__AnonType15<CvsBase.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<CvsBase.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable((Toggle) item.val.tglItem), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ =>
      {
        // ISSUE: object of a compiler-generated type is created
        foreach (\u003C\u003E__AnonType5<CvsBase.ItemInfo, int> anonType5 in ((IEnumerable<CvsBase.ItemInfo>) this.items).Select<CvsBase.ItemInfo, \u003C\u003E__AnonType5<CvsBase.ItemInfo, int>>((Func<CvsBase.ItemInfo, int, \u003C\u003E__AnonType5<CvsBase.ItemInfo, int>>) ((v, i) => new \u003C\u003E__AnonType5<CvsBase.ItemInfo, int>(v, i))))
        {
          if (anonType5.i != item.idx && anonType5.v != null)
          {
            CanvasGroup cgItem = anonType5.v.cgItem;
            if (Object.op_Implicit((Object) cgItem))
              cgItem.Enable(false, false);
          }
        }
        if (Object.op_Implicit((Object) item.val.cgItem))
          item.val.cgItem.Enable(true, false);
        this.customBase.customCtrl.showColorCvs = false;
        this.customBase.customCtrl.showPattern = false;
      }))));
    }

    [Serializable]
    public class ItemInfo
    {
      public UI_ToggleEx tglItem;
      public CanvasGroup cgItem;
    }
  }
}
