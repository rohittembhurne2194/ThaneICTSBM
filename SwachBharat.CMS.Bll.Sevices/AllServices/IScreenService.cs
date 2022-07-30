using SwachBharat.CMS.Bll.ViewModels.ChildModel.Model;
using SwachBharat.CMS.Dal.DataContexts;
using SwachBharat.CMS.Bll.ViewModels.Grid;
using SwachBharat.CMS.Bll.ViewModels.SS2020Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwachBharat.CMS.Bll.Services
{
    public interface IScreenService
    {
        DashBoardVM GetDashBoardDetails();

        DashBoardVM GetLiquidDashBoardDetails();

        DashBoardVM GetStreetDashBoardDetails();
        string Address(string location);
        AreaVM GetAreaDetails(int teamId,string Name);
        void DeletAreaDetails(int teamId);
        void SaveAreaDetails(AreaVM area);
        void LiquidSaveAreaDetails(AreaVM area);

        void StreetSaveAreaDetails(AreaVM area);


        VehicleTypeVM GetVehicleTypeDetails(int teamId);
        VehicleRegVM GetVehicleDetails(int teamId);

        StreetSweepVM GetBeatDetails(int teamId);

        void DeletVehicleTypeDetails(int teamId);
        void SaveVehicleTypeDetails(VehicleTypeVM type);
        void SaveVehicleRegDetails(VehicleRegVM type);

        WardNumberVM GetWardNumberDetails(int teamId,string name);
         void SaveWardNumberDetails(WardNumberVM data);

        void LiquidSaveWardNumberDetails(WardNumberVM data);

        void StreetSaveWardNumberDetails(WardNumberVM data);

        void DeletWardNumberDetails(int teamId);

        HouseDetailsVM GetHouseDetails(int teamId);

        SWMDetailsVM GetSWMDetails(int teamId);

        CommercialDetailsVM GetCommercialDetails(int teamId);
        SBALUserLocationMapView GetHouseByIdforMap(int teamId,int daId);

        SBALUserLocationMapView GetCTPTByIdforMap(int teamId, int daId);
        SBALUserLocationMapView GetLiquidByIdforMap(int teamId, int daId,string EmpType);
        HouseDetailsVM SaveHouseDetails(HouseDetailsVM data);

        SWMDetailsVM SaveSWMDetails(SWMDetailsVM data);

        CommercialDetailsVM SaveCommercialDetails(CommercialDetailsVM data);
        void DeletHouseDetails(int teamId);

        SBALUserLocationMapView GetLocationDetails(int teamId,string Emptype);
        List<SBALUserLocationMapView> GetAllUserLocation(string date,string Emptype);

        List<SBALUserLocationMapView> GetAdminLocation();
        List<SBALUserLocationMapView> GetUserWiseLocation(int userId,string date, string Emptype);
        List<SBALUserLocationMapView> GetUserAttenLocation(int userId);
        List<SBALUserLocationMapView> GetUserAttenRoute(int userId);

        List<SBALUserLocationMapView> GetCTPTUserAttenRoute(int userId);

        //Added By Saurabh(11 July 2019)
        List<SBALUserLocationMapView> GetHouseAttenRoute(int userId,int areaid);

        List<SBALUserLocationMapView> GetCTPTAttenRoute(int userId, int areaid);
        List<SBALUserLocationMapView> GetLiquidAttenRoute(int userId, int areaid);
        List<SBALUserLocationMapView> GetStreetAttenRoute(int userId, int areaid);

        GarbagePointDetailsVM GetGarbagePointDetails(int teamId);
        GarbagePointDetailsVM SaveGarbagePointDetails(GarbagePointDetailsVM data);
        void DeletGarbagePointDetails(int teamId);

        EmployeeDetailsVM GetEmployeeDetails(int teamId);

      

        SBAAttendenceSettingsGridRow GetAttendenceEmployeeById(int teamId);
        void DeleteEmployeeDetails(int teamId);
        string GetLoginidData(string LoginId);

        void SaveEmployeeDetails(EmployeeDetailsVM employee,string Emptype);

        void SaveAttendenceSettingsDetail(SBAAttendenceSettingsGridRow atten);
        ComplaintVM GetCompalint(int teamId);
        void SaveComplaintStatus(ComplaintVM employee);

        ZoneVM GetZone(int teamId);

        ZoneVM StreetGetZone(int teamId);
        void SaveZone(ZoneVM employee);

        void StreetSaveZone(ZoneVM employee);
        ZoneVM GetValidZone(string name,int zoneId);

        //Added By Saurabh

        DumpYardDetailsVM GetDumpYardtDetails(int teamId);
        //Added By Shubham
        StreetSweepVM GetStreetSweepDetails(int teamId);

        LiquidWasteVM GetLiquidWasteDetails(int teamId);
        DumpYardDetailsVM SaveDumpYardtDetails(DumpYardDetailsVM data,string Emptype);

        StreetSweepVM SaveStreetSweepDetails(StreetSweepVM data);
        StreetSweepVM SaveStreetBeatDetails(StreetSweepVM data);

        LiquidWasteVM SaveLiquidWasteDetails(LiquidWasteVM data);
        void DeletDumpYardtDetails(int teamId);

        //Added By Saurabh (27 May 2019)

        List<QrEmployeeMaster> GetUserList(int AppId, int teamId);

        void SaveHSQRStatusHouse(int houseId, string QRStatus);
        void SaveHSQRStatusComr(int comrId, string QRStatus);
        void SaveHSQRStatusCTPT(int CTPTId, string QRStatus);
        void SaveHSQRStatusSWM(int SWMId, string QRStatus);
        void SaveHSQRStatusLW(int LWId, string QRStatus);
        void SaveHSQRStatusSW(int SWId, string QRStatus);

        List<int> GetHSHouseDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder);
        List<int> GetHSComrDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder);
        List<int> GetHSCTPTDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder);
        List<int> GetHSSWMDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder);
        List<int> GetHSLWDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder);
        List<int> GetHSSWDetailsID(DateTime? fromDate, DateTime? toDate, int userId, string searchString, int QRStatus, string sortColumn, string sortOrder);

        SBAHSHouseDetailsGrid GetHouseDetailsById(int houseId);
        SBAHSDumpyardDetailsGrid GetComrDetailsById(int comrId);
        SBAHSDumpyardDetailsGrid GetCTPTDetailsById(int CTPTId);
        SBAHSDumpyardDetailsGrid GetSWMDetailsById(int SWMId);
        SBAHSLiquidDetailsGrid GetLWDetailsById(int LWId);

        SBAHSStreetDetailsGrid GetSWDetailsById(int SWId);

        //Added By Saurabh (03 June 2019)
        HouseScanifyEmployeeDetailsVM GetHSEmployeeDetails(int teamId);

        //Added By Saurabh (04 June 2019)
        void SaveHSEmployeeDetails(HouseScanifyEmployeeDetailsVM employee);

        //Added By Saurabh (04 June 2019)
        HSDashBoardVM GetHSDashBoardDetails();

        List<SBALHSUserLocationMapView> GetHSUserAttenRoute(int qrEmpDaId);

        List<SBAHSHouseDetailsGrid> GetHSQRCodeImageByDate(int type, int UserId, DateTime fDate, DateTime tDate,string QrStatus);

        //Added By Saurabh (06 June 2019)
        List<SBALHouseLocationMapView> GetAllHouseLocation(string date, int userid, int areaid, int wardNo, string SearchString, int? GarbageType, int FilterType,string Emptype,string ctype,int SegType);


        List<SBALCommercialLocationMapView> GetAllCommercialLocation(string date, int userid, int areaid, int wardNo, string SearchString, int? GarbageType, int FilterType, string Emptype, string ctype, int SegType);
        List<SBALCTPTLocationMapView> GetAllCTPTLocation(string date, int userid, int areaid, int wardNo, string SearchString,  int FilterType, string Emptype);

        List<SBALSWMLocationMapView> GetAllSWMLocation(string date, int userid, int areaid, int wardNo, string SearchString, int FilterType, string Emptype);
        //Code Optimization (code)
        //SBALHouseLocationMapView1 GetAllHouseLocation(string date, int userid,int areaid, int wardNo, string SearchString, string start);

        //Added By Saurabh (1 June 2019)
        HouseScanifyEmployeeDetailsVM GetUserDetails(int teamId, string Name);

        //Added By Saurabh (2 July 2019)
        DashBoardVM GetHouseOnMapDetails();
        DashBoardVM GetCommercialOnMapDetails();
        DashBoardVM GetCTPTOnMapDetails();

        DashBoardVM GetSWMOnMapDetails();
        DashBoardVM GetLiquidWasteDetails();

        DashBoardVM GetStreetSweepingDetails();

        //Added By Neha (12 July 2019)
        List<SBAEmplyeeIdelGrid> GetIdleTimeRoute(int userId, string Date);

        OnePoint4VM GetTotalCountDetails(int ANS_ID);



        OnePoint4VM GetMaxINSERTID();

        void Save1Point4(List<OnePoint4VM> point);

        //OnePoint4VM GetQuetionDetails(int teamId);

        List<OnePoint4VM> GetQuetions(string ReportName);

        void Save1Point5(List<OnePoint5VM> point);

        List<OnePoint5VM> GetQuetions1pointfive();

        void SaveTotalCount(OnePoint4VM onepointfour);

        List<OnePoint4VM> GetOnepointfourEditData(int INSERT_ID);

        void Save1Point7(List<OnePointSevenVM> point);

        List<OnePoint7QuestionVM> GetOnePointSevenQuestions();

        List<OnePoint7QuestionVM> GetOnePointSevenAnswers(int INSERT_ID);

        void EditOnePointSeven(List<OnePoint7QuestionVM> OnePoint7QuestionVM);
        List<SelectListItem> LoadListWardNo(int ZoneId);
        List<SelectListItem> LoadListArea(int WardNo);
        //InfotainmentDetailsVW GetInfotainmentDetailsById(int ID);
        //void SaveGameDetails(InfotainmentDetailsVW data);

        SauchalayDetailsVM GetSauchalayDetails(int teamId);

        SauchalayDetailsVM SaveSauchalayDetails(SauchalayDetailsVM data);

        List<SauchalayDetailsVM> GetCTPTLocation();

        #region WasteManagement
        WasteDetailsVM GetWasteDetails(int teamId);

        //List<WasteDetailsVM> SaveWasteDetails(List<WasteDetailsVM> data);
        string SaveWasteDetails(string data);
        string SaveSalesWasteDetails(string data);
        
        List<SelectListItem> LoadListSubCategory(int categoryId);
        List<SelectListItem> GetWMUserDetails();
        #endregion

        List<LogVM> GetLogString();

        List<SBAEmplyeeIdelGrid> GetIdelTimeNotification();
        List<SBAEmplyeeIdelGrid> GetLiquidIdelTimeNotification();

        List<SBAEmplyeeIdelGrid> GetStreetIdelTimeNotification();
        List<SBALUserLocationMapView> GetUserTimeWiseRoute(string date = "", DateTime? fTime = null, DateTime? tTime = null, int? userId = null);

        List<SBALUserLocationMapView> GetHouseTimeWiseRoute(string date = "", DateTime? fTime = null, DateTime? tTime = null, int? userId = null);
    }
}
