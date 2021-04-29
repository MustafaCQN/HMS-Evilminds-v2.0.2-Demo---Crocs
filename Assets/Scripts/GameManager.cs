using HmsPlugin;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject MainPanel;
    public GameObject LeaderboardPanel;
    public GameObject AchievementsPanel;

    private void Start()
    {
        HMSGameManager.Instance.Init();
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("Game");
    }

    public void SignSucc(AuthAccount acc)
    {
        Debug.LogError("Account has been signed in: " + acc.DisplayName);
    }

    public void SignFail(HMSException ex)
    {
        Debug.LogError("Sign in Failed with This exception Code: " + ex.ErrorCode + " and this exception message: " + ex.Message);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowAchievements()
    {

        HMSAchievementsManager.Instance.OnShowAchievementsSuccess = AchSuc;
        HMSAchievementsManager.Instance.OnShowAchievementsFailure = AchFail;
        HMSAchievementsManager.Instance.ShowAchievements();
        
    }

    public void AchSuc()
    {
        Debug.LogError("Achievement Showing Success");
    }

    public void AchFail(HMSException ex)
    {
        Debug.LogError("Achievement Showing Failed, ex: " + ex.ErrorCode + " Message: " + ex.Message);

    }

    public void ShowLeaderboard()
    {

        HMSLeaderboardManager.Instance.ShowLeaderboards();
    }

}
