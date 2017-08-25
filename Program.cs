
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp2
{
    

   
        class CountryData
        {
            public string country_code;
            public float value;
            public string country_name;
        }
        class Program
        {
            static void Main(string[] args)
            {
                string path = @"D:\Indicators.csv";
                string path2 = @"D:\\stackedchart.json";
                string path3 = @"D:\\multilinechart.json";
                string path4 = @"D:\\barchart.json";
                string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT","GEO", "HKG", "IND", "IDN", "IRN", "IRQ","ISR", "JPN", "JOR", "KAZ", "KWT",
                "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL","PRK", "OMN", "PAK", "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM"  };

                List<CountryData> list = new List<CountryData>();
                FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                FileStream fStream2 = new FileStream(path2, FileMode.OpenOrCreate, FileAccess.Write);
                FileStream fStream3 = new FileStream(path3, FileMode.OpenOrCreate, FileAccess.Write);
                FileStream fStream4 = new FileStream(path4, FileMode.OpenOrCreate, FileAccess.Write);

                StreamReader reader = new StreamReader(fStream);
                StreamWriter writer = new StreamWriter(fStream2);
                StreamWriter writer2 = new StreamWriter(fStream3);
                StreamWriter writer3 = new StreamWriter(fStream4);

                var header = reader.ReadLine().Split(',');
                writer.WriteLine("[");
                writer2.WriteLine("[");
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(',');
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i].StartsWith("\""))
                        {
                            if (data[i].EndsWith("\"")) { }
                            else
                            {
                                data[i] = data[i] + data[i + 1];
                                data = data.Where((val, idx) => idx != (i + 1)).ToArray();
                            }
                        }
                    }
                    // first json 
                    for (int j = 0; j < country.Length; j++)
                    {
                        if (country[j] == data[1])
                        {
                            if (data[3] == "SP.DYN.LE00.FE.IN" || data[3] == "SP.DYN.LE00.MA.IN")
                            {
                                if (data[5] == "62.264")
                                {
                                    writer.WriteLine("{" + "\n" + "\"" + header[1] + "\"" + ":" + " \"" + data[1] +
                                   "\"" + "," + "\n" + "\"" + header[2] + "\"" + ":" + data[2] + "," + "\n" + "\"" + header[4] + "\"" + ":" + " \"" + data[4] + "\"" + "," + "\n" + "\"" + header[5] + "\"" + ":" + " \"" + data[5] + "\"" + "\n" + "}" + "\n" + "]");

                                }
                                else
                                {
                                    writer.WriteLine("{" + "\n" + "\"" + header[1] + "\"" + ":" + " \"" + data[1] +
                                        "\"" + "," + "\n" + "\"" + header[2] + "\"" + ":" + data[2] + "," + "\n" + "\"" + header[4] + "\"" + ":" + " \"" + data[4] + "\"" + "," + "\n" + "\"" + header[5] + "\"" + ":" + " \"" + data[5] + "\"" + "\n" + "}" + ",");
                                }
                            }
                        }
                        writer.Flush();
                    }
                    // Second json
                    if (data[1] == "IND")
                    {
                        if (data[4] == "SP.DYN.CBRT.IN" || data[4] == "SP.DYN.CDRT.IN")
                        {
                            if (data[6] == "7.385")
                            {
                                writer2.WriteLine("{" + " \"" + data[5] + "\"" + ":" + "\n" + "{" + "\n"
                                            + "\"" + header[2] + "\"" + ":" + data[2] + "000 people)" + "\"" + "," + "\n" + "\"" + header[5] + "\"" + ":" + " \"" + data[6] + "\"" + "\n" + "}" + "}" + "]");
                            }
                            else
                            {
                                writer2.WriteLine("{" + " \"" +"year"+"\""+":"+ data[5] +
                                             "\r\n\"" + header[2] + "\"" + ":" + data[2] + "000 people)" + "\"" + "," + "\r\n" + "\"" + header[5] + "\"" + ":" + " \"" + data[6] + "\"" + "\n" + "}" + "}" + ",");
                            }
                        }
                        writer2.Flush();
                    }
                    // Third json
                    if (data[3] == "SP.DYN.LE00.IN")
                    {
                        float abc;
                        float.TryParse(data[5], out abc);
                        list.Add(new CountryData() { country_code = data[1], value = abc, country_name = data[0] });
                    }
                }

                var value3 = from m in list group m by m.country_name into t select new { countryname = t.Key, value = t.Sum(o => o.value) };
                var k = value3.OrderByDescending(m => m.value).Take(5);
                writer3.WriteLine("[");
                foreach (var i in k)
                {
                    if (i.countryname == "Norway")
                    {
                        writer3.WriteLine("{" + "\"" + "CountryName" + "\"" + ":" + "\"" + i.countryname + "\"" + "," + "\n" + "\"" + "Values" + "\"" + ":" + "\"" + i.value + "\"" + "\n" + "}");
                    }
                    else
                    {
                        writer3.WriteLine("{" + "\"" + "CountryName" + "\"" + ":" + "\"" + i.countryname + "\"" + "," + "\n" + "\"" + "Values" + "\"" + ":" + "\"" + i.value + "\"" + "\n" + "}" + ",");
                    }
                }
                writer3.WriteLine("]");
                writer3.Flush();
            }
        }
    }
