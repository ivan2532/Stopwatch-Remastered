using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Status
{
    Running,
    Paused,
    Stopped
}

public class StopwatchController : MonoBehaviour
{
    private const string LAST_MEASUREMENT_KEY = "last_measurement";

    private Status currentStatus = Status.Stopped;

    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;

    [SerializeField] private TextMeshProUGUI currentStatusText;
    [SerializeField] private TextMeshProUGUI currentMeasurementText;
    [SerializeField] private TextMeshProUGUI lastMeasurementText;

    private float timer = -1.0f;

    private void Start() => ReadLastMeasurement();

    private void Update()
    {
        if(currentStatus == Status.Running)
        {
            timer += Time.deltaTime;
            currentMeasurementText.text = FormatTime(timer);
        }
    }

    private static string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600.0f);
        int minutes = Mathf.FloorToInt(timeInSeconds % 3600.0f / 60.0f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60.0f);

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        return formattedTime;
    }

    public void ChangeStatus(Status newStatus)
    {
        if (currentStatus == newStatus)
            return;

        if(newStatus == Status.Running)
        {
            startButton.interactable = false;
            pauseButton.interactable = true;
            stopButton.interactable = true;

            currentStatusText.text = "Status: Running";
            currentStatusText.color = Color.green;
            currentMeasurementText.color = Color.green;

            if (currentStatus == Status.Stopped)
                timer = 0.0f;

            currentStatus = Status.Running;
        }
        else if (newStatus == Status.Paused)
        {
            startButton.interactable = true;
            pauseButton.interactable = false;
            stopButton.interactable = true;

            currentStatusText.text = "Status: Paused";
            currentStatusText.color = Color.cyan;
            currentMeasurementText.color = Color.cyan;

            currentStatus = Status.Paused;
        }
        else if(newStatus == Status.Stopped)
        {
            startButton.interactable = true;
            pauseButton.interactable = false;
            stopButton.interactable = false;

            currentStatusText.text = "Status: Stopped";
            currentStatusText.color = Color.gray;
            currentMeasurementText.text = "00:00:00";
            currentMeasurementText.color = Color.gray;

            RecordLastMeasurement(FormatTime(timer));

            timer = -1.0f;

            currentStatus = Status.Stopped;
        }
    }

    public void ReadLastMeasurement() => lastMeasurementText.text = PlayerPrefs.GetString(LAST_MEASUREMENT_KEY, lastMeasurementText.text);

    public void RecordLastMeasurement(string formattedResult)
    {
        lastMeasurementText.text = formattedResult;
        PlayerPrefs.SetString(LAST_MEASUREMENT_KEY, formattedResult);
    }

    public void StartButton() => ChangeStatus(Status.Running);
    public void PauseButton() => ChangeStatus(Status.Paused);
    public void StopButton() => ChangeStatus(Status.Stopped);
}
