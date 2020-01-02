// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomSkinColorPreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomSkinColorPreset : MonoBehaviour
  {
    public CustomSkinColorPreset.ItemInfo[] items;
    public Action<Color> onClick;

    public CustomSkinColorPreset()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    public void Reset()
    {
      this.items = new CustomSkinColorPreset.ItemInfo[7];
      for (int index = 0; index < 7; ++index)
      {
        this.items[index] = new CustomSkinColorPreset.ItemInfo();
        Transform transform1 = ((Component) this).get_transform().Find(string.Format("btnSample{0:00}", (object) (index + 1)));
        if (Object.op_Implicit((Object) transform1))
        {
          this.items[index].button = (Button) ((Component) transform1).GetComponent<Button>();
          Transform transform2 = transform1.Find("imgColor");
          if (Object.op_Implicit((Object) transform2))
            this.items[index].image = (Image) ((Component) transform2).GetComponent<Image>();
        }
      }
    }

    public void Start()
    {
      Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.preset_skin_color);
      for (int key = 0; key < this.items.Length; ++key)
      {
        ListInfoBase listInfoBase;
        if (categoryInfo.TryGetValue(key, out listInfoBase))
        {
          this.items[key].skinColor = Color.HSVToRGB(listInfoBase.GetInfoFloat(ChaListDefine.KeyType.BaseH), listInfoBase.GetInfoFloat(ChaListDefine.KeyType.BaseS), listInfoBase.GetInfoFloat(ChaListDefine.KeyType.BaseV));
          ((Graphic) this.items[key].image).set_color(Color.HSVToRGB(listInfoBase.GetInfoFloat(ChaListDefine.KeyType.SampleH), listInfoBase.GetInfoFloat(ChaListDefine.KeyType.SampleS), listInfoBase.GetInfoFloat(ChaListDefine.KeyType.SampleV)));
        }
      }
      if (!((IEnumerable<CustomSkinColorPreset.ItemInfo>) this.items).Any<CustomSkinColorPreset.ItemInfo>())
        return;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<CustomSkinColorPreset.ItemInfo>) this.items).Select<CustomSkinColorPreset.ItemInfo, \u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>>((Func<CustomSkinColorPreset.ItemInfo, int, \u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.button, (Object) null))).ToList<\u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<CustomSkinColorPreset.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val.button), (Action<M0>) (_ =>
      {
        if (this.onClick == null)
          return;
        this.onClick(item.val.skinColor);
      }))));
    }

    [Serializable]
    public class ItemInfo
    {
      public Color skinColor = Color.get_white();
      public Button button;
      public Image image;
    }
  }
}
