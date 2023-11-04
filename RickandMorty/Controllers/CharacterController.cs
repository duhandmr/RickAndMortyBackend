using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RickandMorty.Models;
using System.Data;
using System.Data.SqlClient;

namespace RickandMorty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public CharacterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("character")]
        public string GetCharacters() 
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("RickandMortyConn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Character", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Character> characterList = new List<Character>();
            Response res = new Response();
            if (dt.Rows.Count > 0 )
            {
                for(int i=0; i<dt.Rows.Count; i++)
                {
                    Character character = new Character
                    {
                        Id = Convert.ToInt32(dt.Rows[i]["CharacterID"]),
                        CharacterName = Convert.ToString(dt.Rows[i]["CharactarName"]),
                        Status = Convert.ToString(dt.Rows[i]["Status"]),
                        Species = Convert.ToString(dt.Rows[i]["Species"]),
                        Gender = Convert.ToString(dt.Rows[i]["Gender"])
                        
                    };
                    characterList.Add(character);
                }
            }
            if (characterList.Count > 0)
            {
                return JsonConvert.SerializeObject(characterList);
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
