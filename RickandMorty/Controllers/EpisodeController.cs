using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RickandMorty.Models;
using System.Data.SqlClient;
using System.Data;

namespace RickandMorty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public EpisodeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("episode")]
        public string GetEpisodes()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("RickandMortyConn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Episode", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Episode> episodeList = new List<Episode>();
            Response res = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Episode episode = new Episode
                    {
                        Id = Convert.ToInt32(dt.Rows[i]["EpisodeID"]),
                        EpisodeName = Convert.ToString(dt.Rows[i]["EpisodeName"]),
                        AirDate = (DateTime)dt.Rows[i]["AirDate"],
                        EpisodeCode = Convert.ToString(dt.Rows[i]["EpisodeCode"]),

                    };
                    episodeList.Add(episode);
                }
            }
            if (episodeList.Count > 0)
            {
                return JsonConvert.SerializeObject(episodeList);
            }
            else
            {
                res.StatusCode = 100;
                res.ErrMessage = "No data found!";
                return JsonConvert.SerializeObject(res);
            }
        }
    }
}
