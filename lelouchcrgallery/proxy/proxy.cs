using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace lelouchcrgallery.proxy
{
    public class pixivproxy
    {
        public async static Task<RootObject> Getimage ()
        {
            var http = new HttpClient();
            var response = await http.GetAsync("http://lelouchcrgallery.tk/rand");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);
            return data;
        }

    }

    [DataContract]
    public class RootObject
    {
       

        [DataMember]
        public string p_ori { get; set; }
        [DataMember]
        public string p_ori_hight { get; set; }
        [DataMember]
        public string p_ori_width { get; set; }
        [DataMember]
        public string p_mid { get; set; }
        [DataMember]
        public string p_mid_hight { get; set; }
        [DataMember]
        public string p_mid_width { get; set; }
        [DataMember]
        public string p_source { get; set; }
        [DataMember]
        public string ranTotal { get; set; }
    }

}
