using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json;

namespace Yunash.Data
{
    public interface IDataService
    {
        void SaveData<T>(T dataObject, string fileName) where T : class;
        bool TryLoadData<T>(string fileName, out T dataObject) where T : class;
        AudioData AudioData { get; }
    }

    public class DataManager : MonoBehaviour, IDataService
    {
        [SerializeField] private AudioData audioData;

        public AudioData AudioData => audioData;

        private void Awake()
        {
            if (audioData == null)
                throw new NullReferenceException("One or more Data File(s) missing.");
            
        }

        public void SaveData<T>(T dataObject, string fileName) where T : class
        {
            string json = JsonConvert.SerializeObject(dataObject, Newtonsoft.Json.Formatting.Indented);

            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);

            Debug.Log($"Saved {fileName} JSON to path{path}");
        }

        public bool TryLoadData<T>(string fileName, out T dataObject) where T : class
        {
            dataObject = null;

            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                dataObject = JsonConvert.DeserializeObject<T>(json);

                return true;
            }
            else
                return false;
        }
    }

}
