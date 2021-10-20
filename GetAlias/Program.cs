using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetAlias
{
   
    class Program
    {
        public class Alias
        {
            public String brand;
            public String correctBrand;
            public bool exist;
            public List<String> variants;

            public Alias(string brand, string correctBrand,bool exist)
            {
                this.brand = brand;
                this.correctBrand = correctBrand;
                this.exist = exist;
            }

        }

        public static string GetAliases(List<string> inputBrands)
        {
            List<Alias> brandsRes = new List<Alias>();
            using (var db = new WebCrossesEntities())
            {
                foreach (String brand in inputBrands)
                {
                    if (brand != "") {
                    var tb = db.AbcpAlias.Where(b => b.Brand == brand).Select(b=>b.Brand).Distinct();
                        if (tb.Count() != 0)
                            foreach (String b in tb)
                            {
                                Alias anew = new Alias(brand, b, true);
                                brandsRes.Add(anew);
                            }
                        else
                        {
                            var tb1 = db.AbcpAlias.Where(a => a.Alias == brand).Select(a => a.Brand).Distinct();
                            if (tb1.Count() != 0)
                                foreach (string a in tb1)
                                {
                                    Alias anew = new Alias(brand, a, true);
                                    brandsRes.Add(anew);
                                }
                            else
                            {
                                Alias anew = new Alias(brand, brand, false);
                                var tb2 = db.AbcpAlias.Where(b => b.Alias.Contains(brand)).Select(b => b.Brand).Distinct();
                                if (tb2.Count() != 0)
                                {
                                    anew.variants = new List<string>();
                                    foreach (string b in tb2)
                                    {
                                        anew.variants.Add(b);
                                    }
                                }
                                brandsRes.Add(anew);
                            }
                        }
                    }

                }
            }
            return JsonConvert.SerializeObject(brandsRes);
        }
        public static void Main()
        {
            string json = GetAliases(new List<string>(){ "Mann-filter", "1-56", "baka","xyz","maruichi","another","старвольт","123","555-twn","555","Mahle","20","вольт", "555","156", "ArvinMeritor", "AUDI", "АВТОДЕЛО","", "Samwoo","TOYOTA", "LEXUS", "LADA", "MB","MITSUBISHI","TOYOTA/LEXUS" });
            Console.WriteLine(json);
        }
    }
}
