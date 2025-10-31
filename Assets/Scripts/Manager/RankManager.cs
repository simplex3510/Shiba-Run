using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;

using Singleton;

namespace Manager
{
    [System.Serializable]
    public class Rank
    {
        public string ID { get; set; }
        public string PW { get; set; }
        public float Score { get; set; }

        public Rank(string id, string pw, float score)
        {
            ID = id;
            PW = pw;
            Score = score;
        }
    }

    [System.Serializable]
    public class DailyRank
    {
        public List<Rank> Ranks { get; set; } = new List<Rank>();
    }

    public class RankManager : SingletonBase<RankManager>
    {
        private Dictionary<string, DailyRank> dailyRankDic = new Dictionary<string, DailyRank>();
        private string savePath;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "rank.json");
            Debug.Log($"[RankManager] Save Path: {savePath}");
            LoadRanks();
        }

        // 랭크 추가
        public void AddRank()
        {
            string dailyKey = DateTime.Now.ToString("yyyyMMdd");

            if (!dailyRankDic.TryGetValue(dailyKey, out var dailyRank))
            {
                dailyRank = new DailyRank();
                dailyRankDic[dailyKey] = dailyRank;
            }

            var rank = new Rank
            (
                GameManager.Instance.idInputField.text,
                GameManager.Instance.pwInputField.text,
                GameManager.Instance.Score
            );

            // rank is null
            dailyRank.Ranks.Add(rank);

            SaveRanks();
        }

        // JSON 저장
        public void SaveRanks()
        {
            var json = JsonConvert.SerializeObject(dailyRankDic, Formatting.Indented);
            File.WriteAllText(savePath, json);
        }

        // JSON 로드
        private void LoadRanks()
        {
            if (!File.Exists(savePath))
                return;

            string json = File.ReadAllText(savePath);
            dailyRankDic = JsonConvert.DeserializeObject<Dictionary<string, DailyRank>>(json)
                           ?? new Dictionary<string, DailyRank>();
        }
        
        // 특정 날짜 랭크 조회
        public List<Rank> GetRanksByDate(string yyyyMMdd)
        {
            if (dailyRankDic.TryGetValue(yyyyMMdd, out var dailyRank))
                return dailyRank.Ranks;

            return  new List<Rank>();
        }
    }
}
