// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.CraftViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class CraftViewer : MonoBehaviour
  {
    [SerializeField]
    private CraftItemListUI _itemListUI;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private RectTransform _resetScrollTarget;
    private Vector2? _resetScrollTargetPos;
    private int _id;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Text _text;
    private IReadOnlyDictionary<int, RecipeDataInfo[]> _info;

    public CraftViewer()
    {
      base.\u002Ector();
    }

    public bool initialized { get; private set; }

    public bool Visible
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        ((Component) this).get_gameObject().SetActive(value);
      }
    }

    public CraftItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public Image cursor
    {
      get
      {
        return this._cursor;
      }
    }

    public void SetID(int id)
    {
      this._id = id;
    }

    public void SetIcon(Sprite sprite)
    {
      this._icon.set_sprite(sprite);
    }

    public void SetIcon(string text)
    {
      this._text.set_text(text);
    }

    private static CraftItemNodeUI.Possible Possible(
      IReadOnlyCollection<IReadOnlyCollection<StuffItem>> storages,
      int nameHash,
      RecipeDataInfo[] info)
    {
      PlayerData playerData = Singleton<Manager.Map>.Instance.Player.PlayerData;
      HashSet<int> craftPossibleTable = playerData.CraftPossibleTable;
      bool success = ((IEnumerable<RecipeDataInfo>) info).Any<RecipeDataInfo>((Func<RecipeDataInfo, bool>) (x => ((IEnumerable<RecipeDataInfo.NeedData>) x.NeedList).All<RecipeDataInfo.NeedData>((Func<RecipeDataInfo.NeedData, bool>) (need => ((IEnumerable<IReadOnlyCollection<StuffItem>>) storages).SelectMany<IReadOnlyCollection<StuffItem>, StuffItem>((Func<IReadOnlyCollection<StuffItem>, IEnumerable<StuffItem>>) (storage => (IEnumerable<StuffItem>) storage)).FindItems(new StuffItem(need.CategoryID, need.ID, 0)).Sum<StuffItem>((Func<StuffItem, int>) (p => p.Count)) / need.Sum > 0))));
      if (success)
        craftPossibleTable.Add(nameHash);
      if (playerData.FirstCreatedItemTable.Contains(nameHash))
        success = false;
      return new CraftItemNodeUI.Possible(!craftPossibleTable.Contains(nameHash), success);
    }

    public void Refresh(CraftUI craftUI)
    {
      this.itemListUI.ClearItems();
      this.ResetScroll();
      craftUI.SetID(this._id);
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<KeyValuePair<int, RecipeDataInfo[]>, int> anonType18 in ((IEnumerable<KeyValuePair<int, RecipeDataInfo[]>>) this._info).Select<KeyValuePair<int, RecipeDataInfo[]>, \u003C\u003E__AnonType18<KeyValuePair<int, RecipeDataInfo[]>, int>>((Func<KeyValuePair<int, RecipeDataInfo[]>, int, \u003C\u003E__AnonType18<KeyValuePair<int, RecipeDataInfo[]>, int>>) ((p, i) => new \u003C\u003E__AnonType18<KeyValuePair<int, RecipeDataInfo[]>, int>(p, i))))
      {
        int hash = anonType18.p.Key;
        RecipeDataInfo[] recipeInfo = anonType18.p.Value;
        StuffItemInfo itemInfo = Singleton<Resources>.Instance.GameInfo.FindItemInfo(hash);
        this.itemListUI.AddItemNode(anonType18.i, new CraftItemNodeUI.StuffItemInfoPack(itemInfo, (Func<CraftItemNodeUI.Possible>) (() => CraftViewer.Possible(craftUI.checkStorages, hash, recipeInfo))), recipeInfo);
      }
    }

    private void ResetScroll()
    {
      if (!this._resetScrollTargetPos.HasValue)
        return;
      this._resetScrollTarget.set_anchoredPosition(this._resetScrollTargetPos.Value);
    }

    private void Awake()
    {
      this._resetScrollTargetPos = this._resetScrollTarget != null ? new Vector2?(this._resetScrollTarget.get_anchoredPosition()) : new Vector2?();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftViewer.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnEnable()
    {
      this.ResetScroll();
    }
  }
}
