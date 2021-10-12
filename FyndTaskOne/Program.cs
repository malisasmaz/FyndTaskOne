using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FyndTaskOne
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine(p.ExtractData());
        }

        public string ExtractData()
        {
            var html = GetFileData();
            var response= ParseHtml(html);
            return response;
        }

        public string GetFileData()
        {
            string path = GetFilePath();

            // Open the file to read from.
            string readText = File.ReadAllText(path);

            return readText;
        }

        public string GetFilePath()
        {
            string workingDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string path = Path.Combine(workingDirectory, @"Resources\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html");
            CheckFilePath(path);

            return path;
        }

        public void CheckFilePath(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File does not exist!");
            }
        }

        public  string ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var hotelName = GetFirstNodeText(htmlDoc, "//*[@id='hp_hotel_name']");
            var address = GetFirstNodeText(htmlDoc, "//*[@id='hp_address_subtitle']");

            var starText = htmlDoc.DocumentNode.SelectNodes("//*[@class='hp__hotel_ratings__stars hp__hotel_ratings__stars__clarification_track']//i").FirstOrDefault()?.GetAttributes("class").FirstOrDefault()?.Value;
            var toBeSearched = "b-sprite stars ratings_stars_";
            var stars = starText.Substring(starText.IndexOf(toBeSearched) + toBeSearched.Length, 1);

            var reviewPoints = GetFirstNodeText(htmlDoc, "//*[@class='big_review_score_detailed js-big_review_score_detailed ind_rev_total hp_review_score']//span//span");
            var numberOfReviews = GetFirstNodeText(htmlDoc, "//*[@class='trackit score_from_number_of_reviews']//*[@class='count']");

            StringBuilder sbDescription = new StringBuilder();
            htmlDoc.DocumentNode.SelectNodes("//*[@class='hotel_description_wrapper_exp ']//p").ToList().ForEach(x => sbDescription.Append(x.InnerText.Trim()));
            var description = sbDescription.ToString();

            StringBuilder sbRoomCategories = new StringBuilder();
            var roomCategories = htmlDoc.DocumentNode.SelectNodes("//*[@class='ftd']").ToList().Select(x => x.InnerText.Trim())?.ToList();

            var alternativeHotels = htmlDoc.DocumentNode.SelectNodes("//*[@id='js--lastViewedList']//p//a").ToList().Select(x => x.InnerText.Trim())?.ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(
                new
                {
                    hotelName = hotelName,
                    address = address,
                    classificationStars = stars,
                    reviewPoints = reviewPoints,
                    numberOfReviews = numberOfReviews,
                    description = description,
                    roomCategories = roomCategories,
                    alternativeHotels = alternativeHotels
                });
        }


        public  string GetFirstNodeText(HtmlDocument htmlDoc, string path)
        {
            try
            {
                return htmlDoc.DocumentNode.SelectNodes(path).FirstOrDefault()?.InnerText?.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Field not found!" + ex.Message);
            }
        }
    }
}
