using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[RequireComponent(typeof(SerialController))]
public class Controller : MonoBehaviour
{
    public AudioSource[] sources;
    public AudioMixerGroup mixer;
    SerialController sc;

    float[] min, max;

    char[] toTrim = {'{', '}', ','};

    void Awake()
    {
        sc = GetComponent<SerialController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (AudioSource source in sources)
        {
            source.volume = 0;
        }
        min = new float[sources.Length];
        max = new float[sources.Length];
        for (int i = 0; i < sources.Length; i++){
            min[i] = 1;
            max[i] = 0;
        }

        StartCoroutine(PrintVolumes());
    }

    // Update is called once per frame
    void Update()
    {
        string s = sc.ReadSerialMessage();
        if (s != null){
            Debug.Log(s);
            s = s.Trim(toTrim);
            string[] arr = s.Split(',');
            for (int i = 0; i < arr.Length && i < sources.Length; i++){
                if (float.TryParse(arr[i], out float value)){
                    if (value < min[i]){
                        min[i] = value;
                    }
                    if (value > max[i]){
                        max[i] = value;
                    }
                    sources[i].volume =  Mathf.InverseLerp(max[i], min[i], value); //map from max to min because of art designer skill issue

                    if (sources[i].volume < .05f){
                        sources[i].volume = 0;
                    }
                }
            }
        }
    }

    IEnumerator PrintVolumes(){

        while(true){
        for (int i = 0; i < sources.Length; i++)
        {
            Debug.Log($"{sources[i].name} - {sources[i].volume} (min - {min[i]}, max - {max[i]})");
        }
        yield return new WaitForSeconds(5);
        }
    }
}
