using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using OpenAI_API.Models;

public class OpenAIController : MonoBehaviour
{
    public Text textField;
    public InputField inputField;
    public Button okBtn;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    public float clearTime;
    public float hitCount;
    public float hpMul;
    public float atkMul;
    public float nextHpMul;
    public float nextAtkMul;
    void Start()
    {
        api = new OpenAIAPI("");
        clearTime = 10;
        hitCount = 10;
        hpMul = 100;
        atkMul = 100;
        StartConversation();
        okBtn.onClick.AddListener(() => GetResponse());
    }
    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "너는 게임을 밸런싱하는 시스템이야. " +
            "이 게임의 평균 클리어 타임은 15분이고, 클리어한 게임의 평균 피격 횟수는 15회야. " +
            "평균 클리어타임보다 짧게 클리어하거나 피격횟수가 평균보다 낮으면 체력배율과 공격력배율을 올리고, " +
            "평균 클리어타임보다 오래 걸려서 클리어하거나 피격횟수가 평균보다 많다면 체력배율과 공격력배율을 낮추는 식으로 밸런싱해줘. " +
            " 만약 너가 클리어 타임, 피격횟수, 몬스터의 현재 체력배율, 몬스터의 현재 공격력배율을 받으면, " +
            " 너는 평균 클리어 타임과 피격 횟수를 입력받은 클리어 타임과 피격횟수를 비교해서 " +
            " 클리어타임과 피격횟수는 말하지말고 '다음 체력배율 : 체력배율 / 다음 공격력배율 : 공격력배율'의 형식을 꼭 맞추고 모든 대답을 단답형으로 하도록해." +
            "'클리어타임 : x / 피격횟수 : x / 체력배율 : x / 공격력배율 : x'의 형식이 아닌 다른 입력을 받으면 '입력 형식이 잘못되었습니다.' 라고 출력해.")
        };

        inputField.text = "클리어타임 :" + clearTime + "분 / 피격횟수 : "+ hitCount + "회 / 체력배율 : " + hpMul + "% / 공격력배율 : " + atkMul + "% ";
        GetResponse();
    }

    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        //버튼 Disable
        okBtn.enabled = false;

        //유저 메세지에 inputField를
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            userMessage.Content = userMessage.Content.Substring(0, 100);
        }
        Debug.Log(string.Format("{0} : {1}", userMessage.Role, userMessage.Content));

        //list에 메세지 추가
        messages.Add(userMessage);

        //textField에 userMessage표시 
        textField.text = string.Format("You: {0}", userMessage.Content);

        //inputField 초기화
        inputField.text = "";

        // 전체 채팅을 openAI 서버에전송하여 다음 메시지(응답)를 가져오도록
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 200,
            Messages = messages
        });

        //응답 가져오기
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        // 응답에서 데이터 값 추출
        int firstColonIndex = responseMessage.Content.IndexOf(":") + 2;
        int percentIndex = responseMessage.Content.IndexOf("%", firstColonIndex);
        nextHpMul = float.Parse(responseMessage.Content.Substring(firstColonIndex, percentIndex - firstColonIndex));
        int secondColonIndex = responseMessage.Content.IndexOf(":", firstColonIndex + 1) + 2;
        percentIndex = responseMessage.Content.IndexOf("%", secondColonIndex);
        nextAtkMul = float.Parse(responseMessage.Content.Substring(secondColonIndex, percentIndex - secondColonIndex));   
        Debug.Log("nextHpMul 값은 ? " + nextHpMul + " / " + firstColonIndex);
        Debug.Log("nextAtkMul 값은 ? " + nextAtkMul + " / " + secondColonIndex);

        //응답을 message리스트에 추가
        messages.Add(responseMessage);

        //textField를 응답에 따라 Update
        textField.text = string.Format("You: {0}\n\nChatGPT:\n{1}", userMessage.Content, responseMessage.Content);

        //Okbtn다시 활성화
        okBtn.enabled = true;
    }

}