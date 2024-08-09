using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Visualization : MonoBehaviour
{
    public TextAsset experimentData;

    [System.Serializable]
    public class DataLog
    {
        public ExperimentLog[] experimentLog;
    }

    [System.Serializable]
    public class ExperimentLog
    {
        public long ticks;
        public string peer;
        public string @event;
        public Arg arg1;
        public Arg arg2;
        public float? arg3;
    }

    [System.Serializable]
    public class Arg
    {
        public float x;
        public float y;
        public float z;
    }

    [Header("Player Prefab")]
    [SerializeField] GameObject playerPrefab;

    [Header("Game Objects")]
    [SerializeField] List<Camera> cameras;
    [SerializeField] List<GameObject> environment1;
    [SerializeField] List<GameObject> environment2;

    [Header("UI Screen")]
    [SerializeField] TMP_Dropdown playerDropdown;
    [SerializeField] TMP_Dropdown cameraDropdown;
    [SerializeField] Button startButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] TextMeshProUGUI condition;

    private List<ExperimentLog> logs;

    private bool isReplayPlayed;
    private bool isReplayPaused;

    // Start is called before the first frame update
    void Start()
    {
        startButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);

        isReplayPlayed = false;
        isReplayPaused = false;

        logs = new List<ExperimentLog>(JsonUtility.FromJson<DataLog>("{\"experimentLog\":" + experimentData.text + "}").experimentLog);
        if (logs != null)
        {
            logs.Sort((a, b) => a.ticks.CompareTo(b.ticks));
            StartCoroutine(ReadExperimentData(logs));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (logs != null)
        //{
        //    if (!isReplayPlayed && isReplayPaused)
        //    {
        //        isReplayPlayed = true;
        //        isReplayPaused = false;
        //        StartCoroutine(ReadExperimentData(logs));
        //    }
        //}
    }

    private IEnumerator ReadExperimentData(List<ExperimentLog> logs)
    {
        long startTime = logs[0].ticks;

        GameObject player1 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player1.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        GameObject player2 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player2.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

        Debug.Log("Begin Replay");

        int i = 0;

        foreach (ExperimentLog log in logs)
        {
            long elapsedTime = log.ticks - startTime;
            yield return new WaitForSeconds(elapsedTime / (float)TimeSpan.TicksPerSecond);

            if (log.@event == "Number 1")
            {
                if (log.arg1 != null)
                {
                    Vector3 newPosition = new Vector3(log.arg1.x, log.arg1.y, log.arg1.z);
                    player1.transform.position = newPosition;
                }
            }
            else if (log.@event == "Number 2")
            {
                if (log.arg1 != null)
                {
                    Vector3 newPosition = new Vector3(log.arg1.x, log.arg1.y, log.arg1.z);
                    player2.transform.position = newPosition;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(log.@event))
                {
                    char lastCharIndexing = log.@event[log.@event.Length - 1];

                    condition.text = "Condition: " + lastCharIndexing;
                    Debug.Log("Condition: " + lastCharIndexing);
                }
            }
        }
    }
}