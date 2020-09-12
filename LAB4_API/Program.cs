using Microsoft.EntityFrameworkCore.Storage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAB4_API
{
    class Program
    {
        //link do api
        public static Website API = new Website("https://api.collegefootballdata.com");
        //teamsy oraz advancedy dla nich
        public static async Task<string> _downloadTeamsFromAPI()
        {
            return await API.DownloadAsync("/teams/fbs");
        }
        public static async Task<string> _downloadAdvancedInfoFromAPI()
        {
            return await API.DownloadAsync("/stats/season/advanced?year=2010");
        }
        //dodaj info
        public static async Task<Teams> _addTeamToDB(Teams item, Context db, Advanced[] advancedDataDeserialized)
        {
            var tmp = new Teams
            {
                School = item.School,
                Abbreviation = item.Abbreviation,
                Conference = item.Conference,
                Divison = item.Divison,
                Color = item.Color,
                Alt_Color = item.Alt_Color,
                Team = await _getAdvancedInfoFromAPI(item.Conference, advancedDataDeserialized)
            };
            db.Teams.Add(tmp);
            return tmp;
        }
        //pobierz advanced data
        public static async Task<string> _getAdvancedInfoFromAPI(string _conferenceName, Advanced[] advancedDataDeserialized)
        {
            List<Advanced> advancedDataList = new List<Advanced>();

            
            foreach (var item in advancedDataDeserialized)
            {
                advancedDataList.Add(new Advanced
                {
                    Team = item.Team,
                    Conference = item.Conference,
                    Year = item.Year,
                    excludeGarbageTime = item.excludeGarbageTime,
                    startWeek = item.startWeek,
                    endWeek = item.endWeek
                });
            }
            return await _lookForMatch(_conferenceName, advancedDataList);
        }
        //znajdz wszystkie odpowiadajace tym druzynom conference i dodaj im parametry z advanceda
        public static async Task<string> _lookForMatch(string _Confa, List<Advanced> lista)
        {
            return lista
                            .Where(x => x.Conference == _Confa)
                            .Select(x => x.Team)
                            .FirstOrDefault();
        }
        public static async Task Main()
        {
            //String hourMinute = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine("Odpaliłem program!\n");
            using var db = new Context();
            db.Database.EnsureCreated();
            Console.WriteLine("Stworzylem baze!\n");
            var teams = await _downloadTeamsFromAPI();

            var deserializer = JsonSerializer.Deserialize<Teams[]>(teams, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            Console.WriteLine("Zdeserializowalem dane 1!\n");
            List<Task<Teams>> tasks = new List<Task<Teams>>();
            var advancedData = await _downloadAdvancedInfoFromAPI();
            var advancedDataDeserialized = JsonSerializer.Deserialize<Advanced[]>(advancedData, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            Console.WriteLine("Zdeserializowalem dane 2!\n");

            foreach (var item in deserializer)
            {
                tasks.Add(_addTeamToDB(item, db, advancedDataDeserialized));
                //db.Teams.Add(await _addTeamToDB(item));
           
            };
            await Task.WhenAll(tasks);
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            Console.WriteLine("Dodalem element do bazy...\n");
            db.SaveChanges();
            Console.WriteLine("Zapisalem dane do bazy!");
        }
    }
}
