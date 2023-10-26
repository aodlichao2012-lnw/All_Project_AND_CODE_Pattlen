using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPos.Class;
using GMap.NET.MapProviders;
using GMap.NET;
using System.Resources;
using AdaPos.Resources_String.Local;

namespace AdaPos.Control
{
    public partial class uCstAddress : UserControl
    {
        private ResourceManager oW_Resource;

        public uCstAddress()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "uCstAddress : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                if (cVB.nVB_AddressTypeCst == 1)
                    opnAddress1.Visible = true;
                else
                    opnAddress2.Visible = true;

                ogmAdrMap.MapProvider = BingMapProvider.Instance;
                GMaps.Instance.Mode = AccessMode.ServerOnly;
                ogmAdrMap.SetPositionByKeywords(new cSP().SP_GETtRegionLocation());

                ocmSchAddZne.BackColor = cVB.oVB_ColNormal;
                ocmSchAddAre.BackColor = cVB.oVB_ColNormal;
                ocmSchAddCountry.BackColor = cVB.oVB_ColNormal;
                ocmSchAddDist.BackColor = cVB.oVB_ColNormal;
                ocmSchAddPvn.BackColor = cVB.oVB_ColNormal;
                ocmSchAddSubDist.BackColor = cVB.oVB_ColNormal;
                ocmSchAddMap.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resCustomer_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resCustomer_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "CUSTOMER";

                olaTitleAddName.Text = oW_Resource.GetString("tName");
                olaTitleAddTax.Text = oW_Resource.GetString("tTax");
                olaTitleAddRemark.Text = oW_Resource.GetString("tRemark");
                olaTitleAddNo.Text = oW_Resource.GetString("tAddressNo");
                olaTitleAddVillage.Text = oW_Resource.GetString("tVillage");
                olaTitleAddSoi.Text = oW_Resource.GetString("tSoi");
                olaTitleAddRoad.Text = oW_Resource.GetString("tRoad");
                olaTitleAddPostcode.Text = oW_Resource.GetString("tPostcode");
                olaTitleAddCountry.Text = oW_Resource.GetString("tCountry");
                olaTitleAddArea.Text = oW_Resource.GetString("tArea");
                olaTitleAddZone.Text = oW_Resource.GetString("tZone");
                olaTitleAddProvince.Text = oW_Resource.GetString("tProvince");
                olaTitleAddDistrict.Text = oW_Resource.GetString("tDistrict");
                olaTitleAddSubDist.Text = oW_Resource.GetString("tSubDistrict");
                olaTitleAddWebSite.Text = oW_Resource.GetString("tWebsite");
                olaTitleSchLoc.Text = oW_Resource.GetString("tSearchLocation");
                olaTitleAddLat.Text = oW_Resource.GetString("tLatitude");
                olaTitleAddLng.Text = oW_Resource.GetString("tLongtitude");
                olaTitleAdd1.Text = oW_Resource.GetString("tAddress") + " 1";
                olaTitleAdd2.Text = oW_Resource.GetString("tAddress") + " 2";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogmMap_MouseClick(object sender, MouseEventArgs e)
        {
            double cLat = 0.0;
            double cLng = 0.0;
            List<Placemark> aoMark = null;
            GeoCoderStatusCode oStatus;

            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    cLat = ogmAdrMap.FromLocalToLatLng(e.X, e.Y).Lat;
                    cLng = ogmAdrMap.FromLocalToLatLng(e.X, e.Y).Lng;

                    oStatus = GMapProviders.GoogleMap.GetPlacemarks(new PointLatLng(cLat, cLng), out aoMark);

                    if (oStatus == GeoCoderStatusCode.G_GEO_SUCCESS && aoMark != null)
                        MessageBox.Show(aoMark[0].Address);
                    else
                        MessageBox.Show(oStatus.ToString());
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "ogmMap_MouseClick : " + oEx.Message); }
            finally
            {
                aoMark = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Search Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSearchMap_Click(object sender, EventArgs e)
        {
            try
            {
                W_SCHxLocation();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "ocmSearchMap_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Search Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbAdrLocation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    W_SCHxLocation();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "otbAdrLocation_KeyDown : " + oEx.Message); }
        }

        /// <summary>
        /// Search Location
        /// </summary>
        private void W_SCHxLocation()
        {
            try
            {
                ogmAdrMap.Zoom = 14;
                ogmAdrMap.SetPositionByKeywords(otbAddLocation.Text);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uCstAddress", "W_SCHxLocation : " + oEx.Message); }
        }
    }
}
