// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.InventoryViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class InventoryViewer : MonoBehaviour
  {
    private MenuUIBehaviour[] _menuUIList;
    [SerializeField]
    private InventoryViewer.IconText _iconText;
    [SerializeField]
    private ItemFilterCategoryUI _categoryUI;
    [SerializeField]
    private ItemListUI _itemListUI;
    [SerializeField]
    private ItemSortUI _sortUI;
    [SerializeField]
    private Button _sortButton;
    [SerializeField]
    private Toggle _sorter;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private ConditionalTextXtoYViewer _slotCounter;
    [SerializeField]
    private Text _emptyText;

    public InventoryViewer()
    {
      base.\u002Ector();
    }

    public MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => new MenuUIBehaviour[3]
        {
          (MenuUIBehaviour) this._categoryUI,
          (MenuUIBehaviour) this._sortUI,
          (MenuUIBehaviour) this._itemListUI
        }));
      }
    }

    public ItemFilterCategoryUI categoryUI
    {
      get
      {
        return this._categoryUI;
      }
    }

    public ItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public ItemSortUI sortUI
    {
      get
      {
        return this._sortUI;
      }
    }

    public Button sortButton
    {
      get
      {
        return this._sortButton;
      }
    }

    public Toggle sorter
    {
      get
      {
        return this._sorter;
      }
    }

    public Image cursor
    {
      get
      {
        return this._cursor;
      }
    }

    public ConditionalTextXtoYViewer slotCounter
    {
      get
      {
        return this._slotCounter;
      }
    }

    public Text emptyText
    {
      get
      {
        return this._emptyText;
      }
    }

    public bool isAutoEmptyText { get; set; }

    public void SortUIBind(ItemSortUI sortUI)
    {
      this._sortUI = sortUI;
    }

    private InventoryViewer.SortData sortData { get; }

    public void SetParentMenuUI(MenuUIBehaviour parentMenuUI)
    {
      this.parentMenuUI = parentMenuUI;
    }

    public bool IsActiveControl
    {
      get
      {
        return Object.op_Equality((Object) this.parentMenuUI, (Object) null) || this.parentMenuUI.IsActiveControl;
      }
    }

    public Action<int> setFocusLevel { get; set; }

    public MenuUIBehaviour parentMenuUI { get; private set; }

    public void ForceSortType(int type)
    {
      this.sortData.ForceSortType(type);
    }

    public int SortType
    {
      get
      {
        return this.sortData.type;
      }
      set
      {
        this.sortData.type = value;
      }
    }

    public void ForceSortAscending(bool ascending)
    {
      this.sortData.ForceSortAscending(ascending);
    }

    public bool SortAscending
    {
      get
      {
        return this.sortData.ascending;
      }
      set
      {
        this.sortData.ascending = value;
      }
    }

    public void SortItemList()
    {
      this._itemListUI.SortItems(this.sortData.type, this.sortData.ascending);
    }

    public bool initialized { get; private set; }

    public bool categoryInitialized { get; private set; }

    public bool sortUIInitialized { get; private set; }

    [DebuggerHidden]
    public IEnumerator CategoryButtonAddEvent(Action<int> action)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryViewer.\u003CCategoryButtonAddEvent\u003Ec__Iterator0()
      {
        action = action,
        \u0024this = this
      };
    }

    public void ChangeTitleIcon(Sprite sprite)
    {
      this._iconText.icon.set_sprite(sprite);
    }

    public void ChangeTitleText(string text)
    {
      this._iconText.text.set_text(text);
    }

    public void SetFocusLevel(int level)
    {
      Singleton<Input>.Instance.FocusLevel = level;
      this._categoryUI.EnabledInput = level == this._categoryUI.FocusLevel;
      this._itemListUI.EnabledInput = level == this._itemListUI.FocusLevel;
      if (Object.op_Inequality((Object) this._sortUI, (Object) null))
        this._sortUI.EnabledInput = level == this._sortUI.FocusLevel;
      Action<int> setFocusLevel = this.setFocusLevel;
      if (setFocusLevel == null)
        return;
      setFocusLevel(level);
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryViewer.\u003CStart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public static IEnumerator Load(
      Transform viewerParent,
      Action<InventoryViewer> onComplete)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryViewer.\u003CLoad\u003Ec__Iterator2()
      {
        onComplete = onComplete,
        viewerParent = viewerParent
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadSortUI(Action<ItemSortUI> onComplete)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryViewer.\u003CLoadSortUI\u003Ec__Iterator3()
      {
        onComplete = onComplete,
        \u0024this = this
      };
    }

    public void SetCursorFocus(Selectable selectable)
    {
      this._categoryUI.useCursor = false;
      CursorFrame.Set(((Graphic) this._cursor).get_rectTransform(), (RectTransform) ((Component) selectable).GetComponent<RectTransform>(), (RectTransform) null);
      ((Behaviour) this._cursor).set_enabled(true);
      if (Singleton<Input>.Instance.FocusLevel != this._categoryUI.FocusLevel)
        Singleton<Input>.Instance.FocusLevel = this._categoryUI.FocusLevel;
      if (this._categoryUI.EnabledInput)
        return;
      this._categoryUI.EnabledInput = true;
    }

    [Serializable]
    private class IconText
    {
      [SerializeField]
      private Image _icon;
      [SerializeField]
      private Text _text;

      public Image icon
      {
        get
        {
          return this._icon;
        }
      }

      public Text text
      {
        get
        {
          return this._text;
        }
      }
    }

    private class SortData
    {
      public IObservable<int> Type
      {
        get
        {
          return (IObservable<int>) this._type;
        }
      }

      public IObservable<bool> Ascending
      {
        get
        {
          return (IObservable<bool>) this._ascending;
        }
      }

      public void ForceSortType(int type)
      {
        ((ReactiveProperty<int>) this._type).SetValueAndForceNotify(type);
      }

      public void ForceSortAscending(bool ascending)
      {
        ((ReactiveProperty<bool>) this._ascending).SetValueAndForceNotify(ascending);
      }

      public int type
      {
        get
        {
          return ((ReactiveProperty<int>) this._type).get_Value();
        }
        set
        {
          ((ReactiveProperty<int>) this._type).set_Value(value);
        }
      }

      public bool ascending
      {
        get
        {
          return ((ReactiveProperty<bool>) this._ascending).get_Value();
        }
        set
        {
          ((ReactiveProperty<bool>) this._ascending).set_Value(value);
        }
      }

      private IntReactiveProperty _type { get; } = new IntReactiveProperty(0);

      private BoolReactiveProperty _ascending { get; } = new BoolReactiveProperty(true);
    }
  }
}
