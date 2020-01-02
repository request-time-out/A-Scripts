// Decompiled with JetBrains decompiler
// Type: ADV.CommandController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Elements.Reference;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public class CommandController : MonoBehaviour
  {
    private bool rootPositionLoaded;
    private Root _rootPosition;
    private CommandList nowCommandList;
    private CommandList backGroundCommandList;
    [SerializeField]
    private bool _useCorrectCamera;
    [SerializeField]
    private CommandController.CharaCorrectHeightCamera _correctCamera;
    private bool useCorrectCameraBakup;

    public CommandController()
    {
      base.\u002Ector();
    }

    public bool GetV3Dic(string arg, out Vector3 pos)
    {
      pos = Vector3.get_zero();
      return !arg.IsNullOrEmpty() && !float.TryParse(arg, out float _) && this.V3Dic.TryGetValue(arg, out pos);
    }

    public CharaData GetChara(int no)
    {
      if (no < 0)
      {
        TextScenario.ParamData data = no != -1 ? this.scenario.heroineList[Mathf.Abs(no + 2)] : this.scenario.player;
        if (data != null)
        {
          foreach (KeyValuePair<int, CharaData> character in this.Characters)
          {
            if (character.Value.data == data)
              return character.Value;
          }
          return new CharaData(data, this.scenario, (CharaData.MotionReserver) null);
        }
      }
      CharaData charaData;
      return this.Characters.TryGetValue(no, out charaData) ? charaData : this.scenario.currentChara;
    }

    public void AddChara(int no, CharaData data)
    {
      this.RemoveChara(no);
      this.Characters[no] = data;
    }

    public void RemoveChara(int no)
    {
      CharaData charaData;
      if (this.Characters.TryGetValue(no, out charaData))
      {
        charaData.Release();
        this.loadingCharaList.Remove(charaData);
      }
      this.Characters.Remove(no);
    }

    private Root rootPosition
    {
      get
      {
        return ((object) this).GetCacheObject<Root>(ref this._rootPosition, (Func<Root>) (() =>
        {
          this.rootPositionLoaded = true;
          return Root.Load(Singleton<Manager.Map>.Instance.MapRoot);
        }));
      }
    }

    public Transform CharaRoot
    {
      get
      {
        return this.rootPosition.charaRoot;
      }
    }

    public Dictionary<string, Transform> characterStandNulls
    {
      get
      {
        return this.rootPosition.characterStandNulls;
      }
    }

    public Transform EventCGRoot
    {
      get
      {
        return this.rootPosition.eventCGRoot;
      }
    }

    public Transform ObjectRoot
    {
      get
      {
        return this.rootPosition.objectRoot;
      }
    }

    public Transform NullRoot
    {
      get
      {
        return this.rootPosition.nullRoot;
      }
    }

    public Transform BaseRoot
    {
      get
      {
        return this.rootPosition.baseRoot;
      }
    }

    public Transform CameraRoot
    {
      get
      {
        return this.rootPosition.cameraRoot;
      }
    }

    public CommandList NowCommandList
    {
      get
      {
        return this.nowCommandList;
      }
    }

    public CommandList BackGroundCommandList
    {
      get
      {
        return this.backGroundCommandList;
      }
    }

    public List<CharaData> LoadingCharaList
    {
      get
      {
        return this.loadingCharaList;
      }
    }

    private List<CharaData> loadingCharaList { get; }

    public Dictionary<int, CharaData> Characters { get; }

    public Dictionary<string, GameObject> Objects { get; }

    public Dictionary<string, Vector3> V3Dic { get; }

    public Dictionary<string, Transform> NullDic { get; }

    public Dictionary<string, Game.Expression> expDic { get; }

    public Dictionary<string, string[]> motionDic { get; }

    public CommandController.FontColor fontColor { get; }

    public bool useCorrectCamera { get; set; }

    private TextScenario scenario { get; set; }

    public void Initialize()
    {
      this.useCorrectCameraBakup = this._useCorrectCamera;
      if (Object.op_Equality((Object) this.scenario, (Object) null))
      {
        this.scenario = (TextScenario) ((Component) this).GetComponent<TextScenario>();
        this.nowCommandList = new CommandList(this.scenario);
        this.backGroundCommandList = new CommandList(this.scenario);
      }
      else
      {
        this.nowCommandList.Clear();
        this.backGroundCommandList.Clear();
      }
      this.loadingCharaList.Clear();
      this.Objects.Clear();
      this.Characters.Clear();
      this.V3Dic.Clear();
      this.NullDic.Clear();
      this.expDic.Clear();
      this.motionDic.Clear();
      this.fontColor.Clear();
      this.rootPosition.SetBackup();
    }

    public void SetObject(GameObject go)
    {
      GameObject gameObject;
      if (this.Objects.TryGetValue(((Object) go).get_name(), out gameObject))
        Object.Destroy((Object) gameObject);
      go.get_transform().SetParent(this.ObjectRoot, false);
      this.Objects[((Object) go).get_name()] = go;
    }

    public void SetNull(Transform nullT)
    {
      Transform transform;
      if (this.NullDic.TryGetValue(((Object) nullT).get_name(), out transform) && Object.op_Inequality((Object) transform, (Object) null))
        Object.Destroy((Object) ((Component) transform).get_gameObject());
      nullT.SetParent(this.NullRoot, false);
      this.NullDic[((Object) nullT).get_name()] = nullT;
    }

    public void ReleaseObject()
    {
      using (Dictionary<string, GameObject>.ValueCollection.Enumerator enumerator = this.Objects.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null))
            Object.Destroy((Object) current);
        }
      }
      this.Objects.Clear();
    }

    public void ReleaseNull()
    {
      using (Dictionary<string, Transform>.ValueCollection.Enumerator enumerator = this.NullDic.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null))
            Object.Destroy((Object) ((Component) current).get_gameObject());
        }
      }
      this.NullDic.Clear();
    }

    public void ReleaseChara()
    {
      foreach (CharaData charaData in this.Characters.Values)
        charaData.Release();
      this.Characters.Clear();
    }

    public void ReleaseEventCG()
    {
      if (!this.rootPositionLoaded || Object.op_Equality((Object) this._rootPosition, (Object) null))
        return;
      Transform eventCgRoot = this._rootPosition.eventCGRoot;
      if (Object.op_Equality((Object) eventCgRoot, (Object) null))
        return;
      ((Component) eventCgRoot).get_gameObject().Children().ForEach((Action<GameObject>) (go => Object.Destroy((Object) go)));
    }

    public void Release()
    {
      this.ReleaseObject();
      this.ReleaseNull();
      this.ReleaseChara();
      this.ReleaseEventCG();
      this._useCorrectCamera = this.useCorrectCameraBakup;
    }

    private void OnDestroy()
    {
      this.Release();
      if (!this.rootPositionLoaded || !Object.op_Inequality((Object) this._rootPosition, (Object) null))
        return;
      Object.Destroy((Object) ((Component) this._rootPosition).get_gameObject());
    }

    public class FontColor : AutoIndexer<Tuple<int, Color>>
    {
      public FontColor()
        : base(Tuple.Create<int, Color>(-1, Color.get_white()))
      {
      }

      public void Set(string key, Color color)
      {
        this.Set(key, this.initializeValue.Item1, color);
      }

      public void Set(string key, int configIndex)
      {
        this.Set(key, configIndex, this.initializeValue.Item2);
      }

      private void Set(string key, int configIndex, Color color)
      {
        this.dic[key] = Tuple.Create<int, Color>(configIndex, color);
      }

      private Color? GetConfigColor(int configIndex)
      {
        return new Color?();
      }

      public Color this[string key]
      {
        get
        {
          if (key != null)
          {
            if (CommandController.FontColor.\u003C\u003Ef__switch\u0024map9 == null)
              CommandController.FontColor.\u003C\u003Ef__switch\u0024map9 = new Dictionary<string, int>(10)
              {
                {
                  "[P]",
                  0
                },
                {
                  "[P姓]",
                  0
                },
                {
                  "[P名]",
                  0
                },
                {
                  "[P名前]",
                  0
                },
                {
                  "[Pあだ名]",
                  0
                },
                {
                  "[H]",
                  1
                },
                {
                  "[H姓]",
                  1
                },
                {
                  "[H名]",
                  1
                },
                {
                  "[H名前]",
                  1
                },
                {
                  "[Hあだ名]",
                  1
                }
              };
            int num;
            if (CommandController.FontColor.\u003C\u003Ef__switch\u0024map9.TryGetValue(key, out num))
            {
              if (num == 0)
                return this.GetConfigColor(0).Value;
              if (num == 1)
                return this.GetConfigColor(1).Value;
            }
          }
          Tuple<int, Color> tuple = base[key];
          Color? configColor = this.GetConfigColor(tuple.Item1);
          return configColor.HasValue ? configColor.Value : tuple.Item2;
        }
        private set
        {
          Debug.LogError((object) "FontColor Set");
        }
      }
    }

    [Serializable]
    private class CharaCorrectHeightCamera
    {
      [SerializeField]
      private CommandController.CharaCorrectHeightCamera.Pair pos;
      [SerializeField]
      private CommandController.CharaCorrectHeightCamera.Pair ang;

      public bool Calculate(IEnumerable<CharaData> datas, out Vector3 pos, out Vector3 ang)
      {
        if (datas == null || !datas.Any<CharaData>())
        {
          pos = Vector3.get_zero();
          ang = Vector3.get_zero();
          return false;
        }
        float shape = datas.Average<CharaData>((Func<CharaData, float>) (item => item.chaCtrl.GetShapeBodyValue(0)));
        pos = MathfEx.GetShapeLerpPositionValue(shape, this.pos.min, this.pos.max);
        ang = MathfEx.GetShapeLerpAngleValue(shape, this.ang.min, this.ang.max);
        return true;
      }

      [Serializable]
      private struct Pair
      {
        public Vector3 min;
        public Vector3 max;
      }
    }
  }
}
