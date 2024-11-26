using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace WpfSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;

       
        
        class Nft
        {
            
            public string Path { get; set; }
            public int Views { get; set; }
            public int Likes { get; set; }

            public Nft(string path, int views, int likes)
            {
                Path = path;
                Views = views;
                Likes = likes;
                Debug.WriteLine("Nft arguments passed");
                
                
            }

            public Nft()
            {
                //throw new Exception("there were no arguments given to Nft");
                Debug.WriteLine("Nft didn't pass argument");
            }
        }

       public MainWindow()
        {
            
            string path = "FailTestDeserializeObject 2";
            int views = 1;
            int likes = 1;
            //string json;

            List<Nft> mediaList = JsonConvert.DeserializeObject<List<Nft>>(JsonString());
            List<Tuple<string, int, int>> nftList = new List<Tuple<string, int, int>>();

            try
            {
                foreach (Nft n in mediaList)
                {
                    /*
                    n.Path = path;
                    n.Views = views;
                    n.Likes = likes;
                    */
                    n.Path = n.Path ?? path; // Use existing value if available, otherwise default
                    n.Views = n.Views != 0 ? n.Views : views;
                    n.Likes = n.Likes != 0 ? n.Likes : likes;

                    nftList.Add(new Tuple<string, int, int>(n.Path, n.Views, n.Likes));

                }
            }
            catch (Exception ex) 
            {
                nftList.Add(new Tuple<string, int, int>("/images/photo10.jpg", 234, 4124));
                nftList.Add(new Tuple<string, int, int>("/images/photo11.jpg", 31241, 1141));
                nftList.Add(new Tuple<string, int, int>("/images/photo12.jpg", 41212, 241));
                nftList.Add(new Tuple<string, int, int>("/images/photo13.jpg", 51234, 412));
                nftList.Add(new Tuple<string, int, int>("/images/photo14.jpg", 612, 7741));
                nftList.Add(new Tuple<string, int, int>("/images/photo14.jpg", 71, 8124));
                Debug.WriteLine(ex.ToString());
            }
            
                           

            Nft nft = new Nft(path, views, likes);



            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WpfSql.Properties.Settings.SocialMetricsDatabaseConnectionString"].ConnectionString;
            
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            if (nftList != null)
            {
                try 
                {
                    foreach (var item in nftList)
                    {
                        nft.Path = item.Item1;
                        nft.Views = item.Item2;
                        nft.Likes = item.Item3;

                        InsertLogs(nft.Path, nft.Views, nft.Likes);
                        Debug.WriteLine("reccords passed if medialist populated");
                    }
                
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                

            }
            else
            {
                InsertLogs(path, views, likes);
                Debug.WriteLine("medialist is null");

            }
        }

        public void InsertLogs(string path, int views, int likes)
        {
            //assigning the values to logs
            Log log = new Log();
            log.Path = path;
            log.Views = views;
            log.Likes = likes;

            try
            {
                //inserting log onto database
                dataContext.Logs.InsertOnSubmit(log);

                dataContext.SubmitChanges();

                MainDataGrid.ItemsSource = dataContext.Logs;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        } //this method recieves string, int, int and logs it into "Logs as path, views and likes
        
        public string JsonString()
        {
            string  json = @"
            [
              {
                ""Path"": ""/images/photo1.jpg"",
                ""Views"": 250,
                ""Likes"": 56
              },
              {
                ""Path"": ""/images/photo2.jpg"",
                ""Views"": 30,
                ""Likes"": 12
              }
,
              {
                ""Path"": ""/images/photo3.jpg"",
                ""Views"": 412,
                ""Likes"": 134
              }
,
              {
                ""Path"": ""/images/photo4.jpg"",
                ""Views"": 64,
                ""Likes"": 4
              }
,
              {
                ""Path"": ""/images/photo5.jpg"",
                ""Views"": 522,
                ""Likes"": 121
              }
            ]"; //the json recieved from the Lambda*/
            return json;
        }

               
        
    }
}
