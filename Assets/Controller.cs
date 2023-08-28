using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(SerialController))]
public class Controller : MonoBehaviour
{
    public AudioSource[] sources;
    public AudioMixerGroup mixer;
    public AudioSlider[] sld;
    SerialController sc;

    [System.Serializable]
    struct AudioSetting {
        public float Min, Max, Floor;
    }

    float[] min, max, floor;
    AudioSetting[] settings;
    string dataPath = "/settings.dat";

    char[] toTrim = {'{', '}', ','};

    void Awake()
    {
        sc = GetComponent<SerialController>();
        dataPath = Application.persistentDataPath + dataPath;
    }
    
    void Start()
    {
        LoadData();

    }

    void Update()
    {
        string s = sc.ReadSerialMessage();
        if (s != null){
            s = s.Trim(toTrim);
            string[] arr = s.Split(',');
            for (int i = 0; i < arr.Length && i < sources.Length; i++){
                if (float.TryParse(arr[i], out float value)){

                    float modVal = Mathf.InverseLerp(sld[i].max.value, sld[i].min.value, value);
                    sources[i].volume = modVal;
                    sld[i].SetValue(modVal);
                }
            }
        }
    }

    IEnumerator PrintVolumes(){

        while(true){
        for (int i = 0; i < sources.Length; i++)
        {
        Debug.Log($"{sources[i].name} - {sources[i].volume} (min - {settings[i].Min}, max - {settings[i].Max})");
        }
        yield return new WaitForSeconds(5);
        }
    }

    void LoadData(){
        settings = new AudioSetting[sources.Length];
        if (File.Exists(dataPath)){
            string[] loadedData = File.ReadAllLines(dataPath);
            for (int i = 0; i < sources.Length; i++){
                settings[i] = JsonUtility.FromJson<AudioSetting>(loadedData[i]);
                sld[i].min.value = settings[i].Min;
                sld[i].max.value = settings[i].Max;
            }
        }
        else {
            for (int i = 0; i < sources.Length; i++){
                settings[i].Min = 0;
                settings[i].Max = 1;
                settings[i].Floor = .05f;
            }
            SaveData();
        }
    }

    public void SaveData(){
        string[] s = new string[sources.Length];
        for (int i = 0; i < sources.Length; i++){
            settings[i].Min = sld[i].min.value;
            settings[i].Max = sld[i].max.value;
            s[i] = JsonUtility.ToJson(settings[i]);
        }

        File.WriteAllLines(dataPath, s);
    }

}
