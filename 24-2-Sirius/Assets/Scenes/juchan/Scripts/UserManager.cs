// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class UserManager : MonoBehaviour
// {
//     [SerializeField] TextMeshProUGUI talkText;
//     public TalkManager talkManager;
//     // Start is called before the first frame update
//     void Start()
//     {
//         Debug.Log(talkManager.GetTalk(0, 0));
//         GetTalk();
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     public void GetTalk()
//     {
//         string Name = "";
//         string Talk = "";
//         foreach(var property in talkManager.GetTalk(0, 0).Properties())
//         {
//             Name = property.Name.ToString();
//             Talk = property.Value.ToString();
//         }
//         talkText.text = Name + " : " + Talk;
//     }
// }
