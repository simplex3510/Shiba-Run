using System.Collections.Generic;
using System.IO;

using UnityEngine;

using Singleton;
using System;

namespace Manager
{
    [System.Serializable]
    public class RankData
    {
        public string ID { get; private set; }
        public string PW { get; private set; }

        public float Score { get; private set; }

        public DateTime Date { get; private set; }

        public RankData(string id, string pw, float score, DateTime date)
        {
            ID = id;
            PW = pw;
            Score = score;
            Date = date;
        }
    }

    [System.Serializable]
    public class DailyRank
    {
        public string Date; // yyyyMMdd
        public List<RankData> Ranks = new List<RankData>();
    }

    public class RankManager : SingletonBase<RankManager>
    {
        private Dictionary<string, DailyRank> dailyRanks = new Dictionary<string, DailyRank>();
        private string savePath;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "rank.json");
            LoadRanks();
        }

        // 랭크 추가
        public void AddRank(RankData rank)
        {
            string dateKey = rank.Date.ToString("yyyyMMdd");

            if (!dailyRanks.TryGetValue(dateKey, out var dailyRank))
            {
                dailyRank = new DailyRank { Date = dateKey };
                dailyRanks[dateKey] = dailyRank;
            }

            dailyRank.Ranks.Add(new RankData
            (
                rank.ID,
                rank.PW,
                rank.Score,
                rank.Date
            ));

            SaveRanks();
        }

        // 특정 날짜 랭크 조회
        // public List<RankData> GetRanksByDate(string yyyyMMdd)
        // {
        //     if (dailyRanks.TryGetValue(yyyyMMdd, out var dailyRank))
        //         return dailyRank.Ranks;

        //     return new List<RankData>();
        // }

        // JSON 저장
        private void SaveRanks()
        {
            var allRanks = new List<DailyRank>(dailyRanks.Values);
            string json = JsonUtility.ToJson(new Wrapper<DailyRank> { Items = allRanks }, true);
            File.WriteAllText(savePath, json);
        }

        // JSON 로드
        private void LoadRanks()
        {
            if (!File.Exists(savePath))
                return;

            string json = File.ReadAllText(savePath);
            var wrapper = JsonUtility.FromJson<Wrapper<DailyRank>>(json);

            dailyRanks.Clear();
            foreach (var daily in wrapper.Items)
            {
                dailyRanks[daily.Date] = daily;
            }
        }

        // JsonUtility는 List 직렬화를 직접 지원하지 않으므로 Wrapper 사용
        [Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}
