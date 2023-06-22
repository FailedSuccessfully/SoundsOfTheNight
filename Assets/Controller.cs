using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SerialController))]
public class Controller : MonoBehaviour
{
    public AudioSource[] sources;
    SerialController sc;

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
                    sources[i].volume = value;
                }
            }
        }
    }
}
