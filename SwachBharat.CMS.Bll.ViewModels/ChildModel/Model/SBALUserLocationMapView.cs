﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachBharat.CMS.Bll.ViewModels.ChildModel.Model
{
    public class SBALUserLocationMapView : BaseVM
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string lat { get; set; }
        public string log { get; set; }
        public string address { get; set; }
        public string vehcileNumber { get; set; }
        public string userMobile { get; set; }

        public string HouseId { get; set; }
        public string HouseOwnerName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string HouseAddress { get; set; }
        public int type { get; set; }

        public string WasteType { get; set; }
        public string gpBeforImage { get; set; }
        public string gpAfterImage{ get; set; }

    }     
}
