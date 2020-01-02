// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.GameCharaFileInfoComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using SceneAssist;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameLoadCharaFileSystem
{
  [DisallowMultipleComponent]
  public class GameCharaFileInfoComponent : MonoBehaviour
  {
    [SerializeField]
    private GameCharaFileInfoComponent.RowInfo[] rows;
    [SerializeField]
    private Texture2D texEmpty;

    public GameCharaFileInfoComponent()
    {
      base.\u002Ector();
    }

    public void SetData(int _index, GameCharaFileInfo _info, Action<bool> _onClickAction)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameCharaFileInfoComponent.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new GameCharaFileInfoComponent.\u003CSetData\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onClickAction = _onClickAction;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0.\u0024this = this;
      bool active = _info != null;
      if (active)
        active = _info.FullPath.IsNullOrEmpty() || !_info.FullPath.IsNullOrEmpty() && File.Exists(_info.FullPath) || _info.pngData != null;
      ((Component) this.rows[_index].tgl).get_gameObject().SetActiveIfDifferent(active);
      ((UnityEventBase) this.rows[_index].tgl.onValueChanged).RemoveAllListeners();
      if (!active)
        return;
      ((UnityEventBase) this.rows[_index].tgl.onValueChanged).RemoveAllListeners();
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.rows[_index].tgl.onValueChanged).AddListener(new UnityAction<bool>((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      this.rows[_index].tgl.SetIsOnWithoutCallback(false);
      ((Selectable) this.rows[_index].tgl).set_interactable(!_info.isInSaveData);
      this.rows[_index].objEntry.SetActiveIfDifferent(_info.isInSaveData);
      this.rows[_index].objNurturing.SetActiveIfDifferent(_info.gameRegistration);
      if (Object.op_Inequality((Object) this.rows[_index].imgThumb.get_texture(), (Object) null) && Object.op_Inequality((Object) this.rows[_index].imgThumb.get_texture(), (Object) this.texEmpty))
      {
        Object.Destroy((Object) this.rows[_index].imgThumb.get_texture());
        this.rows[_index].imgThumb.set_texture((Texture) null);
      }
      if (_info.pngData != null || !_info.FullPath.IsNullOrEmpty())
        this.rows[_index].imgThumb.set_texture((Texture) PngAssist.ChangeTextureFromByte(_info.pngData ?? PngFile.LoadPngBytes(_info.FullPath), 0, 0, (TextureFormat) 5, false));
      else
        this.rows[_index].imgThumb.set_texture((Texture) this.texEmpty);
      this.rows[_index].objSelect.SetActiveIfDifferent(false);
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0.sel = _index;
      // ISSUE: method pointer
      this.rows[_index].pointerAction.listActionEnter.Add(new UnityAction((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__1)));
      // ISSUE: method pointer
      this.rows[_index].pointerAction.listActionExit.Add(new UnityAction((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__2)));
      this.rows[_index].info = _info;
    }

    public void SetToggleON(int _index, bool _isOn)
    {
      this.rows[_index].tgl.SetIsOnWithoutCallback(_isOn);
    }

    public GameCharaFileInfo GetListInfo(int _index)
    {
      return this.rows[_index].info;
    }

    public void SetListInfo(int _index, GameCharaFileInfo _info)
    {
      this.rows[_index].info = _info;
    }

    public void Disable(bool disable)
    {
    }

    public void Disvisible(bool disvisible)
    {
    }

    [Serializable]
    public class RowInfo
    {
      public GameCharaFileInfo info;
      public Toggle tgl;
      public RawImage imgThumb;
      public GameObject objSelect;
      public PointerEnterExitAction pointerAction;
      public GameObject objEntry;
      public GameObject objNurturing;
    }
  }
}
