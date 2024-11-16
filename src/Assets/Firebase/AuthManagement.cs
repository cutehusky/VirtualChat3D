using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
public class AuthManagement : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Register Information")]
    [SerializeField]TMP_InputField registerEmail;
    [SerializeField]TMP_InputField registerPassword;
    [SerializeField]TMP_InputField registerPasswordValid;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");  
            }
        });
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(registerEmail.text, registerPassword.text, registerPasswordValid.text));
    }
    
    private IEnumerator RegisterAsync(string email, string password, string passwordValid)
    {
        if(password != passwordValid)
        {
            Debug.LogError("Password does not match");
            yield break;
        }
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);
        if(registerTask.Exception != null)
        {
            Debug.LogError($"Register failed due to error: {registerTask.Exception}");
            yield break;
        }
        Debug.LogError("Registered successfully!");
    }
}
