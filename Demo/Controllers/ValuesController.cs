using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;
using FatRabbit.Bat;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private BatRequest _batRequest;

        public ValuesController(BatRequest batRequest)
        {
            _batRequest = batRequest;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<List<Dog>> Get(int id)
        {
           
            if (id == 1)
            {
                var g = await _batRequest.GetResponseEntityAsync<List<Dog>>("http://Demo/Api/Values/GetDog");

                return g.Body;

                
            }
            else
            {
                Dog dog = new Dog()
                {
                    Name = "蟾蜍",
                    Age = 2,
                    Sex = true
                };
                var p = await _batRequest.PostResponseEntityAsync<List<Dog>>("http://Demo/Api/Values/PostDog",dog);

                return p.Body;
            }

           
        }

        // POST api/values
        [HttpPost("[Action]")]
        public List<Dog> PostDog([FromBody] Dog value)
        {

            Dog dog = new Dog()
            {
                Name = "欧阳锋是蛤蟆精",
                Age = 2,
                Sex = true
            };
            List<Dog> dogs = new List<Dog>() { value, dog };
            return dogs;
        }
        [HttpGet("[Action]")]
        public List<Dog> GetDog()
        {

            Dog dog = new Dog()
            {
                Name = "欧阳锋是蛤蟆精",
                Age = 2,
                Sex = true
            };
            List<Dog> dogs = new List<Dog>() {  dog };
            return dogs;
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
