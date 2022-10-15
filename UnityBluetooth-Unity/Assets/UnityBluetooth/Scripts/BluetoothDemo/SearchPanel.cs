using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JC.BluetoothUnity.Demo
{
    public class SearchPanel : MonoBehaviour
    {    
        [SerializeField] SearchResult _resultTemplate;
        [SerializeField] Button _startSearchButt;
        [SerializeField] InputField _pinInputField;

        List<SearchResult> _populatedResult = new List<SearchResult>();

        // Start is called before the first frame update
        void Start()
        {
            _resultTemplate.gameObject.SetActive(false);
            _startSearchButt.onClick.AddListener(StartSearching);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void StartSearching()
        {
            BluetoothManager.StartDiscovery();
            BluetoothManager.Toast("Start Searching...");
            StopAllCoroutines();
            StartCoroutine(PopulateListAsync());
        }

        IEnumerator PopulateListAsync()
        {
            while(true)
            {
                // Get List
                string[] deviceList = BluetoothManager.GetAvailableDevices();
                // string[] deviceList = BluetoothManager.StrArrTest(Random.Range(2,9)); // test


                // Delete Old List
                _populatedResult.ForEach(r => Destroy(r.gameObject));
                _populatedResult.Clear();

                // Populate List
                foreach(string deviceRawString in deviceList)
                {
                    string[] deviceStrs = deviceRawString.Split('|'); // split with '|'
                    var newResult = Instantiate(_resultTemplate, _resultTemplate.transform.parent);
                    newResult.gameObject.SetActive(true);
                    newResult.Init(deviceStrs[0], deviceStrs[1], _pinInputField);
                    _populatedResult.Add(newResult);
                }

                // update every 1s
                // print("Updated device list. count="+deviceList.Length);
                yield return new WaitForSecondsRealtime(1);
            }
        }
    }
}