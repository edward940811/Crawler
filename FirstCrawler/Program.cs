using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using CsvHelper;

namespace ConsoleApplication_HtmlAgilityPack
{
    public class htmlelement
    {
        public htmlelement(){
                
            }
        public int Id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string date { get; set; }
    }
    class Program
    {
        static int getPages(string urls)
        {
            WebClient url = new WebClient();
            MemoryStream initialPageMs = new MemoryStream(url.DownloadData(urls));
            // 使用預設編碼讀入 HTML 
            HtmlDocument initdoc = new HtmlDocument();
            initdoc.Load(initialPageMs, Encoding.Default);

            HtmlNode pageNode = initdoc.DocumentNode.SelectSingleNode("//div[@class ='page']//span[1]");
            int pages = Int32.Parse(pageNode.InnerText);
            return pages;
        }
        static List<htmlelement> Craw( string weburl,int pages)
        {
            WebClient url = new WebClient();
            List<htmlelement> list_type = new List<htmlelement>();

            for (int currentPageNumber = 1; currentPageNumber <= pages; currentPageNumber++)
            {
                string pageurl = weburl + "&pn" + currentPageNumber;
                MemoryStream Ms = new MemoryStream(url.DownloadData(pageurl));
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Ms, Encoding.Default);


                HtmlNodeCollection tableNodes = doc.DocumentNode.SelectNodes("//table[@class='listTable']/tbody//tr");


                foreach (HtmlNode nodes in tableNodes)
                {
                    htmlelement item = new htmlelement();
                    item.Id = Int32.Parse(nodes.ChildNodes[0].InnerText);
                    string[] Title = nodes.ChildNodes[1].InnerText.Split('(');
                    string s = WebUtility.HtmlDecode(Title[0]);
                    item.title = s;
                    item.date = nodes.ChildNodes[2].InnerText;
                    item.link = nodes.ChildNodes[1].SelectSingleNode("./a").Attributes["href"].Value;
                    list_type.Add(item);

                }
              
            }
            return list_type;
        }
        static void Main(string[] args)
        {
            //指定來源網頁
            //WebClient url = new WebClient();
            //將網頁來源資料暫存到記憶體內
            string LawWeburl = "https://www.fda.gov.tw/TC/law.aspx?cid=62&pClass=01";
            string LawOrderWeburl = "https://www.fda.gov.tw/TC/law.aspx?cid=62&pClass=02";
            string RulesWeburl = "https://www.fda.gov.tw/TC/law.aspx?cid=62&pClass=03";
            //MemoryStream initialPageMs = new MemoryStream(url.DownloadData(LawWeburl));
            

            // 使用預設編碼讀入 HTML 
            //HtmlDocument initdoc = new HtmlDocument();
            //initdoc.Load(initialPageMs, Encoding.Default);        
            
         
                //get page number 

                int pages = getPages(LawWeburl);
                List<htmlelement> LawRecords = Craw(LawWeburl, pages);
                List<htmlelement> LawOrderRecords = Craw(LawOrderWeburl, pages);
                List<htmlelement> RulesRecords = Craw(RulesWeburl,pages);

                /*
                List<htmlelement> list_type = new List<htmlelement>();
                //process each page 

                for (int currentPageNumber = 1; currentPageNumber <= pages; currentPageNumber++)
                {
                    string pageurl = weburl + "&pn" + currentPageNumber;
                    MemoryStream Ms = new MemoryStream(url.DownloadData(pageurl));
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(Ms, Encoding.Default);

                    
                    HtmlNodeCollection tableNodes = doc.DocumentNode.SelectNodes("//table[@class='listTable']/tbody//tr");


                    foreach (HtmlNode nodes in tableNodes)
                    {
                        htmlelement item = new htmlelement();
                        item.Id = Int32.Parse(nodes.ChildNodes[0].InnerText);
                        string[] Title = nodes.ChildNodes[1].InnerText.Split('(');
                        string s = WebUtility.HtmlDecode(Title[0]);
                        item.title = s;
                        item.date = nodes.ChildNodes[2].InnerText;
                        item.link = nodes.ChildNodes[1].SelectSingleNode("./a").Attributes["href"].Value;
                        list_type.Add(item);

                    }
                   
                }
                */
                using (var textWriter = new StreamWriter(@"C:\Users\wish\Desktop\Law.csv"))
                {
                    var csv = new CsvWriter(textWriter);
                    csv.WriteRecords(LawRecords);
                }
                using (var textWriter = new StreamWriter(@"C:\Users\wish\Desktop\LawOrder.csv"))
                {
                   // var csv = new CsvWriter(textWriter);
                   // csv.WriteRecords(LawOrderRecords);
                }
                using (var textWriter = new StreamWriter(@"C:\Users\wish\Desktop\Rules.csv"))
                {
                   // var csv = new CsvWriter(textWriter);
                   // csv.WriteRecords(RulesRecords);
                }


            

        }
    }

    class Product
    {
        public string type { get; set; }

        public string name { get; set; }
    }
}