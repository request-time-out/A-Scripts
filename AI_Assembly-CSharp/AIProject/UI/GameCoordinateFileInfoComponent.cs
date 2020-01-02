// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GameCoordinateFileInfoComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using SceneAssist;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  [DisallowMultipleComponent]
  public class GameCoordinateFileInfoComponent : MonoBehaviour
  {
    [SerializeField]
    private GameCoordinateFileInfoComponent.RowInfo[] _rows;

    public GameCoordinateFileInfoComponent()
    {
      base.\u002Ector();
    }

    public void SetData(int index, GameCoordinateFileInfo info, Action<bool> onClickAction)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameCoordinateFileInfoComponent.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new GameCoordinateFileInfoComponent.\u003CSetData\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0.onClickAction = onClickAction;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0.\u0024this = this;
      bool active = info != null;
      GameCoordinateFileInfoComponent.RowInfo row = this._rows[index];
      ((Component) row.toggle).get_gameObject().SetActiveIfDifferent(active);
      ((UnityEventBase) row.toggle.onValueChanged).RemoveAllListeners();
      if (!active)
        return;
      ((UnityEventBase) row.toggle.onValueChanged).RemoveAllListeners();
      // ISSUE: method pointer
      ((UnityEvent<bool>) row.toggle.onValueChanged).AddListener(new UnityAction<bool>((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      row.toggle.SetIsOnWithoutCallback(false);
      ((Selectable) row.toggle).set_interactable(!info.IsInSaveData);
      row.imageThumb.set_texture((Texture) PngAssist.ChangeTextureFromByte(info.PngData ?? PngFile.LoadPngBytes(info.FullPath), 0, 0, (TextureFormat) 5, false));
      row.objSelect.SetActiveIfDifferent(false);
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0.sel = index;
      // ISSUE: method pointer
      row.pointerAction.listActionEnter.Add(new UnityAction((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__1)));
      // ISSUE: method pointer
      row.pointerAction.listActionExit.Add(new UnityAction((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__2)));
      row.info = info;
    }

    public void SetToggleOn(int index, bool isOn)
    {
      this._rows[index].toggle.SetIsOnWithoutCallback(isOn);
    }

    public GameCoordinateFileInfo GetListInfo(int index)
    {
      return this._rows[index].info;
    }

    [Serializable]
    public class RowInfo
    {
      public GameCoordinateFileInfo info;
      public Toggle toggle;
      public RawImage imageThumb;
      public GameObject objSelect;
      public PointerEnterExitAction pointerAction;
    }
  }
}
