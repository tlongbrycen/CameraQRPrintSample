using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraQRPrintSample
{
    class DummyData
    {
        public class CrewData
        {
            public string crewID;
            public string crewName;
            public string department;
            public string carNo;
            public string judgment;
            public string measureValue;
            public string photoTime;
            public string longLatitude;
            public string memo;

            public CrewData(bool dummy = true)
            {
                if(dummy == true)
                {
                    crewID = "0665";
                    crewName = "Dang Tung Long";
                    department = "ETS";
                    carNo = "007";
                    judgment = "×";
                    measureValue = "0.34239";
                    photoTime = "2020/09/16 15:03 JST";
                    longLatitude = "31 14";
                    memo = "これはただダミーデータだけです";
                }
            }
        }

        public List<CrewData> crewDatas;

        public DummyData()
        {
            crewDatas = new List<CrewData>();
            for(int i = 0; i < 17; i++)
            {
                crewDatas.Add(new CrewData());
            }
        }
        
    }
}
