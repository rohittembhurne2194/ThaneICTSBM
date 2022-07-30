using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;
using SwachBharat.CMS.Dal.DataContexts;
using SwachBharat.CMS.Bll.ViewModels.ChildModel.Model;
using SwachBharat.CMS.Bll.ViewModels.SS2020Reports;
using System.Xml;
using SwachBharat.CMS.Bll.ViewModels.Grid;
using Newtonsoft.Json;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using Microsoft.SqlServer.Server;

namespace SwachBharat.CMS.Bll.Services
{
    public class ScreenService : AppService, IScreenService
    {
        private int AppID;
        public ScreenService(int AppId) : base(AppId)
        {
            AppID = AppId;
        }

        #region CheckForNull
        public int checkIntNull(string str)
        {
            int result = 0;
            if (str == null || str == "")
            {
                result = 0;
                return result;
            }
            else
            {
                result = Convert.ToInt32(str);
                return result;
            }
        }
        public string checkNull(string str)
        {
            string result = "";
            if (str == null || str == "")
            {
                result = "";
                return result;
            }
            else
            {
                result = str;
                return result;
            }
        }

        #endregion
        #region Dashboard
        public DashBoardVM GetDashBoardDetails()
        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
                    List<ComplaintVM> obj = new List<ComplaintVM>();
                    //if (AppID==1)
                    //{
                    //    string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=1");
                    //     obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    //}
                    if (appdetails.GramPanchyatAppID != null)
                    {
                        string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=" + appdetails.GramPanchyatAppID);
                        obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    }
                    var data = db.SP_Dashboard_Details().First();
                    var data1 = db.SP_CTPTScanify_Count().First();

                    var date = DateTime.Today;
                    var houseCount = db.SP_TotalHouseCollection_Count(date).FirstOrDefault();
                    if (data != null)
                    {

                        model.TodayAttandence = data.TodayAttandence;
                        model.TotalAttandence = data.TotalAttandence;
                        model.TodayCTPTAttandence = data.TodayCTPTAttandence;
                        model.TotalCTPTAttandence = data.TotalCTPTAttandence;
                        model.HouseCollection = data.TotalHouse;
                        model.PointCollection = data.TotalPoint;
                        model.TotalComplaint = obj.Count();
                        model.TotalHouseCount = houseCount.TotalHouseCount;
                        model.MixedCount = houseCount.MixedCount;
                        model.BifurgatedCount = houseCount.BifurgatedCount;
                        model.NotCollected = houseCount.NotCollected;
                        model.DumpYardCount = data.TotalDump;
                        model.NotSpecified = houseCount.NotSpecified;
                        //model.TotalGcWeightCount = houseCount.TotalGcWeightCount;
                        //model.GcWeightCount = Convert.ToDouble(string.Format("{0:0.00}", houseCount.GcWeightCount));
                        //model.DryWeightCount =Convert.ToDouble(string.Format("{0:0.00}", houseCount.DryWeightCount));
                        //model.WetWeightCount =Convert.ToDouble(string.Format("{0:0.00}", houseCount.WetWeightCount));
                        model.GcWeightCount = Convert.ToDouble(houseCount.GcWeightCount);
                        model.DryWeightCount = Convert.ToDouble(houseCount.DryWeightCount);
                        model.WetWeightCount = Convert.ToDouble(houseCount.WetWeightCount);

                        model.TotalGcWeightCount = Convert.ToDouble(houseCount.TotalGcWeightCount);
                        model.TotalDryWeightCount = Convert.ToDouble(houseCount.TotalDryWeightCount);
                        model.TotalWetWeightCount = Convert.ToDouble(houseCount.TotalWetWeightCount);
                        model.TotalHousePropertyCount = Convert.ToInt32(houseCount.TotalHousePropertyCount);
                        model.TotalDumpPropertyCount = Convert.ToInt32(houseCount.TotalDumpPropertyCount);

                        model.TotalCDWCount = Convert.ToInt32(houseCount.TotalCDW);
                        model.TotalHWCount = Convert.ToInt32(houseCount.TotalHW);
                        model.TotalWetWaste = Convert.ToInt32(houseCount.TotalWetWaste);
                        model.TotalDryWaste = Convert.ToInt32(houseCount.TotalDryWaste);
                        model.TotalDHWCount = Convert.ToInt32(houseCount.TotalDHW);
                        model.TotalSWCount = Convert.ToInt32(houseCount.TotalSW);

                        model.ResidentialBuildingTotal = Convert.ToInt32(houseCount.TotalRBW);
                        model.ResidentialSlumTotal = Convert.ToInt32(houseCount.TotalRSW);
                        model.ResidentialBuildingCollection = Convert.ToInt32(houseCount.RBWDailyScanCount);
                        model.ResidentialSlumCollection = Convert.ToInt32(houseCount.RSWDailyScanCount);


                        model.TotalCWCount = Convert.ToInt32(houseCount.TotalCW);

                        //For Liquid Waste

                        model.LWGcWeightCount = Convert.ToDouble(houseCount.LWGcWeightCount);
                        model.LWDryWeightCount = Convert.ToDouble(houseCount.LWDryWeightCount);
                        model.LWWetWeightCount = Convert.ToDouble(houseCount.LWWetWeightCount);
                        model.LWTotalGcWeightCount = Convert.ToDouble(houseCount.LWTotalGcWeightCount);
                        model.LWTotalDryWeightCount = Convert.ToDouble(houseCount.LWTotalDryWeightCount);
                        model.LWTotalWetWeightCount = Convert.ToDouble(houseCount.LWTotalWetWeightCount);

                        //For Street Sweeping
                        model.SSGcWeightCount = Convert.ToDouble(houseCount.SSGcWeightCount);
                        model.SSDryWeightCount = Convert.ToDouble(houseCount.SSDryWeightCount);
                        model.SSWetWeightCount = Convert.ToDouble(houseCount.SSWetWeightCount);
                        model.SSTotalGcWeightCount = Convert.ToDouble(houseCount.SSTotalGcWeightCount);
                        model.SSTotalDryWeightCount = Convert.ToDouble(houseCount.SSTotalDryWeightCount);
                        model.SSTotalWetWeightCount = Convert.ToDouble(houseCount.SSTotalWetWeightCount);

                        //for Horticulture waste & Construction demolation waste
                        model.GcHWWeightCount = Convert.ToDouble(houseCount.GcHWWeightCount);
                        model.GcCDWWeightCount = Convert.ToDouble(houseCount.GcCDWWeightCount);

                        //For Commercial
                        model.TotalCommercialCount = Convert.ToDouble(houseCount.TotalCommercialCount);
                        model.CommercialMixCount = Convert.ToDouble(houseCount.TotalCommercialMixCount);
                        model.CommercialWetCount = Convert.ToDouble(houseCount.TotalCommercialWetCount);
                        model.CommercialDryCount = Convert.ToDouble(houseCount.TotalCommercialDryCount);
                        model.CommercialNotCollectedCount = Convert.ToDouble(houseCount.TotalCommercialNotCollectedCount);
                        model.CommercialNotSpecifiedCount = Convert.ToDouble(houseCount.TotalCommercialNotSpecifiedCount);
                        model.CommercialSegregetedCount = Convert.ToDouble(houseCount.TotalCommercialSegregeted);

                        model.TotalCommercialCDWCount = Convert.ToInt32(houseCount.TotalCommercialCDW);
                        model.TotalCommercialHWCount = Convert.ToInt32(houseCount.TotalCommercialHW);

                        model.TotalConstDemo = Convert.ToInt32(houseCount.TotalConstDemo);
                        model.TotalHorticulture = Convert.ToInt32(houseCount.TotalHorticulture);

                        model.TotalCommercialCurrent = data.TotalCommercialCurrent;
                        model.TotalCommercial = Convert.ToDouble(houseCount.TotalCommercial);

                        model.TotalCTPT = houseCount.TotalCTPT;
                        model.TotalCTPTCurrent = Convert.ToDouble(data.TotalCTPTCurrent);

                        model.TotalCTPTCount = data1.TotalCTPTCount;
                        model.TotalCTPTScanCount = data1.TotalCTPTScanCount;
                        model.TodayCTPTScanCount = data1.TodayScanCTPTCount;
                        model.TotalPTCount = data1.TotalPTCount;
                        model.TotalCTCount = data1.TotalCTCount;
                        model.TotalUCount = data1.TotalUCount;

                        model.TotalSWM = houseCount.TotalSWM;
                        model.TotalSWMCurrent = Convert.ToDouble(data.TotalSWMCurrent);
                        return model;
                    }

                    // String.Format("{0:0.00}", 123.4567); 

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception)
            {
                return model;
            }
        }
        #endregion

        #region Area
        public AreaVM GetAreaDetails(int teamId, string name)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.TeritoryMasters.Where(x => x.Id == teamId || x.Area.ToUpper()
                    == name.ToUpper() || x.AreaMar == name).FirstOrDefault();
                    if (Details != null)
                    {
                        AreaVM area = FillAreaViewModel(Details);
                        area.WardList = ListWardNo();
                        return area;
                    }
                    else
                    {
                        AreaVM area = new AreaVM();
                        area.WardList = ListWardNo();

                        return area;
                    }
                }
            }
            catch (Exception)
            {
                return new AreaVM();
            }
        }
        public void SaveAreaDetails(AreaVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.Id > 0)
                    {
                        var model = db.TeritoryMasters.Where(x => x.Id == data.Id).FirstOrDefault();
                        if (model != null)
                        {
                            model.Id = data.Id;
                            model.Area = data.Name;
                            model.AreaMar = data.NameMar;
                            model.wardId = data.wardId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var area = FillAreaDataModel(data);
                        db.TeritoryMasters.Add(area);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LiquidSaveAreaDetails(AreaVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.LWId > 0)
                    {
                        var model = db.TeritoryMasters.Where(x => x.Id == data.LWId).FirstOrDefault();
                        if (model != null)
                        {
                            model.Id = data.LWId;
                            model.Area = data.LWName;
                            model.AreaMar = data.LWNameMar;
                            model.wardId = data.LWwardId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var area = LiquidFillAreaDataModel(data);
                        db.TeritoryMasters.Add(area);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void StreetSaveAreaDetails(AreaVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.SSId > 0)
                    {
                        var model = db.TeritoryMasters.Where(x => x.Id == data.SSId).FirstOrDefault();
                        if (model != null)
                        {
                            model.Id = data.SSId;
                            model.Area = data.SSName;
                            model.AreaMar = data.SSNameMar;
                            model.wardId = data.SSwardId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var area = StreetFillAreaDataModel(data);
                        db.TeritoryMasters.Add(area);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletAreaDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.TeritoryMasters.Where(x => x.Id == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.TeritoryMasters.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Vehicle
        public VehicleTypeVM GetVehicleTypeDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.VehicleTypes.Where(x => x.vtId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        VehicleTypeVM type = FillVehicleViewModel(Details);
                        return type;
                    }
                    else
                    {
                        return new VehicleTypeVM();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveVehicleTypeDetails(VehicleTypeVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.Id > 0)
                    {
                        var model = db.VehicleTypes.Where(x => x.vtId == data.Id).FirstOrDefault();
                        if (model != null)
                        {
                            model.vtId = data.Id;
                            model.description = data.description;
                            model.descriptionMar = data.descriptionMar;
                            model.isActive = data.isActive;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillVehicleDataModel(data);
                        db.VehicleTypes.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeletVehicleTypeDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.VehicleTypes.Where(x => x.vtId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.VehicleTypes.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Vechile Registration

        public VehicleRegVM GetVehicleDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.VehicleRegistrations.Where(x => x.vehicleId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        VehicleRegVM vechile = FillVehicleTegViewModel(Details);
                        vechile.AreaList = ListArea();
                        vechile.WardList = ListWardNo();
                        vechile.VehicleList = ListVehicle();
                        return vechile;
                    }
                    else
                    {
                        VehicleRegVM vechile = new VehicleRegVM();
                        vechile.AreaList = ListArea();
                        vechile.WardList = ListWardNo();
                        vechile.VehicleList = ListVehicle();
                        return vechile;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StreetSweepVM GetBeatDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.SP_StreetSweepList().Where(x => x.SSId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        StreetSweepVM vechile = new StreetSweepVM();
                        vechile.BeatList = ListBeat();
                        return vechile;
                    }
                    else
                    {
                        StreetSweepVM vechile = new StreetSweepVM();
                        vechile.BeatList = ListBeat();
                        return vechile;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveVehicleRegDetails(VehicleRegVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.vehicleId > 0)
                    {
                        var model = db.VehicleRegistrations.Where(x => x.vehicleId == data.vehicleId).FirstOrDefault();
                        if (model != null)
                        {
                            model.vehicleId = data.vehicleId;
                            model.vehicleType = data.vehicleType;
                            model.vehicleNo = data.vehicleNumber;
                            model.areaId = data.AreaId;
                            model.isActive = data.isActive;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillVehicleRegDataModel(data);
                        db.VehicleRegistrations.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion





        #region Ward Number
        public WardNumberVM GetWardNumberDetails(int teamId, string name)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.WardNumbers.Where(x => x.Id == teamId || x.WardNo == name).FirstOrDefault();
                    if (Details != null)
                    {
                        WardNumberVM type = FillWardViewModel(Details);
                        type.ZoneList = ListZone();
                        return type;
                    }
                    else
                    {
                        WardNumberVM type = new WardNumberVM();
                        type.ZoneList = ListZone();
                        return type;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveWardNumberDetails(WardNumberVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.Id > 0)
                    {
                        var model = db.WardNumbers.Where(x => x.Id == data.Id).FirstOrDefault();
                        if (model != null)
                        {
                            model.Id = data.Id;
                            model.WardNo = data.WardNo;
                            model.zoneId = data.zoneId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillWardDataModel(data);
                        db.WardNumbers.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LiquidSaveWardNumberDetails(WardNumberVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.LWId > 0)
                    {
                        var model = db.WardNumbers.Where(x => x.Id == data.LWId).FirstOrDefault();
                        if (model != null)
                        {
                            model.Id = data.LWId;
                            model.WardNo = data.LWWardNo;
                            model.zoneId = data.LWzoneId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = LiquidFillWardDataModel(data);
                        db.WardNumbers.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void StreetSaveWardNumberDetails(WardNumberVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.SSId > 0)
                    {
                        var model = db.WardNumbers.Where(x => x.Id == data.SSId).FirstOrDefault();
                        if (model != null)
                        {
                            model.Id = data.SSId;
                            model.WardNo = data.SSWardNo;
                            model.zoneId = data.SSzoneId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = StreetFillWardDataModel(data);
                        db.WardNumbers.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void DeletWardNumberDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.WardNumbers.Where(x => x.Id == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.WardNumbers.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region House Details
        public HouseDetailsVM GetHouseDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.HouseQRCode + "/";
                HouseDetailsVM house = new HouseDetailsVM();

                var Details = db.HouseMasters.Where(x => x.houseId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    house = FillHouseDetailsViewModel(Details);
                    if (house.houseQRCode != null && house.houseQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + house.houseQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                house.houseQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                house.houseQRCode = ThumbnaiUrlCMS + house.houseQRCode.Trim();


                                int n = teamId;
                                double refer1 = Convert.ToDouble((n + 1));
                                double xyz = refer1 / 100;

                                string s = xyz.ToString("0.00", CultureInfo.InvariantCulture);
                                string[] parts = s.Split('.');
                                int i1 = int.Parse(parts[0]);
                                int i2 = int.Parse(parts[1]);

                                if (i2 == 0)
                                {
                                    //i1 = i1 + 1;
                                    s = "S" + i1.ToString();
                                }
                                else
                                {
                                    s = "S" + (i1 + 1);
                                }

                                house.SerielNo = s;
                            }
                        }
                        catch (Exception e) { house.houseQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        house.houseQRCode = "/Images/default_not_upload.png";
                    }

                    house.WardList = LoadListWardNo(Convert.ToInt32(house.ZoneId)); //ListWardNo();
                    house.AreaList = LoadListArea(Convert.ToInt32(house.WardNo)); //ListArea();
                    house.ZoneList = ListZone();
                    return house;
                }
                else if (teamId == -2)
                {
                    var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                    int number = 1000;
                    string refer = "HPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    //house.WardList = ListWardNo();
                    //house.AreaList = ListArea();

                    var WWWW = new List<SelectListItem>();
                    SelectListItem itemAdd = new SelectListItem() { Text = "Select Ward / Prabhag", Value = "0" };
                    WWWW.Insert(0, itemAdd);

                    var ARRR = new List<SelectListItem>();
                    SelectListItem itemAddARR = new SelectListItem() { Text = "Select Area", Value = "0" };
                    ARRR.Insert(0, itemAddARR);


                    house.WardList = WWWW;
                    house.AreaList = ARRR;
                    house.ZoneList = ListZone();
                    house.houseId = 0;
                    return house;
                }
                else
                {
                    var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                    int number = 1000;
                    string refer = "HPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    house.WardList = ListWardNo();
                    house.AreaList = ListArea();
                    house.ZoneList = ListZone();
                    house.houseId = id;
                    return house;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SWMDetailsVM GetSWMDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.SWMQRCode + "/";
                SWMDetailsVM house = new SWMDetailsVM();

                var Details = db.SWMMasters.Where(x => x.swmId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    house = FillSWMDetailsViewModel(Details);
                    if (house.swmQRCode != null && house.swmQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + house.swmQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                house.swmQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                house.swmQRCode = ThumbnaiUrlCMS + house.swmQRCode.Trim();


                                int n = teamId;
                                double refer1 = Convert.ToDouble((n + 1));
                                double xyz = refer1 / 100;

                                string s = xyz.ToString("0.00", CultureInfo.InvariantCulture);
                                string[] parts = s.Split('.');
                                int i1 = int.Parse(parts[0]);
                                int i2 = int.Parse(parts[1]);

                                if (i2 == 0)
                                {
                                    //i1 = i1 + 1;
                                    s = "S" + i1.ToString();
                                }
                                else
                                {
                                    s = "S" + (i1 + 1);
                                }

                                house.SerielNo = s;
                            }
                        }
                        catch (Exception e) { house.swmQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        house.swmQRCode = "/Images/default_not_upload.png";
                    }

                    house.WardList = LoadListWardNo(Convert.ToInt32(house.ZoneId)); //ListWardNo();
                    house.AreaList = LoadListArea(Convert.ToInt32(house.WardNo)); //ListArea();
                    house.ZoneList = ListZone();
                    return house;
                }
                else if (teamId == -2)
                {
                    var id = db.SWMMasters.OrderByDescending(x => x.swmId).Select(x => x.swmId).FirstOrDefault();
                    int number = 1000;
                    string refer = "SWMSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.swmQRCode = "/Images/QRcode.png";
                    //house.WardList = ListWardNo();
                    //house.AreaList = ListArea();

                    var WWWW = new List<SelectListItem>();
                    SelectListItem itemAdd = new SelectListItem() { Text = "Select Ward / Prabhag", Value = "0" };
                    WWWW.Insert(0, itemAdd);

                    var ARRR = new List<SelectListItem>();
                    SelectListItem itemAddARR = new SelectListItem() { Text = "Select Area", Value = "0" };
                    ARRR.Insert(0, itemAddARR);


                    house.WardList = WWWW;
                    house.AreaList = ARRR;
                    house.ZoneList = ListZone();
                    house.swmId = 0;
                    return house;
                }
                else
                {
                    var id = db.SWMMasters.OrderByDescending(x => x.swmId).Select(x => x.swmId).FirstOrDefault();
                    int number = 1000;
                    string refer = "SWMSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.swmQRCode = "/Images/QRcode.png";
                    house.WardList = ListWardNo();
                    house.AreaList = ListArea();
                    house.ZoneList = ListZone();
                    house.swmId = id;
                    return house;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CommercialDetailsVM GetCommercialDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.CommercialQRCode + "/";
                CommercialDetailsVM house = new CommercialDetailsVM();

                var Details = db.CommercialMasters.Where(x => x.commercialId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    house = FillCommericalDetailsViewModel(Details);
                    if (house.houseQRCode != null && house.houseQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + house.houseQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                house.houseQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                house.houseQRCode = ThumbnaiUrlCMS + house.houseQRCode.Trim();


                                int n = teamId;
                                double refer1 = Convert.ToDouble((n + 1));
                                double xyz = refer1 / 100;

                                string s = xyz.ToString("0.00", CultureInfo.InvariantCulture);
                                string[] parts = s.Split('.');
                                int i1 = int.Parse(parts[0]);
                                int i2 = int.Parse(parts[1]);

                                if (i2 == 0)
                                {
                                    //i1 = i1 + 1;
                                    s = "S" + i1.ToString();
                                }
                                else
                                {
                                    s = "S" + (i1 + 1);
                                }

                                house.SerielNo = s;
                            }
                        }
                        catch (Exception e) { house.houseQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        house.houseQRCode = "/Images/default_not_upload.png";
                    }

                    house.WardList = LoadListWardNo(Convert.ToInt32(house.ZoneId)); //ListWardNo();
                    house.AreaList = LoadListArea(Convert.ToInt32(house.WardNo)); //ListArea();
                    house.ZoneList = ListZone();
                    return house;
                }
                else if (teamId == -2)
                {
                    var id = db.CommercialMasters.OrderByDescending(x => x.commercialId).Select(x => x.commercialId).FirstOrDefault();
                    int number = 1000;
                    string refer = "CPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    //house.WardList = ListWardNo();
                    //house.AreaList = ListArea();

                    var WWWW = new List<SelectListItem>();
                    SelectListItem itemAdd = new SelectListItem() { Text = "Select Ward / Prabhag", Value = "0" };
                    WWWW.Insert(0, itemAdd);

                    var ARRR = new List<SelectListItem>();
                    SelectListItem itemAddARR = new SelectListItem() { Text = "Select Area", Value = "0" };
                    ARRR.Insert(0, itemAddARR);


                    house.WardList = WWWW;
                    house.AreaList = ARRR;
                    house.ZoneList = ListZone();
                    house.houseId = 0;
                    return house;
                }
                else
                {
                    var id = db.CommercialMasters.OrderByDescending(x => x.commercialId).Select(x => x.commercialId).FirstOrDefault();
                    int number = 1000;
                    string refer = "CPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    house.WardList = ListWardNo();
                    house.AreaList = ListArea();
                    house.ZoneList = ListZone();
                    house.houseId = id;
                    return house;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SBALUserLocationMapView GetHouseByIdforMap(int teamId, int daId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.HouseQRCode + "/";
                SBALUserLocationMapView house = new SBALUserLocationMapView();

                var Details = db.HouseMasters.Where(x => x.houseId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    house = FillHouseDetailsViewModelforMap(Details);
                    Daily_Attendance Daily_Attendanceuser = new Daily_Attendance();
                    Daily_Attendanceuser = db.Daily_Attendance.Where(x => x.daID == daId).FirstOrDefault();
                    UserMaster user = new UserMaster();
                    user = db.UserMasters.Where(x => x.userId == Daily_Attendanceuser.userId).FirstOrDefault();
                    house.userName = user.userName;
                    if (house.houseQRCode != null && house.houseQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + house.houseQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                house.houseQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                house.houseQRCode = ThumbnaiUrlCMS + house.houseQRCode.Trim();
                            }
                        }
                        catch (Exception e) { house.houseQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        house.houseQRCode = "/Images/default_not_upload.png";
                    }

                    house.WardList = LoadListWardNo(Convert.ToInt32(house.ZoneId)); //ListWardNo();
                    house.AreaList = LoadListArea(Convert.ToInt32(house.WardNo)); //ListArea();
                    house.ZoneList = ListZone();
                    return house;
                }


                else
                {
                    Daily_Attendance Daily_Attendanceuser = new Daily_Attendance();
                    Daily_Attendanceuser = db.Daily_Attendance.Where(x => x.daID == daId).FirstOrDefault();
                    UserMaster user = new UserMaster();
                    user = db.UserMasters.Where(x => x.userId == Daily_Attendanceuser.userId).FirstOrDefault();
                    house.userName = user.userName;

                    var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                    int number = 1000;
                    string refer = "HPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    house.WardList = ListWardNo();
                    house.AreaList = ListArea();
                    house.ZoneList = ListZone();
                    house.houseId = id;
                    return house;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SBALUserLocationMapView GetCTPTByIdforMap(int teamId, int daId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.CTPTQRCode + "/";
                SBALUserLocationMapView house = new SBALUserLocationMapView();

                // var Details = db.HouseMasters.Where(x => x.houseId == teamId).FirstOrDefault();
                var Details = db.SauchalayAddresses.Where(x => x.Id == teamId).FirstOrDefault();
                if (Details != null)
                {
                    house = FillCTPTDetailsViewModelforMap(Details);
                    Daily_Attendance Daily_Attendanceuser = new Daily_Attendance();
                    Daily_Attendanceuser = db.Daily_Attendance.Where(x => x.daID == daId).FirstOrDefault();
                    UserMaster user = new UserMaster();
                    user = db.UserMasters.Where(x => x.userId == Daily_Attendanceuser.userId).FirstOrDefault();
                    house.userName = user.userName;
                    if (house.houseQRCode != null && house.houseQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + house.houseQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                house.houseQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                house.houseQRCode = ThumbnaiUrlCMS + house.houseQRCode.Trim();
                            }
                        }
                        catch (Exception e) { house.houseQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        house.houseQRCode = "/Images/default_not_upload.png";
                    }

                    house.WardList = LoadListWardNo(Convert.ToInt32(house.ZoneId)); //ListWardNo();
                    house.AreaList = LoadListArea(Convert.ToInt32(house.WardNo)); //ListArea();
                    house.ZoneList = ListZone();
                    return house;
                }


                else
                {
                    Daily_Attendance Daily_Attendanceuser = new Daily_Attendance();
                    Daily_Attendanceuser = db.Daily_Attendance.Where(x => x.daID == daId).FirstOrDefault();
                    UserMaster user = new UserMaster();
                    user = db.UserMasters.Where(x => x.userId == Daily_Attendanceuser.userId).FirstOrDefault();
                    house.userName = user.userName;

                    var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                    int number = 1000;
                    string refer = "HPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    house.WardList = ListWardNo();
                    house.AreaList = ListArea();
                    house.ZoneList = ListZone();
                    house.houseId = id;
                    return house;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SBALUserLocationMapView GetLiquidByIdforMap(int teamId, int daId, string EmpType)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.HouseQRCode + "/";
                SBALUserLocationMapView house = new SBALUserLocationMapView();

                var Details = db.HouseMasters.Where(x => x.houseId == teamId).FirstOrDefault();
                if (Details != null)
                {

                    house = FillHouseDetailsViewModelforMap(Details);
                    Daily_Attendance Daily_Attendanceuser = new Daily_Attendance();
                    Daily_Attendanceuser = db.Daily_Attendance.Where(x => x.daID == daId & x.EmployeeType == EmpType).FirstOrDefault();
                    UserMaster user = new UserMaster();
                    user = db.UserMasters.Where(x => x.userId == Daily_Attendanceuser.userId & x.EmployeeType == EmpType).FirstOrDefault();
                    house.userName = user.userName;
                    if (house.houseQRCode != null && house.houseQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + house.houseQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                house.houseQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                house.houseQRCode = ThumbnaiUrlCMS + house.houseQRCode.Trim();
                            }
                        }
                        catch (Exception e) { house.houseQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        house.houseQRCode = "/Images/default_not_upload.png";
                    }

                    house.WardList = LoadListWardNo(Convert.ToInt32(house.ZoneId)); //ListWardNo();
                    house.AreaList = LoadListArea(Convert.ToInt32(house.WardNo)); //ListArea();
                    house.ZoneList = ListZone();
                    return house;
                }


                else
                {
                    Daily_Attendance Daily_Attendanceuser = new Daily_Attendance();
                    Daily_Attendanceuser = db.Daily_Attendance.Where(x => x.daID == daId).FirstOrDefault();
                    UserMaster user = new UserMaster();
                    user = db.UserMasters.Where(x => x.userId == Daily_Attendanceuser.userId).FirstOrDefault();
                    house.userName = user.userName;

                    var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                    int number = 1000;
                    string refer = "HPSBA" + (number + id + 1);
                    house.ReferanceId = refer;
                    house.houseQRCode = "/Images/QRcode.png";
                    house.WardList = ListWardNo();
                    house.AreaList = ListArea();
                    house.ZoneList = ListZone();
                    house.houseId = id;
                    return house;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public HouseDetailsVM SaveHouseDetails(HouseDetailsVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.houseId > 0)
                    {
                        var model = db.HouseMasters.Where(x => x.houseId == data.houseId).FirstOrDefault();
                        if (model != null)
                        {
                            model.WardNo = data.WardNo;
                            model.AreaId = data.AreaId;
                            model.houseOwner = data.houseOwner;
                            model.houseOwnerMar = data.houseOwnerMar;
                            model.houseAddress = data.houseAddress;
                            model.houseOwnerMobile = data.houseMobile;
                            model.houseNumber = data.houseNumber;
                            model.houseQRCode = data.houseQRCode;
                            model.houseLat = data.houseLat;
                            model.houseLong = data.houseLong;
                            model.ZoneId = data.ZoneId;
                            model.lastModifiedEntry = DateTime.Now;
                            model.CType = (data.houseCategory == "RW") ? null : data.houseCategory;
                            //if(data.WasteType== "DW")
                            //{ 
                            //model.WasteType = data.WasteType;
                            //}
                            //if (data.WasteType == "WW")
                            //{
                            //    model.WasteType = data.WasteType;
                            //}
                            //model.userId = data.userId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                        //int number = 1000;
                        //string refer = "SBA" + (number + id + 1);
                        // data.ReferanceId = refer;
                        var type = FillHouseDetailsDataModel(data);
                        db.HouseMasters.Add(type);
                        db.SaveChanges();
                    }
                }
                var houseid = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                HouseDetailsVM vv = GetHouseDetails(houseid);
                return vv;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SWMDetailsVM SaveSWMDetails(SWMDetailsVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.swmId > 0)
                    {
                        var model = db.SWMMasters.Where(x => x.swmId == data.swmId).FirstOrDefault();
                        if (model != null)
                        {
                            model.WardNo = data.WardNo;
                            model.AreaId = data.AreaId;
                            model.swmName = data.swmName;
                            model.swmManager = data.swmManager;
                            model.swmOwnerMar = data.swmOwnerMar;
                            model.swmAddress = data.swmAddress;
                            model.swmOwnerMobile = data.swmMobile;
                            model.swmNumber = data.swmNumber;
                            model.swmQRCode = data.swmQRCode;
                            model.swmLat = data.swmLat;
                            model.swmLong = data.swmLong;
                            model.ZoneId = data.ZoneId;
                            model.lastModifiedEntry = DateTime.Now;
                            model.swmType = (data.swmType == "RW") ? null : data.swmType;
                            model.swmSubType = data.swmSubType;
                            //if(data.WasteType== "DW")
                            //{ 
                            //model.WasteType = data.WasteType;
                            //}
                            //if (data.WasteType == "WW")
                            //{
                            //    model.WasteType = data.WasteType;
                            //}
                            //model.userId = data.userId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                        //int number = 1000;
                        //string refer = "SBA" + (number + id + 1);
                        // data.ReferanceId = refer;
                        var type = FillSWMDetailsDataModel(data);
                        db.SWMMasters.Add(type);
                        db.SaveChanges();
                    }
                }
                var swmid = db.SWMMasters.OrderByDescending(x => x.swmId).Select(x => x.swmId).FirstOrDefault();
                SWMDetailsVM vv = GetSWMDetails(swmid);
                return vv;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CommercialDetailsVM SaveCommercialDetails(CommercialDetailsVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.houseId > 0)
                    {
                        var model = db.CommercialMasters.Where(x => x.commercialId == data.houseId).FirstOrDefault();
                        if (model != null)
                        {
                            model.WardNo = data.WardNo;
                            model.AreaId = data.AreaId;
                            model.commercialOwner = data.houseOwner;
                            model.commercialOwnerMar = data.houseOwnerMar;
                            model.commercialAddress = data.houseAddress;
                            model.commercialOwnerMobile = data.houseMobile;
                            model.commercialNumber = data.houseNumber;
                            model.commercialQRCode = data.houseQRCode;
                            model.commercialLat = data.houseLat;
                            model.commercialLong = data.houseLong;
                            model.ZoneId = data.ZoneId;
                            model.lastModifiedEntry = DateTime.Now;
                            model.CType = (data.houseCategory == "RW") ? null : data.houseCategory;
                            //if(data.WasteType== "DW")
                            //{ 
                            //model.WasteType = data.WasteType;
                            //}
                            //if (data.WasteType == "WW")
                            //{
                            //    model.WasteType = data.WasteType;
                            //}
                            //model.userId = data.userId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //var id = db.HouseMasters.OrderByDescending(x => x.houseId).Select(x => x.houseId).FirstOrDefault();
                        //int number = 1000;
                        //string refer = "SBA" + (number + id + 1);
                        // data.ReferanceId = refer;
                        var type = FillCommercialDetailsDataModel(data);
                        db.CommercialMasters.Add(type);
                        db.SaveChanges();
                    }
                }
                var houseid = db.CommercialMasters.OrderByDescending(x => x.commercialId).Select(x => x.commercialId).FirstOrDefault();
                CommercialDetailsVM vv = GetCommercialDetails(houseid);
                return vv;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void DeletHouseDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.HouseMasters.Where(x => x.houseId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.HouseMasters.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Employee
        public EmployeeDetailsVM GetEmployeeDetails(int teamId)
        {
            try
            {
                EmployeeDetailsVM type = new EmployeeDetailsVM();
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.UserProfile + "/";
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.UserMasters.Where(x => x.userId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        type = FillEmployeeViewModel(Details);
                        if (type.userProfileImage != null && type.userProfileImage != "")
                        {
                            type.userProfileImage = ThumbnaiUrlCMS + type.userProfileImage.Trim();
                        }
                        else
                        {
                            type.userProfileImage = "/Images/default_not_upload.png";
                        }
                        return type;
                    }
                    else
                    {
                        type.userProfileImage = "/Images/add_image_square.png";
                        return type;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




        public SBAAttendenceSettingsGridRow GetAttendenceEmployeeById(int teamId)
        {
            try
            {
                SBAAttendenceSettingsGridRow type = new SBAAttendenceSettingsGridRow();
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.UserProfile + "/";
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.Daily_Attendance.Where(x => x.userId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        type = FillAttendenceEmployeeViewModel(Details);

                        return type;
                    }
                    else
                    {
                        return type;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteEmployeeDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.UserMasters.Where(x => x.userId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.UserMasters.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public string GetLoginidData(string LoginId)
        {

            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var isrecord = db.QrEmployeeMasters.Where(x => x.qrEmpLoginId == LoginId && x.isActive == true).FirstOrDefault();
                var isrecord1 = db.UserMasters.Where(x => x.userLoginId == LoginId && x.isActive == true).FirstOrDefault();
                if (isrecord == null && isrecord1 == null)
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }



        }

        public void SaveEmployeeDetails(EmployeeDetailsVM data, string Emptype)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.userId > 0)
                    {
                        var model = db.UserMasters.Where(x => x.userId == data.userId).FirstOrDefault();
                        if (model != null)
                        {
                            if (data.userProfileImage != null)
                            {
                                model.userProfileImage = data.userProfileImage;
                            }
                            model.userId = data.userId;
                            model.userAddress = data.userAddress;
                            model.userLoginId = data.userLoginId;
                            model.userMobileNumber = data.userMobileNumber;
                            model.userName = data.userName;
                            model.userNameMar = data.userNameMar;
                            model.userPassword = data.userPassword;
                            model.userEmployeeNo = data.userEmployeeNo;
                            model.imoNo = data.imoNo;
                            model.isActive = data.isActive;
                            model.bloodGroup = data.bloodGroup;
                            model.gcTarget = data.gcTarget;
                            model.ComgcTarget = data.ComgcTarget;
                            model.userDesignation = data.userDesignation;
                            //model.EmployeeType = Emptype;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillEmployeeDataModel(data, Emptype);
                        db.UserMasters.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SaveAttendenceSettingsDetail(SBAAttendenceSettingsGridRow data)
        {

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    List<SBAAttendenceSettingsGridRow> datalist = new List<SBAAttendenceSettingsGridRow>();
                    var data1 = db.Vw_MsgNotification.ToList();
                    foreach (var x in data1)
                    {
                        datalist.Add(new SBAAttendenceSettingsGridRow()
                        {
                            userId = Convert.ToInt32(x.userId),
                            userName = db.UserMasters.Where(c => c.userId == x.userId).FirstOrDefault().userName,
                        });

                        string mes = "कचरा संकलन कर्मचारी" + db.UserMasters.Where(c => c.userId == x.userId).FirstOrDefault().userName + "आज ड्युटी वर गैरहजर आहे";
                        string housemob = "8830635095";
                        sendSMS(mes, housemob);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void sendSMS(string sms, string MobilNumber)
        {
            try
            {
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=YOCCAG&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message=" + sms + "&response=Y");
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=YOCCAG&dest_mobileno=" + MobilNumber + "&message=" + sms + "&response=Y");
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=artiyocc&pass=123456&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message=" + sms + "%20&response=Y");

                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&message=" + sms + "%20&response=Y");

                //Get response from Ozeki NG SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
            }
            catch { }

        }

        #endregion
        #region Compalint

        public ComplaintVM GetCompalint(int teamId)
        {
            ComplaintVM comp = new ComplaintVM();
            DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
            var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
            // string json = new WebClient().DownloadString("http://192.168.200.3:8077////api/Get/Complaint?appId=1");
            string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=1");
            List<ComplaintVM> obj2 = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).ToList();
            var data = obj2.Where(c => c.complaintId == teamId).FirstOrDefault();
            comp.complaintId = data.complaintId;
            comp.status = data.status;
            string image = "";
            if (data.endImage == null || data.endImage == "")
            {
                image = "/Images/default_not_upload.png";
            }
            else
            {
                comp.endImage = data.endImage;
            }
            comp.comment = data.comment;

            return comp;

        }
        public void SaveComplaintStatus(ComplaintVM data)
        {
            //try
            //{
            //    using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            //    {
            //        if (data.userId > 0)
            //        {
            //            var model = db.UserMasters.Where(x => x.userId == data.userId).FirstOrDefault();
            //            if (model != null)
            //            {
            //                if (data.userProfileImage != null)
            //                {
            //                    model.userProfileImage = data.userProfileImage;
            //                }
            //                model.userId = data.userId;
            //                model.userAddress = data.userAddress;
            //                model.userLoginId = data.userLoginId;
            //                model.userMobileNumber = data.userMobileNumber;
            //                model.userName = data.userName;
            //                model.userNameMar = data.userNameMar;
            //                model.userPassword = data.userPassword;
            //                model.userEmployeeNo = data.userEmployeeNo;
            //                model.imoNo = data.imoNo;
            //                db.SaveChanges();
            //            }
            //        }
            //        else
            //        {
            //            var type = FillEmployeeDataModel(data);
            //            db.UserMasters.Add(type);
            //            db.SaveChanges();
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        #endregion

        #region Location
        public string Address(string location)
        {
            if (location != string.Empty && location != null)
            {
                string lat = null, log = null;
                string[] arr = new string[2];
                arr = location.Split(',');
                lat = arr[0];
                log = arr[1];
                XmlDocument doc = new XmlDocument();
                string Address = "";

                //doc.Load("http://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat + "," + log + "& sensor=false");
                doc.Load("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat + "," + log + "&sensor=false&key=AIzaSyBy6BUqH6o1r7JBS8s1Tk7cmllapL6xuMA");

                XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
                if (element.InnerText == "ZERO_RESULTS")
                {
                    Console.WriteLine("No data available for the specified location");
                }
                else
                {
                    XmlNode xnList1 = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");

                    if (xnList1 != null)
                    {
                        Address = xnList1.InnerText;
                    }
                    else
                    {
                        Address = "";
                    }
                }
                return Address;
            }
            else
            {
                return "";
            }


        }
        public SBALUserLocationMapView GetLocationDetails(int teamId, string Emptype)
        {
            try
            {
                if (Emptype == "NULL" || Emptype == null)
                {
                    using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                    {
                        //var Details = db.Locations.Where(c => c.EmployeeType == null).FirstOrDefault();
                        var Details = db.Locations.FirstOrDefault();

                        if (teamId > 0)
                        {
                            //Details = db.Locations.Where(c => c.locId == teamId && c.EmployeeType == null).FirstOrDefault();
                            Details = db.Locations.Where(c => c.locId == teamId).FirstOrDefault();
                        }
                        if (Details != null)
                        {
                            //var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId && c.EmployeeType == null).FirstOrDefault();
                            var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId).FirstOrDefault();
                            SBALUserLocationMapView loc = new SBALUserLocationMapView();
                            //var user = db.UserMasters.Where(c => c.userId == Details.userId && c.EmployeeType == null).FirstOrDefault();
                            var user = db.UserMasters.Where(c => c.userId == Details.userId).FirstOrDefault();
                            loc.userName = user.userName;
                            loc.date = Convert.ToDateTime(Details.datetime).ToString("dd/MM/yyyy");
                            loc.time = Convert.ToDateTime(Details.datetime).ToString("hh:mm tt");
                            loc.address = checkNull(Details.address).Replace("Unnamed Road, ", "");
                            loc.lat = Details.lat;
                            loc.log = Details.@long;
                            loc.UserList = ListUser(null);
                            loc.userMobile = user.userMobileNumber;
                            loc.type = Convert.ToInt32(user.Type);
                            try { loc.vehcileNumber = atten.vehicleNumber; } catch { loc.vehcileNumber = ""; }

                            return loc;
                        }
                        else
                        {
                            return new SBALUserLocationMapView();
                        }

                    }
                }
                else if (Emptype == "L")
                {
                    using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                    {
                        //var Details = db.Locations.Where(c => c.EmployeeType == Emptype).FirstOrDefault();
                        var Details = db.Locations.FirstOrDefault();

                        if (teamId > 0)
                        {
                            // Details = db.Locations.Where(c => c.locId == teamId && c.EmployeeType == Emptype).FirstOrDefault();
                            Details = db.Locations.Where(c => c.locId == teamId).FirstOrDefault();
                        }

                        if (Details != null)
                        {
                            //var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId && c.EmployeeType == Emptype).FirstOrDefault();
                            var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId).FirstOrDefault();
                            SBALUserLocationMapView loc = new SBALUserLocationMapView();
                            var user = db.UserMasters.Where(c => c.userId == Details.userId).FirstOrDefault();
                            loc.userName = user.userName;
                            loc.date = Convert.ToDateTime(Details.datetime).ToString("dd/MM/yyyy");
                            loc.time = Convert.ToDateTime(Details.datetime).ToString("hh:mm tt");
                            loc.address = checkNull(Details.address).Replace("Unnamed Road, ", "");
                            loc.lat = Details.lat;
                            loc.log = Details.@long;
                            loc.UserList = ListUser(Emptype);
                            loc.userMobile = user.userMobileNumber;
                            loc.type = Convert.ToInt32(user.Type);
                            try { loc.vehcileNumber = atten.vehicleNumber; } catch { loc.vehcileNumber = ""; }

                            return loc;
                        }
                        else
                        {
                            return new SBALUserLocationMapView();
                        }

                    }
                }
                else if (Emptype == "S")
                {
                    using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                    {
                        //var Details = db.Locations.Where(c => c.EmployeeType == Emptype).FirstOrDefault();
                        var Details = db.Locations.FirstOrDefault();

                        if (teamId > 0)
                        {
                            //Details = db.Locations.Where(c => c.locId == teamId && c.EmployeeType == Emptype).FirstOrDefault();
                            Details = db.Locations.Where(c => c.locId == teamId).FirstOrDefault();

                        }

                        if (Details != null)
                        {
                            //var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId && c.EmployeeType == Emptype).FirstOrDefault();
                            var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId).FirstOrDefault();
                            SBALUserLocationMapView loc = new SBALUserLocationMapView();
                            var user = db.UserMasters.Where(c => c.userId == Details.userId).FirstOrDefault();
                            loc.userName = user.userName;
                            loc.date = Convert.ToDateTime(Details.datetime).ToString("dd/MM/yyyy");
                            loc.time = Convert.ToDateTime(Details.datetime).ToString("hh:mm tt");
                            loc.address = checkNull(Details.address).Replace("Unnamed Road, ", "");
                            loc.lat = Details.lat;
                            loc.log = Details.@long;
                            loc.UserList = ListUser(Emptype);
                            loc.userMobile = user.userMobileNumber;
                            loc.type = Convert.ToInt32(user.Type);
                            try { loc.vehcileNumber = atten.vehicleNumber; } catch { loc.vehcileNumber = ""; }

                            return loc;
                        }
                        else
                        {
                            return new SBALUserLocationMapView();
                        }

                    }
                }

                else if (Emptype == "CT")
                {
                    using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                    {
                        //var Details = db.Locations.Where(c => c.EmployeeType == Emptype).FirstOrDefault();
                        var Details = db.Locations.FirstOrDefault();

                        if (teamId > 0)
                        {
                            //Details = db.Locations.Where(c => c.locId == teamId && c.EmployeeType == Emptype).FirstOrDefault();
                            Details = db.Locations.Where(c => c.locId == teamId).FirstOrDefault();

                        }

                        if (Details != null)
                        {
                            //var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId && c.EmployeeType == Emptype).FirstOrDefault();
                            var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId).FirstOrDefault();
                            SBALUserLocationMapView loc = new SBALUserLocationMapView();
                            var user = db.UserMasters.Where(c => c.userId == Details.userId).FirstOrDefault();
                            loc.userName = user.userName;
                            loc.date = Convert.ToDateTime(Details.datetime).ToString("dd/MM/yyyy");
                            loc.time = Convert.ToDateTime(Details.datetime).ToString("hh:mm tt");
                            loc.address = checkNull(Details.address).Replace("Unnamed Road, ", "");
                            loc.lat = Details.lat;
                            loc.log = Details.@long;
                            loc.UserList = CTPTListUser(Emptype);
                            loc.userMobile = user.userMobileNumber;
                            loc.type = Convert.ToInt32(user.Type);
                            try { loc.vehcileNumber = atten.vehicleNumber; } catch { loc.vehcileNumber = ""; }

                            return loc;
                        }
                        else
                        {
                            return new SBALUserLocationMapView();
                        }

                    }
                }

                else
                {
                    using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                    {
                        var Details = db.Locations.Where(c => c.EmployeeType == Emptype).FirstOrDefault();

                        if (teamId > 0)
                        {
                            //Details = db.Locations.Where(c => c.locId == teamId && c.EmployeeType == Emptype).FirstOrDefault();
                            Details = db.Locations.Where(c => c.locId == teamId).FirstOrDefault();

                        }

                        if (Details != null)
                        {
                            //var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId && c.EmployeeType == Emptype).FirstOrDefault();
                            var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId).FirstOrDefault();
                            SBALUserLocationMapView loc = new SBALUserLocationMapView();
                            var user = db.UserMasters.Where(c => c.userId == Details.userId).FirstOrDefault();
                            loc.userName = user.userName;
                            loc.date = Convert.ToDateTime(Details.datetime).ToString("dd/MM/yyyy");
                            loc.time = Convert.ToDateTime(Details.datetime).ToString("hh:mm tt");
                            loc.address = checkNull(Details.address).Replace("Unnamed Road, ", "");
                            loc.lat = Details.lat;
                            loc.log = Details.@long;
                            loc.UserList = ListUser(Emptype);
                            loc.userMobile = user.userMobileNumber;
                            loc.type = Convert.ToInt32(user.Type);
                            try { loc.vehcileNumber = atten.vehicleNumber; } catch { loc.vehcileNumber = ""; }

                            return loc;
                        }
                        else
                        {
                            return new SBALUserLocationMapView();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                return new SBALUserLocationMapView();
            }
        }

        public List<SBALUserLocationMapView> GetAllUserLocation(string date, string Emptype)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();

            if (Emptype == null)
            {
                var data = db.CurrentAllUserLocationTest1().ToList();
                foreach (var x in data)
                {
                    //string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    //string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    ////var atten = db.Daily_Attendance.Where(c => c.userId == x.userid && (c.daDate ==EntityFunctions.TruncateTime(x.datetime) && (c.endTime == null || c.endTime == ""))).FirstOrDefault();

                    //var atten = db.Daily_Attendance.Where(c => c.userId == x.userid && (c.endTime == null || c.endTime =="")).FirstOrDefault();

                    //if (atten != null)
                    //{
                    //    userLocation.Add(new SBALUserLocationMapView()
                    //    {
                    //        userId = Convert.ToInt32(x.userid),
                    //        userName = x.userName,
                    //        date = dat,
                    //        time = tim,
                    //        lat = x.lat,
                    //        log = x.@long,
                    //        address = Address(x.lat + "," + x.@long),
                    //        vehcileNumber = atten.vehicleNumber,
                    //        userMobile = x.userMobileNumber,
                    //    });
                    //}


                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userId = Convert.ToInt32(x.userid),
                        userName = x.userName,
                        date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        time = Convert.ToDateTime(x.datt).ToString("hh:mm tt"),
                        lat = x.lat,
                        log = x.lang,
                        address = checkNull(x.addr).Replace("Unnamed Road, ", ""),
                        vehcileNumber = x.v,
                        userMobile = x.mobile,
                    });
                }
            }
            else if (Emptype == "L")
            {
                var data = db.LiquidCurrentAllUserLocationTest1().ToList();
                foreach (var x in data)
                {
                    //string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    //string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    ////var atten = db.Daily_Attendance.Where(c => c.userId == x.userid && (c.daDate ==EntityFunctions.TruncateTime(x.datetime) && (c.endTime == null || c.endTime == ""))).FirstOrDefault();

                    //var atten = db.Daily_Attendance.Where(c => c.userId == x.userid && (c.endTime == null || c.endTime =="")).FirstOrDefault();

                    //if (atten != null)
                    //{
                    //    userLocation.Add(new SBALUserLocationMapView()
                    //    {
                    //        userId = Convert.ToInt32(x.userid),
                    //        userName = x.userName,
                    //        date = dat,
                    //        time = tim,
                    //        lat = x.lat,
                    //        log = x.@long,
                    //        address = Address(x.lat + "," + x.@long),
                    //        vehcileNumber = atten.vehicleNumber,
                    //        userMobile = x.userMobileNumber,
                    //    });
                    //}


                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userId = Convert.ToInt32(x.userid),
                        userName = x.userName,
                        date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        time = Convert.ToDateTime(x.datt).ToString("hh:mm tt"),
                        lat = x.lat,
                        log = x.lang,
                        address = checkNull(x.addr).Replace("Unnamed Road, ", ""),
                        vehcileNumber = x.v,
                        userMobile = x.mobile,
                    });
                }
            }

            else if (Emptype == "S")
            {
                var data = db.StreetCurrentAllUserLocationTest1().ToList();
                foreach (var x in data)
                {

                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userId = Convert.ToInt32(x.userid),
                        userName = x.userName,
                        date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        time = Convert.ToDateTime(x.datt).ToString("hh:mm tt"),
                        lat = x.lat,
                        log = x.lang,
                        address = checkNull(x.addr).Replace("Unnamed Road, ", ""),
                        vehcileNumber = x.v,
                        userMobile = x.mobile,
                    });
                }
            }
            else
            {
                var data = db.CurrentAllUserLocationTest1().ToList();
                foreach (var x in data)
                {

                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userId = Convert.ToInt32(x.userid),
                        userName = x.userName,
                        date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        time = Convert.ToDateTime(x.datt).ToString("hh:mm tt"),
                        lat = x.lat,
                        log = x.lang,
                        address = checkNull(x.addr).Replace("Unnamed Road, ", ""),
                        vehcileNumber = x.v,
                        userMobile = x.mobile,
                    });
                }
            }
            return userLocation;
        }


        public List<SBALUserLocationMapView> GetAdminLocation()
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            var data = dbMain.AppDetails.Where(c => c.IsActive == true && c.Latitude != null && c.Logitude != null).ToList();
            foreach (var x in data)
            {

                userLocation.Add(new SBALUserLocationMapView()
                {
                    userId = Convert.ToInt32(x.AppId),
                    userName = x.AppName,
                    date = DateTime.Now.ToString("dd/MM/yyyy"),
                    time = DateTime.Now.ToString("hh:mm:ss tt"),
                    lat = x.Latitude,
                    log = x.Logitude,
                    address = "",
                    vehcileNumber = "",
                    userMobile = "",
                });
            }

            return userLocation;
        }


        public List<SBALUserLocationMapView> GetUserWiseLocation(int userId, string date, string Emptype)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;


            DateTime dt = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
            // for both "1/1/2000" or "25/1/2000" formats
            //  string newString = dt.ToString("MM/dd/yyyy");
            //var dat1 = DateTime.ParseExact(date, "M/d/yyyy", CultureInfo.InvariantCulture);

            if (Emptype == null)
            {
                var data = db.Locations.Where(c => c.userId == userId && c.EmployeeType == null).ToList();
                foreach (var x in data.Where(c => Convert.ToDateTime(c.datetime).Date == Convert.ToDateTime(dt)))
                {
                    string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == userId).FirstOrDefault();
                    var atten = db.Daily_Attendance.Where(c => c.userId == x.userId && (c.daDate == EntityFunctions.TruncateTime(x.datetime))).FirstOrDefault();
                    string v = "";
                    //if (atten != null)
                    //{
                    //    v = atten.vehicleNumber;
                    //}
                    //else { v = ""; }

                    v = (string.IsNullOrEmpty(atten.vehicleNumber) ? "" : atten.vehicleNumber);

                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userName = userName.userName,
                        date = dat,
                        time = tim,
                        lat = x.lat,
                        log = x.@long,
                        address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                        vehcileNumber = v,
                        userMobile = userName.userMobileNumber,
                    });
                }
            }
            else if (Emptype == "L")
            {
                var data = db.Locations.Where(c => c.userId == userId && c.EmployeeType == "L").ToList();
                foreach (var x in data.Where(c => Convert.ToDateTime(c.datetime).Date == Convert.ToDateTime(dt)))
                {
                    string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == userId).FirstOrDefault();
                    var atten = db.Daily_Attendance.Where(c => c.userId == x.userId && (c.daDate == EntityFunctions.TruncateTime(x.datetime))).FirstOrDefault();
                    string v = "";
                    //if (atten != null)
                    //{
                    //    v = atten.vehicleNumber;
                    //}
                    //else { v = ""; }

                    v = (string.IsNullOrEmpty(atten.vehicleNumber) ? "" : atten.vehicleNumber);

                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userName = userName.userName,
                        date = dat,
                        time = tim,
                        lat = x.lat,
                        log = x.@long,
                        address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                        vehcileNumber = v,
                        userMobile = userName.userMobileNumber,
                    });
                }
            }
            else if (Emptype == "S")
            {
                var data = db.Locations.Where(c => c.userId == userId && c.EmployeeType == "S").ToList();
                foreach (var x in data.Where(c => Convert.ToDateTime(c.datetime).Date == Convert.ToDateTime(dt)))
                {
                    string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == userId).FirstOrDefault();
                    var atten = db.Daily_Attendance.Where(c => c.userId == x.userId && (c.daDate == EntityFunctions.TruncateTime(x.datetime))).FirstOrDefault();
                    string v = "";
                    //if (atten != null)
                    //{
                    //    v = atten.vehicleNumber;
                    //}
                    //else { v = ""; }

                    v = (string.IsNullOrEmpty(atten.vehicleNumber) ? "" : atten.vehicleNumber);

                    userLocation.Add(new SBALUserLocationMapView()
                    {
                        userName = userName.userName,
                        date = dat,
                        time = tim,
                        lat = x.lat,
                        log = x.@long,
                        address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                        vehcileNumber = v,
                        userMobile = userName.userMobileNumber,
                    });
                }
            }
            return userLocation;
        }

        public List<SBALUserLocationMapView> GetUserAttenLocation(int daId)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var data = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            var user = db.UserMasters.Where(c => c.userId == data.userId).FirstOrDefault();
            if (data.startLat.Trim() != null & data.startLat.Trim() != "")
            {
                userLocation.Add(new SBALUserLocationMapView()
                {
                    userName = user.userName,
                    date = Convert.ToDateTime(data.daDate).ToString("dd/MM/yyyy"),
                    time = data.startTime,
                    address = Address(data.startLat + "," + data.startLong),
                    lat = data.startLat,
                    log = data.startLong,
                    vehcileNumber = data.vehicleNumber,
                    userMobile = user.userMobileNumber

                });
            }
            if (data.endLat.Trim() != null & data.endLat.Trim() != "")
            {
                userLocation.Add(new SBALUserLocationMapView()
                {
                    userName = user.userName,
                    date = Convert.ToDateTime(data.daDate).ToString("dd/MM/yyyy"),
                    time = data.endTime,
                    address = Address(data.endLat + "," + data.endLong),
                    lat = data.endLat,
                    log = data.endLong,
                    vehcileNumber = data.vehicleNumber,
                    userMobile = user.userMobileNumber
                });
            }
            return userLocation;
        }

        //public List<SBALUserLocationMapView> GetUserAttenRoute(int daId)
        //{
        //    List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
        //    DateTime newdate = DateTime.Now.Date;
        //    var datt = newdate;

        //    var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();

        //    string Time = att.startTime;
        //    DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
        //    string t = date.ToString("hh:mm:ss tt");
        //    string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
        //    DateTime? fdate = Convert.ToDateTime(dt + " " + t);
        //    DateTime? edate;
        //    if (att.endTime == "" | att.endTime == null)
        //    {
        //        edate = DateTime.Now;
        //    }
        //    else
        //    {
        //        string Time2 = att.endTime;
        //        DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
        //        string t2 = date2.ToString("hh:mm:ss tt");
        //        string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
        //        edate = Convert.ToDateTime(dt2 + " " + t2);
        //    }

        //    var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate).ToList();

        //    foreach (var x in data)
        //    {

        //        if (x.type == 1)
        //        {
        //            string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
        //            string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
        //            var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();

        //            var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).FirstOrDefault();

        //            var house = db.HouseMasters.Where(c => c.houseId == gcd.houseId).FirstOrDefault();

        //            userLocation.Add(new SBALUserLocationMapView()
        //            {
        //                userName = userName.userName,
        //                date = dat,
        //                time = tim,
        //                lat = x.lat,
        //                log = x.@long,
        //                vehcileNumber = att.vehicleNumber,
        //                HouseOwnerName = house.houseOwner,
        //                OwnerMobileNo = house.houseOwnerMobile,
        //                HouseId = house.ReferanceId,
        //                HouseAddress = house.houseAddress,
        //                type = Convert.ToInt32(x.type),
        //            });

        //        }

        //        else
        //        {
        //            string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
        //            string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
        //            var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();

        //            //   var gcd = db.GarbageCollectionDetails.Where(c => c.userId == x.userId & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).FirstOrDefault();

        //            //var house = db.HouseMasters.Where(c => c.houseId == gcd.houseId).FirstOrDefault();

        //            userLocation.Add(new SBALUserLocationMapView()
        //            {
        //                userName = userName.userName,
        //                date = dat,
        //                time = tim,
        //                lat = x.lat,
        //                log = x.@long,
        //                vehcileNumber = att.vehicleNumber,
        //                HouseOwnerName = "",
        //                OwnerMobileNo = "",
        //                HouseId = "",
        //                address = x.address,
        //                type = 0,
        //            });

        //        }
        //    }
        //    return userLocation;
        //}

        //public list<sbaluserlocationmapview> getuserattenroute(int daid)
        //{
        //    list<sbaluserlocationmapview> userlocation = new list<sbaluserlocationmapview>();
        //    datetime newdate = datetime.now.date;
        //    var datt = newdate;
        //    var att = db.daily_attendance.where(c => c.daid == daid).firstordefault();
        //    string time = att.starttime;
        //    datetime date = datetime.parse(time, system.globalization.cultureinfo.currentculture);
        //    string t = date.tostring("hh:mm:ss tt");
        //    string dt = convert.todatetime(att.dadate).tostring("mm/dd/yyyy");
        //    datetime? fdate = convert.todatetime(dt + " " + t);
        //    datetime? edate;
        //    if (att.endtime == "" | att.endtime == null)
        //    {
        //        edate = datetime.now;
        //    }
        //    else {
        //        string time2 = att.endtime;
        //        datetime date2 = datetime.parse(time2, system.globalization.cultureinfo.currentculture);
        //        string t2 = date2.tostring("hh:mm:ss tt");
        //        string dt2 = convert.todatetime(att.daenddate).tostring("mm/dd/yyyy");
        //        edate = convert.todatetime(dt2 + " " + t2);
        //    }
        //    var data = db.locations.where(c => c.userid == att.userid & c.datetime >= fdate & c.datetime <= edate).tolist();
        //    //foreach (var x in data)
        //    //{
        //    //    string dat = convert.todatetime(x.datetime).tostring("dd/mm/yyyy");
        //    //    string tim = convert.todatetime(x.datetime).tostring("hh:mm tt");
        //    //    var username = db.usermasters.where(c => c.userid == att.userid).firstordefault();
        //    //    userlocation.add(new sbaluserlocationmapview()
        //    //    {
        //    //        username = username.username,
        //    //        date = dat,
        //    //        time = tim,
        //    //        lat = x.lat,
        //    //        log = x.@long,
        //    //        address = x.address,
        //    //        vehcilenumber = att.vehiclenumber,
        //    //        usermobile = username.usermobilenumber,
        //    //    });
        //    //}

        //    foreach (var x in data)
        //    {
        //        string dat = convert.todatetime(x.datetime).tostring("dd/mm/yyyy");
        //        string tim = convert.todatetime(x.datetime).tostring("hh:mm tt");
        //        var username = db.usermasters.where(c => c.userid == att.userid).firstordefault();

        //        var gcd = db.garbagecollectiondetails.where(c => c.userid == x.userid & entityfunctions.truncatetime(c.gcdate) == entityfunctions.truncatetime(x.datetime)).firstordefault();


        //        var house = db.housemasters.where(c => c.houseid == gcd.houseid).firstordefault();

        //        userlocation.add(new sbaluserlocationmapview()
        //        {
        //            username = username.username,
        //            date = dat,
        //            time = tim,
        //            lat = x.lat,
        //            log = x.@long,
        //            vehcilenumber = att.vehiclenumber,
        //            houseownername = house.houseowner,
        //            ownermobileno = house.houseownermobile,
        //            houseid = house.referanceid,
        //            houseaddress = house.houseaddress
        //        });
        //    }


        //    return userlocation;
        //}


        public List<SBALUserLocationMapView> GetUserAttenRoute(int daId)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }
            var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == null).OrderBy(x => x.locId).ToList();


            foreach (var x in data)
            {

                string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();

                userLocation.Add(new SBALUserLocationMapView()
                {
                    userId = userName.userId,
                    userName = userName.userName,
                    datetime = Convert.ToDateTime(x.datetime).ToString("HH:mm"),
                    date = dat,
                    time = tim,
                    lat = x.lat,
                    log = x.@long,
                    address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    vehcileNumber = att.vehicleNumber,
                    userMobile = userName.userMobileNumber,
                    // type = Convert.ToInt32(x.type),

                });

            }

            return userLocation;
        }

        public List<SBALUserLocationMapView> GetCTPTUserAttenRoute(int daId)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }
            var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == null).OrderBy(x => x.locId).ToList();


            foreach (var x in data)
            {

                string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();

                userLocation.Add(new SBALUserLocationMapView()
                {
                    userId = userName.userId,
                    userName = userName.userName,
                    datetime = Convert.ToDateTime(x.datetime).ToString("HH:mm"),
                    date = dat,
                    time = tim,
                    lat = x.lat,
                    log = x.@long,
                    address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    vehcileNumber = att.vehicleNumber,
                    userMobile = userName.userMobileNumber,
                    // type = Convert.ToInt32(x.type),

                });

            }

            return userLocation;
        }

        // Added By Saurabh (11 July 2019)
        public List<SBALUserLocationMapView> GetHouseAttenRoute(int daId, int areaid)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }
            var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == 1).OrderByDescending(a => a.datetime).ToList();

            foreach (var x in data)
            {
                if (x.type == 1)
                {

                    // string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    //string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();
                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).FirstOrDefault();

                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcDate).ToList();//.ToList();

                    var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & (c.houseId != null || c.dyId != null || c.commercialId != null)) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcId).ToList();//.ToList();


                    foreach (var d in gcd)
                    {
                        //DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                        string dat = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy");
                        string tim = Convert.ToDateTime(d.gcDate).ToString("hh:mm tt");
                        if (d.houseId != null)
                        {
                            if (areaid != 0)
                            {
                                var house = db.HouseMasters.Where(c => c.houseId == d.houseId & c.AreaId == areaid).FirstOrDefault();
                                if (house != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userId = userName.userId,
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        new_datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy hh:mm tt"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        HouseId = house.ReferanceId,
                                        HouseAddress = (house.houseAddress == null ? "" : house.houseAddress.Replace("Unnamed Road, ", "")),
                                        HouseOwnerName = house.houseOwner,
                                        OwnerMobileNo = house.houseOwnerMobile,
                                        WasteType = d.garbageType.ToString(),
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var house = db.HouseMasters.Where(c => c.houseId == d.houseId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userId = userName.userId,
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    new_datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy hh:mm tt"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    HouseId = house.ReferanceId,
                                    HouseAddress = (house.houseAddress == null ? "" : house.houseAddress.Replace("Unnamed Road, ", "")),
                                    HouseOwnerName = house.houseOwner,
                                    OwnerMobileNo = house.houseOwnerMobile,
                                    WasteType = d.garbageType.ToString(),
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    ZoneList = ListZone(),

                                });
                            }
                        }

                        // Roshan 25-03-2022
                        if (d.commercialId != null)
                        {
                            if (areaid != 0)
                            {
                                var commercial = db.CommercialMasters.Where(c => c.commercialId == d.commercialId & c.AreaId == areaid).FirstOrDefault();
                                if (commercial != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userId = userName.userId,
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        new_datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy hh:mm tt"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        CommercialId = commercial.ReferanceId,
                                        HouseAddress = (commercial.commercialAddress == null ? "" : commercial.commercialAddress.Replace("Unnamed Road, ", "")),
                                        HouseOwnerName = commercial.commercialOwner,
                                        OwnerMobileNo = commercial.commercialOwnerMobile,
                                        WasteType = d.garbageType.ToString(),
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var commercial = db.CommercialMasters.Where(c => c.commercialId == d.commercialId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userId = userName.userId,
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    new_datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy hh:mm tt"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    CommercialId = commercial.ReferanceId,
                                    HouseAddress = (commercial.commercialAddress == null ? "" : commercial.commercialAddress.Replace("Unnamed Road, ", "")),
                                    HouseOwnerName = commercial.commercialOwner,
                                    OwnerMobileNo = commercial.commercialOwnerMobile,
                                    WasteType = d.garbageType.ToString(),
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    ZoneList = ListZone(),

                                });
                            }
                        }
                        //End

                        if (d.dyId != null)
                        {
                            if (areaid != 0)
                            {
                                var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId & c.areaId == areaid).FirstOrDefault();
                                if (dump != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userId = userName.userId,
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        new_datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy hh:mm tt"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        DyId = dump.ReferanceId,
                                        DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                                        DumpYardName = dump.dyName,
                                        OwnerMobileNo = dump.dyNameMar,
                                        WasteType = d.garbageType.ToString(),
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        DryWaste = d.totalDryWeight.ToString(),
                                        WetWaste = d.totalWetWeight.ToString(),
                                        TotWaste = d.totalGcWeight.ToString(),
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userId = userName.userId,
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    new_datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy hh:mm tt"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    DyId = dump.ReferanceId,
                                    DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                                    DumpYardName = dump.dyName,
                                    OwnerMobileNo = dump.dyNameMar,
                                    WasteType = d.garbageType.ToString(),
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    DryWaste = d.totalDryWeight.ToString(),
                                    WetWaste = d.totalWetWeight.ToString(),
                                    TotWaste = d.totalGcWeight.ToString(),
                                    ZoneList = ListZone(),

                                });
                            }
                        }

                    }
                    break;
                }

            }

            return userLocation;
        }

        public List<SBALUserLocationMapView> GetCTPTAttenRoute(int daId, int areaid)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }
            var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == 1).OrderByDescending(a => a.datetime).ToList();

            foreach (var x in data)
            {
                if (x.type == 1)
                {

                    // string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    //string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();
                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).FirstOrDefault();

                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcDate).ToList();//.ToList();

                    var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & (c.CTPTId != null || c.dyId != null)) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcId).ToList();//.ToList();


                    foreach (var d in gcd)
                    {
                        //DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                        string dat = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy");
                        string tim = Convert.ToDateTime(d.gcDate).ToString("hh:mm tt");
                        if (d.CTPTId != null)
                        {
                            if (areaid != 0)
                            {
                                var house = db.SauchalayAddresses.Where(c => c.Id == d.CTPTId & c.AreaId == areaid).FirstOrDefault();
                                if (house != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userId = userName.userId,
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        HouseId = house.ReferanceId,
                                        HouseAddress = (house.Address == null ? "" : house.Address.Replace("Unnamed Road, ", "")),
                                        HouseOwnerName = house.Name,
                                        OwnerMobileNo = house.Mobile,
                                        WasteType = d.TOT.ToString(),
                                        //WasteType = d.garbageType.ToString(),
                                        //WasteType = "",
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var house = db.SauchalayAddresses.Where(c => c.Id == d.CTPTId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userId = userName.userId,
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    //vehcileNumber = "",
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    HouseId = house.ReferanceId,
                                    HouseAddress = (house.Address == null ? "" : house.Address.Replace("Unnamed Road, ", "")),
                                    HouseOwnerName = house.Name,
                                    OwnerMobileNo = house.Mobile,
                                    WasteType = d.TOT.ToString(),
                                    //WasteType = d.garbageType.ToString(),
                                    //WasteType = "",
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    ZoneList = ListZone(),

                                });
                            }


                        }
                        if (d.dyId != null)
                        {
                            if (areaid != 0)
                            {
                                var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId & c.areaId == areaid).FirstOrDefault();
                                if (dump != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userId = userName.userId,
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        DyId = dump.ReferanceId,
                                        DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                                        DumpYardName = dump.dyName,
                                        OwnerMobileNo = dump.dyNameMar,
                                        WasteType = d.garbageType.ToString(),
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        DryWaste = d.totalDryWeight.ToString(),
                                        WetWaste = d.totalWetWeight.ToString(),
                                        TotWaste = d.totalGcWeight.ToString(),
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userId = userName.userId,
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    DyId = dump.ReferanceId,
                                    DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                                    DumpYardName = dump.dyName,
                                    OwnerMobileNo = dump.dyNameMar,
                                    WasteType = d.garbageType.ToString(),
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    DryWaste = d.totalDryWeight.ToString(),
                                    WetWaste = d.totalWetWeight.ToString(),
                                    TotWaste = d.totalGcWeight.ToString(),
                                    ZoneList = ListZone(),

                                });
                            }


                        }

                    }
                    break;
                }

            }

            return userLocation;
        }
        public List<SBALUserLocationMapView> GetLiquidAttenRoute(int daId, int areaid)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }
            var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == 1).OrderByDescending(a => a.datetime).ToList();

            foreach (var x in data)
            {
                if (x.type == 1)
                {

                    // string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    //string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();
                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).FirstOrDefault();

                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcDate).ToList();//.ToList();

                    var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & (c.LWId != null || c.dyId != null)) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcId).ToList();//.ToList();


                    foreach (var d in gcd)
                    {
                        //DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                        string dat = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy");
                        string tim = Convert.ToDateTime(d.gcDate).ToString("hh:mm tt");
                        if (d.LWId != null)
                        {
                            if (areaid != 0)
                            {
                                var house = db.LiquidWasteDetails.Where(c => c.LWId == d.LWId & c.areaId == areaid).FirstOrDefault();
                                if (house != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        HouseId = house.ReferanceId,
                                        HouseAddress = (house.LWAddreLW == null ? "" : house.LWAddreLW.Replace("Unnamed Road, ", "")),
                                        HouseOwnerName = house.LWName,
                                        //OwnerMobileNo = house.houseOwnerMobile,
                                        WasteType = d.gcType.ToString(),
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var house = db.LiquidWasteDetails.Where(c => c.LWId == d.LWId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    HouseId = house.ReferanceId,
                                    HouseAddress = (house.LWAddreLW == null ? "" : house.LWAddreLW.Replace("Unnamed Road, ", "")),
                                    HouseOwnerName = house.LWName,
                                    //OwnerMobileNo = house.houseOwnerMobile,
                                    WasteType = d.gcType.ToString(),
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    ZoneList = ListZone(),

                                });
                            }


                        }
                        //if (d.dyId != null)
                        //{
                        //    if (areaid != 0)
                        //    {
                        //        var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId & c.areaId == areaid).FirstOrDefault();
                        //        if (dump != null)
                        //        {
                        //            userLocation.Add(new SBALUserLocationMapView()
                        //            {
                        //                userName = userName.userName,
                        //                datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy HH:mm"),
                        //                date = dat,
                        //                time = tim,
                        //                lat = d.Lat,
                        //                log = d.Long,
                        //                address = x.address,
                        //                vehcileNumber = att.vehicleNumber,
                        //                userMobile = userName.userMobileNumber,
                        //                type = Convert.ToInt32(x.type),
                        //                DyId = dump.ReferanceId,
                        //                DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                        //                DumpYardName = dump.dyName,
                        //                OwnerMobileNo = dump.dyNameMar,
                        //                WasteType = d.garbageType.ToString(),
                        //                gpBeforImage = d.gpBeforImage,
                        //                gpAfterImage = d.gpAfterImage,
                        //                DryWaste = d.totalDryWeight.ToString(),
                        //                WetWaste = d.totalWetWeight.ToString(),
                        //                TotWaste = d.totalGcWeight.ToString(),
                        //                ZoneList = ListZone(),

                        //            });
                        //        }

                        //    }
                        //    else
                        //    {
                        //        var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId).FirstOrDefault();
                        //        userLocation.Add(new SBALUserLocationMapView()
                        //        {
                        //            userName = userName.userName,
                        //            datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy HH:mm"),
                        //            date = dat,
                        //            time = tim,
                        //            lat = d.Lat,
                        //            log = d.Long,
                        //            address = x.address,
                        //            vehcileNumber = att.vehicleNumber,
                        //            userMobile = userName.userMobileNumber,
                        //            type = Convert.ToInt32(x.type),
                        //            DyId = dump.ReferanceId,
                        //            DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                        //            DumpYardName = dump.dyName,
                        //            OwnerMobileNo = dump.dyNameMar,
                        //            WasteType = d.garbageType.ToString(),
                        //            gpBeforImage = d.gpBeforImage,
                        //            gpAfterImage = d.gpAfterImage,
                        //            DryWaste = d.totalDryWeight.ToString(),
                        //            WetWaste = d.totalWetWeight.ToString(),
                        //            TotWaste = d.totalGcWeight.ToString(),
                        //            ZoneList = ListZone(),

                        //        });
                        //    }


                        //}







                    }
                    break;
                }

            }




            return userLocation;
        }

        public List<SBALUserLocationMapView> GetStreetAttenRoute(int daId, int areaid)
        {
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Daily_Attendance.Where(c => c.daID == daId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }
            var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == 1).OrderByDescending(a => a.datetime).ToList();

            foreach (var x in data)
            {
                if (x.type == 1)
                {

                    // string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                    //string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                    var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();
                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).FirstOrDefault();

                    //var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & c.houseId != null) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcDate).ToList();//.ToList();

                    var gcd = db.GarbageCollectionDetails.Where(c => (c.userId == x.userId & (c.SSId != null || c.dyId != null)) & EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(x.datetime)).OrderBy(c => c.gcId).ToList();//.ToList();


                    foreach (var d in gcd)
                    {
                        //DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                        string dat = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy");
                        string tim = Convert.ToDateTime(d.gcDate).ToString("hh:mm tt");
                        if (d.SSId != null)
                        {
                            if (areaid != 0)
                            {
                                var house = db.StreetSweepingDetails.Where(c => c.SSId == d.SSId & c.areaId == areaid).FirstOrDefault();
                                if (house != null)
                                {
                                    userLocation.Add(new SBALUserLocationMapView()
                                    {
                                        userName = userName.userName,
                                        datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                        date = dat,
                                        time = tim,
                                        lat = d.Lat,
                                        log = d.Long,
                                        address = x.address,
                                        vehcileNumber = att.vehicleNumber,
                                        userMobile = userName.userMobileNumber,
                                        type = Convert.ToInt32(x.type),
                                        HouseId = house.ReferanceId,
                                        HouseAddress = (house.SSAddress == null ? "" : house.SSAddress.Replace("Unnamed Road, ", "")),
                                        HouseOwnerName = house.SSName,
                                        //OwnerMobileNo = house.houseOwnerMobile,
                                        WasteType = d.gcType.ToString(),
                                        gpBeforImage = d.gpBeforImage,
                                        gpAfterImage = d.gpAfterImage,
                                        ZoneList = ListZone(),

                                    });
                                }

                            }
                            else
                            {
                                var house = db.StreetSweepingDetails.Where(c => c.SSId == d.SSId).FirstOrDefault();
                                userLocation.Add(new SBALUserLocationMapView()
                                {
                                    userName = userName.userName,
                                    datetime = Convert.ToDateTime(d.gcDate).ToString("HH:mm"),
                                    date = dat,
                                    time = tim,
                                    lat = d.Lat,
                                    log = d.Long,
                                    address = x.address,
                                    vehcileNumber = att.vehicleNumber,
                                    userMobile = userName.userMobileNumber,
                                    type = Convert.ToInt32(x.type),
                                    HouseId = house.ReferanceId,
                                    HouseAddress = (house.SSAddress == null ? "" : house.SSAddress.Replace("Unnamed Road, ", "")),
                                    HouseOwnerName = house.SSName,
                                    //OwnerMobileNo = house.houseOwnerMobile,
                                    WasteType = d.gcType.ToString(),
                                    gpBeforImage = d.gpBeforImage,
                                    gpAfterImage = d.gpAfterImage,
                                    ZoneList = ListZone(),

                                });
                            }


                        }
                        //if (d.dyId != null)
                        //{
                        //    if (areaid != 0)
                        //    {
                        //        var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId & c.areaId == areaid).FirstOrDefault();
                        //        if (dump != null)
                        //        {
                        //            userLocation.Add(new SBALUserLocationMapView()
                        //            {
                        //                userName = userName.userName,
                        //                datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy HH:mm"),
                        //                date = dat,
                        //                time = tim,
                        //                lat = d.Lat,
                        //                log = d.Long,
                        //                address = x.address,
                        //                vehcileNumber = att.vehicleNumber,
                        //                userMobile = userName.userMobileNumber,
                        //                type = Convert.ToInt32(x.type),
                        //                DyId = dump.ReferanceId,
                        //                DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                        //                DumpYardName = dump.dyName,
                        //                OwnerMobileNo = dump.dyNameMar,
                        //                WasteType = d.garbageType.ToString(),
                        //                gpBeforImage = d.gpBeforImage,
                        //                gpAfterImage = d.gpAfterImage,
                        //                DryWaste = d.totalDryWeight.ToString(),
                        //                WetWaste = d.totalWetWeight.ToString(),
                        //                TotWaste = d.totalGcWeight.ToString(),
                        //                ZoneList = ListZone(),

                        //            });
                        //        }

                        //    }
                        //    else
                        //    {
                        //        var dump = db.DumpYardDetails.Where(c => c.dyId == d.dyId).FirstOrDefault();
                        //        userLocation.Add(new SBALUserLocationMapView()
                        //        {
                        //            userName = userName.userName,
                        //            datetime = Convert.ToDateTime(d.gcDate).ToString("dd/MM/yyyy HH:mm"),
                        //            date = dat,
                        //            time = tim,
                        //            lat = d.Lat,
                        //            log = d.Long,
                        //            address = x.address,
                        //            vehcileNumber = att.vehicleNumber,
                        //            userMobile = userName.userMobileNumber,
                        //            type = Convert.ToInt32(x.type),
                        //            DyId = dump.ReferanceId,
                        //            DumpAddress = (dump.dyAddress == null ? "" : dump.dyAddress.Replace("Unnamed Road, ", "")),
                        //            DumpYardName = dump.dyName,
                        //            OwnerMobileNo = dump.dyNameMar,
                        //            WasteType = d.garbageType.ToString(),
                        //            gpBeforImage = d.gpBeforImage,
                        //            gpAfterImage = d.gpAfterImage,
                        //            DryWaste = d.totalDryWeight.ToString(),
                        //            WetWaste = d.totalWetWeight.ToString(),
                        //            TotWaste = d.totalGcWeight.ToString(),
                        //            ZoneList = ListZone(),

                        //        });
                        //    }

                        //}

                    }
                    break;
                }

            }




            return userLocation;
        }

        // Added By Saurabh (06 June 2019)

        public List<SBALHouseLocationMapView> GetAllHouseLocation(string date, int userid, int areaid, int wardNo, string SearchString, int? GarbageType, int FilterType, string Emptype, string ctype, int SegType)
        {

            List<SBALHouseLocationMapView> houseLocation = new List<SBALHouseLocationMapView>();
            var zoneId = 0;
            DateTime dt1 = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
            if (Emptype == null)
            {
                var data = db.SP_HouseOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, GarbageType, FilterType, SegType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALHouseLocationMapView()
                    {
                        ssid = Convert.ToInt32(x.SSId),
                        lwid = Convert.ToInt32(x.LWId),
                        houseId = Convert.ToInt32(x.houseId),
                        ReferanceId = x.ReferanceId,
                        houseOwnerName = (x.houseOwner == null ? "" : x.houseOwner.ToUpper()),
                        houseOwnerMobile = (x.houseOwnerMobile == null ? "" : x.houseOwnerMobile),
                        houseAddress = checkNull(x.houseAddress).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        houseLat = x.houseLat,
                        houseLong = x.houseLong,
                        address = x.houseAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        garbageType = x.garbageType,
                        Ctype = x.CType,
                        wet = x.Wet,
                        dry = x.Dry,
                        domestic = x.Domestic,
                        sanitary = x.Sanitary

                    });
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.HouseMasters.ToList();
                    var model = houseLocation.Where(c => c.houseOwnerName.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.houseOwnerName.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    //var model = houseLocation.Where(c => ((string.IsNullOrEmpty(c.ReferanceId) ? " " : c.houseOwnerName) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseOwnerName) ? " " : c.houseOwnerName) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseOwnerMobile) ? " " : c.houseOwnerMobile) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseAddress) ? " " : c.houseAddress)).ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();


                    //var model = data.Where(c => ((string.IsNullOrEmpty(c.WardNo) ? " " : c.WardNo) + " " +
                    //                        (string.IsNullOrEmpty(c.zone) ? " " : c.zone) + " " +
                    //                        (string.IsNullOrEmpty(c.Area) ? " " : c.Area) + " " +
                    //                        (string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " +
                    //                        (string.IsNullOrEmpty(c.houseNo) ? " " : c.houseNo) + " " +
                    //                        (string.IsNullOrEmpty(c.Mobile) ? " " : c.Mobile) + " " +
                    //                        (string.IsNullOrEmpty(c.Address) ? " " : c.Address) + " " +
                    //                        (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId) + " " +
                    //                        (string.IsNullOrEmpty(c.QRCode) ? " " : c.QRCode)).ToUpper().Contains(SearchString.ToUpper())).ToList();

                }

                if (ctype == "0")
                {
                    houseLocation = houseLocation.ToList();
                }
                else if (ctype == "NULL")
                {
                    houseLocation = houseLocation.Where(c => c.Ctype == null).ToList();
                }
                else if (ctype != "0" || ctype != "NULL")
                {

                    houseLocation = houseLocation.Where(c => c.Ctype == ctype).ToList();
                }

            }
            else if (Emptype == "L")
            {
                var data = db.SP_LiquidWasteOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, GarbageType, FilterType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALHouseLocationMapView()
                    {
                        houseId = Convert.ToInt32(x.LWId),
                        ReferanceId = x.ReferanceId,
                        houseOwnerName = (x.LWName == null ? "" : x.LWName),
                        //houseOwnerMobile = (x.houseOwnerMobile == null ? "" : x.houseOwnerMobile),
                        houseAddress = checkNull(x.LWAddreLW).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        houseLat = x.LWLat,
                        houseLong = x.LWLong,
                        // address = x.houseAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        garbageType = x.gcType,

                    });
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.HouseMasters.ToList();
                    var model = houseLocation.Where(c => c.houseOwnerName.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.houseOwnerName.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();

                }
            }

            else if (Emptype == "S")
            {
                var data = db.SP_StreetSweepingOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, GarbageType, FilterType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALHouseLocationMapView()
                    {
                        houseId = Convert.ToInt32(x.SSId),
                        ReferanceId = x.ReferanceId,
                        houseOwnerName = (x.SSName == null ? "" : x.SSName),
                        //houseOwnerMobile = (x.houseOwnerMobile == null ? "" : x.houseOwnerMobile),
                        houseAddress = checkNull(x.SSAddress).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        houseLat = x.SSLat,
                        houseLong = x.SSLong,
                        // address = x.houseAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        garbageType = x.gcType,
                        BeatId = db.StreetSweepingBeats.Where(s => s.ReferanceId1 == x.ReferanceId || s.ReferanceId2 == x.ReferanceId || s.ReferanceId3 == x.ReferanceId || s.ReferanceId4 == x.ReferanceId || s.ReferanceId5 == x.ReferanceId).Select(a => a.BeatId).FirstOrDefault()

                    });
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.HouseMasters.ToList();
                    var model = houseLocation.Where(c => c.houseOwnerName.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.houseOwnerName.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();

                }
            }
            return houseLocation;

        }


        public List<SBALCommercialLocationMapView> GetAllCommercialLocation(string date, int userid, int areaid, int wardNo, string SearchString, int? GarbageType, int FilterType, string Emptype, string ctype, int SegType)
        {

            List<SBALCommercialLocationMapView> houseLocation = new List<SBALCommercialLocationMapView>();
            var zoneId = 0;
            DateTime dt1 = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
            if (Emptype == null)
            {
                var data = db.SP_CommercialOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, GarbageType, FilterType, SegType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALCommercialLocationMapView()
                    {
                        ssid = Convert.ToInt32(x.SSId),
                        lwid = Convert.ToInt32(x.LWId),
                        commercialId = Convert.ToInt32(x.commercialId),
                        ReferanceId = x.ReferanceId,
                        commercialOwnerName = (x.commercialOwner == null ? "" : x.commercialOwner.ToUpper()),
                        commercialOwnerMobile = (x.commercialOwnerMobile == null ? "" : x.commercialOwnerMobile),
                        commercialAddress = checkNull(x.commercialAddress).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        commercialLat = x.commercialLat,
                        commercialLong = x.commercialLong,
                        address = x.commercialAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        garbageType = x.garbageType,
                        Ctype = x.CType,
                        wet = x.Wet,
                        dry = x.Dry

                    });
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.HouseMasters.ToList();
                    var model = houseLocation.Where(c => c.commercialOwnerName.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.commercialOwnerName.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    //var model = houseLocation.Where(c => ((string.IsNullOrEmpty(c.ReferanceId) ? " " : c.houseOwnerName) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseOwnerName) ? " " : c.houseOwnerName) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseOwnerMobile) ? " " : c.houseOwnerMobile) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseAddress) ? " " : c.houseAddress)).ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();


                    //var model = data.Where(c => ((string.IsNullOrEmpty(c.WardNo) ? " " : c.WardNo) + " " +
                    //                        (string.IsNullOrEmpty(c.zone) ? " " : c.zone) + " " +
                    //                        (string.IsNullOrEmpty(c.Area) ? " " : c.Area) + " " +
                    //                        (string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " +
                    //                        (string.IsNullOrEmpty(c.houseNo) ? " " : c.houseNo) + " " +
                    //                        (string.IsNullOrEmpty(c.Mobile) ? " " : c.Mobile) + " " +
                    //                        (string.IsNullOrEmpty(c.Address) ? " " : c.Address) + " " +
                    //                        (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId) + " " +
                    //                        (string.IsNullOrEmpty(c.QRCode) ? " " : c.QRCode)).ToUpper().Contains(SearchString.ToUpper())).ToList();

                }

                if (ctype == "0")
                {
                    houseLocation = houseLocation.ToList();
                }

                else
                {
                    houseLocation = houseLocation.ToList();
                }
            }
            else if (Emptype == "L")
            {
                var data = db.SP_LiquidWasteOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, GarbageType, FilterType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALCommercialLocationMapView()
                    {
                        commercialId = Convert.ToInt32(x.LWId),
                        ReferanceId = x.ReferanceId,
                        commercialOwnerName = (x.LWName == null ? "" : x.LWName),
                        //commercialOwnerMobile = (x.commercialOwnerMobile == null ? "" : x.commercialOwnerMobile),
                        commercialAddress = checkNull(x.LWAddreLW).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        commercialLat = x.LWLat,
                        commercialLong = x.LWLong,
                        // address = x.commercialAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        garbageType = x.gcType,

                    });
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.HouseMasters.ToList();
                    var model = houseLocation.Where(c => c.commercialOwnerName.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.commercialOwnerName.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();

                }
            }

            else if (Emptype == "S")
            {
                var data = db.SP_StreetSweepingOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, GarbageType, FilterType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALCommercialLocationMapView()
                    {
                        commercialId = Convert.ToInt32(x.SSId),
                        ReferanceId = x.ReferanceId,
                        commercialOwnerName = (x.SSName == null ? "" : x.SSName),
                        //commercialOwnerMobile = (x.commercialOwnerMobile == null ? "" : x.commercialOwnerMobile),
                        commercialAddress = checkNull(x.SSAddress).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        commercialLat = x.SSLat,
                        commercialLong = x.SSLong,
                        // address = x.commercialAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        garbageType = x.gcType,

                    });
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.commercialMasters.ToList();
                    var model = houseLocation.Where(c => c.commercialOwnerName.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.commercialOwnerName.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();

                }
            }
            return houseLocation;

        }

        public List<SBALCTPTLocationMapView> GetAllCTPTLocation(string date, int userid, int areaid, int wardNo, string SearchString, int FilterType, string Emptype)
        {

            List<SBALCTPTLocationMapView> houseLocation = new List<SBALCTPTLocationMapView>();
            var zoneId = 0;
            DateTime dt1 = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
            if (Emptype == null)
            {
                var data = db.SP_CTPTOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, FilterType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALCTPTLocationMapView()
                    {

                        CTPTId = Convert.ToInt32(x.Id),
                        CTPTName = x.Name,
                        ReferanceId = x.ReferanceId,
                        CTPTAddress = checkNull(x.Address).Replace("Unnamed Road, ", ""),
                        gcDate = dt.ToString("dd-MM-yyyy"),
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                         //string gcTime = x.gcDate.ToString(),
                                                         //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                         //myDateTime.ToString("HH:mm:ss")
                        ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                        //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                        CTPTLat = x.Lat,
                        CTPTLong = x.Long,
                        TOT = x.tot,
                        // address = x.commercialAddress,
                        //vehcileNumber = x.v,
                        //userMobile = x.mobile,
                        //garbageType = x.garbageType
                    });
                }


                if (!string.IsNullOrEmpty(SearchString))
                {
                    // var abc = db.HouseMasters.ToList();
                    var model = houseLocation.Where(c => c.ReferanceId.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.ReferanceId.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    //var model = houseLocation.Where(c => ((string.IsNullOrEmpty(c.ReferanceId) ? " " : c.houseOwnerName) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseOwnerName) ? " " : c.houseOwnerName) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseOwnerMobile) ? " " : c.houseOwnerMobile) + " " +
                    //                                     (string.IsNullOrEmpty(c.houseAddress) ? " " : c.houseAddress)).ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();


                    //var model = data.Where(c => ((string.IsNullOrEmpty(c.WardNo) ? " " : c.WardNo) + " " +
                    //                        (string.IsNullOrEmpty(c.zone) ? " " : c.zone) + " " +
                    //                        (string.IsNullOrEmpty(c.Area) ? " " : c.Area) + " " +
                    //                        (string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " +
                    //                        (string.IsNullOrEmpty(c.houseNo) ? " " : c.houseNo) + " " +
                    //                        (string.IsNullOrEmpty(c.Mobile) ? " " : c.Mobile) + " " +
                    //                        (string.IsNullOrEmpty(c.Address) ? " " : c.Address) + " " +
                    //                        (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId) + " " +
                    //                        (string.IsNullOrEmpty(c.QRCode) ? " " : c.QRCode)).ToUpper().Contains(SearchString.ToUpper())).ToList();

                }
                houseLocation = houseLocation.ToList();

            }

            return houseLocation;

        }


        public List<SBALSWMLocationMapView> GetAllSWMLocation(string date, int userid, int areaid, int wardNo, string SearchString, int FilterType, string Emptype)
        {

            List<SBALSWMLocationMapView> houseLocation = new List<SBALSWMLocationMapView>();
            var zoneId = 0;
            DateTime dt1 = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
            if (Emptype == null)
            {
                var data = db.SP_SWMOnMapDetails(Convert.ToDateTime(dt1), userid == -1 ? 0 : userid, zoneId, areaid, wardNo, FilterType).ToList();
                foreach (var x in data)
                {

                    DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                    //string gcTime = x.gcDate.ToString();
                    houseLocation.Add(new SBALSWMLocationMapView()
                    {

                        SWMId = Convert.ToInt32(x.swmId),
                        ReferanceId = x.ReferanceId,
                        swmname = x.swmname,
                        swmOwnerMobile = x.swmOwnerMobile,
                        SWMAddress = checkNull(x.swmAddress).Replace("Unnamed Road, ", ""),
                        //    gcDate = dt.ToString("dd-MM-yyyy"),
                        gcDate = x.gcDate,
                        gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock                                               
                        SWMLat = x.swmLat,
                        SWMLong = x.swmLong,
                        SWMType = x.swmType,

                    });
                }


                if (!string.IsNullOrEmpty(SearchString))
                {

                    var model = houseLocation.Where(c => c.ReferanceId.Contains(SearchString) || c.ReferanceId.Contains(SearchString)
                                                         || c.ReferanceId.ToLower().Contains(SearchString) || c.ReferanceId.ToLower().Contains(SearchString)).ToList();

                    houseLocation = model.ToList();



                }
                houseLocation = houseLocation.ToList();

            }

            return houseLocation;

        }

        // Added By Saurabh (02 July 2019)
        public DashBoardVM GetHouseOnMapDetails()

        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    //var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
                    //List<ComplaintVM> obj = new List<ComplaintVM>();
                    //if (AppID == 1)
                    //{
                    //    string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=1");
                    //    obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    //}


                    var data = db.SP_HouseScanify_Count().First();

                    //var date = DateTime.Today;
                    //var houseCount = db.SP_TotalHouseCollection_Count(date).FirstOrDefault();
                    if (data != null)
                    {

                        model.TotalHouseCount = data.TotalHouseCount;
                        model.HouseCollection = data.TotalHouseLatLongCount;
                        model.TotalScanHouseCount = data.TotalScanHouseCount;
                        model.MixedCount = data.MixedCount;
                        model.BifurgatedCount = data.BifurgatedCount;
                        model.NotCollected = data.NotCollected;
                        model.NotSpecified = data.NotSpecified;
                        model.LiquidCollection = data.TotalLiquidLatLongCount;
                        model.StreetCollection = data.TotalStreetLatLongCount;

                        model.ResidentialCollection = data.TotalRWLatLongCount;
                        model.ResidentialBuildingCollection = data.TotalRBWLatLongCount;
                        model.ResidentialSlumCollection = data.TotalRSWLatLongCount;
                        model.CommercialCollection = data.TotalCWLatLongCount;

                        model.ConstructionDemolitionCount = data.ConstructionDemolition;
                        model.HorticultureCount = data.Horticulture;
                        model.WetWasteCount = data.WetWaste;
                        model.DryWasteCount = data.DryWaste;
                        model.DemosticHazardousCount = data.DemosticHazardous;
                        model.SanitaryCount = data.Sanitory;
                        model.DumpYardCount = data.TotalDumpScanCount;

                        return model;
                    }

                    // String.Format("{0:0.00}", 123.4567); 

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public DashBoardVM GetCommercialOnMapDetails()

        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    //var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
                    //List<ComplaintVM> obj = new List<ComplaintVM>();
                    //if (AppID == 1)
                    //{
                    //    string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=1");
                    //    obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    //}


                    var data = db.SP_CommercialScanify_Count().First();

                    //var date = DateTime.Today;
                    //var houseCount = db.SP_TotalHouseCollection_Count(date).FirstOrDefault();
                    if (data != null)
                    {

                        model.TotalcommercialCount = data.TotalcommercialCount;
                        model.commercialCollection = data.TotalcommercialLatLongCount;
                        model.TotalScancommercialCount = data.TotalScancommercialCount;
                        model.MixedCount = data.MixedCount;
                        model.BifurgatedCount = data.BifurgatedCount;
                        model.NotCollected = data.NotCollected;
                        model.NotSpecified = data.NotSpecified;
                        model.LiquidCollection = data.TotalLiquidLatLongCount;
                        model.StreetCollection = data.TotalStreetLatLongCount;

                        model.ResidentialCollection = data.TotalRWLatLongCount;
                        model.ResidentialBuildingCollection = data.TotalRBWLatLongCount;
                        model.ResidentialSlumCollection = data.TotalRSWLatLongCount;
                        model.CommercialCollection = data.TotalCWLatLongCount;

                        model.ConstructionDemolitionCount = data.ConstructionDemolition;
                        model.HorticultureCount = data.Horticulture;
                        model.WetWasteCount = data.WetWaste;
                        model.DryWasteCount = data.DryWaste;
                        model.DemosticHazardousCount = data.DemosticHazardous;
                        model.SanitaryCount = data.Sanitory;

                        return model;
                    }

                    // String.Format("{0:0.00}", 123.4567); 

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }


        public DashBoardVM GetCTPTOnMapDetails()

        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    //var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
                    //List<ComplaintVM> obj = new List<ComplaintVM>();
                    //if (AppID == 1)
                    //{
                    //    string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=1");
                    //    obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    //}


                    var data = db.SP_CTPTScanify_Count().First();

                    //var date = DateTime.Today;
                    //var houseCount = db.SP_TotalHouseCollection_Count(date).FirstOrDefault();
                    if (data != null)
                    {

                        model.TotalCTPTCount = data.TotalCTPTCount;
                        model.TotalCTPTScanCount = data.TotalCTPTScanCount;
                        model.TodayCTPTScanCount = data.TodayScanCTPTCount;
                        model.TotalPTCount = data.TotalPTCount;
                        model.TotalCTCount = data.TotalCTCount;
                        model.TotalUCount = data.TotalUCount;

                        return model;
                    }

                    // String.Format("{0:0.00}", 123.4567); 

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public DashBoardVM GetSWMOnMapDetails()

        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();


                    var data = db.SP_SWMScanify_Count().First();


                    if (data != null)
                    {

                        model.TotalSWMCount = data.TotalSWMCount;
                        model.TotalSWMScanCount = data.TotalSWMScanCount;
                        model.TodayScanSWMCount = data.TodayScanSWMCount;
                        return model;
                    }

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public DashBoardVM GetLiquidWasteDetails()
        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    var data = db.SP_LWaste_Count().First();
                    if (data != null)
                    {
                        model.LiquidWasteCollection = data.TotalLiquidLatLongCount;
                        model.LiquidWasteScanedHouse = data.TotalScanHouseCount;
                        //model.TotalScanHouseCount = data.TotalScanHouseCount;
                        //model.MixedCount = data.MixedCount;
                        //model.BifurgatedCount = data.BifurgatedCount;
                        //model.NotCollected = data.NotCollected;
                        //model.NotSpecified = data.NotSpecified;
                        return model;
                    }
                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }


        public DashBoardVM GetStreetSweepingDetails()
        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    var data = db.SP_SSweeping_Count().First();
                    if (data != null)
                    {
                        model.StreetWasteCollection = data.TotalStreetLatLongCount;
                        model.StreetWasteScanedHouse = data.TotalScanHouseCount;
                        //model.TotalScanHouseCount = data.TotalScanHouseCount;
                        //model.MixedCount = data.MixedCount;
                        //model.BifurgatedCount = data.BifurgatedCount;
                        //model.NotCollected = data.NotCollected;
                        //model.NotSpecified = data.NotSpecified;
                        return model;
                    }
                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }
        #endregion

        #region Garbage Point Details
        public GarbagePointDetailsVM GetGarbagePointDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.PointQRCode + "/";
                GarbagePointDetailsVM point = new GarbagePointDetailsVM();

                var Details = db.GarbagePointDetails.Where(x => x.gpId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    point = FillGarbagePointDetailsViewModel(Details);
                    if (point.qrCode != null && point.qrCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + point.qrCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                point.qrCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                point.qrCode = ThumbnaiUrlCMS + point.qrCode.Trim();
                            }
                        }
                        catch (Exception e) { point.qrCode = "/Images/default_not_upload.png"; }

                        //  point.qrCode = ThumbnaiUrlCMS + point.qrCode.Trim();
                    }
                    else
                    {
                        point.qrCode = "/Images/default_not_upload.png";
                    }

                    // house.WardList = ListWardNo();
                    point.AreaList = LoadListArea(Convert.ToInt32(point.WardNo)); //ListArea();
                    point.ZoneList = ListZone();
                    point.WardList = LoadListWardNo(Convert.ToInt32(point.ZoneId)); //ListWardNo();

                    return point;
                }
                else
                {
                    var id = db.GarbagePointDetails.OrderByDescending(x => x.gpId).Select(x => x.gpId).FirstOrDefault();
                    int number = 1000;
                    string refer = "GPSBA" + (number + id + 1);
                    point.ReferanceId = refer;
                    point.qrCode = "/Images/QRcode.png";
                    point.WardList = ListWardNo();
                    point.AreaList = ListArea();
                    point.ZoneList = ListZone();
                    return point;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public GarbagePointDetailsVM SaveGarbagePointDetails(GarbagePointDetailsVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.gpId > 0)
                    {
                        var model = db.GarbagePointDetails.Where(x => x.gpId == data.gpId).FirstOrDefault();
                        if (model != null)
                        {
                            model.zoneId = data.ZoneId;
                            model.wardId = data.WardNo;
                            model.areaId = data.areaId;
                            model.gpAddress = data.gpAddress;
                            model.gpLat = data.gpLat;
                            model.gpLong = data.gpLong;
                            model.gpName = data.gpName;
                            model.gpNameMar = data.gpNameMar;
                            model.qrCode = data.qrCode;
                            model.ReferanceId = data.ReferanceId;
                            model.modified = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillGarbagePointDetailsDataModel(data);
                        db.GarbagePointDetails.Add(type);
                        db.SaveChanges();
                    }
                }
                var gpid = db.GarbagePointDetails.OrderByDescending(x => x.gpId).Select(x => x.gpId).FirstOrDefault();
                GarbagePointDetailsVM vv = GetGarbagePointDetails(gpid);
                return vv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeletGarbagePointDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.GarbagePointDetails.Where(x => x.gpId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.GarbagePointDetails.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region DataModel
        private TeritoryMaster FillAreaDataModel(AreaVM data)
        {
            TeritoryMaster model = new TeritoryMaster();
            model.Id = data.Id;
            model.Area = data.Name;
            model.AreaMar = data.NameMar;
            model.wardId = data.wardId;

            return model;
        }

        private TeritoryMaster LiquidFillAreaDataModel(AreaVM data)
        {
            TeritoryMaster model = new TeritoryMaster();
            model.Id = data.LWId;
            model.Area = data.LWName;
            model.AreaMar = data.LWNameMar;
            model.wardId = data.LWwardId;

            return model;
        }

        private TeritoryMaster StreetFillAreaDataModel(AreaVM data)
        {
            TeritoryMaster model = new TeritoryMaster();
            model.Id = data.SSId;
            model.Area = data.SSName;
            model.AreaMar = data.SSNameMar;
            model.wardId = data.SSwardId;

            return model;
        }
        private VehicleType FillVehicleDataModel(VehicleTypeVM data)
        {
            VehicleType model = new VehicleType();
            model.vtId = data.Id;
            model.description = data.description;
            model.descriptionMar = data.descriptionMar;
            model.isActive = data.isActive;
            return model;
        }

        private VehicleRegistration FillVehicleRegDataModel(VehicleRegVM data)
        {
            VehicleRegistration model = new VehicleRegistration();
            model.vehicleId = data.vehicleId;
            model.vehicleType = data.vehicleType;
            model.vehicleNo = data.vehicleNumber;
            model.areaId = data.AreaId;
            model.isActive = data.isActive;
            return model;
        }

        private WardNumber FillWardDataModel(WardNumberVM data)
        {
            WardNumber model = new WardNumber();
            model.Id = data.Id;
            model.WardNo = data.WardNo;
            model.zoneId = data.zoneId;
            return model;
        }


        private WardNumber LiquidFillWardDataModel(WardNumberVM data)
        {
            WardNumber model = new WardNumber();
            model.Id = data.LWId;
            model.WardNo = data.LWWardNo;
            model.zoneId = data.LWzoneId;
            return model;
        }

        private WardNumber StreetFillWardDataModel(WardNumberVM data)
        {
            WardNumber model = new WardNumber();
            model.Id = data.SSId;
            model.WardNo = data.SSWardNo;
            model.zoneId = data.SSzoneId;
            return model;
        }

        private HouseMaster FillHouseDetailsDataModel(HouseDetailsVM data)
        {
            HouseMaster model = new HouseMaster();
            model.houseId = data.houseId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.houseOwner = data.houseOwner;
            model.houseOwnerMar = data.houseOwnerMar;
            model.houseAddress = data.houseAddress;
            model.houseOwnerMobile = data.houseMobile;
            model.houseNumber = data.houseNumber;
            model.houseQRCode = data.houseQRCode;
            model.houseLat = data.houseLat;
            model.houseLong = data.houseLong;
            model.ZoneId = data.ZoneId;
            model.ReferanceId = data.ReferanceId;
            model.modified = DateTime.Now;
            model.CType = (data.houseCategory == "RW") ? null : data.houseCategory;
            //if (data.WasteType == "DW")
            //{
            //    model.WasteType = data.WasteType;
            //}
            //if (data.WasteType == "WW")
            //{
            //    model.WasteType = data.WasteType;
            //}
            // model.userId = data.userId;
            return model;
        }

        private SWMMaster FillSWMDetailsDataModel(SWMDetailsVM data)
        {
            SWMMaster model = new SWMMaster();
            model.swmId = data.swmId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.swmName = data.swmName;
            model.swmManager = data.swmManager;
            model.swmOwnerMar = data.swmOwnerMar;
            model.swmAddress = data.swmAddress;
            model.swmOwnerMobile = data.swmMobile;
            model.swmNumber = data.swmNumber;
            model.swmQRCode = data.swmQRCode;
            model.swmLat = data.swmLat;
            model.swmLong = data.swmLong;
            model.ZoneId = data.ZoneId;
            model.ReferanceId = data.ReferanceId;
            model.modified = DateTime.Now;
            model.swmType = (data.swmType == "RW") ? null : data.swmType;
            model.swmSubType = data.swmSubType;

            return model;
        }

        private CommercialMaster FillCommercialDetailsDataModel(CommercialDetailsVM data)
        {
            CommercialMaster model = new CommercialMaster();
            model.commercialId = data.houseId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.commercialOwner = data.houseOwner;
            model.commercialOwnerMar = data.houseOwnerMar;
            model.commercialAddress = data.houseAddress;
            model.commercialOwnerMobile = data.houseMobile;
            model.commercialNumber = data.houseNumber;
            model.commercialQRCode = data.houseQRCode;
            model.commercialLat = data.houseLat;
            model.commercialLong = data.houseLong;
            model.ZoneId = data.ZoneId;
            model.ReferanceId = data.ReferanceId;
            model.modified = DateTime.Now;
            model.CType = (data.houseCategory == "RW") ? null : data.houseCategory;
            //if (data.WasteType == "DW")
            //{
            //    model.WasteType = data.WasteType;
            //}
            //if (data.WasteType == "WW")
            //{
            //    model.WasteType = data.WasteType;
            //}
            // model.userId = data.userId;
            return model;
        }
        private GarbagePointDetail FillGarbagePointDetailsDataModel(GarbagePointDetailsVM data)
        {
            GarbagePointDetail model = new GarbagePointDetail();
            model.zoneId = data.ZoneId;
            model.wardId = data.WardNo;
            model.areaId = data.areaId;
            model.gpId = data.gpId;
            model.gpAddress = data.gpAddress;
            model.gpLat = data.gpLat;
            model.gpLong = data.gpLong;
            model.gpName = data.gpName;
            model.gpNameMar = data.gpNameMar;
            model.qrCode = data.qrCode;
            model.ReferanceId = data.ReferanceId;
            model.modified = DateTime.Now;
            return model;
        }
        private UserMaster FillEmployeeDataModel(EmployeeDetailsVM data, string Emptype)
        {
            UserMaster model = new UserMaster();
            model.userId = data.userId;
            model.userAddress = data.userAddress;
            model.userLoginId = data.userLoginId;
            model.userMobileNumber = data.userMobileNumber;
            model.userName = data.userName;
            model.userNameMar = data.userNameMar;
            model.userPassword = data.userPassword;
            model.userProfileImage = data.userProfileImage;
            model.userEmployeeNo = data.userEmployeeNo;
            model.bloodGroup = data.bloodGroup;
            model.isActive = data.isActive;
            model.gcTarget = data.gcTarget;
            model.ComgcTarget = data.ComgcTarget;
            model.EmployeeType = Emptype;
            model.userDesignation = data.userDesignation;
            return model;
        }

        // Added By Saurabh

        private DumpYardDetail FillDumpYardDetailsDataModel(DumpYardDetailsVM data, string Emptype)
        {
            DumpYardDetail model = new DumpYardDetail();
            model.areaId = data.areaId;
            model.wardId = data.WardNo;
            model.zoneId = data.ZoneId;
            model.dyId = data.dyId;
            model.dyAddress = data.dyAddress;
            model.dyLat = data.dyLat;
            model.dyLong = data.dyLong;
            model.dyName = data.dyName;
            model.dyNameMar = data.dyNameMar;
            model.dyQRCode = data.dyQRCode;
            model.ReferanceId = data.ReferanceId;
            model.lastModifiedDate = DateTime.Now;
            model.EmployeeType = Emptype;
            return model;
        }

        private StreetSweepingDetail FillStreetSweepDetailsDataModel(StreetSweepVM data)
        {
            StreetSweepingDetail model = new StreetSweepingDetail();
            model.areaId = data.areaId;
            model.wardId = data.WardNo;
            model.zoneId = data.ZoneId;
            model.SSId = data.SSId;
            model.SSAddress = data.SSAddress;
            model.SSLat = data.SSLat;
            model.SSLong = data.SSLong;
            model.SSName = data.SSName;
            model.SSNameMar = data.SSNameMar;
            model.SSQRCode = data.SSQRCode;
            model.ReferanceId = data.ReferanceId;
            model.lastModifiedDate = DateTime.Now;
            return model;
        }

        private StreetSweepingBeat FillStreetBeatDetailsDataModel(StreetSweepVM data)
        {
            StreetSweepingBeat model = new StreetSweepingBeat();
            model.CreateDate = DateTime.Now;
            model.ReferanceId1 = data.SSBeatone;
            model.ReferanceId2 = data.SSBeattwo;
            model.ReferanceId3 = data.SSBeatthree;
            model.ReferanceId4 = data.SSBeatfour;
            model.ReferanceId5 = data.SSBeatfive;

            return model;
        }
        private LiquidWasteDetail FillLiquidWasteDetailsDataModel(LiquidWasteVM data)
        {
            LiquidWasteDetail model = new LiquidWasteDetail();
            model.areaId = data.areaId;
            model.wardId = data.WardNo;
            model.zoneId = data.ZoneId;
            model.LWId = data.LWId;
            model.LWAddreLW = data.LWAddress;
            model.LWLat = data.LWLat;
            model.LWLong = data.LWLong;
            model.LWName = data.LWName;
            model.LWNameMar = data.LWNameMar;
            model.LWQRCode = data.LWQRCode;
            model.ReferanceId = data.ReferanceId;
            model.lastModifiedDate = DateTime.Now;
            return model;
        }

        // Added By saurabh (04 June 2019)
        private QrEmployeeMaster FillHSEmployeeDataModel(HouseScanifyEmployeeDetailsVM data)
        {
            QrEmployeeMaster model = new QrEmployeeMaster();
            model.qrEmpId = data.qrEmpId;
            model.qrEmpAddress = data.qrEmpAddress;
            model.qrEmpLoginId = data.qrEmpLoginId;
            model.qrEmpPassword = data.qrEmpPassword;
            model.qrEmpMobileNumber = data.qrEmpMobileNumber;
            model.qrEmpName = data.qrEmpName;
            model.qrEmpNameMar = data.qrEmpNameMar;
            model.bloodGroup = data.bloodGroup;
            model.isActive = data.isActive;
            model.typeId = 1;
            model.type = "Employee";
            return model;
        }

        private SauchalayAddress FillSauchalayDetailsDataModel(SauchalayDetailsVM data)
        {
            SauchalayAddress model = new SauchalayAddress();
            model.Id = data.Id;
            model.SauchalayID = data.SauchalayID;
            model.ImageUrl = data.Image;
            model.QrImageUrl = data.QrImage;
            model.Name = data.Name;
            model.Address = data.Address;
            model.Lat = data.Lat;
            model.Long = data.Long;
            model.Mobile = data.Mobile;
            model.CreatedDate = DateTime.Now;
            model.Tot = data.Tot;
            model.Tns = data.Tns;
            model.SauchalayQRCode = data.SauchalayQRCode;
            model.ReferanceId = data.ReferanceId;
            model.lastModifiedDateEntry = DateTime.Now;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.WardNo = data.WardNo;
            model.TOEMC = data.TOEMC;
            model.TOC = data.TOC;
            //model.userId = data.userId;
            return model;
        }
        #endregion

        #region List

        //public List<SelectListItem> ListLanguage()
        //{
        //    var lstLanguage = new List<SelectListItem>();
        //    SelectListItem itemAdd = new SelectListItem() { Text = "Select Language", Value = "-1" };

        //    try
        //    {
        //        lstLanguage = db.LanguageInfoes.AsNoTracking()
        //            .Select(x => new SelectListItem
        //            {
        //                Text = x.languageType,
        //                Value = x.id.ToString()
        //            }).OrderBy(t => t.Text).ToList();

        //        lstLanguage.Insert(0, itemAdd);
        //    }
        //    catch (Exception ex) { throw ex; }

        //    return lstLanguage;
        //}
        public List<SelectListItem> ListArea()
        {
            var Area = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "Select Area", Value = "0" };

            try
            {
                Area = db.TeritoryMasters.ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.Area,
                        Value = x.Id.ToString()
                    }).OrderBy(t => t.Text).ToList();

                Area.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return Area;
        }
        public List<SelectListItem> ListVehicle()
        {
            var Vehicle = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Vehicle--", Value = "0" };

            try
            {
                Vehicle = db.VehicleTypes.Where(v => v.isActive == true).ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.description,
                        Value = x.vtId.ToString()
                    }).OrderBy(t => t.Text).ToList();

                Vehicle.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return Vehicle;
        }


        public List<SelectListItem> ListBeat()
        {
            var Vehicle = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Beat--", Value = "0" };

            try
            {
                Vehicle = db.SP_StreetSweepList().ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ReferanceId,
                        Value = x.ReferanceId.ToString()
                    }).OrderBy(t => t.Text).ToList();

                Vehicle.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return Vehicle;
        }

        public List<SelectListItem> ListWardNo()
        {
            var WardNo = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "Select Ward / Prabhag", Value = "0" };

            try
            {

                WardNo = db.WardNumbers.ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.WardNo + " (" + db.ZoneMasters.Where(c => c.zoneId == x.zoneId).FirstOrDefault().name + ")",
                        Value = x.Id.ToString()
                    }).OrderBy(t => t.Text).ToList();

                WardNo.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return WardNo;
        }
        public List<SelectListItem> ListZone()
        {
            var Zone = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Zone--", Value = "0" };

            try
            {
                Zone = db.ZoneMasters.ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.name,
                        Value = x.zoneId.ToString()
                    }).OrderBy(t => t.Text).ToList();

                Zone.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return Zone;
        }
        public List<SelectListItem> ListUser(string Emptype)
        {
            var user = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Employee--", Value = "0" };

            try
            {
                user = db.UserMasters.Where(c => c.isActive == true && c.EmployeeType == Emptype).ToList()
                //user=  from UserMasters in db.UserMasters join garbage in db.GarbageCollectionDetails on UserMasters.userId equals garbage.userId where garbage.EmployeeType=="CT"
                    .Select(x => new SelectListItem
                    {
                        Text = x.userName,
                        Value = x.userId.ToString()
                    }).OrderBy(t => t.Text).ToList();

            }
            catch (Exception ex) { throw ex; }

            return user;
        }

        public List<SelectListItem> CTPTListUser(string Emptype)
        {
            var user = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Employee--", Value = "0" };

            try
            {
                //user = db.UserMasters.Where(c => c.EmployeeType == Emptype).ToList()
                //    // user=  from UserMasters in db.UserMasters join garbage in db.GarbageCollectionDetails on UserMasters.userId equals garbage.userId where garbage.EmployeeType=="CT"
                //    .Select(x => new SelectListItem
                //    {
                //        Text = x.userName,
                //        Value = x.userId.ToString()
                //    }).OrderBy(t => t.Text).ToList();

                user = db.UserMasters.
       Join(db.Daily_Attendance, u => u.userId, uir => uir.userId,
       (u, uir) => new { u, uir })
      .Where(m => m.uir.EmployeeType == "CT")
       .Select(m => new SelectListItem
       {
           Text = m.u.userName,
           Value = m.u.userId.ToString()

       }).OrderBy(t => t.Text).Distinct().ToList();
                //    user = new SelectList(new List<SelectListItem>{
                //from u in db.UserMasters
                //    join b in db.GarbageCollectionDetails
                //    on u.userId equals b.userId
                //    where b.EmployeeType == "CT"
                //    select new SelectListItem
                //    {
                //        Value = u.userId.ToString(),
                //        Text = u.userName,
                //        Selected = u.userId ==b.userId
                //    }
                //}.ToList());


            }
            catch (Exception ex) { throw ex; }

            return user;
        }

        public List<SelectListItem> LoadListWardNo(Int32 ZoneId)
        {
            var WardNo = new List<SelectListItem>();
            if (ZoneId == 0)
            {
                WardNo = ListWardNo();
                return WardNo;
            }
            else
            {
                try
                {
                    SelectListItem itemAdd = new SelectListItem() { Text = "--Select Ward No.--", Value = "0" };
                    WardNo = db.WardNumbers.Where(c => c.zoneId == ZoneId).ToList()
                        .Select(x => new SelectListItem
                        {
                            Text = x.WardNo + " (" + db.ZoneMasters.Where(c => c.zoneId == x.zoneId).FirstOrDefault().name + ")",
                            Value = x.Id.ToString()
                        }).OrderBy(t => t.Text).ToList();
                    WardNo.Insert(0, itemAdd);
                }
                catch (Exception ex) { throw ex; }
                return WardNo;
            }
        }

        public List<SelectListItem> LoadListArea(Int32 WardNo)
        {
            var Area = new List<SelectListItem>();
            if (WardNo == 0)
            {
                Area = ListArea();
                return Area;
            }
            else
            {
                try
                {
                    SelectListItem itemAdd = new SelectListItem() { Text = "--Select Area--", Value = "0" };
                    Area = db.TeritoryMasters.Where(c => c.wardId == WardNo).ToList()
                        .Select(x => new SelectListItem
                        {
                            Text = x.Area,
                            Value = x.Id.ToString()
                        }).OrderBy(t => t.Text).ToList();
                    Area.Insert(0, itemAdd);
                }
                catch (Exception ex) { throw ex; }
                return Area;
            }
        }
        #endregion

        #region ViewModel 
        private AreaVM FillAreaViewModel(TeritoryMaster data)
        {
            AreaVM model = new AreaVM();
            model.Id = data.Id;
            model.Name = data.Area;
            model.NameMar = data.AreaMar;
            model.wardId = data.wardId;
            return model;
        }

        private HouseScanifyEmployeeDetailsVM FillUserViewModel(QrEmployeeMaster data, UserMaster data1)
        {
            try
            {
                HouseScanifyEmployeeDetailsVM model = new HouseScanifyEmployeeDetailsVM();
                // model.qrEmpId = (data.qrEmpId == null ? 0 : data.qrEmpId) ;
                if (data != null)
                {
                    model.qrEmpLoginId = (data.qrEmpLoginId == null ? "" : data.qrEmpLoginId);
                }
                else
                {
                    model.qrEmpLoginId = "";
                }
                if (data1 != null)
                {
                    model.LoginId = (data1.userLoginId.ToString() == "" ? "" : data1.userLoginId);
                }
                else
                {
                    model.LoginId = "";
                }

                //model.NameMar = data.AreaMar;
                //model.wardId = data.wardId;
                return model;
            }
            catch
            {
                throw;
            }
        }

        private UserMasterVM FillUserMasterViewModel(UserMaster data)
        {
            UserMasterVM model = new UserMasterVM();
            model.LoginId = data.userLoginId;
            // model.qrEmpLoginId = data.qrEmpLoginId;
            //model.NameMar = data.AreaMar;
            //model.wardId = data.wardId;
            return model;
        }
        private VehicleTypeVM FillVehicleViewModel(VehicleType data)
        {
            VehicleTypeVM model = new VehicleTypeVM();
            model.Id = data.vtId;
            model.description = data.description;
            model.descriptionMar = data.descriptionMar;
            model.isActive = data.isActive;
            return model;
        }

        private VehicleRegVM FillVehicleTegViewModel(VehicleRegistration data)
        {
            VehicleRegVM model = new VehicleRegVM();
            model.vehicleId = data.vehicleId;
            model.vehicleNumber = data.vehicleNo;
            model.vehicleType = data.vehicleType;
            model.AreaId = data.areaId;
            model.isActive = data.isActive;
            return model;
        }

        private StreetSweepVM FillBeatModel(StreetSweepVM data)
        {
            StreetSweepVM model = new StreetSweepVM();
            model.BeatId = data.SSId;
            return model;
        }

        private WardNumberVM FillWardViewModel(WardNumber data)
        {
            WardNumberVM model = new WardNumberVM();
            model.Id = data.Id;
            model.WardNo = data.WardNo;
            model.zoneId = data.zoneId;
            return model;
        }
        private HouseDetailsVM FillHouseDetailsViewModel(HouseMaster data)
        {

            HouseDetailsVM model = new HouseDetailsVM();
            model.houseId = data.houseId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.houseOwner = data.houseOwner;
            model.houseOwnerMar = data.houseOwnerMar;
            model.houseAddress = data.houseAddress;
            model.houseMobile = data.houseOwnerMobile;
            model.houseNumber = data.houseNumber;
            model.houseQRCode = data.houseQRCode;
            model.houseLat = data.houseLat;
            model.houseLong = data.houseLong;
            model.ReferanceId = data.ReferanceId;
            model.houseCategory = (data.CType is null) ? "RW" : data.CType;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                if (data.AreaId > 0)
                {
                    model.areaName = db.TeritoryMasters.Where(c => c.Id == data.AreaId).FirstOrDefault().Area;
                }
                else
                {
                    model.areaName = "";
                }


                if (data.WardNo > 0)
                {
                    model.wardName = db.WardNumbers.Where(c => c.Id == data.WardNo).FirstOrDefault().WardNo;
                }
                else
                {
                    model.wardName = "";
                }




            }

            return model;
        }

        private SWMDetailsVM FillSWMDetailsViewModel(SWMMaster data)
        {

            SWMDetailsVM model = new SWMDetailsVM();
            model.swmId = data.swmId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.swmName = data.swmName;
            model.swmManager = data.swmManager;
            model.swmOwnerMar = data.swmOwnerMar;
            model.swmAddress = data.swmAddress;
            model.swmMobile = data.swmOwnerMobile;
            model.swmNumber = data.swmNumber;
            model.swmQRCode = data.swmQRCode;
            model.swmLat = data.swmLat;
            model.swmLong = data.swmLong;
            model.ReferanceId = data.ReferanceId;
            model.swmType = (data.swmType is null) ? "RW" : data.swmType;
            model.swmSubType = data.swmSubType;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                if (data.AreaId > 0)
                {
                    model.areaName = db.TeritoryMasters.Where(c => c.Id == data.AreaId).FirstOrDefault().Area;
                }
                else
                {
                    model.areaName = "";
                }


                if (data.WardNo > 0)
                {
                    model.wardName = db.WardNumbers.Where(c => c.Id == data.WardNo).FirstOrDefault().WardNo;
                }
                else
                {
                    model.wardName = "";
                }

            }

            return model;
        }

        private CommercialDetailsVM FillCommericalDetailsViewModel(CommercialMaster data)
        {

            CommercialDetailsVM model = new CommercialDetailsVM();
            model.houseId = data.commercialId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.houseOwner = data.commercialOwner;
            model.houseOwnerMar = data.commercialOwnerMar;
            model.houseAddress = data.commercialAddress;
            model.houseMobile = data.commercialOwnerMobile;
            model.houseNumber = data.commercialNumber;
            model.houseQRCode = data.commercialQRCode;
            model.houseLat = data.commercialLat;
            model.houseLong = data.commercialLong;
            model.ReferanceId = data.ReferanceId;
            model.houseCategory = (data.CType is null) ? "RW" : data.CType;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                if (data.AreaId > 0)
                {
                    model.areaName = db.TeritoryMasters.Where(c => c.Id == data.AreaId).FirstOrDefault().Area;
                }
                else
                {
                    model.areaName = "";
                }


                if (data.WardNo > 0)
                {
                    model.wardName = db.WardNumbers.Where(c => c.Id == data.WardNo).FirstOrDefault().WardNo;
                }
                else
                {
                    model.wardName = "";
                }




            }

            return model;
        }


        private SBALUserLocationMapView FillHouseDetailsViewModelforMap(HouseMaster data)
        {

            SBALUserLocationMapView model = new SBALUserLocationMapView();
            model.houseId = data.houseId;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.houseOwner = data.houseOwner;
            model.houseOwnerMar = data.houseOwnerMar;
            model.houseAddress = data.houseAddress;
            model.houseMobile = data.houseOwnerMobile;
            model.houseNumber = data.houseNumber;
            model.houseQRCode = data.houseQRCode;
            model.houseLat = data.houseLat;
            model.houseLong = data.houseLong;
            model.ReferanceId = data.ReferanceId;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                if (data.AreaId > 0)
                {
                    model.areaName = db.TeritoryMasters.Where(c => c.Id == data.AreaId).FirstOrDefault().Area;
                }
                else
                {
                    model.areaName = "";
                }


                if (data.WardNo > 0)
                {
                    model.wardName = db.WardNumbers.Where(c => c.Id == data.WardNo).FirstOrDefault().WardNo;
                }
                else
                {
                    model.wardName = "";
                }

            }

            return model;
        }

        private SBALUserLocationMapView FillCTPTDetailsViewModelforMap(SauchalayAddress data)
        {

            SBALUserLocationMapView model = new SBALUserLocationMapView();
            model.houseId = data.Id;
            model.WardNo = data.WardNo;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.houseOwner = data.Name;
            model.houseOwnerMar = data.Name;
            model.houseAddress = data.Address;
            model.houseMobile = data.Mobile;
            model.houseNumber = data.Mobile;
            model.houseQRCode = data.SauchalayQRCode;
            model.houseLat = data.Lat;
            model.houseLong = data.Long;
            model.ReferanceId = data.ReferanceId;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                if (data.AreaId > 0)
                {
                    model.areaName = db.TeritoryMasters.Where(c => c.Id == data.AreaId).FirstOrDefault().Area;
                }
                else
                {
                    model.areaName = "";
                }


                if (data.WardNo > 0)
                {
                    model.wardName = db.WardNumbers.Where(c => c.Id == data.WardNo).FirstOrDefault().WardNo;
                }
                else
                {
                    model.wardName = "";
                }

            }

            return model;
        }

        private GarbagePointDetailsVM FillGarbagePointDetailsViewModel(GarbagePointDetail data)
        {
            GarbagePointDetailsVM model = new GarbagePointDetailsVM();
            model.areaId = data.areaId;
            model.WardNo = data.wardId;
            model.ZoneId = data.zoneId;
            model.gpId = data.gpId;
            model.gpAddress = data.gpAddress;
            model.gpLat = data.gpLat;
            model.gpLong = data.gpLong;
            model.gpName = data.gpName;
            model.gpNameMar = data.gpNameMar;
            model.qrCode = data.qrCode;
            model.ReferanceId = data.ReferanceId;
            return model;
        }
        private EmployeeDetailsVM FillEmployeeViewModel(UserMaster data)
        {
            EmployeeDetailsVM model = new EmployeeDetailsVM();
            model.userId = data.userId;
            model.userAddress = data.userAddress;
            model.userLoginId = data.userLoginId;
            model.userMobileNumber = data.userMobileNumber;
            model.userName = data.userName;
            model.userNameMar = data.userNameMar;
            model.userPassword = data.userPassword;
            model.userProfileImage = data.userProfileImage;
            model.userEmployeeNo = data.userEmployeeNo;
            model.imoNo = data.imoNo;
            model.isActive = data.isActive;
            model.bloodGroup = data.bloodGroup;
            model.gcTarget = data.gcTarget;
            model.ComgcTarget = data.ComgcTarget;
            model.EmployeeType = data.EmployeeType;
            model.userDesignation = data.userDesignation;
            return model;
        }

        //private EmployeeDetailsVM FillLiquidEmployeeViewModel(UserMaster_Liquid data)
        //{
        //    EmployeeDetailsVM model = new EmployeeDetailsVM();
        //    model.userId = data.userId;
        //    model.userAddress = data.userAddress;
        //    model.userLoginId = data.userLoginId;
        //    model.userMobileNumber = data.userMobileNumber;
        //    model.userName = data.userName;
        //    model.userNameMar = data.userNameMar;
        //    model.userPassword = data.userPassword;
        //    model.userProfileImage = data.userProfileImage;
        //    model.userEmployeeNo = data.userEmployeeNo;
        //    model.imoNo = data.imoNo;
        //    model.isActive = data.isActive;
        //    model.bloodGroup = data.bloodGroup;
        //    model.gcTarget = data.gcTarget;
        //    model.EmployeeType = data.EmployeeType;
        //    return model;
        //}

        private SBAAttendenceSettingsGridRow FillAttendenceEmployeeViewModel(Daily_Attendance data)
        {
            SBAAttendenceSettingsGridRow model = new SBAAttendenceSettingsGridRow();
            model.userId = data.userId;
            model.userName = db.UserMasters.Where(c => c.userId == data.userId).FirstOrDefault().userName;
            model.startTime = data.startTime;
            model.NotificationTime = data.startTime;
            model.NotificationMobileNumber = db.UserMasters.Where(c => c.userId == data.userId).FirstOrDefault().userMobileNumber;
            return model;
        }


        // Added By Saurabh

        private DumpYardDetailsVM FillDumpYardDetailsViewModel(DumpYardDetail data)
        {
            DumpYardDetailsVM model = new DumpYardDetailsVM();
            model.areaId = data.areaId;
            model.WardNo = data.wardId;
            model.ZoneId = data.zoneId;
            model.dyId = data.dyId;
            model.dyAddress = data.dyAddress;
            model.dyLat = data.dyLat;
            model.dyLong = data.dyLong;
            model.dyName = data.dyName;
            model.dyNameMar = data.dyNameMar;
            model.dyQRCode = data.dyQRCode;
            model.ReferanceId = data.ReferanceId;
            //model.lastModifiedDate = data.lastModifiedDate;
            return model;
        }

        private StreetSweepVM FillStreetSweepDetailsViewModel(StreetSweepingDetail data)
        {
            StreetSweepVM model = new StreetSweepVM();
            model.areaId = data.areaId;
            model.WardNo = data.wardId;
            model.ZoneId = data.zoneId;
            model.SSId = data.SSId;
            model.SSAddress = data.SSAddress;
            model.SSLat = data.SSLat;
            model.SSLong = data.SSLong;
            model.SSName = data.SSName;
            model.SSNameMar = data.SSNameMar;
            model.SSQRCode = data.SSQRCode;
            model.ReferanceId = data.ReferanceId;
            //model.lastModifiedDate = data.lastModifiedDate;
            return model;
        }

        private LiquidWasteVM FillLiquidWasteDetailsViewModel(LiquidWasteDetail data)
        {
            LiquidWasteVM model = new LiquidWasteVM();
            model.areaId = data.areaId;
            model.WardNo = data.wardId;
            model.ZoneId = data.zoneId;
            model.LWId = data.LWId;
            model.LWAddress = data.LWAddreLW;
            model.LWLat = data.LWLat;
            model.LWLong = data.LWLong;
            model.LWName = data.LWName;
            model.LWNameMar = data.LWNameMar;
            model.LWQRCode = data.LWQRCode;
            model.ReferanceId = data.ReferanceId;
            //model.lastModifiedDate = data.lastModifiedDate;
            return model;
        }


        //Added By Saurabh (04 June 2019)
        private HouseScanifyEmployeeDetailsVM FillHSEmployeeViewModel(QrEmployeeMaster data)
        {
            HouseScanifyEmployeeDetailsVM model = new HouseScanifyEmployeeDetailsVM();
            model.qrEmpId = data.qrEmpId;
            model.qrEmpAddress = data.qrEmpAddress;
            model.qrEmpLoginId = data.qrEmpLoginId;
            model.qrEmpPassword = data.qrEmpPassword;
            model.qrEmpMobileNumber = data.qrEmpMobileNumber;
            model.qrEmpName = data.qrEmpName;
            model.qrEmpNameMar = data.qrEmpNameMar;
            model.imoNo = data.imoNo;
            model.isActive = data.isActive;
            model.bloodGroup = data.bloodGroup;
            model.userEmployeeNo = data.userEmployeeNo;
            return model;
        }

        private SS_1_4_ANSWER FillSS1Point4DataModel(OnePoint4VM data)
        {
            SS_1_4_ANSWER model = new SS_1_4_ANSWER();
            model.ANS_ID = data.ANS_ID;
            model.Q_ID = data.Q_ID;
            model.TOTAL_COUNT = data.TOTAL_COUNT.ToString();
            int INSERT_ID = db.SS_1_4_ANSWER.Max(p => p.INSERT_ID) == null ? 0 : db.SS_1_4_ANSWER.Max(p => p.INSERT_ID.Value);
            if (HttpContext.Current.Session["DateTime"] == null)
            {
                HttpContext.Current.Session["DateTime"] = DateTime.Now;
                HttpContext.Current.Session["INSERT_ID"] = INSERT_ID + 1;
            }
            model.INSERT_DATE = Convert.ToDateTime(HttpContext.Current.Session["DateTime"]);
            model.INSERT_ID = Convert.ToInt32(HttpContext.Current.Session["INSERT_ID"]);
            return model;
        }

        private SS_1_4_ANSWER FillSS1Point5DataModel(OnePoint5VM data)
        {
            SS_1_4_ANSWER model = new SS_1_4_ANSWER();
            model.ANS_ID = data.ANS_ID;
            model.TOTAL_COUNT = data.TOTAL_COUNT;
            model.Q_ID = data.Q_ID;
            if (HttpContext.Current.Session["DateTime"] == null)
            {
                HttpContext.Current.Session["DateTime"] = DateTime.Now;
            }
            model.INSERT_DATE = Convert.ToDateTime(HttpContext.Current.Session["DateTime"]);
            return model;
        }
        private SS_1_7_ANSWER FillSS1Point7DataModel(OnePointSevenVM data)
        {
            SS_1_7_ANSWER model = new SS_1_7_ANSWER();
            model.id = data.id;
            model.No_water_bodies = data.No_water_bodies;
            model.No_drain_nallas = data.No_drain_nallas;
            model.No_locations = data.No_locations;
            model.No_outlets = data.No_outlets;
            int INSERT_ID = db.SS_1_7_ANSWER.Max(p => p.INSERT_ID) == null ? 0 : db.SS_1_7_ANSWER.Max(p => p.INSERT_ID.Value);
            if (HttpContext.Current.Session["INSERT_ID"] == null)
            {
                HttpContext.Current.Session["INSERT_ID"] = INSERT_ID + 1;
                HttpContext.Current.Session["DateTime"] = DateTime.Now;
            }

            model.INSERT_ID = Convert.ToInt32(HttpContext.Current.Session["INSERT_ID"]);
            model.INSERT_DATE = Convert.ToDateTime(HttpContext.Current.Session["DateTime"]);
            return model;
        }

        private SauchalayDetailsVM FillSauchalayDetailsViewModel(SauchalayAddress data)
        {

            SauchalayDetailsVM model = new SauchalayDetailsVM();
            model.Id = data.Id;
            model.SauchalayID = data.SauchalayID;
            model.Lat = data.Lat;
            model.Long = data.Long;
            model.Name = data.Name;
            model.Address = data.Address;
            model.Mobile = data.Mobile;
            model.Image = data.ImageUrl;
            model.QrImage = data.QrImageUrl;
            model.CreatedDate = data.CreatedDate;
            model.Tot = data.Tot;
            model.Tns = data.Tns;
            model.SauchalayQRCode = data.SauchalayQRCode;
            model.ReferanceId = data.ReferanceId;
            model.lastModifiedDateEntry = data.lastModifiedDateEntry;
            model.AreaId = data.AreaId;
            model.ZoneId = data.ZoneId;
            model.WardNo = data.WardNo;
            model.userId = data.userId;
            model.TOEMC = data.TOEMC;
            model.TOC = data.TOC;
            return model;
        }
        #endregion

        #region Ward Number
        public ZoneVM GetZone(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.ZoneMasters.Where(x => x.zoneId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        ZoneVM obj = new ZoneVM();
                        obj.id = Details.zoneId;
                        obj.name = Details.name;
                        return obj;
                    }
                    else
                    {
                        return new ZoneVM();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ZoneVM StreetGetZone(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.ZoneMasters.Where(x => x.zoneId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        ZoneVM obj = new ZoneVM();
                        obj.id = Details.zoneId;
                        obj.name = Details.name;
                        return obj;
                    }
                    else
                    {
                        return new ZoneVM();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveZone(ZoneVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.id > 0)
                    {
                        var model = db.ZoneMasters.Where(x => x.zoneId == data.id).FirstOrDefault();
                        if (model != null)
                        {
                            model.zoneId = data.id;
                            model.name = data.name;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ZoneMaster obj = new ZoneMaster();
                        obj.name = data.name;
                        obj.zoneId = data.id;
                        db.ZoneMasters.Add(obj);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void StreetSaveZone(ZoneVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.SSid > 0)
                    {
                        var model = db.ZoneMasters.Where(x => x.zoneId == data.SSid).FirstOrDefault();
                        if (model != null)
                        {
                            model.zoneId = data.SSid;
                            model.name = data.SSname;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ZoneMaster obj = new ZoneMaster();
                        obj.name = data.SSname;
                        obj.zoneId = data.SSid;
                        db.ZoneMasters.Add(obj);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ZoneVM GetValidZone(string name, int zoneId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.ZoneMasters.Where(x => x.name == name || x.zoneId == zoneId).FirstOrDefault();
                    if (Details != null)
                    {
                        ZoneVM obj = new ZoneVM();
                        obj.id = Details.zoneId;
                        obj.name = Details.name;
                        return obj;
                    }
                    else
                    {
                        return new ZoneVM();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //Added By Saurabh

        #region Dump Yard Details
        public DumpYardDetailsVM GetDumpYardtDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.DumpYardQRCode + "/";
                DumpYardDetailsVM dumpYard = new DumpYardDetailsVM();

                var Details = db.DumpYardDetails.Where(x => x.dyId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    dumpYard = FillDumpYardDetailsViewModel(Details);
                    if (dumpYard.dyQRCode != null && dumpYard.dyQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + dumpYard.dyQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                dumpYard.dyQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                dumpYard.dyQRCode = ThumbnaiUrlCMS + dumpYard.dyQRCode.Trim();
                            }
                        }
                        catch (Exception e) { dumpYard.dyQRCode = "/Images/default_not_upload.png"; }

                        //  point.qrCode = ThumbnaiUrlCMS + point.qrCode.Trim();
                    }
                    else
                    {
                        dumpYard.dyQRCode = "/Images/default_not_upload.png";
                    }

                    // house.WardList = ListWardNo();
                    dumpYard.AreaList = LoadListArea(Convert.ToInt32(dumpYard.WardNo));//ListArea();
                    dumpYard.ZoneList = ListZone();
                    dumpYard.WardList = LoadListWardNo(Convert.ToInt32(dumpYard.ZoneId));//ListWardNo();

                    return dumpYard;
                }
                else
                {
                    var id = db.DumpYardDetails.OrderByDescending(x => x.dyId).Select(x => x.dyId).FirstOrDefault();
                    int number = 1000;
                    string refer = "DYSBA" + (number + id + 1);
                    dumpYard.ReferanceId = refer;
                    dumpYard.dyQRCode = "/Images/QRcode.png";
                    dumpYard.WardList = ListWardNo();
                    dumpYard.AreaList = ListArea();
                    dumpYard.ZoneList = ListZone();
                    return dumpYard;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        public StreetSweepVM GetStreetSweepDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.StreetQRCode + "/";
                StreetSweepVM StreetSweep = new StreetSweepVM();

                var Details = db.StreetSweepingDetails.Where(x => x.SSId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    StreetSweep = FillStreetSweepDetailsViewModel(Details);
                    if (StreetSweep.SSQRCode != null && StreetSweep.SSQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + StreetSweep.SSQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                StreetSweep.SSQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                StreetSweep.SSQRCode = ThumbnaiUrlCMS + StreetSweep.SSQRCode.Trim();


                                int n = teamId;
                                double refer1 = Convert.ToDouble((n + 1));
                                double xyz = refer1 / 100;

                                string s = xyz.ToString("0.00", CultureInfo.InvariantCulture);
                                string[] parts = s.Split('.');
                                int i1 = int.Parse(parts[0]);
                                int i2 = int.Parse(parts[1]);

                                if (i2 == 0)
                                {
                                    //i1 = i1 + 1;
                                    s = "S" + i1.ToString();
                                }
                                else
                                {
                                    s = "S" + (i1 + 1);
                                }

                                StreetSweep.SerielNo = s;
                            }
                        }
                        catch (Exception e) { StreetSweep.SSQRCode = "/Images/default_not_upload.png"; }

                        //  point.qrCode = ThumbnaiUrlCMS + point.qrCode.Trim();
                    }
                    else
                    {
                        StreetSweep.SSQRCode = "/Images/default_not_upload.png";
                    }

                    // house.WardList = ListWardNo();
                    StreetSweep.AreaList = LoadListArea(Convert.ToInt32(StreetSweep.WardNo));//ListArea();
                    StreetSweep.ZoneList = ListZone();
                    StreetSweep.WardList = LoadListWardNo(Convert.ToInt32(StreetSweep.ZoneId));//ListWardNo();

                    return StreetSweep;
                }
                else
                {
                    var id = db.StreetSweepingDetails.OrderByDescending(x => x.SSId).Select(x => x.SSId).FirstOrDefault();
                    int number = 1000;
                    string refer = "SSSBA" + (number + id + 1);
                    StreetSweep.ReferanceId = refer;
                    StreetSweep.SSQRCode = "/Images/QRcode.png";
                    StreetSweep.WardList = ListWardNo();
                    StreetSweep.AreaList = ListArea();
                    StreetSweep.ZoneList = ListZone();
                    return StreetSweep;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public LiquidWasteVM GetLiquidWasteDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.LiquidQRCode + "/";
                LiquidWasteVM LiquidWaste = new LiquidWasteVM();

                var Details = db.LiquidWasteDetails.Where(x => x.LWId == teamId).FirstOrDefault();
                if (Details != null)
                {
                    LiquidWaste = FillLiquidWasteDetailsViewModel(Details);
                    if (LiquidWaste.LWQRCode != null && LiquidWaste.LWQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + LiquidWaste.LWQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                LiquidWaste.LWQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                LiquidWaste.LWQRCode = ThumbnaiUrlCMS + LiquidWaste.LWQRCode.Trim();


                                int n = teamId;
                                double refer1 = Convert.ToDouble((n + 1));
                                double xyz = refer1 / 100;

                                string s = xyz.ToString("0.00", CultureInfo.InvariantCulture);
                                string[] parts = s.Split('.');
                                int i1 = int.Parse(parts[0]);
                                int i2 = int.Parse(parts[1]);

                                if (i2 == 0)
                                {
                                    //i1 = i1 + 1;
                                    s = "S" + i1.ToString();
                                }
                                else
                                {
                                    s = "S" + (i1 + 1);
                                }

                                LiquidWaste.SerielNo = s;
                            }
                        }
                        catch (Exception e) { LiquidWaste.LWQRCode = "/Images/default_not_upload.png"; }

                        //  point.qrCode = ThumbnaiUrlCMS + point.qrCode.Trim();
                    }
                    else
                    {
                        LiquidWaste.LWQRCode = "/Images/default_not_upload.png";
                    }

                    // house.WardList = ListWardNo();
                    LiquidWaste.AreaList = LoadListArea(Convert.ToInt32(LiquidWaste.WardNo));//ListArea();
                    LiquidWaste.ZoneList = ListZone();
                    LiquidWaste.WardList = LoadListWardNo(Convert.ToInt32(LiquidWaste.ZoneId));//ListWardNo();

                    return LiquidWaste;
                }
                else
                {
                    var id = db.LiquidWasteDetails.OrderByDescending(x => x.LWId).Select(x => x.LWId).FirstOrDefault();
                    int number = 1000;
                    string refer = "LWSBA" + (number + id + 1);
                    LiquidWaste.ReferanceId = refer;
                    LiquidWaste.LWQRCode = "/Images/QRcode.png";
                    LiquidWaste.WardList = ListWardNo();
                    LiquidWaste.AreaList = ListArea();
                    LiquidWaste.ZoneList = ListZone();
                    return LiquidWaste;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DumpYardDetailsVM SaveDumpYardtDetails(DumpYardDetailsVM data, string Emptype)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.dyId > 0)
                    {
                        var model = db.DumpYardDetails.Where(x => x.dyId == data.dyId).FirstOrDefault();
                        if (model != null)
                        {
                            model.zoneId = data.ZoneId;
                            model.wardId = data.WardNo;
                            model.areaId = data.areaId;
                            model.dyAddress = data.dyAddress;
                            model.dyLat = data.dyLat;
                            model.dyLong = data.dyLong;
                            model.dyName = data.dyName;
                            model.dyNameMar = data.dyNameMar;
                            model.dyQRCode = data.dyQRCode;
                            model.ReferanceId = data.ReferanceId;
                            model.lastModifiedDate = DateTime.Now;
                            //model.EmployeeType = data.EmployeType;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillDumpYardDetailsDataModel(data, Emptype);
                        db.DumpYardDetails.Add(type);
                        db.SaveChanges();
                    }
                }
                var dyId = db.DumpYardDetails.OrderByDescending(x => x.dyId).Select(x => x.dyId).FirstOrDefault();
                DumpYardDetailsVM vv = GetDumpYardtDetails(dyId);
                return vv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public StreetSweepVM SaveStreetSweepDetails(StreetSweepVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.SSId > 0)
                    {
                        var model = db.StreetSweepingDetails.Where(x => x.SSId == data.SSId).FirstOrDefault();
                        if (model != null)
                        {
                            model.zoneId = data.ZoneId;
                            model.wardId = data.WardNo;
                            model.areaId = data.areaId;
                            model.SSAddress = data.SSAddress;
                            model.SSLat = data.SSLat;
                            model.SSLong = data.SSLong;
                            model.SSName = data.SSName;
                            model.SSNameMar = data.SSNameMar;
                            model.SSQRCode = data.SSQRCode;
                            model.ReferanceId = data.ReferanceId;
                            model.lastModifiedDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillStreetSweepDetailsDataModel(data);
                        db.StreetSweepingDetails.Add(type);
                        db.SaveChanges();
                    }
                }
                var SSId = db.StreetSweepingDetails.OrderByDescending(x => x.SSId).Select(x => x.SSId).FirstOrDefault();
                StreetSweepVM vv = GetStreetSweepDetails(SSId);
                return vv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public StreetSweepVM SaveStreetBeatDetails(StreetSweepVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.BeatId > 0)
                    {
                        var model = db.StreetSweepingBeats.Where(x => x.BeatId == data.BeatId).FirstOrDefault();
                        if (model != null)
                        {
                            model.CreateDate = DateTime.Now;
                            model.ReferanceId1 = data.SSBeatone;
                            model.ReferanceId2 = data.SSBeattwo;
                            model.ReferanceId3 = data.SSBeatthree;
                            model.ReferanceId4 = data.SSBeatfour;
                            model.ReferanceId5 = data.SSBeatfive;

                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillStreetBeatDetailsDataModel(data);
                        db.StreetSweepingBeats.Add(type);
                        db.SaveChanges();
                    }

                }
                var SSId = db.StreetSweepingBeats.OrderByDescending(x => x.BeatId).Select(x => x.BeatId).FirstOrDefault();
                StreetSweepVM vv = GetBeatDetails(SSId);
                return vv;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public LiquidWasteVM SaveLiquidWasteDetails(LiquidWasteVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.LWId > 0)
                    {
                        var model = db.LiquidWasteDetails.Where(x => x.LWId == data.LWId).FirstOrDefault();
                        if (model != null)
                        {
                            model.zoneId = data.ZoneId;
                            model.wardId = data.WardNo;
                            model.areaId = data.areaId;
                            model.LWAddreLW = data.LWAddress;
                            model.LWLat = data.LWLat;
                            model.LWLong = data.LWLong;
                            model.LWName = data.LWName;
                            model.LWNameMar = data.LWNameMar;
                            model.LWQRCode = data.LWQRCode;
                            model.ReferanceId = data.ReferanceId;
                            model.lastModifiedDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillLiquidWasteDetailsDataModel(data);
                        db.LiquidWasteDetails.Add(type);
                        db.SaveChanges();
                    }
                }
                var LWId = db.LiquidWasteDetails.OrderByDescending(x => x.LWId).Select(x => x.LWId).FirstOrDefault();
                LiquidWasteVM vv = GetLiquidWasteDetails(LWId);
                return vv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeletDumpYardtDetails(int teamId)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.GarbagePointDetails.Where(x => x.gpId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        db.GarbagePointDetails.Remove(Details);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion


        // Added By Saurabh (27 May 2019)
        #region HouseScanify

        public List<QrEmployeeMaster> GetUserList(int AppId, int teamId)
        {
            //try
            //{
            var appdetails = dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefault();
            using (DevChildSwachhBharatNagpurEntities db = new DevChildSwachhBharatNagpurEntities(AppId))
            //using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                // var Details = db.Locations.FirstOrDefault();

                // if (AppId > 0)
                // {
                //     Details = db.Locations.Where(c => c.locId == teamId).FirstOrDefault();

                // }

                //// var atten = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(Details.datetime) && c.userId == Details.userId).FirstOrDefault();
                // if (Details != null)
                // {
                //SBALUserLocationMapView loc = new SBALUserLocationMapView();
                List<QrEmployeeMaster> user = new List<QrEmployeeMaster>();
                user = db.QrEmployeeMasters.OrderBy(x => x.qrEmpName).ToList();
                //loc.userName = user.userName;
                //loc.date = Convert.ToDateTime(Details.datetime).ToString("dd/MM/yyyy");
                //loc.time = Convert.ToDateTime(Details.datetime).ToString("hh:mm tt");
                //loc.address = Details.address;
                //loc.lat = Details.lat;
                //loc.log = Details.@long;
                //loc.UserList = ListUser();
                //loc.userMobile = user.userMobileNumber;
                // try { loc.vehcileNumber = atten.vehicleNumber; } catch { loc.vehcileNumber = ""; }

                return user;
                //}
                //else
                //{
                //    return new SBALUserLocationMapView();
                //}

            }
            //}
            //catch (Exception ex)
            //{
            //    return new SBALUserLocationMapView();
            //}
        }


        //Added By Saurabh (03 June 2019)
        public HouseScanifyEmployeeDetailsVM GetHSEmployeeDetails(int teamId)
        {
            try
            {
                HouseScanifyEmployeeDetailsVM type = new HouseScanifyEmployeeDetailsVM();
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.UserProfile + "/";
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.QrEmployeeMasters.Where(x => x.qrEmpId == teamId).FirstOrDefault();
                    if (Details != null)
                    {
                        type = FillHSEmployeeViewModel(Details);
                        //if (type.userProfileImage != null && type.userProfileImage != "")
                        //{
                        //    type.userProfileImage = ThumbnaiUrlCMS + type.userProfileImage.Trim();
                        //}
                        //else
                        //{
                        //    type.userProfileImage = "/Images/default_not_upload.png";
                        //}
                        return type;
                    }
                    else
                    {
                        //type.userProfileImage = "/Images/add_image_square.png";
                        return type;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void SaveHSEmployeeDetails(HouseScanifyEmployeeDetailsVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.qrEmpId > 0)
                    {
                        var model = db.QrEmployeeMasters.Where(x => x.qrEmpId == data.qrEmpId).FirstOrDefault();
                        if (model != null)
                        {

                            model.qrEmpId = data.qrEmpId;
                            model.qrEmpAddress = data.qrEmpAddress;
                            model.qrEmpLoginId = data.qrEmpLoginId;
                            model.qrEmpMobileNumber = data.qrEmpMobileNumber;
                            model.qrEmpName = data.qrEmpName;
                            model.qrEmpNameMar = data.qrEmpNameMar;
                            model.qrEmpPassword = data.qrEmpPassword;
                            model.imoNo = data.imoNo;
                            model.isActive = data.isActive;
                            model.bloodGroup = data.bloodGroup;
                            model.userEmployeeNo = data.userEmployeeNo;
                            model.typeId = 1;
                            model.type = "Employee";
                            db.SaveChanges();

                        }
                    }
                    else
                    {
                        var type = FillHSEmployeeDataModel(data);
                        db.QrEmployeeMasters.Add(type);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HSDashBoardVM GetHSDashBoardDetails()
        {
            HSDashBoardVM model = new HSDashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();

                    var data = db.SP_HouseScanifyDetails().First();


                    if (data != null)
                    {

                        model.TotalHouse = data.TotalHouse;
                        model.TotalHouseUpdated = data.TotalHouseUpdated;
                        model.TotalHouseUpdated_CurrentDay = data.TotalHouseUpdated_CurrentDay;
                        model.TotalPoint = data.TotalPoint;
                        model.TotalPointUpdated = data.TotalPointUpdated;
                        model.TotalPointUpdated_CurrentDay = data.TotalPointUpdated_CurrentDay;
                        model.TotalDump = data.TotalDump;
                        model.TotalDumpUpdated = data.TotalDumpUpdated;
                        model.TotalDumpUpdated_CurrentDay = data.TotalDumpUpdated_CurrentDay;

                        model.TotalLiquid = data.TotalLiquid;
                        model.TotalLiquidUpdated = data.TotalLiquidUpdated;
                        model.TotalLiquidUpdated_CurrentDay = data.TotalLiquidUpdated_CurrentDay;

                        model.TotalStreet = data.TotalStreet;
                        model.TotalStreetUpdated = data.TotalStreetUpdated;
                        model.TotalStreetUpdated_CurrentDay = data.TotalStreetUpdated_CurrentDay;

                        model.TotalResidentialUpdated = data.TotalResidential;
                        model.TotalResidentialUpdated_CurrentDay = data.TotalResidentialUpdated_CurrentDay;

                        model.TotalBuildingUpdated = data.TotalBuilding;
                        model.TotalBuildingUpdated_CurrentDay = data.TotalBuildingUpdated_CurrentDay;

                        model.TotalSlumUpdated = data.TotalSlum;
                        model.TotalSlumUpdated_CurrentDay = data.TotalSlumUpdated_CurrentDay;

                        model.TotalCommercialUpdated = data.TotalCommercial;
                        model.TotalCommercialUpdated_CurrentDay = data.TotalCommercialUpdated_CurrentDay;
                        model.TotalCommercial = data.TotalCommercialUpdated;

                        model.TotalSWMUpdated = data.TotalSWM;
                        model.TotalSWMUpdated_CurrentDay = data.TotalSWMUpdated_CurrentDay;
                        model.TotalSWM = data.TotalSWMUpdated;

                        model.TotalCTPTUpdated = data.TotalCTPT;
                        model.TotalCTPTUpdated_CurrentDay = data.TotalCTPTUpdated_CurrentDay;
                        model.TotalCTPT = data.TotalCTPTUpdated;

                        // For Blink in House Scanify 
                        model.HouseMinutes = data.HouseMinutes;
                        model.LiquidMinutes = data.LiquidMinutes;
                        model.StreetMinutes = data.StreetMinutes;
                        model.DumpYardMinutes = data.DumpYardMinutes;
                        model.CommercialMinutes = data.CommercialMinutes;
                        model.SWMMinutes = data.SWMMinutes;
                        model.CTPTMinutes = data.CTPTMinutes;



                        return model;
                    }

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception)
            {
                return model;
            }
        }

        public HouseScanifyEmployeeDetailsVM GetUserDetails(int teamId, string name)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var Details = db.QrEmployeeMasters.Where(x => x.qrEmpId == teamId || x.qrEmpLoginId.ToUpper()
                    == name.ToUpper()).FirstOrDefault();

                    var Details1 = db.UserMasters.Where(x => x.userId == teamId || x.userLoginId.ToUpper()
                    == name.ToUpper()).FirstOrDefault();

                    if (Details != null || Details1 != null)
                    {
                        HouseScanifyEmployeeDetailsVM user = FillUserViewModel(Details, Details1);
                        // area.WardList = ListWardNo();
                        return user;
                    }
                    //if (Details1 != null)
                    //{
                    //    HouseScanifyEmployeeDetailsVM user = FillUserViewModel(Details1);
                    //    // area.WardList = ListWardNo();
                    //    return user;
                    //}
                    else
                    {
                        HouseScanifyEmployeeDetailsVM user = new HouseScanifyEmployeeDetailsVM();
                        // area.WardList = ListWardNo();

                        return user;
                    }
                }
            }
            catch (Exception)
            {

                return new HouseScanifyEmployeeDetailsVM();
            }
        }

        public List<SBALHSUserLocationMapView> GetHSUserAttenRoute(int qrEmpDaId)
        {
            List<SBALHSUserLocationMapView> userLocation = new List<SBALHSUserLocationMapView>();
            DateTime newdate = DateTime.Now.Date;
            var datt = newdate;
            var att = db.Qr_Employee_Daily_Attendance.Where(c => c.qrEmpDaId == qrEmpDaId).FirstOrDefault();
            string Time = att.startTime;
            DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            string t = date.ToString("hh:mm:ss tt");
            string dt = Convert.ToDateTime(att.startDate).ToString("MM/dd/yyyy");
            DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            DateTime? edate;
            if (att.endTime == "" | att.endTime == null)
            {
                edate = DateTime.Now;
            }
            else
            {
                string Time2 = att.endTime;
                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                string t2 = date2.ToString("hh:mm:ss tt");
                string dt2 = Convert.ToDateTime(att.endDate).ToString("MM/dd/yyyy");
                edate = Convert.ToDateTime(dt2 + " " + t2);
            }


            var data = (from t1 in db.Qr_Location.Where(c => c.empId == att.qrEmpId & c.datetime >= fdate & c.datetime <= edate)
                        join t2 in db.QrEmployeeMasters on t1.empId equals t2.qrEmpId
                        select new { t1.locId, t1.batteryStatus, t1.datetime, t1.lat, t1.@long, t1.address, t2.qrEmpName, t2.qrEmpMobileNumber }).ToList();

            // var data =   db.Qr_Location.Where(c => c.empId == att.qrEmpId & c.datetime >= fdate & c.datetime <= edate).ToList();


            foreach (var x in data)
            {

                string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                // var userName = db.UserMasters.Where(c => c.userId == att.userId).FirstOrDefault();

                userLocation.Add(new SBALHSUserLocationMapView()
                {
                    userName = x.qrEmpName,
                    date = dat,
                    time = tim,
                    lat = x.lat,
                    log = x.@long,
                    address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    //vehcileNumber = att.vehicleNumber,
                    userMobile = x.qrEmpMobileNumber,
                    // type = Convert.ToInt32(x.type),

                });

            }

            return userLocation;
        }

        public List<SBAHSHouseDetailsGrid> GetHSQRCodeImageByDate1(int type, int UserId, DateTime fDate, DateTime tDate)
        {
            List<SBAHSHouseDetailsGrid> data = new List<SBAHSHouseDetailsGrid>();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (UserId > 0)
                    {
                        if (type == 0)
                        {
                            data = db.HouseMasters.Where(a => a.modified > fDate && a.modified < tDate && !string.IsNullOrEmpty(a.houseLat) && !string.IsNullOrEmpty(a.houseLong) && !string.IsNullOrEmpty(a.QRCodeImage) && a.userId == UserId).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.houseId,
                                Name = x.houseOwner,
                                HouseLat = x.houseLat,
                                HouseLong = x.houseLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 1)
                        {
                            data = db.CommercialMasters.Where(a => a.modified > fDate && a.modified < tDate && !string.IsNullOrEmpty(a.commercialLat) && !string.IsNullOrEmpty(a.commercialLong) && !string.IsNullOrEmpty(a.QRCodeImage) && a.userId == UserId).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.commercialId,
                                Name = x.commercialOwner,
                                HouseLat = x.commercialLat,
                                HouseLong = x.commercialLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 2)
                        {
                            data = db.LiquidWasteDetails.Where(a => a.lastModifiedDate > fDate && a.lastModifiedDate < tDate && !string.IsNullOrEmpty(a.LWLat) && !string.IsNullOrEmpty(a.LWLong) && !string.IsNullOrEmpty(a.QRCodeImage) && a.userId == UserId).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.LWId,
                                Name = x.LWName,
                                HouseLat = x.LWLat,
                                HouseLong = x.LWLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 3)
                        {
                            data = db.StreetSweepingDetails.Where(a => a.lastModifiedDate > fDate && a.lastModifiedDate < tDate && !string.IsNullOrEmpty(a.SSLat) && !string.IsNullOrEmpty(a.SSLong) && !string.IsNullOrEmpty(a.QRCodeImage) && a.userId == UserId).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.SSId,
                                Name = x.SSName,
                                HouseLat = x.SSLat,
                                HouseLong = x.SSLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 4)
                        {
                            data = db.SauchalayAddresses.Where(a => a.lastModifiedDate > fDate && a.lastModifiedDate < tDate && !string.IsNullOrEmpty(a.Lat) && !string.IsNullOrEmpty(a.Long) && !string.IsNullOrEmpty(a.QRCodeImage) && a.userId == UserId).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.Id,
                                Name = x.Name,
                                HouseLat = x.Lat,
                                HouseLong = x.Long,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 5)
                        {
                            data = db.SWMMasters.Where(a => a.modified > fDate && a.modified < tDate && !string.IsNullOrEmpty(a.swmLat) && !string.IsNullOrEmpty(a.swmLong) && !string.IsNullOrEmpty(a.QRCodeImage) && a.userId == UserId).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.swmId,
                                Name = x.swmName,
                                HouseLat = x.swmLat,
                                HouseLong = x.swmLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                    }
                    else
                    {
                        if (type == 0)
                        {
                            data = db.HouseMasters.Where(a => a.modified > fDate && a.modified < tDate && !string.IsNullOrEmpty(a.houseLat) && !string.IsNullOrEmpty(a.houseLong) && !string.IsNullOrEmpty(a.QRCodeImage)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.houseId,
                                Name = x.houseOwner,
                                HouseLat = x.houseLat,
                                HouseLong = x.houseLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 1)
                        {
                            data = db.CommercialMasters.Where(a => a.modified > fDate && a.modified < tDate && !string.IsNullOrEmpty(a.commercialLat) && !string.IsNullOrEmpty(a.commercialLong) && !string.IsNullOrEmpty(a.QRCodeImage)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.commercialId,
                                Name = x.commercialOwner,
                                HouseLat = x.commercialLat,
                                HouseLong = x.commercialLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 2)
                        {
                            data = db.LiquidWasteDetails.Where(a => a.lastModifiedDate > fDate && a.lastModifiedDate < tDate && !string.IsNullOrEmpty(a.LWLat) && !string.IsNullOrEmpty(a.LWLong) && !string.IsNullOrEmpty(a.QRCodeImage)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.LWId,
                                Name = x.LWName,
                                HouseLat = x.LWLat,
                                HouseLong = x.LWLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 3)
                        {
                            data = db.StreetSweepingDetails.Where(a => a.lastModifiedDate > fDate && a.lastModifiedDate < tDate && !string.IsNullOrEmpty(a.SSLat) && !string.IsNullOrEmpty(a.SSLong) && !string.IsNullOrEmpty(a.QRCodeImage)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.SSId,
                                Name = x.SSName,
                                HouseLat = x.SSLat,
                                HouseLong = x.SSLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 4)
                        {
                            data = db.SauchalayAddresses.Where(a => a.lastModifiedDate > fDate && a.lastModifiedDate < tDate && !string.IsNullOrEmpty(a.Lat) && !string.IsNullOrEmpty(a.Long) && !string.IsNullOrEmpty(a.QRCodeImage)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.Id,
                                Name = x.Name,
                                HouseLat = x.Lat,
                                HouseLong = x.Long,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else if (type == 5)
                        {
                            data = db.SWMMasters.Where(a => a.modified > fDate && a.modified < tDate && !string.IsNullOrEmpty(a.swmLat) && !string.IsNullOrEmpty(a.swmLong) && !string.IsNullOrEmpty(a.QRCodeImage)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.swmId,
                                Name = x.swmName,
                                HouseLat = x.swmLat,
                                HouseLong = x.swmLong,
                                QRCodeImage = x.QRCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return data;
            }
            return data;
        }

        public List<SBAHSHouseDetailsGrid> GetHSQRCodeImageByDate(int type, int UserId, DateTime fDate, DateTime tDate, string QrStatus)
        {

            bool? bQRStatus = null;
            if (QrStatus == "1")
            {
                bQRStatus = true;
            }
            else if (QrStatus == "2")
            {
                bQRStatus = false;

            }
            else
            {
                bQRStatus = null;
            }
            List<SBAHSHouseDetailsGrid> data = new List<SBAHSHouseDetailsGrid>();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    if (type == 0)
                    {
                        //data = db.HouseMasters.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && ((bQRStatus != null && (a.QRStatusDate >= fDate && a.QRStatusDate <= tDate)) || (bQRStatus == null && (a.modified >= fDate && a.modified <= tDate))) && !string.IsNullOrEmpty(a.houseLat) && !string.IsNullOrEmpty(a.houseLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid

                        if (QrStatus == "3")
                        {
                            data = db.HouseMasters.Where(a => (a.QRStatus==null) && (a.modified >= fDate && a.modified <= tDate) && !string.IsNullOrEmpty(a.houseLat) && !string.IsNullOrEmpty(a.houseLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.houseId,
                                Name = x.houseOwner,
                                HouseLat = x.houseLat,
                                HouseLong = x.houseLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else
                        {
                            data = db.HouseMasters.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && (a.modified >= fDate && a.modified <= tDate) && !string.IsNullOrEmpty(a.houseLat) && !string.IsNullOrEmpty(a.houseLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.houseId,
                                Name = x.houseOwner,
                                HouseLat = x.houseLat,
                                HouseLong = x.houseLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }

                    }
                    else if (type == 1)
                    {

                        if (QrStatus == "3")
                        {
                            data = db.CommercialMasters.Where(a => (a.QRStatus == null) && (a.modified >= fDate && a.modified <= tDate) && !string.IsNullOrEmpty(a.commercialLat) && !string.IsNullOrEmpty(a.commercialLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.commercialId,
                                Name = x.commercialOwner,
                                HouseLat = x.commercialLat,
                                HouseLong = x.commercialLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else
                        {
                            data = db.CommercialMasters.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && (a.modified >= fDate && a.modified <= tDate) && !string.IsNullOrEmpty(a.commercialLat) && !string.IsNullOrEmpty(a.commercialLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.commercialId,
                                Name = x.commercialOwner,
                                HouseLat = x.commercialLat,
                                HouseLong = x.commercialLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }

                      
                    }
                    else if (type == 2)
                    {

                        if (QrStatus == "3")
                        {
                            data = db.LiquidWasteDetails.Where(a => (a.QRStatus == null) && (a.lastModifiedDate >= fDate && a.lastModifiedDate <= tDate) && !string.IsNullOrEmpty(a.LWLat) && !string.IsNullOrEmpty(a.LWLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.LWId,
                                Name = x.LWName,
                                HouseLat = x.LWLat,
                                HouseLong = x.LWLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else
                        {
                            data = db.LiquidWasteDetails.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && (a.lastModifiedDate >= fDate && a.lastModifiedDate <= tDate) && !string.IsNullOrEmpty(a.LWLat) && !string.IsNullOrEmpty(a.LWLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.LWId,
                                Name = x.LWName,
                                HouseLat = x.LWLat,
                                HouseLong = x.LWLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }

                           
                    }
                    else if (type == 3)
                    {
                        if (QrStatus == "3")
                        {
                            data = db.StreetSweepingDetails.Where(a => (a.QRStatus == null) && (a.lastModifiedDate >= fDate && a.lastModifiedDate <= tDate) && !string.IsNullOrEmpty(a.SSLat) && !string.IsNullOrEmpty(a.SSLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.SSId,
                                Name = x.SSName,
                                HouseLat = x.SSLat,
                                HouseLong = x.SSLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else
                        {
                            data = db.StreetSweepingDetails.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && (a.lastModifiedDate >= fDate && a.lastModifiedDate <= tDate) && !string.IsNullOrEmpty(a.SSLat) && !string.IsNullOrEmpty(a.SSLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.SSId,
                                Name = x.SSName,
                                HouseLat = x.SSLat,
                                HouseLong = x.SSLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }

                           
                    }
                    else if (type == 4)
                    {
                        if (QrStatus == "3")
                        {
                            data = db.SauchalayAddresses.Where(a => (a.QRStatus == null) && (a.lastModifiedDate >= fDate && a.lastModifiedDate <= tDate) && !string.IsNullOrEmpty(a.Lat) && !string.IsNullOrEmpty(a.Long) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.Id,
                                Name = x.Name,
                                HouseLat = x.Lat,
                                HouseLong = x.Long,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else
                        {
                            data = db.SauchalayAddresses.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && (a.lastModifiedDate >= fDate && a.lastModifiedDate <= tDate) && !string.IsNullOrEmpty(a.Lat) && !string.IsNullOrEmpty(a.Long) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.Id,
                                Name = x.Name,
                                HouseLat = x.Lat,
                                HouseLong = x.Long,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                       
                    }
                    else if (type == 5)
                    {
                        if (QrStatus == "3")
                        {
                            data = db.SWMMasters.Where(a => (a.QRStatus == null) && (a.modified >= fDate && a.modified <= tDate) && !string.IsNullOrEmpty(a.swmLat) && !string.IsNullOrEmpty(a.swmLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.swmId,
                                Name = x.swmName,
                                HouseLat = x.swmLat,
                                HouseLong = x.swmLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                        else
                        {
                            data = db.SWMMasters.Where(a => ((bQRStatus != null && a.QRStatus == bQRStatus) || bQRStatus == null) && (a.modified >= fDate && a.modified <= tDate) && !string.IsNullOrEmpty(a.swmLat) && !string.IsNullOrEmpty(a.swmLong) && ((UserId > 0 && a.userId == UserId) || UserId <= 0) && (a.BinaryQrCodeImage != null)).Select(x => new SBAHSHouseDetailsGrid
                            {
                                houseId = x.swmId,
                                Name = x.swmName,
                                HouseLat = x.swmLat,
                                HouseLong = x.swmLong,
                                //QRCodeImage = x.QRCodeImage,
                                BinaryQrCodeImage = x.BinaryQrCodeImage,
                                ReferanceId = x.ReferanceId
                            }).OrderBy(a => a.houseId).ToList();
                        }
                       
                    }

                }
            }
            catch (Exception ex)
            {
                return data;
            }
            return data;
        }


        public void SaveHSQRStatusHouse(int houseId, string QRStatus)
        {
            bool? bQRStatus = null;
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (houseId > 0 && !string.IsNullOrEmpty(QRStatus))
                    {
                        if (QRStatus == "1")
                        {
                            bQRStatus = true;
                        }
                        else if (QRStatus == "0")
                        {
                            bQRStatus = false;
                        }
                        else
                        {
                            bQRStatus = null;
                        }

                        var model = db.HouseMasters.Where(x => x.houseId == houseId).FirstOrDefault();
                        if (model != null)
                        {
                            model.QRStatus = bQRStatus;
                            model.QRStatusDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SaveHSQRStatusComr(int comrId, string QRStatus)
        {
            bool? bQRStatus = null;
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (comrId > 0 && !string.IsNullOrEmpty(QRStatus))
                    {
                        if (QRStatus == "1")
                        {
                            bQRStatus = true;
                        }
                        else if (QRStatus == "0")
                        {
                            bQRStatus = false;
                        }
                        else
                        {
                            bQRStatus = null;
                        }

                        var model = db.CommercialMasters.Where(x => x.commercialId == comrId).FirstOrDefault();
                        if (model != null)
                        {
                            model.QRStatus = bQRStatus;
                            model.QRStatusDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveHSQRStatusCTPT(int CTPTId, string QRStatus)
        {
            bool? bQRStatus = null;
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (CTPTId > 0 && !string.IsNullOrEmpty(QRStatus))
                    {
                        if (QRStatus == "1")
                        {
                            bQRStatus = true;
                        }
                        else if (QRStatus == "0")
                        {
                            bQRStatus = false;
                        }
                        else
                        {
                            bQRStatus = null;
                        }

                        var model = db.SauchalayAddresses.Where(x => x.Id == CTPTId).FirstOrDefault();
                        if (model != null)
                        {
                            model.QRStatus = bQRStatus;
                            model.QRStatusDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveHSQRStatusSWM(int SWMId, string QRStatus)
        {
            bool? bQRStatus = null;
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (SWMId > 0 && !string.IsNullOrEmpty(QRStatus))
                    {
                        if (QRStatus == "1")
                        {
                            bQRStatus = true;
                        }
                        else if (QRStatus == "0")
                        {
                            bQRStatus = false;
                        }
                        else
                        {
                            bQRStatus = null;
                        }

                        var model = db.SWMMasters.Where(x => x.swmId == SWMId).FirstOrDefault();
                        if (model != null)
                        {
                            model.QRStatus = bQRStatus;
                            model.QRStatusDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveHSQRStatusLW(int LWId, string QRStatus)
        {
            bool? bQRStatus = null;
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (LWId > 0 && !string.IsNullOrEmpty(QRStatus))
                    {
                        if (QRStatus == "1")
                        {
                            bQRStatus = true;
                        }
                        else if (QRStatus == "0")
                        {
                            bQRStatus = false;
                        }
                        else
                        {
                            bQRStatus = null;
                        }

                        var model = db.LiquidWasteDetails.Where(x => x.LWId == LWId).FirstOrDefault();
                        if (model != null)
                        {
                            model.QRStatus = bQRStatus;
                            model.QRStatusDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveHSQRStatusSW(int SWId, string QRStatus)
        {
            bool? bQRStatus = null;
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (SWId > 0 && !string.IsNullOrEmpty(QRStatus))
                    {
                        if (QRStatus == "1")
                        {
                            bQRStatus = true;
                        }
                        else if (QRStatus == "0")
                        {
                            bQRStatus = false;
                        }
                        else
                        {
                            bQRStatus = null;
                        }

                        var model = db.StreetSweepingDetails.Where(x => x.SSId == SWId).FirstOrDefault();
                        if (model != null)
                        {
                            model.QRStatus = bQRStatus;
                            model.QRStatusDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<int> GetHSHouseDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            List<int> lstIDs = new List<int>() { };

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var data = db.SP_GetHSHouseDetailsID(fromDate, toDate, userId, QRStatus, sortColumn, sortOrder, searchString).ToList();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var i in data)
                        {
                            lstIDs.Add(i ?? 0);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstIDs;
        }
        public List<int> GetHSComrDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {


            List<int> lstIDs = new List<int>() { };
            bool? bQRStatus = null;
            if (QRStatus == 1)
            {
                bQRStatus = true;
            }
            else if (QRStatus == 2)
            {
                bQRStatus = false;

            }
            else
            {
                bQRStatus = null;
            }

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var model = db.CommercialMasters
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.modified,
                                           userId = p.c.userId,
                                           houseId = p.c.commercialId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.commercialLat,
                                           HouseLong = p.c.commercialLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(c => ((bQRStatus != null && c.QRStatus == bQRStatus) || bQRStatus == null) && (c.modifiedDate >= fromDate && c.modifiedDate <= toDate) && c.HouseLat != null && c.HouseLong != null).OrderBy(c => c.houseId).ToList();
                    //}).Where(c => ((bQRStatus != null && c.QRStatus == bQRStatus) || bQRStatus == null) && ((bQRStatus != null && (c.QRStatusDate >= fromDate && c.QRStatusDate <= toDate)) || (bQRStatus == null && (c.modifiedDate >= fromDate && c.modifiedDate <= toDate))) && c.HouseLat != null && c.HouseLong != null).OrderBy(c => c.houseId).ToList();

                    if (QRStatus == 3)
                    {
                        model = model.Where(c => (c.QRStatus == null)).ToList();
                    }

                    if (fromDate != null && toDate != null)
                    {
                        if (Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"))
                        {
                            model = model.Where(c => (Convert.ToDateTime(c.modifiedDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy"))).ToList();
                        }
                        else
                        {

                            model = model.Where(c => (c.modifiedDate >= fromDate && c.modifiedDate <= toDate)).ToList();
                        }
                    }
                    if (userId > 0)
                    {
                        model = model.Where(c => c.userId == userId).ToList();

                    }

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(c => ((string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " + (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId)).ToUpper().Contains(searchString.ToUpper())
                         ).ToList();

                    }

                    lstIDs = model.Select(x => x.houseId).ToList();

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstIDs;
        }

        public List<int> GetHSCTPTDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {


            List<int> lstIDs = new List<int>() { };
            bool? bQRStatus = null;
            if (QRStatus == 1)
            {
                bQRStatus = true;
            }
            else if (QRStatus == 2)
            {
                bQRStatus = false;

            }
            else
            {
                bQRStatus = null;
            }

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var model = db.SauchalayAddresses
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.lastModifiedDate,
                                           userId = p.c.userId,
                                           houseId = p.c.Id,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.Lat,
                                           HouseLong = p.c.Long,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(c => ((bQRStatus != null && c.QRStatus == bQRStatus) || bQRStatus == null) && (c.modifiedDate >= fromDate && c.modifiedDate <= toDate) && c.HouseLat != null && c.HouseLong != null).OrderBy(c => c.houseId).ToList();

                    if (QRStatus == 3)
                    {
                        model = model.Where(c => (c.QRStatus == null)).ToList();
                    }

                    if (fromDate != null && toDate != null)
                    {
                        if (Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"))
                        {
                            model = model.Where(c => (Convert.ToDateTime(c.modifiedDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy"))).ToList();
                        }
                        else
                        {

                            model = model.Where(c => (c.modifiedDate >= fromDate && c.modifiedDate <= toDate)).ToList();
                        }
                    }
                    if (userId > 0)
                    {
                        model = model.Where(c => c.userId == userId).ToList();

                    }

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(c => ((string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " + (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId)).ToUpper().Contains(searchString.ToUpper())
                         ).ToList();

                    }

                    lstIDs = model.Select(x => x.houseId).ToList();

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstIDs;
        }

        public List<int> GetHSSWMDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {


            List<int> lstIDs = new List<int>() { };
            bool? bQRStatus = null;
            if (QRStatus == 1)
            {
                bQRStatus = true;
            }
            else if (QRStatus == 2)
            {
                bQRStatus = false;

            }
            else
            {
                bQRStatus = null;
            }

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var model = db.SWMMasters
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.modified,
                                           userId = p.c.userId,
                                           houseId = p.c.swmId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.swmLat,
                                           HouseLong = p.c.swmLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(c => ((bQRStatus != null && c.QRStatus == bQRStatus) || bQRStatus == null) && (c.modifiedDate >= fromDate && c.modifiedDate <= toDate) && c.HouseLat != null && c.HouseLong != null).OrderBy(c => c.houseId).ToList();

                    if (QRStatus == 3)
                    {
                        model = model.Where(c => (c.QRStatus == null)).ToList();
                    }

                    if (fromDate != null && toDate != null)
                    {
                        if (Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"))
                        {
                            model = model.Where(c => (Convert.ToDateTime(c.modifiedDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy"))).ToList();
                        }
                        else
                        {

                            model = model.Where(c => (c.modifiedDate >= fromDate && c.modifiedDate <= toDate)).ToList();
                        }
                    }
                    if (userId > 0)
                    {
                        model = model.Where(c => c.userId == userId).ToList();

                    }

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(c => ((string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " + (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId)).ToUpper().Contains(searchString.ToUpper())
                         ).ToList();

                    }

                    lstIDs = model.Select(x => x.houseId).ToList();

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstIDs;
        }

        public List<int> GetHSLWDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {


            List<int> lstIDs = new List<int>() { };
            bool? bQRStatus = null;
            if (QRStatus == 1)
            {
                bQRStatus = true;
            }
            else if (QRStatus == 2)
            {
                bQRStatus = false;

            }
            else
            {
                bQRStatus = null;
            }

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var model = db.LiquidWasteDetails
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.lastModifiedDate,
                                           userId = p.c.userId,
                                           houseId = p.c.LWId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.LWLat,
                                           HouseLong = p.c.LWLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(c => ((bQRStatus != null && c.QRStatus == bQRStatus) || bQRStatus == null) && (c.modifiedDate >= fromDate && c.modifiedDate <= toDate) && c.HouseLat != null && c.HouseLong != null).OrderBy(c => c.houseId).ToList();

                    if (QRStatus == 3)
                    {
                        model = model.Where(c => (c.QRStatus == null)).ToList();
                    }

                    if (fromDate != null && toDate != null)
                    {
                        if (Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"))
                        {
                            model = model.Where(c => (Convert.ToDateTime(c.modifiedDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy"))).ToList();
                        }
                        else
                        {

                            model = model.Where(c => (c.modifiedDate >= fromDate && c.modifiedDate <= toDate)).ToList();
                        }
                    }
                    if (userId > 0)
                    {
                        model = model.Where(c => c.userId == userId).ToList();

                    }

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(c => ((string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " + (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId)).ToUpper().Contains(searchString.ToUpper())
                         ).ToList();

                    }

                    lstIDs = model.Select(x => x.houseId).ToList();

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstIDs;
        }

        public List<int> GetHSSWDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {


            List<int> lstIDs = new List<int>() { };
            bool? bQRStatus = null;
            if (QRStatus == 1)
            {
                bQRStatus = true;
            }
            else if (QRStatus == 2)
            {
                bQRStatus = false;

            }
            else
            {
                bQRStatus = null;
            }

            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    var model = db.StreetSweepingDetails
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.lastModifiedDate,
                                           userId = p.c.userId,
                                           houseId = p.c.SSId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.SSLat,
                                           HouseLong = p.c.SSLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(c => ((bQRStatus != null && c.QRStatus == bQRStatus) || bQRStatus == null) && (c.modifiedDate >= fromDate && c.modifiedDate <= toDate) && c.HouseLat != null && c.HouseLong != null).OrderBy(c => c.houseId).ToList();
                    if (QRStatus == 3)
                    {
                        model = model.Where(c => (c.QRStatus == null)).ToList();
                    }

                    if (fromDate != null && toDate != null)
                    {
                        if (Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"))
                        {
                            model = model.Where(c => (Convert.ToDateTime(c.modifiedDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy"))).ToList();
                        }
                        else
                        {

                            model = model.Where(c => (c.modifiedDate >= fromDate && c.modifiedDate <= toDate)).ToList();
                        }
                    }
                    if (userId > 0)
                    {
                        model = model.Where(c => c.userId == userId).ToList();

                    }

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(c => ((string.IsNullOrEmpty(c.Name) ? " " : c.Name) + " " + (string.IsNullOrEmpty(c.ReferanceId) ? " " : c.ReferanceId)).ToUpper().Contains(searchString.ToUpper())
                         ).ToList();

                    }

                    lstIDs = model.Select(x => x.houseId).ToList();

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstIDs;
        }

        public SBAHSHouseDetailsGrid GetHouseDetailsById(int houseId)
        {
            SBAHSHouseDetailsGrid data = null;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                data = db.SP_GetHSHouseDetailsById(houseId).Select(x => new SBAHSHouseDetailsGrid
                {
                    houseId = x.houseId,
                    Name = x.qrEmpName,
                    HouseLat = x.houseLat,
                    HouseLong = x.houseLong,
                    QRCodeImage = x.QRCodeImage,
                    ReferanceId = x.ReferanceId,
                    modifiedDate = x.modified.HasValue ? Convert.ToDateTime(x.modified).ToString("dd/MM/yyyy hh:mm tt") : "",
                    QRStatusDate = x.QRStatusDate.HasValue ? Convert.ToDateTime(x.QRStatusDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                    QRStatus = x.QRStatus
                }).FirstOrDefault();
            }
            return data;
        }

        public SBAHSDumpyardDetailsGrid GetComrDetailsById(int comrId)
        {
            SBAHSDumpyardDetailsGrid data = null;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var model = db.CommercialMasters
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.modified,
                                           userId = p.c.userId,
                                           dumpId = p.c.commercialId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.commercialLat,
                                           HouseLong = p.c.commercialLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(a => a.dumpId == comrId).FirstOrDefault();

                if (model != null)
                {
                    data = new SBAHSDumpyardDetailsGrid()
                    {
                        dumpId = model.dumpId,
                        Name = model.Name,
                        HouseLat = model.HouseLat,
                        HouseLong = model.HouseLong,
                        //QRCodeImage = model.QRCodeImage,
                        QRCodeImage = (model.QRCodeImage == null || model.QRCodeImage.Length == 0) ? "/Images/default_not_upload.png" : ("data:image/jpeg;base64," + System.Convert.ToBase64String(model.QRCodeImage)),
                        ReferanceId = model.ReferanceId,
                        modifiedDate = model.modifiedDate.HasValue ? Convert.ToDateTime(model.modifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatusDate = model.QRStatusDate.HasValue ? Convert.ToDateTime(model.QRStatusDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatus = model.QRStatus
                    };
                }
            }
            return data;
        }


        public SBAHSDumpyardDetailsGrid GetCTPTDetailsById(int CTPTId)
        {
            SBAHSDumpyardDetailsGrid data = null;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var model = db.SauchalayAddresses
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.lastModifiedDate,
                                           userId = p.c.userId,
                                           dumpId = p.c.Id,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.Lat,
                                           HouseLong = p.c.Long,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(a => a.dumpId == CTPTId).FirstOrDefault();

                if (model != null)
                {
                    data = new SBAHSDumpyardDetailsGrid()
                    {
                        dumpId = model.dumpId,
                        Name = model.Name,
                        HouseLat = model.HouseLat,
                        HouseLong = model.HouseLong,
                        //QRCodeImage = model.QRCodeImage,
                        QRCodeImage = (model.QRCodeImage == null || model.QRCodeImage.Length == 0) ? "/Images/default_not_upload.png" : ("data:image/jpeg;base64," + System.Convert.ToBase64String(model.QRCodeImage)),
                        ReferanceId = model.ReferanceId,
                        modifiedDate = model.modifiedDate.HasValue ? Convert.ToDateTime(model.modifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatusDate = model.QRStatusDate.HasValue ? Convert.ToDateTime(model.QRStatusDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatus = model.QRStatus
                    };
                }
            }
            return data;
        }

        public SBAHSDumpyardDetailsGrid GetSWMDetailsById(int SWMId)
        {
            SBAHSDumpyardDetailsGrid data = null;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var model = db.SWMMasters
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.modified,
                                           userId = p.c.userId,
                                           dumpId = p.c.swmId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.swmLat,
                                           HouseLong = p.c.swmLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(a => a.dumpId == SWMId).FirstOrDefault();

                if (model != null)
                {
                    data = new SBAHSDumpyardDetailsGrid()
                    {
                        dumpId = model.dumpId,
                        Name = model.Name,
                        HouseLat = model.HouseLat,
                        HouseLong = model.HouseLong,
                        //QRCodeImage = model.QRCodeImage,
                        QRCodeImage = (model.QRCodeImage == null || model.QRCodeImage.Length == 0) ? "/Images/default_not_upload.png" : ("data:image/jpeg;base64," + System.Convert.ToBase64String(model.QRCodeImage)),
                        ReferanceId = model.ReferanceId,
                        modifiedDate = model.modifiedDate.HasValue ? Convert.ToDateTime(model.modifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatusDate = model.QRStatusDate.HasValue ? Convert.ToDateTime(model.QRStatusDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatus = model.QRStatus
                    };
                }
            }
            return data;
        }
        public SBAHSLiquidDetailsGrid GetLWDetailsById(int LWId)
        {
            SBAHSLiquidDetailsGrid data = null;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var model = db.LiquidWasteDetails
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.lastModifiedDate,
                                           userId = p.c.userId,
                                           dumpId = p.c.LWId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.LWLat,
                                           HouseLong = p.c.LWLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(a => a.dumpId == LWId).FirstOrDefault();

                if (model != null)
                {
                    data = new SBAHSLiquidDetailsGrid()
                    {
                        liquidId = model.dumpId,
                        Name = model.Name,
                        HouseLat = model.HouseLat,
                        HouseLong = model.HouseLong,
                        //QRCodeImage = model.QRCodeImage,
                        QRCodeImage = (model.QRCodeImage == null || model.QRCodeImage.Length == 0) ? "/Images/default_not_upload.png" : ("data:image/jpeg;base64," + System.Convert.ToBase64String(model.QRCodeImage)),
                        ReferanceId = model.ReferanceId,
                        modifiedDate = model.modifiedDate.HasValue ? Convert.ToDateTime(model.modifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatusDate = model.QRStatusDate.HasValue ? Convert.ToDateTime(model.QRStatusDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatus = model.QRStatus
                    };
                }
            }
            return data;
        }

        public SBAHSStreetDetailsGrid GetSWDetailsById(int SWId)
        {
            SBAHSStreetDetailsGrid data = null;
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var model = db.StreetSweepingDetails
                         .GroupJoin(db.QrEmployeeMasters,
                                      a => a.userId,
                                      b => b.qrEmpId,
                                      (a, b) => new { c = a, d = b.DefaultIfEmpty() })
                           .SelectMany(r => r.d.DefaultIfEmpty(),
                                       (p, b) => new
                                       {
                                           modifiedDate = p.c.lastModifiedDate,
                                           userId = p.c.userId,
                                           dumpId = p.c.SSId,
                                           Name = b.qrEmpName,
                                           HouseLat = p.c.SSLat,
                                           HouseLong = p.c.SSLong,
                                           //QRCodeImage = string.IsNullOrEmpty(p.c.QRCodeImage) ? "/Images/default_not_upload.png" : p.c.QRCodeImage,
                                           QRCodeImage = p.c.BinaryQrCodeImage,
                                           ReferanceId = p.c.ReferanceId,
                                           QRStatus = p.c.QRStatus,
                                           QRStatusDate = p.c.QRStatusDate
                                       }).Where(a => a.dumpId == SWId).FirstOrDefault();

                if (model != null)
                {
                    data = new SBAHSStreetDetailsGrid()
                    {
                        streetId = model.dumpId,
                        Name = model.Name,
                        HouseLat = model.HouseLat,
                        HouseLong = model.HouseLong,
                        //QRCodeImage = model.QRCodeImage,
                        QRCodeImage = (model.QRCodeImage == null || model.QRCodeImage.Length == 0) ? "/Images/default_not_upload.png" : ("data:image/jpeg;base64," + System.Convert.ToBase64String(model.QRCodeImage)),
                        ReferanceId = model.ReferanceId,
                        modifiedDate = model.modifiedDate.HasValue ? Convert.ToDateTime(model.modifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatusDate = model.QRStatusDate.HasValue ? Convert.ToDateTime(model.QRStatusDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                        QRStatus = model.QRStatus
                    };
                }
            }
            return data;
        }


        #endregion

        // Added By Neha(12 July 2019)
        #region idleTime map
        public List<SBAEmplyeeIdelGrid> GetIdleTimeRoute(int userId, string date)
        {
            DateTime date1 = DateTime.ParseExact(date, "dd/MM/yyyy", null);
            List<SBAEmplyeeIdelGrid> obj = new List<SBAEmplyeeIdelGrid>();
            var data = db.SP_IdelTime(userId, date1, date1).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList();
            foreach (var x in data)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes(Convert.ToDouble(x.IdelTime));
                string workHours = spWorkMin.ToString(@"hh\:mm");
                obj.Add(new SBAEmplyeeIdelGrid()
                {
                    UserName = x.userName,
                    Date = Convert.ToDateTime(x.date).ToString("dd/MM/yyyy"),
                    StartTime = x.StartTime,
                    EndTime = x.LastTime,
                    StartAddress = checkNull(x.StartAddress).Replace("Unnamed Road, ", ""),
                    EndAddress = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    IdelTime = workHours,
                    startLat = x.StarLat,
                    startLong = x.StartLog,
                    EndLat = x.lat,
                    EndLong = x.@long,
                    userId = x.userId

                });


            }
            return obj;
        }
        #endregion

        public void Save1Point7(List<OnePointSevenVM> data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    foreach (var item in data)
                    {
                        var type = FillSS1Point7DataModel(item);
                        db.SS_1_7_ANSWER.Add(type);
                        db.SaveChanges();
                    }
                    HttpContext.Current.Session["INSERT_ID"] = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<OnePoint7QuestionVM> GetOnePointSevenQuestions()
        {
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var data = db.RPT_1point7_questions().Select(x => new OnePoint7QuestionVM
                {
                    Id = x.Id,
                    Area = x.Area,
                    wardId = x.wardId.Value,
                }).ToList();
                return data;
            }
        }

        public List<OnePoint7QuestionVM> GetOnePointSevenAnswers(int INSERT_ID)
        {
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var data = db.RPT_1POINT7_DETAILS(INSERT_ID).Select(x => new OnePoint7QuestionVM
                {
                    Id = x.Id,
                    Area = x.Area,
                    No_drain_nallas = x.No_drain_nallas.ToString() == "0" ? "" : x.No_drain_nallas.ToString(),
                    No_Water_Bodies = x.No_water_bodies.ToString() == "0" ? "" : x.No_water_bodies.ToString(),
                    No_locations = x.No_locations.ToString() == "0" ? "" : x.No_locations.ToString(),
                    No_outlets = x.No_outlets.ToString() == "0" ? "" : x.No_outlets.ToString(),
                    INSERT_ID = x.INSERT_ID.Value

                }).ToList();
                return data;
            }
        }

        public void Save1Point4(List<OnePoint4VM> data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    foreach (var item in data)
                    {
                        var type = FillSS1Point4DataModel(item);
                        db.SS_1_4_ANSWER.Add(type);
                        db.SaveChanges();
                    }
                    HttpContext.Current.Session["DateTime"] = null;
                    HttpContext.Current.Session["INSERT_ID"] = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save1Point5(List<OnePoint5VM> data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    foreach (var item in data)
                    {
                        var type = FillSS1Point5DataModel(item);
                        db.SS_1_4_ANSWER.Add(type);
                        db.SaveChanges();

                    }
                    HttpContext.Current.Session["DateTime"] = null;
                    HttpContext.Current.Session["INSERT_ID"] = null;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }
        }


        public List<OnePoint4VM> GetQuetions(string ReportName)
        {
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var data = db.SS_1_4_QUESTION.Where(p => p.Q_NO == ReportName).Select(x => new OnePoint4VM
                {
                    Q_ID = x.Q_ID,
                    QUESTIOND = x.QUESTIOND,
                }).ToList();
                return data;
            }
        }

        public List<OnePoint5VM> GetQuetions1pointfive()
        {
            using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var data = db.SS_1_4_QUESTION.Where(p => p.Q_NO == "1.5").Select(x => new OnePoint5VM
                {
                    Q_ID = x.Q_ID,
                    QUESTIOND = x.QUESTIOND,
                }).ToList();

                return data;
            }
        }
        public List<OnePoint4VM> GetOnepointfourEditData(int INSERT_ID)
        {
            using (var dbMain = new DevChildSwachhBharatNagpurEntities(AppID))
            {
                var data = dbMain.SS_1_4_ANSWER.Where(m => m.INSERT_ID == INSERT_ID).
                    Select(x => new OnePoint4VM
                    {
                        INSERT_DATE = x.INSERT_DATE.Value,
                        INSERT_ID = x.INSERT_ID.Value,
                        ANS_ID = x.ANS_ID,
                        Q_ID = x.Q_ID,
                        TOTAL_COUNT = x.TOTAL_COUNT
                    }).ToList();

                return data;
            }
        }

        public OnePoint4VM GetTotalCountDetails(int ANS_ID)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    OnePoint4VM model = new OnePoint4VM();
                    var Data = db.SS_1_4_ANSWER.Where(m => m.ANS_ID == ANS_ID).SingleOrDefault();
                    if (Data != null)
                    {
                        model.TOTAL_COUNT = Data.TOTAL_COUNT;
                        model.INSERT_ID = Data.INSERT_ID.Value;
                    }
                    return model;
                }
            }
            catch (Exception)
            {
                return new OnePoint4VM();
            }
        }
        public OnePoint4VM GetMaxINSERTID()
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    OnePoint4VM model = new OnePoint4VM();
                    var Data = db.SS_1_4_ANSWER.OrderByDescending(p => p.INSERT_ID).FirstOrDefault();


                    if (Data != null)
                    {
                        model.INSERT_ID = Data.INSERT_ID.Value;
                    }
                    return model;
                }
            }
            catch (Exception)
            {
                return new OnePoint4VM();
            }
        }

        public void SaveTotalCount(OnePoint4VM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    if (data.ANS_ID > 0)
                    {
                        var model = db.SS_1_4_ANSWER.Where(x => x.ANS_ID == data.ANS_ID).FirstOrDefault();
                        if (model != null)
                        {
                            model.TOTAL_COUNT = data.TOTAL_COUNT;

                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public void EditOnePointSeven(List<OnePoint7QuestionVM> data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    foreach (var item in data)
                    {
                        //var type = FillSS1Point7DataModel(item);
                        var model = db.SS_1_7_ANSWER.Where(x => x.id == item.Id && x.INSERT_ID == item.INSERT_ID).FirstOrDefault();
                        if (model != null)
                        {
                            model.No_drain_nallas = Convert.ToInt32(item.No_drain_nallas);
                            model.No_water_bodies = Convert.ToInt32(item.No_Water_Bodies);
                            model.No_locations = Convert.ToInt32(item.No_locations);
                            model.No_outlets = Convert.ToInt32(item.No_outlets);
                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }



        #region Infotainment

        //public InfotainmentDetailsVW GetInfotainmentDetailsById(int ID)
        //{
        //    try
        //    {
        //        InfotainmentDetailsVW type = new InfotainmentDetailsVW();
        //        DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();

        //        //using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
        //        //{
        //        var Details = dbMain.GameDetails.Where(x => x.GameDetailsID == ID).FirstOrDefault();
        //            if (Details != null)
        //            {
        //                type = FillInfotainmentViewModel(Details);
        //                if (type.Image != null && type.Image != "")
        //                {
        //                    type.Image = type.Image.Trim(); //ThumbnaiUrlCMS + type.Image.Trim();
        //                }
        //                else
        //                {
        //                    type.Image = "/Images/default_not_upload.png";
        //                }
        //                return type;
        //            }
        //            else
        //            {
        //                type.GameMasterList = LoadGameList();
        //                type.SloganList = LoadSloganList();
        //                type.AnswerTypeList = LoadAnswerTypeList();
        //                type.Image = "/Images/add_image_square.png";
        //                return type;
        //            }
        //        //}
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        #endregion

        #region Sauchalay
        public SauchalayDetailsVM GetSauchalayDetailsOld(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.HouseQRCode + "/";
                SauchalayDetailsVM data = new SauchalayDetailsVM();

                var Details = db.SauchalayAddresses.Where(x => x.Id == teamId).FirstOrDefault();
                if (Details != null)
                {
                    data = FillSauchalayDetailsViewModel(Details);
                    if (data.Image != null && data.Image != "")
                    {
                        data.Image = data.Image.Trim(); //ThumbnaiUrlCMS + type.Image.Trim();
                    }
                    else
                    {
                        data.Image = "/Images/default_not_upload.png";
                    }
                    if (data.QrImage != null && data.QrImage != "")
                    {
                        data.QrImage = data.QrImage.Trim(); //ThumbnaiUrlCMS + type.Image.Trim();
                    }
                    else
                    {
                        data.QrImage = "/Images/default_not_upload.png";
                    }

                    return data;
                }
                else if (teamId == -2)
                {
                    var id = db.SauchalayAddresses.OrderByDescending(x => x.SauchalayID).Select(x => x.SauchalayID).FirstOrDefault();
                    if (id == null)
                    {
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = appName + '_' + 'S' + '_' + ("0" + 1);
                        data.SauchalayID = name;
                        data.Image = "/Images/add_image_square.png";
                        data.QrImage = "/Images/add_image_square.png";
                        data.Id = 0;
                    }
                    else
                    {
                        var sId = id.Split('_').Last();
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = Convert.ToInt32(sId) < 9 ? appName + '_' + 'S' + '_' + ("0" + (Convert.ToInt32(sId) + 1)) : appName + '_' + 'S' + '_' + ((Convert.ToInt32(sId)) + (1));
                        data.SauchalayID = name;
                        data.Id = 0;
                        data.Image = "/Images/add_image_square.png";
                        data.QrImage = "/Images/add_image_square.png";

                    }
                    return data;
                }
                else
                {
                    var id = db.SauchalayAddresses.OrderByDescending(x => x.SauchalayID).Select(x => x.SauchalayID).FirstOrDefault();

                    if (id == null)
                    {
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = appName + '_' + 'S' + '_' + ("0" + 1);
                        data.SauchalayID = name;
                        data.Id = 0;
                    }
                    else
                    {
                        var sId = id.Split('_').Last();
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = Convert.ToInt32(sId) < 9 ? appName + '_' + 'S' + '_' + ("0" + (Convert.ToInt32(sId) + 1)) : appName + '_' + 'S' + '_' + ((Convert.ToInt32(sId)) + (1));
                        data.Id = Convert.ToInt32(sId);
                    }
                    return data;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public SauchalayDetailsVM GetSauchalayDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.CTPTQRCode + "/";
                SauchalayDetailsVM data = new SauchalayDetailsVM();

                var Details = db.SauchalayAddresses.Where(x => x.Id == teamId).FirstOrDefault();
                if (Details != null)
                {
                    data = FillSauchalayDetailsViewModel(Details);
                    if (data.Image != null && data.Image != "")
                    {
                        data.Image = data.Image.Trim(); //ThumbnaiUrlCMS + type.Image.Trim();
                    }
                    else
                    {
                        data.Image = "/Images/default_not_upload.png";
                    }
                    if (data.QrImage != null && data.QrImage != "")
                    {
                        data.QrImage = data.QrImage.Trim(); //ThumbnaiUrlCMS + type.Image.Trim();
                    }
                    else
                    {
                        data.QrImage = "/Images/default_not_upload.png";
                    }

                    if (data.SauchalayQRCode != null && data.SauchalayQRCode != "")
                    {
                        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(ThumbnaiUrlCMS + data.SauchalayQRCode.Trim());
                        HttpWebResponse httpRes = null;
                        try
                        {
                            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
                            if (httpRes.StatusCode == HttpStatusCode.NotFound)
                            {
                                data.SauchalayQRCode = "/Images/default_not_upload.png";
                            }
                            else
                            {
                                data.SauchalayQRCode = ThumbnaiUrlCMS + data.SauchalayQRCode.Trim();

                                int n = teamId;
                                double refer1 = Convert.ToDouble((n + 1));
                                double xyz = refer1 / 100;

                                string s = xyz.ToString("0.00", CultureInfo.InvariantCulture);
                                string[] parts = s.Split('.');
                                int i1 = int.Parse(parts[0]);
                                int i2 = int.Parse(parts[1]);

                                if (i2 == 0)
                                {
                                    //i1 = i1 + 1;
                                    s = "S" + i1.ToString();
                                }
                                else
                                {
                                    s = "S" + (i1 + 1);
                                }

                                data.SerielNo = s;

                            }
                        }
                        catch (Exception e) { data.SauchalayQRCode = "/Images/default_not_upload.png"; }

                    }
                    else
                    {
                        data.SauchalayQRCode = "/Images/default_not_upload.png";
                    }
                    if (data.ReferanceId == null || data.ReferanceId == "")
                    {
                        var id = db.SauchalayAddresses.OrderByDescending(x => x.SauchalayID).Select(x => x.SauchalayID).FirstOrDefault();
                        if (id == null)
                        {
                            int number = 1000;
                            string refer = "CTPTSBA" + (number + 1);
                            data.ReferanceId = refer;
                        }
                        else
                        {
                            int number = 1000;
                            var sId = id.Split('_').Last();
                            string refer = "CTPTSBA" + (number + (Convert.ToInt32(sId)) + 1);
                            data.ReferanceId = refer;
                        }
                    }
                    data.WardList = LoadListWardNo(Convert.ToInt32(data.ZoneId)); //ListWardNo();
                    data.AreaList = LoadListArea(Convert.ToInt32(data.WardNo)); //ListArea();
                    data.ZoneList = ListZone();
                    return data;
                }
                else if (teamId == -2)
                {
                    var id = db.SauchalayAddresses.OrderByDescending(x => x.SauchalayID).Select(x => x.SauchalayID).FirstOrDefault();
                    if (id == null)
                    {
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = appName + '_' + 'S' + '_' + ("0" + 1);
                        data.SauchalayID = name;
                        data.Image = "/Images/add_image_square.png";
                        data.QrImage = "/Images/add_image_square.png";
                        data.Id = 0;
                        int number = 1000;
                        string refer = "CTPTSBA" + (number + 1);
                        data.ReferanceId = refer;
                        data.SauchalayQRCode = "/Images/QRcode.png";
                    }
                    else
                    {
                        var sId = id.Split('_').Last();
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = Convert.ToInt32(sId) < 9 ? appName + '_' + 'S' + '_' + ("0" + (Convert.ToInt32(sId) + 1)) : appName + '_' + 'S' + '_' + ((Convert.ToInt32(sId)) + (1));
                        data.SauchalayID = name;
                        data.Id = 0;
                        int number = 1000;
                        data.Image = "/Images/add_image_square.png";
                        data.QrImage = "/Images/add_image_square.png";
                        string refer = "CTPTSBA" + (number + (Convert.ToInt32(sId)) + 1);
                        data.ReferanceId = refer;
                        data.SauchalayQRCode = "/Images/QRcode.png";

                    }
                    var WWWW = new List<SelectListItem>();
                    SelectListItem itemAdd = new SelectListItem() { Text = "Select Ward / Prabhag", Value = "0" };
                    WWWW.Insert(0, itemAdd);

                    var ARRR = new List<SelectListItem>();
                    SelectListItem itemAddARR = new SelectListItem() { Text = "Select Area", Value = "0" };
                    ARRR.Insert(0, itemAddARR);


                    data.WardList = WWWW;
                    data.AreaList = ARRR;
                    data.ZoneList = ListZone();
                    return data;
                }
                else
                {
                    var id = db.SauchalayAddresses.OrderByDescending(x => x.SauchalayID).Select(x => x.SauchalayID).FirstOrDefault();

                    if (id == null)
                    {
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = appName + '_' + 'S' + '_' + ("0" + 1);
                        data.SauchalayID = name;

                        data.Image = "/Images/add_image_square.png";
                        data.QrImage = "/Images/add_image_square.png";
                        data.Id = 0;
                        int number = 1000;
                        string refer = "CTPTSBA" + (number + 1);
                        data.ReferanceId = refer;
                        data.SauchalayQRCode = "/Images/QRcode.png";
                        data.WardList = ListWardNo();
                        data.AreaList = ListArea();
                        data.ZoneList = ListZone();
                    }
                    else
                    {
                        var sId = id.Split('_').Last();
                        string appName = (appDetails.AppName).Split(' ').First();
                        string name = Convert.ToInt32(sId) < 9 ? appName + '_' + 'S' + '_' + ("0" + (Convert.ToInt32(sId) + 1)) : appName + '_' + 'S' + '_' + ((Convert.ToInt32(sId)) + (1));
                        data.SauchalayID = name;
                        data.Id = 0;
                        data.Image = "/Images/add_image_square.png";
                        data.QrImage = "/Images/add_image_square.png";

                        int number = 1000;
                        string refer = "CTPTSBA" + (number + (Convert.ToInt32(sId)) + 1);
                        data.ReferanceId = refer;
                        data.SauchalayQRCode = "/Images/QRcode.png";
                        data.WardList = ListWardNo();
                        data.AreaList = ListArea();
                        data.ZoneList = ListZone();
                    }
                    return data;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public SauchalayDetailsVM SaveSauchalayDetails(SauchalayDetailsVM data)
        {
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {
                    if (data.Id > 0)
                    {
                        var model = db.SauchalayAddresses.Where(x => x.Id == data.Id).FirstOrDefault();
                        if (model != null)
                        {
                            if (data.Image != null)
                            {
                                model.ImageUrl = data.Image;
                            }
                            if (data.QrImage != null)
                            {
                                model.QrImageUrl = data.QrImage;
                            }
                            model.SauchalayID = data.SauchalayID;
                            model.Name = data.Name;
                            model.Address = data.Address;
                            model.Lat = data.Lat;
                            model.Long = data.Long;
                            model.Mobile = data.Mobile;
                            //model.CreatedDate = DateTime.Now;
                            model.Tot = data.Tot;
                            model.Tns = data.Tns;
                            model.SauchalayQRCode = data.SauchalayQRCode;
                            model.ReferanceId = data.ReferanceId;
                            model.lastModifiedDateEntry = DateTime.Now;
                            model.AreaId = data.AreaId;
                            model.ZoneId = data.ZoneId;
                            model.WardNo = data.WardNo;
                            model.TOEMC = data.TOEMC;
                            model.TOC = data.TOC;
                            //model.userId = data.userId;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var type = FillSauchalayDetailsDataModel(data);
                        db.SauchalayAddresses.Add(type);
                        db.SaveChanges();
                    }
                }

                var id = db.SauchalayAddresses.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                SauchalayDetailsVM vv = GetSauchalayDetails(id);
                return vv;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<SauchalayDetailsVM> GetCTPTLocation()
        {
            List<SauchalayDetailsVM> ctptLocation = new List<SauchalayDetailsVM>();
            var data = db.SauchalayAddresses.Where(c => c.Lat != null && c.Long != null).ToList();
            foreach (var x in data)
            {
                ctptLocation.Add(new SauchalayDetailsVM()
                {
                    SauchalayID = x.SauchalayID,
                    Address = x.Address,
                    Lat = x.Lat,
                    Long = x.Long,
                    Name = x.Name,
                    Image = (string.IsNullOrEmpty(x.ImageUrl) ? "" : x.ImageUrl),
                    QrImage = (string.IsNullOrEmpty(x.QrImageUrl) ? "" : x.QrImageUrl),
                    Mobile = x.Mobile
                });
            }

            return ctptLocation;
        }

        #endregion

        #region WasteManagement

        public List<SelectListItem> WasteCategoryList()
        {
            var Category = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Category--", Value = "0" };

            try
            {
                Category = db.WM_GarbageCategory.ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.Category,
                        Value = x.CategoryID.ToString()
                    }).OrderBy(t => t.Text).ToList();

                Category.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return Category;
        }
        public List<SelectListItem> WasteSubCategoryList()
        {
            var SubCategory = new List<SelectListItem>();
            SelectListItem itemAdd = new SelectListItem() { Text = "Select Sub-Category", Value = "0" };

            try
            {
                //WardNo = db.WardNumbers.ToList()
                //    .Select(x => new SelectListItem
                //    {
                //        Text = x.WardNo + " (" + db.ZoneMasters.Where(c => c.zoneId == x.zoneId).FirstOrDefault().name + ")",
                //        Value = x.Id.ToString()
                //    }).OrderBy(t => t.Text).ToList();

                SubCategory = db.WM_GarbageSubCategory.ToList()
                   .Select(x => new SelectListItem
                   {
                       Text = x.SubCategory + " (" + db.WM_GarbageCategory.Where(c => c.CategoryID == x.CategoryID).FirstOrDefault().Category + ")",
                       Value = x.SubCategoryID.ToString()
                   }).OrderBy(t => t.Text).ToList();

                SubCategory.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }

            return SubCategory;
        }
        public List<SelectListItem> LoadListSubCategory(Int32 categoryId)
        {
            var SubCategory = new List<SelectListItem>();
            try
            {
                SelectListItem itemAdd = new SelectListItem() { Text = "--Select Sub-Category--", Value = "0" };
                SubCategory = db.WM_GarbageSubCategory.Where(c => c.CategoryID == categoryId).ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.SubCategory,
                        Value = x.SubCategoryID.ToString()
                    }).OrderBy(t => t.Text).ToList();
                SubCategory.Insert(0, itemAdd);
            }
            catch (Exception ex) { throw ex; }
            return SubCategory;
        }

        public List<SelectListItem> GetWMUserDetails()
        {
            var Category = new List<SelectListItem>();
            //var Category1 = new SelectList();
            SelectListItem itemAdd = new SelectListItem() { Text = "--Select Category--", Value = "0" };
            //Category.Add(new SelectListItem() { Text = "--Select Category--", Value = "0" });
            //Category.Add(new SelectListItem() { Text = "--Select Category1--", Value = "1" });
            try
            {

                Category = db.UserMasters.Where(c => c.Type == "2").ToList()
                    .Select(x => new SelectListItem
                    {
                        Text = x.userName,
                        Value = x.userId.ToString()
                    })
                    .OrderBy(t => t.Text).ToList();
                //Category.Add(new SelectListItem() { Text = "--Select Category--", Value = "0" });
                //Category.Add(new SelectListItem() { Text = "Admin", Value = "-3" });
                //Category.Add(new SelectListItem() { Text = "Employees", Value = "-2" });
                //Category1 = new SelectList(Category, "Value", "Text");
                // Category.Insert(0, itemAdd);

                // return Category1;
            }
            catch (Exception ex) { throw ex; }

            return Category;
        }
        public WasteDetailsVM GetWasteDetails(int teamId)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                WasteDetailsVM waste = new WasteDetailsVM();

                if (teamId == -2)
                {
                    var WWWW = new List<SelectListItem>();
                    SelectListItem itemAdd = new SelectListItem() { Text = "Select Category", Value = "0" };
                    WWWW.Insert(0, itemAdd);

                    var ARRR = new List<SelectListItem>();
                    SelectListItem itemAddARR = new SelectListItem() { Text = "Select Sub-Category", Value = "0" };
                    ARRR.Insert(0, itemAddARR);

                    waste.WasteCategoryList = WWWW;
                    waste.WasteSubCategoryList = ARRR;

                    return waste;
                }

                else
                {
                    waste.WasteCategoryList = WasteCategoryList();
                    waste.WasteSubCategoryList = WasteSubCategoryList();
                    return waste;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        //public List<WasteDetailsVM> SaveWasteDetails(List<WasteDetailsVM> data)
        //{
        //    try
        //    {
        //        DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
        //        var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

        //        WasteDetailsVM waste = new WasteDetailsVM();
        //        List<WasteDetailsVM> details = new List<WasteDetailsVM>();

        //        string appId = (appDetails.AppId).ToString();
        //        foreach (var x in data)
        //        {
        //            //bool IsChecked = (bool)((dat))
        //            x.ID = data.IndexOf(x);
        //            x.SubCategoryID = x.SubCategoryID;
        //            x.Weight = x.Weight;
        //            x.UserID = x.UserID;

        //            details.Add(new WasteDetailsVM()
        //            {
        //                ID = x.ID,
        //                SubCategoryID = x.SubCategoryID,
        //                Weight = x.Weight,
        //                UserID = x.UserID
        //        });
        //        }
        //        try
        //        {
        //            WebClient client = new WebClient();
        //            client.Headers["Content-type"] = "application/json";
        //            client.Headers.Add("AppId", appId);
        //            string data1 = JsonConvert.SerializeObject(details);
        //            string json = client.UploadString(appDetails.baseImageUrl + "api/Save/GarbageDetails", data1);
        //            List<WasteDetailsVM> obj2 = JsonConvert.DeserializeObject<List<WasteDetailsVM>>(json).ToList();

        //        }
        //        catch (WebException ex)
        //        {
        //            var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
        //            return "An error occurred, status code: " + statusCode;
        //            // failed...
        //            using (StreamReader r = new StreamReader(
        //                ex.Response.GetResponseStream()))
        //            {
        //                string responseContent = r.ReadToEnd();
        //                // ... do whatever ...
        //            }
        //        }
        //        //var statusCode = ((HttpWebResponse)ex.Response).StatusCode;


        //        return details;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public string SaveWasteDetails(string data)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                WasteDetailsVM waste = new WasteDetailsVM();
                List<WasteDetailsVM> details = new List<WasteDetailsVM>();
                var abcd = JsonConvert.DeserializeObject<List<WasteDetailsVM>>(data).ToList();
                string appId = (appDetails.AppId).ToString();
                foreach (var x in abcd)
                {
                    //bool IsChecked = (bool)((dat))
                    x.ID = abcd.IndexOf(x);
                    x.SubCategoryID = x.SubCategoryID;
                    x.Weight = x.Weight;
                    x.UnitID = x.UnitID;
                    x.Source = 1;

                    details.Add(new WasteDetailsVM()
                    {
                        ID = x.ID,
                        SubCategoryID = x.SubCategoryID,
                        Weight = x.Weight,
                        UnitID = x.UnitID,
                        Source = x.Source

                    });
                }
                try
                {
                    string URI = appDetails.baseImageUrl + "/api/Save/GarbageDetails";
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Headers.Add("AppId", appId);
                    string data1 = JsonConvert.SerializeObject(details);
                    string json = client.UploadString(URI, data1);
                    List<WasteDetailsVM> obj2 = JsonConvert.DeserializeObject<List<WasteDetailsVM>>(json).ToList();

                    return "Success";
                }
                catch (WebException ex)
                {
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    return "An error occurred, status code: " + statusCode;

                    // failed...
                    //using (StreamReader r = new StreamReader(
                    //    ex.Response.GetResponseStream()))
                    //{
                    //    string responseContent = r.ReadToEnd();
                    //    // ... do whatever ...
                    //}
                }
                //var statusCode = ((HttpWebResponse)ex.Response).StatusCode;



            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SaveSalesWasteDetails(string data)
        {
            try
            {
                DevSwachhBharatMainEntities dbMain = new DevSwachhBharatMainEntities();
                var appDetails = dbMain.AppDetails.Where(x => x.AppId == AppID).FirstOrDefault();

                WasteDetailsVM waste = new WasteDetailsVM();
                List<WasteDetailsVM> details = new List<WasteDetailsVM>();
                var abcd = JsonConvert.DeserializeObject<List<WasteDetailsVM>>(data).ToList();
                string appId = (appDetails.AppId).ToString();
                foreach (var x in abcd)
                {
                    //bool IsChecked = (bool)((dat))
                    x.ID = abcd.IndexOf(x);
                    x.SubCategoryID = x.SubCategoryID;
                    x.PartyName = x.PartyName;
                    x.SalesWeight = x.SalesWeight;
                    x.Amount = x.Amount;
                    x.UnitID = x.UnitID;
                    x.Source = 1;

                    details.Add(new WasteDetailsVM()
                    {
                        ID = x.ID,
                        SubCategoryID = x.SubCategoryID,
                        PartyName = x.PartyName,
                        SalesWeight = x.SalesWeight,
                        Amount = x.Amount,
                        UnitID = x.UnitID,
                        Source = x.Source

                    });
                }
                try
                {
                    string URI = appDetails.baseImageUrl + "api/Save/GarbageSales";
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Headers.Add("AppId", appId);
                    string data1 = JsonConvert.SerializeObject(details);
                    string json = client.UploadString(URI, data1);
                    List<WasteDetailsVM> obj2 = JsonConvert.DeserializeObject<List<WasteDetailsVM>>(json).ToList();

                    return "Success";
                }
                catch (WebException ex)
                {
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    return "An error occurred, status code: " + statusCode;

                    // failed...
                    //using (StreamReader r = new StreamReader(
                    //    ex.Response.GetResponseStream()))
                    //{
                    //    string responseContent = r.ReadToEnd();
                    //    // ... do whatever ...
                    //}
                }
                //var statusCode = ((HttpWebResponse)ex.Response).StatusCode;



            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public List<LogVM> GetLogString()
        {
            List<LogVM> _LogVM = new List<LogVM>();

            var data = db.SP_GetLiveTracking().ToList();

            foreach (var x in data)
            {
                _LogVM.Add(new LogVM()
                {
                    Logstring = x.gcId + "   |   " + x.ReferanceId.ToString() + "   |   " + x.Name + "   |   " + x.userName + "   |   " + x.gcDate,
                    gcId = x.gcId,
                });
            }

            return _LogVM;
        }

        public List<SBAEmplyeeIdelGrid> GetIdelTimeNotification()
        {
            List<SBAEmplyeeIdelGrid> obj = new List<SBAEmplyeeIdelGrid>();
            // DateTime? fdate = null
            string dt = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime fdate = Convert.ToDateTime(dt + " " + "00:00:00");
            DateTime tdate = Convert.ToDateTime(dt + " " + "23:59:59");

            var data = db.SP_IdelTime(0, fdate, tdate).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList().OrderByDescending(c => c.StartTime);
            //var data = db.SP_IdelTime(0, fdate, fdate).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList();
            foreach (var x in data)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes(Convert.ToDouble(x.IdelTime));
                string workHours = spWorkMin.ToString(@"hh\:mm");

                string displayTime = Convert.ToDateTime(x.date).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                string time = Convert.ToDateTime(x.StartTime).ToString("HH:mm:ss");

                obj.Add(new SBAEmplyeeIdelGrid()
                {
                    UserName = x.userName,
                    Date = Convert.ToDateTime(x.date).ToString("dd/MM/yyyy"),
                    StartTime = x.StartTime,
                    EndTime = x.LastTime,
                    StartAddress = checkNull(x.StartAddress).Replace("Unnamed Road, ", ""),
                    EndAddress = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    IdelTime = workHours,
                    userId = x.userId,
                    startLat = x.StarLat,
                    startLong = x.StartLog,
                    EndLat = x.lat,
                    EndLong = x.@long,
                    daDateTIme = (displayTime + " " + time)

                });
            }
            return obj;
        }

        public List<SBAEmplyeeIdelGrid> GetLiquidIdelTimeNotification()
        {
            List<SBAEmplyeeIdelGrid> obj = new List<SBAEmplyeeIdelGrid>();
            // DateTime? fdate = null
            string dt = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime fdate = Convert.ToDateTime(dt + " " + "00:00:00");
            DateTime tdate = Convert.ToDateTime(dt + " " + "23:59:59");

            var data = db.SP_IdelTimeLiquid(0, fdate, tdate).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList().OrderByDescending(c => c.StartTime);
            //var data = db.SP_IdelTime(0, fdate, fdate).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList();
            foreach (var x in data)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes(Convert.ToDouble(x.IdelTime));
                string workHours = spWorkMin.ToString(@"hh\:mm");

                string displayTime = Convert.ToDateTime(x.date).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                string time = Convert.ToDateTime(x.StartTime).ToString("HH:mm:ss");

                obj.Add(new SBAEmplyeeIdelGrid()
                {
                    UserName = x.userName,
                    Date = Convert.ToDateTime(x.date).ToString("dd/MM/yyyy"),
                    StartTime = x.StartTime,
                    EndTime = x.LastTime,
                    StartAddress = checkNull(x.StartAddress).Replace("Unnamed Road, ", ""),
                    EndAddress = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    IdelTime = workHours,
                    userId = x.userId,
                    startLat = x.StarLat,
                    startLong = x.StartLog,
                    EndLat = x.lat,
                    EndLong = x.@long,
                    daDateTIme = (displayTime + " " + time)

                });
            }
            return obj;
        }

        public List<SBAEmplyeeIdelGrid> GetStreetIdelTimeNotification()
        {
            List<SBAEmplyeeIdelGrid> obj = new List<SBAEmplyeeIdelGrid>();
            // DateTime? fdate = null
            string dt = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime fdate = Convert.ToDateTime(dt + " " + "00:00:00");
            DateTime tdate = Convert.ToDateTime(dt + " " + "23:59:59");

            var data = db.SP_IdelTimestreet(0, fdate, tdate).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList().OrderByDescending(c => c.StartTime);
            //var data = db.SP_IdelTime(0, fdate, fdate).Where(c => c.IdelTime != null & c.IdelTime > 15).ToList();
            foreach (var x in data)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes(Convert.ToDouble(x.IdelTime));
                string workHours = spWorkMin.ToString(@"hh\:mm");

                string displayTime = Convert.ToDateTime(x.date).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                string time = Convert.ToDateTime(x.StartTime).ToString("HH:mm:ss");

                obj.Add(new SBAEmplyeeIdelGrid()
                {
                    UserName = x.userName,
                    Date = Convert.ToDateTime(x.date).ToString("dd/MM/yyyy"),
                    StartTime = x.StartTime,
                    EndTime = x.LastTime,
                    StartAddress = checkNull(x.StartAddress).Replace("Unnamed Road, ", ""),
                    EndAddress = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    IdelTime = workHours,
                    userId = x.userId,
                    startLat = x.StarLat,
                    startLong = x.StartLog,
                    EndLat = x.lat,
                    EndLong = x.@long,
                    daDateTIme = (displayTime + " " + time)

                });
            }
            return obj;
        }

        public List<SBALUserLocationMapView> GetUserTimeWiseRoute(string adate = "", DateTime? fTime = null, DateTime? tTime = null, int? userId = null)
        {

            // SBALUserLocationMapView userLocation = new SBALUserLocationMapView();
            // date = "03-09-2020";
            // //fTime = "10:00 AM";
            //// tTime = "10:30 AM";

            //DateTime a = Convert.ToDateTime(fTime);s
            //DateTime b = Convert.ToDateTime(tTime);
            //int result = DateTime.Compare(a, b);

            DateTime dateTime = new DateTime();
            // dateTime = Convert.ToDateTime(DateTime.ParseExact("date", "MM/dd/yyyy", CultureInfo.InvariantCulture));

            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            DateTime firstdate = DateTime.ParseExact(adate,
                                         "dd/MM/yyyy",
                                         CultureInfo.InvariantCulture);

            var firstDateString = firstdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);


            // string cc = Convert.ToDateTime(adate).ToString("MM/dd/yyyy");
            //string dt1 = Convert.ToDateTime(firstDateString).ToString("MM/dd/yyyy");
            //string displayTime = Convert.ToDateTime(firstDateString).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            // string dt = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
            string ft = Convert.ToDateTime(fTime).ToString("HH:mm:ss");
            string tt = Convert.ToDateTime(tTime).ToString("HH:mm:ss");
            DateTime fdate = Convert.ToDateTime(firstDateString + " " + ft);
            DateTime tdate = Convert.ToDateTime(firstDateString + " " + tt);

            var data = db.Locations.Where(c => c.userId == userId & c.datetime >= fdate & c.datetime <= tdate & c.type == null).ToList();

            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            //DateTime newdate = DateTime.Now.Date;
            //var datt = newdate;
            //var att = db.Daily_Attendance.Where(c => c.daID == userId).FirstOrDefault();
            //string Time = att.startTime;
            //DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);
            //string t = date.ToString("hh:mm:ss tt");
            //string dt = Convert.ToDateTime(att.daDate).ToString("MM/dd/yyyy");
            //DateTime? fdate = Convert.ToDateTime(dt + " " + t);
            //DateTime? edate;
            //if (att.endTime == "" | att.endTime == null)
            //{
            //    edate = DateTime.Now;
            //}
            //else {
            //    string Time2 = att.endTime;
            //    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
            //    string t2 = date2.ToString("hh:mm:ss tt");
            //    string dt2 = Convert.ToDateTime(att.daEndDate).ToString("MM/dd/yyyy");
            //    edate = Convert.ToDateTime(dt2 + " " + t2);
            //}
            //var data = db.Locations.Where(c => c.userId == att.userId & c.datetime >= fdate & c.datetime <= edate & c.type == null).ToList();


            foreach (var x in data)
            {

                string dat = Convert.ToDateTime(x.datetime).ToString("dd/MM/yyyy");
                string tim = Convert.ToDateTime(x.datetime).ToString("hh:mm tt");
                var userName = db.UserMasters.Where(c => c.userId == userId).FirstOrDefault();

                userLocation.Add(new SBALUserLocationMapView()
                {
                    userName = userName.userName,
                    date = dat,
                    time = tim,
                    lat = x.lat,
                    log = x.@long,
                    address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    // vehcileNumber = att.vehicleNumber,
                    userMobile = userName.userMobileNumber,
                    // type = Convert.ToInt32(x.type),

                });

            }


            return userLocation;
        }

        public List<SBALUserLocationMapView> GetHouseTimeWiseRoute(string adate = "", DateTime? fTime = null, DateTime? tTime = null, int? userId = null)
        {
            DateTime dateTime = new DateTime();
            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            DateTime firstdate = DateTime.ParseExact(adate,
                                         "dd/MM/yyyy",
                                         CultureInfo.InvariantCulture);

            var firstDateString = firstdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            string ft = Convert.ToDateTime(fTime).ToString("HH:mm:ss");
            string tt = Convert.ToDateTime(tTime).ToString("HH:mm:ss");
            DateTime fdate = Convert.ToDateTime(firstDateString + " " + ft);
            DateTime tdate = Convert.ToDateTime(firstDateString + " " + tt);

            //var data = db.Locations.Where(c => c.userId == userId & c.datetime >= fdate & c.datetime <= tdate & c.type == 1).ToList();
            var data = db.GarbageCollectionDetails.Where(c => c.userId == userId & c.gcDate >= fdate & c.gcDate <= tdate).ToList();
            List<SBALUserLocationMapView> userLocation = new List<SBALUserLocationMapView>();
            foreach (var x in data)
            {
                string dat = Convert.ToDateTime(x.gcDate).ToString("dd/MM/yyyy");
                string tim = Convert.ToDateTime(x.gcDate).ToString("hh:mm tt");
                var userName = db.UserMasters.Where(c => c.userId == userId).FirstOrDefault();
                var house = db.HouseMasters.Where(h => h.houseId == x.houseId).FirstOrDefault();
                var d = db.GarbageCollectionDetails.Where(a => a.houseId == house.houseId).FirstOrDefault();
                userLocation.Add(new SBALUserLocationMapView()
                {
                    //userName = userName.userName,
                    //date = dat,
                    //time = tim,
                    //lat = x.lat,
                    //log = x.@long,
                    //address = checkNull(x.address).Replace("Unnamed Road, ", ""),
                    //userMobile = userName.userMobileNumber,
                    //// type = Convert.ToInt32(x.type),
                    ///
                    userId = userName.userId,
                    userName = userName.userName,
                    date = dat,
                    time = tim,
                    lat = d.Lat,
                    log = d.Long,
                    address = house.houseAddress,
                    //vehcileNumber = att.vehicleNumber,
                    userMobile = userName.userMobileNumber,
                    type = 1,
                    HouseId = house.ReferanceId,
                    HouseAddress = (house.houseAddress == null ? "" : house.houseAddress.Replace("Unnamed Road, ", "")),
                    HouseOwnerName = house.houseOwner,
                    OwnerMobileNo = house.houseOwnerMobile,
                    WasteType = d.garbageType.ToString(),
                    gpBeforImage = d.gpBeforImage,
                    gpAfterImage = d.gpAfterImage,
                    ZoneList = ListZone(),

                });

            }


            return userLocation;
        }

        public DashBoardVM GetLiquidDashBoardDetails()
        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
                    List<ComplaintVM> obj = new List<ComplaintVM>();
                    //if (AppID==1)
                    //{
                    //    string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=1");
                    //     obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    //}
                    //if (appdetails.GramPanchyatAppID != null)
                    //{
                    //    string json = new WebClient().DownloadString(appdetails.Grampanchayat_Pro + "/api/Get/Complaint?appId=" + appdetails.GramPanchyatAppID);
                    //    obj = JsonConvert.DeserializeObject<List<ComplaintVM>>(json).Where(c => Convert.ToDateTime(c.createdDate2).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    //}
                    var data = db.SP_LiquidDashboard_Details().First();

                    var date = DateTime.Today;
                    var houseCount = db.SP_TotalHouseCollection_Count(date).FirstOrDefault();
                    if (data != null)
                    {

                        model.TodayAttandence = data.TodayAttandence;
                        model.TotalAttandence = data.TotalAttandence;
                        model.HouseCollection = data.TotalHouse;
                        model.LiquidCollection = data.TotalLiquid;
                        model.TotalComplaint = obj.Count();
                        model.TotalHouseCount = houseCount.TotalHouseCount;
                        model.MixedCount = houseCount.MixedCount;
                        model.BifurgatedCount = houseCount.BifurgatedCount;
                        model.NotCollected = houseCount.NotCollected;
                        model.NotSpecified = houseCount.NotSpecified;
                        //model.TotalGcWeightCount = houseCount.TotalGcWeightCount;
                        //model.GcWeightCount = Convert.ToDouble(string.Format("{0:0.00}", houseCount.GcWeightCount));
                        //model.DryWeightCount =Convert.ToDouble(string.Format("{0:0.00}", houseCount.DryWeightCount));
                        //model.WetWeightCount =Convert.ToDouble(string.Format("{0:0.00}", houseCount.WetWeightCount));
                        model.GcWeightCount = Convert.ToDouble(houseCount.GcWeightCount);
                        model.DryWeightCount = Convert.ToDouble(houseCount.DryWeightCount);
                        model.WetWeightCount = Convert.ToDouble(houseCount.WetWeightCount);
                        model.TotalGcWeightCount = Convert.ToDouble(houseCount.TotalGcWeightCount);
                        model.TotalDryWeightCount = Convert.ToDouble(houseCount.TotalDryWeightCount);
                        model.TotalWetWeightCount = Convert.ToDouble(houseCount.TotalWetWeightCount);
                        model.DumpYardCount = data.TotalDump;
                        model.TotalLiquidCount = houseCount.TotalLiquidCount;
                        model.TotalStreetCount = houseCount.TotalStreetCount;
                        model.TotalLiquidPropertyCount = houseCount.TotalLiquidPropertyCount;
                        model.TotalStreetPropertyCount = houseCount.TotalStreetPropertyCount;

                        //model.LWGcWeightCount = 4;
                        //model.LWDryWeightCount = 1;
                        //model.LWWetWeightCount = 3;
                        model.LWGcWeightCount = Convert.ToDouble(houseCount.LWGcWeightCount);
                        model.LWDryWeightCount = Convert.ToDouble(houseCount.LWDryWeightCount);
                        model.LWWetWeightCount = Convert.ToDouble(houseCount.LWWetWeightCount);

                        return model;
                    }

                    // String.Format("{0:0.00}", 123.4567); 

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public DashBoardVM GetStreetDashBoardDetails()
        {
            DashBoardVM model = new DashBoardVM();
            try
            {
                using (var db = new DevChildSwachhBharatNagpurEntities(AppID))
                {

                    DevSwachhBharatMainEntities dbm = new DevSwachhBharatMainEntities();
                    var appdetails = dbm.AppDetails.Where(c => c.AppId == AppID).FirstOrDefault();
                    List<ComplaintVM> obj = new List<ComplaintVM>();
                    var data = db.SP_StreetDashboard_Details().First();

                    var date = DateTime.Today;
                    var houseCount = db.SP_TotalHouseCollection_Count(date).FirstOrDefault();
                    if (data != null)
                    {

                        model.TodayAttandence = data.TodayAttandence;
                        model.TotalAttandence = data.TotalAttandence;
                        model.HouseCollection = data.TotalHouse;
                        model.StreetCollection = data.TotalStreet;
                        model.TotalComplaint = obj.Count();
                        model.TotalHouseCount = houseCount.TotalHouseCount;
                        model.MixedCount = houseCount.MixedCount;
                        model.BifurgatedCount = houseCount.BifurgatedCount;
                        model.NotCollected = houseCount.NotCollected;
                        model.NotSpecified = houseCount.NotSpecified;
                        model.GcWeightCount = Convert.ToDouble(houseCount.GcWeightCount);
                        model.DryWeightCount = Convert.ToDouble(houseCount.DryWeightCount);
                        model.WetWeightCount = Convert.ToDouble(houseCount.WetWeightCount);
                        model.TotalGcWeightCount = Convert.ToDouble(houseCount.TotalGcWeightCount);
                        model.TotalDryWeightCount = Convert.ToDouble(houseCount.TotalDryWeightCount);
                        model.TotalWetWeightCount = Convert.ToDouble(houseCount.TotalWetWeightCount);
                        model.DumpYardCount = data.TotalDump;
                        model.TotalLiquidCount = houseCount.TotalLiquidCount;
                        model.TotalStreetCount = houseCount.TotalStreetCount;
                        model.TotalStreetPropertyCount = houseCount.TotalStreetPropertyCount;
                        model.TotalLiquidPropertyCount = houseCount.TotalLiquidPropertyCount;
                        //model.TotalStreetCount = 1;
                        //model.TotalStreetPropertyCount = 50;
                        //model.SSGcWeightCount = 4;
                        //model.SSDryWeightCount = 1;
                        //model.SSWetWeightCount = 3;
                        model.SSGcWeightCount = Convert.ToDouble(houseCount.SSGcWeightCount);
                        model.SSDryWeightCount = Convert.ToDouble(houseCount.SSDryWeightCount);
                        model.SSWetWeightCount = Convert.ToDouble(houseCount.SSWetWeightCount);




                        return model;
                    }

                    // String.Format("{0:0.00}", 123.4567); 

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception)
            {
                return model;
            }
        }
    }
}
