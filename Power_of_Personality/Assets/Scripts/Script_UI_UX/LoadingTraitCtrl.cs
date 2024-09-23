
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Newtonsoft.Json; // JSON 파싱용

// JSON 데이터 구조 정의
[System.Serializable]
public class Quote
{
    public string author;
    public string text;
}

// 전체 JSON 파일 구조
[System.Serializable]
public class QuoteSheet
{
    public List<Quote> quotes;
}

public class LoadingTraitCtrl : MonoBehaviour
{
    public TextMeshProUGUI innerText;
    private QuoteSheet quoteSheet;

    void Start()
    {
        // JSON 파일 로드 및 파싱
        LoadQuotesFromJson();

        // 랜덤한 명언 선택 및 표시
        if (quoteSheet != null && quoteSheet.quotes.Count > 0)
        {
            DisplayRandomQuote();
        }
        else
        {
            Debug.LogError("JSON data 가 없음.");
        }
    }

    // JSON 파일을 로드하고 데이터를 파싱하는 함수
    private void LoadQuotesFromJson()
    {
        // Resources 폴더에서 JSON 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/LoadingTraitdata");

        if (jsonFile != null)
        {
            string jsonData = jsonFile.text;
            quoteSheet = JsonConvert.DeserializeObject<QuoteSheet>(jsonData);
        }
        else
        {
            Debug.LogError("JSON file 이 없음.");
        }
    }

    // 랜덤한 명언을 선택하고 TextMeshPro에 표시하는 함수
    private void DisplayRandomQuote()
    {
        int randomIndex = Random.Range(0, quoteSheet.quotes.Count);
        Quote randomQuote = quoteSheet.quotes[randomIndex];

        innerText.text = randomQuote.text;
    }
}