using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace VenkatBalaOLO
{
    class Program
    {
        private static List<oloorders> DownloadAndDeserializeJsonData(string url) 
        {
            using (var webClient = new WebClient())
            {
                var jsonData = string.Empty;
                try
                {
                    jsonData = webClient.DownloadString(url);
                }
                catch (Exception) { }
                
                return (List<oloorders>)JsonConvert.DeserializeObject<List<oloorders>>(jsonData);


            }
        }

        static void Main(string[] args)
        {
           
            List<oloorders> oloorderslist = DownloadAndDeserializeJsonData("http://files.olo.com/pizzas.json");
                        
            var orderhavingmorethanonetoppings = from d in oloorderslist where d.toppings.Count() > 1 select d;
            List<string> SortedconcatinatedToppingsList = new List<string>();
            foreach (oloorders oloorder in orderhavingmorethanonetoppings)
            {
                SortedconcatinatedToppingsList.Add(string.Join("+", (oloorder.toppings.OrderBy(t => t.ToString()).ToArray())));               
                
            }

            var top20List = (from r in SortedconcatinatedToppingsList
                      orderby r.ToString()
                     group r by r.ToString() into grp
                     select new { key = grp.Key, cnt = grp.Count() }).OrderByDescending(c => c.cnt).Take(20);

            //print the top 20 topping combinations
            foreach(var orders in top20List)
            {
                Console.Write(orders.key + " " + orders.cnt + Environment.NewLine);
            }

            Console.ReadKey();

        }

        public class oloorders
        {
            public List<string> toppings { get; set; }
        }
    }
}
