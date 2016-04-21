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

using Newtonsoft.Json;
using System.Net.Http;
using System.IO;

namespace Parser_Google1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Search1();
            Search2();
        }

        private async void Search2()//better
        {
            string query = "Радіо%20КПІ";
            string json = "";
            Worker w = new Worker();

            QueryTbl.Text ="QUERY:  " + query;

            for (int i = 1; i < 2; i++)
            {
                using (var client = new HttpClient())
                {
                    json = await client.GetStringAsync("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&rsz=large&start=" +i.ToString()+ "&q=" + query);
                }

                GoogleObject googleObject = JsonConvert.DeserializeObject<GoogleObject>(json);
                foreach (var item in googleObject.responseData.results)
                {
                    listBox1.Items.Add(new ListBoxItem() { Content = "TITLE:  " + item.titleNoFormatting + "\n" + "URL:  " + item.unescapedUrl + "\n" + "CONTENT:  " + item.content });

                    List<string> referenceURLs = new List<string>();
                    referenceURLs = w.GetChildRefByURL(item.unescapedUrl);
                }
            }
        }

        /// <summary>
        /// I dont remember, but it should be wrond way
        /// </summary>
        private async void Search1()
        {
            var client = new HttpClient();
            var address = new Uri("https://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=" + "cat");

            HttpResponseMessage response = await client.GetAsync(address);
            String stream = await response.Content.ReadAsStringAsync();

            dynamic jObj = JsonConvert.DeserializeObject(stream);
            foreach (var res in jObj.responseData.results)
            {
                listBox1.Items.Add(new ListBoxItem() { Content = res.title + "\n" + res.url });//"{0} => {1}\n", res.title, res.url);
            }
            
        }
    }
}
