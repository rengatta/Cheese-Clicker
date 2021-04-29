using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{

    public TextMeshProUGUI leaderboardText;
    public GameObject leaderBoardRoot;
    public SaveManager saveManager;

    public void PopulateThenOpenLeaderboard() {

        saveManager.CreateSaveData();
        if (GlobalHelper.global.auth == null || saveManager.gameData.username == "Guest")
        {

            Debug.Log("Default instance is null");
            leaderboardText.text = "Could not reach authentication servers to populate leaderboard. (Not available in Guest mode)";
            leaderBoardRoot.SetActive(true);
            return;

        }
        

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users/" + GlobalHelper.global.auth.CurrentUser.UserId + "/saveData");

        string referenceString = "users/";

        List<SaveData> saveDataList = new List<SaveData>();



        FirebaseDatabase.DefaultInstance.GetReference(referenceString).GetValueAsync().ContinueWithOnMainThread(t => {

            leaderboardText.text = "Failed to populate leaderboard.";
            if (t.IsCanceled)
            {
                Debug.Log("FirebaseDatabaseError: IsCanceled: " + t.Exception);
                leaderBoardRoot.SetActive(true);
                return;
            }

            if (t.IsFaulted)
            {
                Debug.Log("FirebaseDatabaseError: IsFaulted:" + t.Exception);
                leaderBoardRoot.SetActive(true);
                return;
            }

            if (t.Result.Exists)
            {
                Debug.Log("Populating leaderboard.");
                DataSnapshot snapshot = t.Result;
    
                foreach(DataSnapshot child in snapshot.Children) {

                    DataSnapshot saveData = child.Child("saveData");

                    SaveData saveData1 = JsonUtility.FromJson<SaveData>(saveData.GetRawJsonValue());

                    saveDataList.Add(saveData1);
                }
                saveDataList = saveDataList.OrderByDescending(o => o.scoreData.totalCheeseCollected).ToList();
                string leaderboardContent = "";
                int maxLeaderBoardCount = 10;
                int j = 0;
                for (int i=0; i < saveDataList.Count; i++) {
                    leaderboardContent += (i + 1).ToString() + ".) " + saveDataList[i].username + ": " + saveDataList[i].scoreData.totalCheeseCollected + "\n";
                    j++;
                    if (j > maxLeaderBoardCount) break;
                }

              
                int nilPopulate = maxLeaderBoardCount - saveDataList.Count;
                if(nilPopulate > 0) {
                    for(int i=0; i < nilPopulate; i++) {
                        leaderboardContent += (i + 1 + saveDataList.Count).ToString() + ".) nil\n";

                    }

                }


                leaderboardText.text = leaderboardContent;
                leaderBoardRoot.SetActive(true);

            }
            else
            {
                Debug.Log("No compatible data exists.");
                leaderBoardRoot.SetActive(true);
            }

        });




    }
    
}
