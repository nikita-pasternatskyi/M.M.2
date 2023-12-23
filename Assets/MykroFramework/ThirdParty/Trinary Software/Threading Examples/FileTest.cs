using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace MEC
{
    public class FileTest : MonoBehaviour
    {
        private const string SaveString =
            "THIS IS A STRING OF TEXT TO SAVE TO DISK. WHEN DRINKS ARE MADE OF PURPLE MIST. WHEN SOMETHING COLD TOUCHES YOUR WRIST. " +
            "WHEN DATA IS SAVED INTO A LIST. I THINK NOW YOU GET THE JIST. I'M BASICALLY DONE WRITING THIS.";

        private void Start()
        {
            int num = 1;
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
            Timing.RunCoroutine(_Save(num++));
        }

        private IEnumerator<float> _Save(int num)
        {
            string filePath = Application.persistentDataPath + "/SavedFile" + num + ".txt";
            Debug.Log("Attempting to save " + filePath);
            float number = Random.Range(0.2f, 0.3f);


            yield return Threading.SwitchToExternalThread((System.Threading.ThreadPriority)Random.Range(0, 5));

            #region External Thread

            // NOTE: Sleeping here is not nessessary, it's just done here to SIMULATE writing a huge file.
            Threading.Sleep(number);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(SaveString);
            writer.Close();

            #endregion

            yield return Threading.SwitchBackToGUIThread;

            Debug.Log("Saved " + filePath);
        }
    }
}
