using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, Sprite> portraitData;

    string jsonText;
    JObject jObject;
    JArray greetingArray;
    JArray refuseArray;
    JArray tradeArray;
    JArray bargainArray;
    JArray conversation;
    JArray rerequestArray;
    JArray angryArray;
    JArray overrequestArray;

    public Sprite[] portraitArr;

    private int currentGreetingIndex;

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
        refuseArray = (JArray)jObject["refuse"];
        tradeArray = (JArray)jObject["trade"];
        bargainArray = (JArray)jObject["bargain"];
        rerequestArray = (JArray)jObject["rerequest"];
        angryArray = (JArray)jObject["angry"];
        overrequestArray = (JArray)jObject["overrequest"];

        currentGreetingIndex = 0;
    }

    // Update is called once per frame
    void GenerateData()
    {
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
    }
    public void reset()
    {
        currentGreetingIndex = 0;
    }

    public bool GetTalk(int scenid, int talkIndex, int talkType, out string name, out string comment)//type 0:refuse, 1:trade, 2:bargain, 3:rerequest, 4:angry
    {
        name = "???";
        comment = "...";

        JArray array;

        if (talkType == 0)
        {
            array = refuseArray;
        }
        else if (talkType == 1)
        {
            array = tradeArray;
        }
        else if (talkType == 2)
        {
            array = bargainArray;
        }
        else if (talkType == 3)
        {
            array = rerequestArray;
        }
        else if (talkType == 4)
        {
            array = angryArray;
        }
        else if (talkType == 5)
        {
            array = overrequestArray;
        }
        else
        {
            return false;
        }

        if (talkIndex >= conversation.Count || scenid >= array.Count)
        {

            return false;

        }
        else
        {
            conversation = (JArray)array[scenid];

            var talkProperty = ((JObject)conversation[talkIndex]).Properties().First();
            string _name = talkProperty.Name;
            string talkText = talkProperty.Value.ToString();

            string processedText = talkText.Split("/")[0];
            int portraitNum = int.Parse(talkText.Split("/")[1]);

            name = _name;

            comment = processedText;

            return true;
        }
    }

    public bool GetGreetingTalk(out string name, out string comment)
    {
        comment = "";
        name = "";
        conversation = (JArray)greetingArray[0];

        if (currentGreetingIndex == conversation.Count)
        {
            currentGreetingIndex = 0; //reset
            return false;

        }
        else
        {
            var talkProperty = ((JObject)conversation[currentGreetingIndex]).Properties().First();
            string _name = talkProperty.Name;
            string talkText = talkProperty.Value.ToString();

            string processedText = talkText.Split("/")[0];
            int portraitNum = int.Parse(talkText.Split("/")[1]);

            name = _name;
            comment = processedText;

            currentGreetingIndex++;

            return true;
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
