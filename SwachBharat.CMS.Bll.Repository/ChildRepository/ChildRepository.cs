using SwachBharat.CMS.Bll.ViewModels.Grid;
using SwachBharat.CMS.Bll.Services;
using SwachBharat.CMS.Bll.ViewModels.ChildModel.Model;
using SwachBharat.CMS.Bll.ViewModels.MainModel;
using SwachBharat.CMS.Dal.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SwachBharat.CMS.Bll.ViewModels.SS2020Reports;
using System.Web.Mvc;

namespace SwachBharat.CMS.Bll.Repository.ChildRepository
{
   public class ChildRepository:IChildRepository
    {
        private int AppID;
        IScreenService screenService;
        
        public ChildRepository(int AppId)
        {
           screenService = new ScreenService(AppId);
           
            AppID = AppId;
        }
        public DashBoardVM GetDashBoardDetails()
        {
            return screenService.GetDashBoardDetails();

        }

        public DashBoardVM GetLiquidDashBoardDetails()
        {
            return screenService.GetLiquidDashBoardDetails();

        }

        public DashBoardVM GetStreetDashBoardDetails()
        {
            return screenService.GetStreetDashBoardDetails();

        }

        public string Address(string location)
        {
            return screenService.Address(location);
        }
        public AreaVM GetArea(int teamId,string name)
        {
            return screenService.GetAreaDetails(teamId,name);
        }
        public void DeletArea(int teamId)
        {
             screenService.DeletAreaDetails(teamId);
        }
        public void SaveArea(AreaVM area)
        {
            if (area.Id<=0)
            {
                area.Id = 0;
            }
            screenService.SaveAreaDetails(area);
        }


        public void LiquidSaveArea(AreaVM area)
        {
            if (area.Id <= 0)
            {
                area.Id = 0;
            }
            screenService.LiquidSaveAreaDetails(area);
        }

        public void StreetSaveArea(AreaVM area)
        {
            if (area.Id <= 0)
            {
                area.Id = 0;
            }
            screenService.StreetSaveAreaDetails(area);
        }

        public VehicleTypeVM GetVehicleType(int teamId)
        {
            return screenService.GetVehicleTypeDetails(teamId);
        }

        public VehicleRegVM GetVehicleReg(int teamId) {
            return screenService.GetVehicleDetails(teamId);
        }

        public StreetSweepVM GetBeat(int teamId)
        {
            return screenService.GetBeatDetails(teamId);
        }

        public void DeletVehicleType(int teamId)
        {
            screenService.DeletVehicleTypeDetails(teamId);
        }
        public void SaveVehicleType(VehicleTypeVM type)
        {
            if (type.Id <= 0)
            {
                type.Id = 0;
            }
            screenService.SaveVehicleTypeDetails(type);
        }

        public void SaveVehicleReg(VehicleRegVM type)
        {
            if (type.vehicleId <= 0)
            {
                type.vehicleId = 0;
            }
            screenService.SaveVehicleRegDetails(type);
        }

        public WardNumberVM GetWardNumber(int teamId,string name)
        {
            return screenService.GetWardNumberDetails(teamId,name);
        }
        public void DeleteWardNumber(int teamId)
        {
            screenService.DeletWardNumberDetails(teamId);
        } 
        public void SaveWardNumber(WardNumberVM type)
        {
            if (type.Id <= 0)
            {
                type.Id = 0;
            }
            screenService.SaveWardNumberDetails(type);
        }


        public void LiquidSaveWardNumber(WardNumberVM type)
        {
            if (type.LWId <= 0)
            {
                type.LWId = 0;
            }
            screenService.LiquidSaveWardNumberDetails(type);
        }

        public void StreetSaveWardNumber(WardNumberVM type)
        {
            if (type.SSId <= 0)
            {
                type.SSId = 0;
            }
            screenService.StreetSaveWardNumberDetails(type);
        }


        public HouseDetailsVM GetHouseById(int teamId)
        {
            return screenService.GetHouseDetails(teamId);
        }

        public SWMDetailsVM GetSWMById(int teamId)
        {
            return screenService.GetSWMDetails(teamId);
        }

        public CommercialDetailsVM GetCommercailById(int teamId)
        {
            return screenService.GetCommercialDetails(teamId);
        }

        public SBALUserLocationMapView GetHouseByIdforMap(int teamId,int daId)
        {
            return screenService.GetHouseByIdforMap(teamId, daId);
        }

        public SBALUserLocationMapView GetCTPTByIdforMap(int teamId, int daId)
        {
            return screenService.GetCTPTByIdforMap(teamId, daId);
        }

        public SBALUserLocationMapView GetLiquidByIdforMap(int teamId, int daId,string EmpType)
        {
            return screenService.GetLiquidByIdforMap(teamId, daId, EmpType);
        }

        public HouseDetailsVM SaveHouse(HouseDetailsVM data)
        {
            if (data.houseId <= 0)
            {
                data.houseId = 0;
            }
            HouseDetailsVM dd =screenService.SaveHouseDetails(data);
            return dd;
        }


        public CommercialDetailsVM SaveHouse(CommercialDetailsVM data)
        {
            if (data.houseId <= 0)
            {
                data.houseId = 0;
            }
            CommercialDetailsVM dd = screenService.SaveCommercialDetails(data);
            return dd;
        }

        public SWMDetailsVM SaveSWM(SWMDetailsVM data)
        {
            if (data.swmId <= 0)
            {
                data.swmId = 0;
            }
            SWMDetailsVM dd = screenService.SaveSWMDetails(data);
            return dd;
        }
        public void DeletHouse(int teamId)
        {
            screenService.DeletHouseDetails(teamId);
        } 


        public SBALUserLocationMapView GetLocation(int teamId,string Emptype)
        {
            return screenService.GetLocationDetails(teamId, Emptype);
        }

        public List<SBALUserLocationMapView> GetAllUserLocation(string date,string Emptype)
        {
            return screenService.GetAllUserLocation(date, Emptype);
        }

        public List<SBALUserLocationMapView> GetAdminLocation()
        {
            return screenService.GetAdminLocation();
        }


        public List<SBALUserLocationMapView> GetUserWiseLocation(int userId,string date,string Emptype)
        {
            return screenService.GetUserWiseLocation(userId,date, Emptype);
        }

        public List<SBALUserLocationMapView> GetUserAttenLocation(int userId)
        {
            return screenService.GetUserAttenLocation(userId);
        }
        public List<SBALUserLocationMapView> GetUserAttenRoute(int daId)
        {
            return screenService.GetUserAttenRoute(daId);
        }

        public List<SBALUserLocationMapView> GetCTPTUserAttenRoute(int daId)
        {
            return screenService.GetCTPTUserAttenRoute(daId);
        }

        //Added By Saurabh(11 July 2019)
        public List<SBALUserLocationMapView> GetHouseAttenRoute(int daId,int areaid)
        {
            return screenService.GetHouseAttenRoute(daId,areaid);
        }

        public List<SBALUserLocationMapView> GetCTPTAttenRoute(int daId, int areaid)
        {
            return screenService.GetCTPTAttenRoute(daId, areaid);
        } 

        public List<SBALUserLocationMapView> GetLiquidAttenRoute(int daId, int areaid)
        {
            return screenService.GetLiquidAttenRoute(daId, areaid);
        }

        public List<SBALUserLocationMapView> GetStreetAttenRoute(int daId, int areaid)
        {
            return screenService.GetStreetAttenRoute(daId, areaid);
        }

        public GarbagePointDetailsVM GetGarbagePointById(int teamId)
        {
            return screenService.GetGarbagePointDetails(teamId);
        }
        public GarbagePointDetailsVM SaveGarbagePoint(GarbagePointDetailsVM data)
        {
            if (data.gpId <= 0)
            {
                data.gpId = 0;
            }
            GarbagePointDetailsVM dd = screenService.SaveGarbagePointDetails(data);
            return dd;
        }
        public void DeletGarbagePoint(int teamId)
        {
            screenService.DeletGarbagePointDetails(teamId);
        }

        
        public EmployeeDetailsVM GetEmployeeById(int teamId)
        {
           return screenService.GetEmployeeDetails(teamId);
        }

        //public EmployeeDetailsVM GetLiquidEmployeeById(int teamId)
        //{
        //    return screenService.GetLiquidEmployeeDetails(teamId);
        //}


        public SBAAttendenceSettingsGridRow GetAttendenceEmployeeById(int teamId)
        {
            return screenService.GetAttendenceEmployeeById(teamId);
        }

        public void DeleteEmployee(int teamId)
        {
            screenService.DeleteEmployeeDetails(teamId);
        }

        public string GetLoginidData(string LoginId)
        {
            return screenService.GetLoginidData(LoginId);
        }

        public void SaveEmployee(EmployeeDetailsVM employee,string Emptype)
        {
            screenService.SaveEmployeeDetails(employee, Emptype);
        }

      public  void SaveAttendenceSettingsDetail(SBAAttendenceSettingsGridRow atten)
        {
            screenService.SaveAttendenceSettingsDetail(atten);
        }
        public ComplaintVM GetComplaint(int teamId)
        {
            return screenService.GetCompalint(teamId);
        }

        public void SaveComplaintStatus(ComplaintVM comp)
        {
            screenService.SaveComplaintStatus(comp);
        }

        public ZoneVM GetZone(int teamId)
        {
            return screenService.GetZone(teamId);
        }

        public ZoneVM StreetGetZone(int teamId)
        {
            return screenService.StreetGetZone(teamId);
        }

        public void SaveZone(ZoneVM type)
        {
            if (type.id <= 0)
            {
                type.id = 0;
            }
            screenService.SaveZone(type);
        }

        public void StreetSaveZone(ZoneVM type)
        {
            if (type.id <= 0)
            {
                type.id = 0;
            }
            screenService.StreetSaveZone(type);
        }
        public ZoneVM GetValidZone(string name,int zoneId)
        {
            return screenService.GetValidZone(name,zoneId);
        }

        //Added By Saurabh
        public DumpYardDetailsVM GetDumpYardById(int teamId)
        {
            return screenService.GetDumpYardtDetails(teamId);
        }

        public StreetSweepVM GetStreetSweepId(int teamId)
        {
            return screenService.GetStreetSweepDetails(teamId);
        }

        public LiquidWasteVM GetLiquidWasteId(int teamId)
        {
            return screenService.GetLiquidWasteDetails(teamId);
        }

        public DumpYardDetailsVM SaveDumpYard(DumpYardDetailsVM data,string Emptype)
        {
            if (data.dyId <= 0)
            {
                data.dyId = 0;
            }
            DumpYardDetailsVM dd = screenService.SaveDumpYardtDetails(data, Emptype);
            return dd;
        }
        public LiquidWasteVM SaveLiquidWastes(LiquidWasteVM data)
        {
            if (data.LWId <= 0)
            {
                data.LWId = 0;
            }
            LiquidWasteVM dd = screenService.SaveLiquidWasteDetails(data);
            return dd;
        }
        public StreetSweepVM SaveStreetSweep(StreetSweepVM data)
        {
            if (data.SSId <= 0)
            {
                data.SSId = 0;
            }
            StreetSweepVM dd = screenService.SaveStreetSweepDetails(data);
            return dd;
        }

        public StreetSweepVM SaveStreetBeat(StreetSweepVM data)
        {
            if (data.BeatId <= 0)
            {
                data.BeatId = 0;
            }
            StreetSweepVM dd = screenService.SaveStreetBeatDetails(data);
            return dd;
        }

        #region HouseScanify
        //Added By Saurabh

        public List<QrEmployeeMaster> GetUserList(int AppId , int teamId)
        {
            return screenService.GetUserList(AppId, teamId);
        }
        public void SaveHSQRStatusHouse(int houseId, string QRStatus)
        {
            screenService.SaveHSQRStatusHouse(houseId, QRStatus);
        }
        public void SaveHSQRStatusComr(int comrId, string QRStatus)
        {
            screenService.SaveHSQRStatusComr(comrId, QRStatus);
        }
        public void SaveHSQRStatusCTPT(int CTPTId, string QRStatus)
        {
            screenService.SaveHSQRStatusCTPT(CTPTId, QRStatus);
        }
        public void SaveHSQRStatusSWM(int SWMId, string QRStatus)
        {
            screenService.SaveHSQRStatusSWM(SWMId, QRStatus);
        }

        public void SaveHSQRStatusLW(int LWId, string QRStatus)
        {
            screenService.SaveHSQRStatusLW(LWId, QRStatus);
        }
        public void SaveHSQRStatusSW(int SWId, string QRStatus)
        {
            screenService.SaveHSQRStatusSW(SWId, QRStatus);
        }
        public List<int> GetHSHouseDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            return screenService.GetHSHouseDetailsID(fromDate, toDate, userId, searchString, QRStatus, sortColumn, sortOrder);
        }
        public List<int> GetHSComrDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            return screenService.GetHSComrDetailsID(fromDate, toDate, userId, searchString, QRStatus, sortColumn, sortOrder);
        }
        public List<int> GetHSCTPTDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            return screenService.GetHSCTPTDetailsID(fromDate, toDate, userId, searchString, QRStatus, sortColumn, sortOrder);
        }

        public List<int> GetHSSWMDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            return screenService.GetHSSWMDetailsID(fromDate, toDate, userId, searchString, QRStatus, sortColumn, sortOrder);
        }
        public List<int> GetHSLWDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            return screenService.GetHSLWDetailsID(fromDate, toDate, userId, searchString, QRStatus, sortColumn, sortOrder);
        }

        public List<int> GetHSSWDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder)
        {
            return screenService.GetHSSWDetailsID(fromDate, toDate, userId, searchString, QRStatus, sortColumn, sortOrder);
        }
        public SBAHSHouseDetailsGrid GetHouseDetailsById(int houseId)
        {
            return screenService.GetHouseDetailsById(houseId);
        }

        public SBAHSDumpyardDetailsGrid GetComrDetailsById(int comrId)
        {
            return screenService.GetComrDetailsById(comrId);
        }
        public SBAHSDumpyardDetailsGrid GetCTPTDetailsById(int CTPTId)
        {
            return screenService.GetCTPTDetailsById(CTPTId);
        }
        public SBAHSDumpyardDetailsGrid GetSWMDetailsById(int SWMId)
        {
            return screenService.GetSWMDetailsById(SWMId);
        }

        public SBAHSLiquidDetailsGrid GetLWDetailsById(int LWId)
        {
            return screenService.GetLWDetailsById(LWId);
        }
        public SBAHSStreetDetailsGrid GetSWDetailsById(int SWId)
        {
            return screenService.GetSWDetailsById(SWId);
        }
        // Addded By Saurabh (03 June 2019)
        public HouseScanifyEmployeeDetailsVM GetHSEmployeeById(int teamId)
        {
            return screenService.GetHSEmployeeDetails(teamId);
        }

        // Addded By Saurabh (04 June 2019)
        public void SaveHSEmployee(HouseScanifyEmployeeDetailsVM employee)
        {
            screenService.SaveHSEmployeeDetails(employee);
        }

        // Addded By Saurabh (04 June 2019)
        public HSDashBoardVM GetHSDashBoardDetails()
        {
            return screenService.GetHSDashBoardDetails();

        }

        public List<SBALHSUserLocationMapView> GetHSUserAttenRoute(int qrEmpDaId)
        {
            return screenService.GetHSUserAttenRoute(qrEmpDaId);
        }

        public List<SBAHSHouseDetailsGrid> GetHSQRCodeImageByDate(int type, int UserId, DateTime fDate, DateTime tDate, string QrStatus)
        {
            return screenService.GetHSQRCodeImageByDate(type, UserId, fDate, tDate, QrStatus);
        }

        #endregion
        // Addded By Saurabh (06 June 2019)
        public List<SBALHouseLocationMapView> GetAllHouseLocation(string date, int userid, int areaid, int wardNo, string SearchString, int? GarbageType, int FilterType,string Emptype,string ctype,int SegType)
        {
            return screenService.GetAllHouseLocation(date, userid, areaid, wardNo, SearchString, GarbageType, FilterType, Emptype, ctype, SegType);
        }

        public List<SBALCommercialLocationMapView> GetAllCommercialLocation(string date, int userid, int areaid, int wardNo, string SearchString, int? GarbageType, int FilterType, string Emptype, string ctype, int SegType)
        {
            return screenService.GetAllCommercialLocation(date, userid, areaid, wardNo, SearchString, GarbageType, FilterType, Emptype, ctype, SegType);
        }

        public List<SBALCTPTLocationMapView> GetAllCTPTLocation(string date, int userid, int areaid, int wardNo, string SearchString,  int FilterType, string Emptype)
        {
            return screenService.GetAllCTPTLocation(date, userid, areaid, wardNo, SearchString, FilterType, Emptype);
        }

        public List<SBALSWMLocationMapView> GetAllSWMLocation(string date, int userid, int areaid, int wardNo, string SearchString, int FilterType, string Emptype)
        {
            return screenService.GetAllSWMLocation(date, userid, areaid, wardNo, SearchString, FilterType, Emptype);
        }
        //Code Optimization (code)
        //public SBALHouseLocationMapView1 GetAllHouseLocation(string date, int userid, int areaid, int wardNo, string SearchString, string start)
        //{
        //    return screenService.GetAllHouseLocation( date, userid,areaid, wardNo, SearchString, start);
        //}

        // Addded By Saurabh (21 June 2019)
        public HouseScanifyEmployeeDetailsVM GetUser(int teamId, string name)
        {
            return screenService.GetUserDetails(teamId, name);
        }

        public DashBoardVM GetHouseOnMapDetails()
        {
            return screenService.GetHouseOnMapDetails();

        }

        public DashBoardVM GetCommercialOnMapDetails()
        {
            return screenService.GetCommercialOnMapDetails();

        }
        public DashBoardVM GetCTPTOnMapDetails()
        {
            return screenService.GetCTPTOnMapDetails();

        }

        public DashBoardVM GetSWMOnMapDetails()
        {
            return screenService.GetSWMOnMapDetails();

        }

        public DashBoardVM GetLiquidWasteDetails()
        {
            return screenService.GetLiquidWasteDetails();

        }

        public DashBoardVM GetStreetSweepingDetails()
        {
            return screenService.GetStreetSweepingDetails();

        }


        // Addded By neha (12 July 2019)
        public List<SBAEmplyeeIdelGrid> GetIdleTimeRoute(int userId, string Date)
        {
            return screenService.GetIdleTimeRoute(userId, Date);
        }
        public void Save1Point4(List<OnePoint4VM> point)
        {
            screenService.Save1Point4(point);

        }

        public List<OnePoint4VM> GetQuetions(string ReportName)
        {
            return screenService.GetQuetions(ReportName);
        }

        public void Save1Point5(List<OnePoint5VM> point)
        {
            screenService.Save1Point5(point);

        }

        public List<OnePoint5VM> GetQuetions1pointfive()
        {
            return screenService.GetQuetions1pointfive();
        }
        public List<OnePoint4VM> GetOnepointfourEditData(int INSERT_ID)
        {
            return screenService.GetOnepointfourEditData(INSERT_ID);
        }

        public OnePoint4VM GetOnePointFourTotalCount(int ANS_ID)
        {
            return screenService.GetTotalCountDetails(ANS_ID);
        }
        public OnePoint4VM GetMaxINSERTID()
        {
            return screenService.GetMaxINSERTID();
        }
        public void SaveTotalCount(OnePoint4VM onepointfour)
        {
            screenService.SaveTotalCount(onepointfour);
        }

        public void Save1Point7(List<OnePointSevenVM> point)
        {
            screenService.Save1Point7(point);

        }

        public List<OnePoint7QuestionVM> GetOnePointSevenQuestions()
        {
            return screenService.GetOnePointSevenQuestions();
        }

        public void EditOnePointSeven(List<OnePoint7QuestionVM> OnePoint7QuestionVM)
        {
            screenService.EditOnePointSeven(OnePoint7QuestionVM);
        }

        public List<OnePoint7QuestionVM> GetOnePointSevenAnswers(int INSERT_ID)
        {
            return screenService.GetOnePointSevenAnswers(INSERT_ID);
        }

        public List<SelectListItem> LoadListWardNo(int ZoneId)
        {
            return screenService.LoadListWardNo(ZoneId);
        }

        public List<SelectListItem> LoadListArea(int WardNo)
        {
            return screenService.LoadListArea(WardNo);
        }

        //public InfotainmentDetailsVW GetInfotainmentDetailsById(int ID)
        //{
        //    return screenService.GetInfotainmentDetailsById(ID);
        //}

        //public void SaveGameDetails(InfotainmentDetailsVW data)
        //{
        //    if (data.GameDetailsId <= 0)
        //    {
        //        data.GameDetailsId = 0;
        //    }
        //    screenService.SaveGameDetails(data);
        //}

        public SauchalayDetailsVM GetSauchalayById(int teamId)
        {
            return screenService.GetSauchalayDetails(teamId);
        }

        public SauchalayDetailsVM SaveSauchalay(SauchalayDetailsVM data)
        {
            if (data.Id <= 0)
            {
                data.Id = 0;
            }
            SauchalayDetailsVM dd = screenService.SaveSauchalayDetails(data);
            return dd;
        }

        public List<SauchalayDetailsVM> GetCTPTLocation()
        {
            return screenService.GetCTPTLocation();
        }

        #region WasteManagement
        public WasteDetailsVM GetWasteById(int teamId)
        {
            return screenService.GetWasteDetails(teamId);
        }

        //public List<WasteDetailsVM> SaveWaste(List<WasteDetailsVM> data)
        //{
        //    return screenService.SaveWasteDetails(data);
        //}
        public string SaveWaste(string data)
        {
            return screenService.SaveWasteDetails(data);
        }

        public string SaveSalesWaste(string data)
        {
            return screenService.SaveSalesWasteDetails(data);
        }
        
        public List<SelectListItem> LoadListSubCategory(int categoryId)
        {
            return screenService.LoadListSubCategory(categoryId);
        }

        public List<SelectListItem> GetWMUser()
        {
            return screenService.GetWMUserDetails();
        }
        #endregion

        public List<LogVM> GetLogString()
        {
            return screenService.GetLogString();
        }

        public List<SBAEmplyeeIdelGrid> GetIdelTimeNotification()
        {
            return screenService.GetIdelTimeNotification();
        }
        public List<SBAEmplyeeIdelGrid> GetLiquidIdelTimeNotification()
        {
            return screenService.GetLiquidIdelTimeNotification();
        }

        public List<SBAEmplyeeIdelGrid> GetStreetIdelTimeNotification()
        {
            return screenService.GetStreetIdelTimeNotification();
        }

        public List<SBALUserLocationMapView> GetUserTimeWiseRoute(string date = "", DateTime? fTime = null, DateTime? tTime = null, int? userId = null)
        {
            return screenService.GetUserTimeWiseRoute(date,fTime, tTime, userId);
        }

        public List<SBALUserLocationMapView> GetHouseTimeWiseRoute(string date = "", DateTime? fTime = null, DateTime? tTime = null, int? userId = null)
        {
            return screenService.GetHouseTimeWiseRoute(date, fTime, tTime, userId);
        }


    }
}

