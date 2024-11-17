using System.Collections;
using System.Collections.Generic;
using Core.FirebaseDatabaseModule.Model;
using Core.MVC;
using Core.UserAccountModule.Model;
using Firebase;
using Firebase.Database;
using UnityEngine;
using Firebase.Extensions;
using QFramework;

public class FirebaseStart : ControllerBase
{
    public FirebaseApp app;
    
    public override void OnInit(ViewBase view)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase started");
                this.GetModel<FirebaseRealTimeDatabaseModel>().InitFirebase();
                this.GetModel<FirebaseAuthModel>().InitFirebase();
                this.GetModel<FirebaseStorageModel>().InitFirebase();
            } else {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
}
