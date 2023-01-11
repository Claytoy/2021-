using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeodesyLib
{
    public class Report
    {
        DataEntity Obs;
        public Report(string dataFile)
        {
            Obs = FileHelper.Read(dataFile);
        }

        public string GetReort()
        {
            var report = Header();

            var pro = new BesselInverse(Obs.Datum);
            pro.InversePro(Obs.Data);
            report += InvReport();


            var pro2 = new BesselDirect(Obs.Datum);
            for (int i = 0; i < Obs.Data.Count; i++)
            {
               Obs.Data[i].S += 1000.0;

            }
            pro2.DirecPro(Obs.Data);
            report += DirectReport();

            return report;
        }

        string Header()
        {
            string str = "利用白塞尔法进行大地主题正反算\n";
            return str;
        }

        string InvReport()
        {
            string str = "大地主题反算\n";
            str += "------------------------------------------------\n";
            str += string.Format("{0,-5} {1,14} {2,14} {3,-5} {4,14} {5,14} \n --> {6,15} {7,15} {8,15}\n",
                "起点", "B1", "L1", "终点", "B2", "L2", "A1", "A2", "S");
            foreach (var d in Obs.Data)
            {
                str += string.Format("{0,-5} {1,14} {2,14} {3,-5} {4,14}  {5,14} \n --> {6,15} {7,15} {8,12:f3} \n",
                d.P1.Name, GeoPro.DMS2String(d.P1.B), GeoPro.DMS2String(d.P1.L),
                d.P2.Name, GeoPro.DMS2String(d.P2.B), GeoPro.DMS2String(d.P2.L),
                GeoPro.DMS2String(d.A12), GeoPro.DMS2String(d.A21), d.S);
            }
            return str;
        }
        string DirectReport()
        {
            string str = "大地主题正算\n";
            str += "------------------------------------------------\n";
            str += string.Format("{0,-5} {1,15} {2,15}  {3,15} {4,12:f3}  \n --> {5,-5} {6,15} {7,15} {8,15}\n",
                "起点", "B1", "L1", "A1", "S  ", "  终点", "B2", "L2",  "A2");
            foreach (var d in Obs.Data)
            {
                str += string.Format("{0,-5} {1,15} {2,15} {3,15}  {4,15:f4} \n --> {5,-5} {6,15} {7,15} {8,15} \n",
                d.P1.Name, GeoPro.DMS2String(d.P1.B), GeoPro.DMS2String(d.P1.L),
                GeoPro.DMS2String(d.A12), d.S, d.P2.Name, GeoPro.DMS2String(d.P2.B), GeoPro.DMS2String(d.P2.L),
                GeoPro.DMS2String(d.A21));
            }
            return str;
        }
    }
}
