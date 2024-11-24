using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, Sprite> portraitData;

    string jsonText;
    JObject jObject;
    JArray greetingArray;
    JArray conversation;

    public Sprite[] portraitArr;
    void Awake()
    {
        string filePath = Application.dataPath + "/Scenes/juchan/TextReasources/NPC_1000.json";
        portraitData = new Dictionary<int, Sprite>();
        //GenerateData();

        jsonText = LoadJsonFile(filePath);
        if (jsonText == null)
        {
            Debug.LogError("JSON 파일이 존재하지 않습니다: " + filePath);
            return;
        }
        jObject = JObject.Parse(jsonText);
        greetingArray = (JArray)jObject["greeting"];
    }

    // Update is called once per frame
    void GenerateData()
    {
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
    }

    public JObject GetTalk(int scenid, int talkIndex)
    {
        conversation = (JArray)greetingArray[scenid];
        if (talkIndex == conversation.Count){
            return null;
                
        } else {
            return (JObject)conversation[talkIndex];
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

    string LoadJsonFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                string jsonText = File.ReadAllText(filePath);
                Debug.Log("JSON 파일 로드 성공: " + jsonText);
                return jsonText;
            }
            catch (Exception ex)
            {
                Debug.LogError("JSON 파일 로드 중 오류 발생: " + ex.Message);
            }
        }
        return null;
    }
}
