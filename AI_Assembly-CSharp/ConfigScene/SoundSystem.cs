// Decompiled with JetBrains decompiler
// Type: ConfigScene.SoundSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace ConfigScene
{
  public class SoundSystem : BaseSystem
  {
    public SoundData Master = new SoundData();
    public SoundData BGM = new SoundData();
    public SoundData ENV = new SoundData();
    public SoundData SystemSE = new SoundData();
    public SoundData GameSE = new SoundData();
    private SoundData[] _sounds;

    public SoundSystem(string elementName)
      : base(elementName)
    {
      this.Master.ChangeEvent += (Action<SoundData>) (sd =>
      {
        MixerVolume.Set(Sound.Mixer, MixerVolume.Names.MasterVolume, sd.GetVolume());
        Debug.Log((object) ("Master : " + (string) sd));
      });
      this.BGM.ChangeEvent += (Action<SoundData>) (sd =>
      {
        MixerVolume.Set(Sound.Mixer, MixerVolume.Names.BGMVolume, sd.GetVolume());
        Debug.Log((object) ("BGM: " + (string) sd));
      });
      this.ENV.ChangeEvent += (Action<SoundData>) (sd =>
      {
        MixerVolume.Set(Sound.Mixer, MixerVolume.Names.ENVVolume, sd.GetVolume());
        Debug.Log((object) ("ENV: " + (string) sd));
      });
      this.SystemSE.ChangeEvent += (Action<SoundData>) (sd =>
      {
        MixerVolume.Set(Sound.Mixer, MixerVolume.Names.SystemSEVolume, sd.GetVolume());
        Debug.Log((object) ("SystemSE: " + (string) sd));
      });
      this.GameSE.ChangeEvent += (Action<SoundData>) (sd =>
      {
        MixerVolume.Set(Sound.Mixer, MixerVolume.Names.GameSEVolume, sd.GetVolume());
        Debug.Log((object) ("GameSE: " + (string) sd));
      });
    }

    public SoundData[] Sounds
    {
      get
      {
        SoundData[] sounds = this._sounds;
        if (sounds != null)
          return sounds;
        return this._sounds = new SoundData[5]
        {
          this.Master,
          this.BGM,
          this.ENV,
          this.SystemSE,
          this.GameSE
        };
      }
    }

    public override void Init()
    {
      foreach (SoundData sound in this.Sounds)
        sound.Mute = false;
      this.Master.Volume = 100;
      this.BGM.Volume = 40;
      this.ENV.Volume = 80;
      this.SystemSE.Volume = 50;
      this.GameSE.Volume = 70;
    }

    public override void Read(string rootName, XmlDocument xml)
    {
      try
      {
        FieldInfo[] fieldInfos = this.FieldInfos;
        for (int index = 0; index < fieldInfos.Length; ++index)
        {
          string xpath = rootName + "/" + this.elementName + "/" + fieldInfos[index].Name;
          XmlNodeList xmlNodeList = xml.SelectNodes(xpath);
          if (xmlNodeList != null && xmlNodeList.Item(0) is XmlElement xmlElement)
            this.Sounds[index].Parse(xmlElement.InnerText);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
      }
    }

    public override void Write(XmlWriter writer)
    {
      FieldInfo[] fieldInfos = this.FieldInfos;
      writer.WriteStartElement(this.elementName);
      for (int index = 0; index < fieldInfos.Length; ++index)
      {
        writer.WriteStartElement(fieldInfos[index].Name);
        writer.WriteValue((string) this.Sounds[index]);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }
}
