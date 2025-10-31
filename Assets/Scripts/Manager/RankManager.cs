using System.IO;
using System.Threading.Tasks;
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

    public class RankManager : SingletonBase<RankManager>
    {
        public List<Rank> Ranks { get; private set; } = new();
        private string savePath;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "rank.json");
            LoadRanks();
        }

        #region Basic Rank Methods
        // 랭크 추가
        public void AddRank()
        {
            string id = GameManager.Instance.idInputField.text;
            string pw = GameManager.Instance.pwInputField.text;
            float score = GameManager.Instance.Score;

            if (id == "" || pw == "")
                return;

            // 기존 유저 검색
            Rank user = Ranks.Find(r => r.ID == id);

            if (user == null)
            {
                // 신규 유저 등록
                var newRank = new Rank(id, pw, score);
                Ranks.Add(newRank);
            }
            else
            {
                // 점수 비교 후 높은 점수만 갱신
                if (user.PW == pw && score > user.Score)
                {
                    user.Score = score;
                }
            }

            SaveRanks();

            SortRanks();
        }

        // JSON 저장
        public void SaveRanks()
        {
            string json = JsonConvert.SerializeObject(Ranks, Formatting.Indented);
            File.WriteAllText(savePath, json);
        }

        // JSON 로드
        private void LoadRanks()
        {
            if (!File.Exists(savePath))
                return;

            string json = File.ReadAllText(savePath);
            Ranks = JsonConvert.DeserializeObject<List<Rank>>(json) ?? new List<Rank>();

            SortRanks();
        }
        #endregion

        #region Helper Methods
        // 정렬 (내림차순)
        private void SortRanks()
        {
            Ranks.Sort((a, b) => b.Score.CompareTo(a.Score));
        }

        // ID 중복 검사
        public bool IsIdExists(string id)
        {
            return Ranks.Exists(r => r.ID == id);
        }

        // 계정 검색 (점수 + 랭킹 반환)
        public (float score, int rank) GetAccountInfo(string id, string pw)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                var r = Ranks[i];
                if (r.ID == id && r.PW == pw)
                {
                    return (r.Score, i);
                }
            }

            return (-1.0f, -1);
        }

        // 전체 랭킹 조회
        public List<Rank> GetAllRanks() => new List<Rank>(Ranks);

        // 상위 N명 조회
        public List<Rank> GetTopRanks(int count = 5)
        {
            return Ranks.Count > count ? Ranks.GetRange(0, count) : new List<Rank>(Ranks);
        }

        // 전체 초기화
        public async Task ClearRanksAsync()
        {
            Ranks.Clear();
            if (File.Exists(savePath))
                File.Delete(savePath);

            await Task.Yield();
            Debug.Log("[RankManager] 랭크 데이터 초기화 완료.");
        }
        #endregion
    }
}
