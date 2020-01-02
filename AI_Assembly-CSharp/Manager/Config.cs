// Decompiled with JetBrains decompiler
// Type: Manager.Config
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ConfigScene;
using Illusion.Elements.Xml;
using System.Diagnostics;
using UnityEngine;

namespace Manager
{
  public sealed class Config : Singleton<Config>
  {
    private const string UserPath = "config";
    private const string FileName = "system.xml";
    private const string RootName = "System";
    private Control xmlCtrl;

    public static bool initialized { get; private set; }

    public static ActionSystem ActData { get; private set; }

    public static GraphicSystem GraphicData { get; private set; }

    public static SoundSystem SoundData { get; private set; }

    public static GameConfigSystem GameData { get; private set; }

    public static HSystem HData { get; private set; }

    public void Reset()
    {
      if (this.xmlCtrl == null)
        return;
      this.xmlCtrl.Init();
    }

    public void Load()
    {
      this.xmlCtrl.Read();
    }

    public void Save()
    {
      this.xmlCtrl.Write();
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    }

    private void Start()
    {
      Config.ActData = new ActionSystem("Action");
      Config.GraphicData = new GraphicSystem("Graphic");
      Config.SoundData = new SoundSystem("Sound");
      Config.GameData = new GameConfigSystem("Game");
      Config.HData = new HSystem("H");
      this.xmlCtrl = new Control("config", "system.xml", "System", new Illusion.Elements.Xml.Data[5]
      {
        (Illusion.Elements.Xml.Data) Config.ActData,
        (Illusion.Elements.Xml.Data) Config.GraphicData,
        (Illusion.Elements.Xml.Data) Config.SoundData,
        (Illusion.Elements.Xml.Data) Config.GameData,
        (Illusion.Elements.Xml.Data) Config.HData
      });
      this.Load();
      Config.initialized = true;
    }

    [Conditional("__DEBUG_PROC__")]
    private void DBLog(object o)
    {
      Debug.Log(o, (Object) this);
    }
  }
}
