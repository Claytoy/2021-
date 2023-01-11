using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelLib
{
    /// <summary>
    /// 产生数据处理报告
    /// </summary>
    public class Report
    {
        ObsData obs;
        DataEnitity enities;
        public Report(ObsData obsData)
        {
            obs = obsData;
        }
        public Report(ObsData obsData, DataEnitity dataEnitity)
        {
            obs = obsData;
            enities = dataEnitity;
        }

        string Title()
        {
            string str = "\t 附合水准测量近似平差\n";
            return str;
        }
                
        public override string ToString()
        {
            string str = Title();
            str += obs.ToString();            
            return str;
        }

    }
}
