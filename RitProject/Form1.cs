using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Configuration;

namespace RitProject
{
    public partial class Form1 : Form
    {

        private DataBase data;

        private bool isMarkerEnter;
        private int? currentMarker;
        private bool mouseIsDown;
        private Point mouseDownPoint;

        public Form1()
        {
            InitializeComponent();
        }

        private void getCurrPosition(object sender, MouseEventArgs e)
        {

            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            textBox3.Text = Convert.ToString(lat);
            textBox4.Text = Convert.ToString(lng);

            if (isMarkerEnter && mouseIsDown)
            {
                GMapMarker marker = getMarkerById(currentMarker);

                if (marker != null)
                {
                    PointLatLng point = gMapControl1.FromLocalToLatLng(e.Location.X, e.Location.Y);
                    marker.Position = new PointLatLng(point.Lat, point.Lng);
                }
            }

        }



        private GMapOverlay fillMapMarkers(List<Machinery> machineries)
        {
            GMapOverlay markers = new GMapOverlay("markers");
            foreach (Machinery mach in machineries)
            {
                PointLatLng point = new PointLatLng(mach.Lat, mach.Lng);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red);
                marker.ToolTipText = mach.Name;
                marker.Tag = mach.Id.ToString();
                markers.Markers.Add(marker);
            }


            return markers;
        }



        private void fillTableGrid(List<Machinery> machineries)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("id", "Id");
            dataGridView1.Columns.Add("name", "Name");
            dataGridView1.Columns.Add("lat", "Lat");
            dataGridView1.Columns.Add("lng", "Lng");
            for (int i = 0; i < machineries.Count; i++)
            {
                dataGridView1.Rows.Add(machineries[i].Id, machineries[i].Name, machineries[i].Lat, machineries[i].Lng);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            data = new DataBase(ConfigurationManager.ConnectionStrings["KeyConnection"].ConnectionString);
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            loadingAllData();
            isMarkerEnter = false;
            mouseIsDown = false;
            currentMarker = null;
            double defaultLat = 22;
            double defaultLng = 33;

            gMapControl1.Position = new PointLatLng(defaultLat, defaultLng);



            gMapControl1.MinZoom = 5;
            gMapControl1.MaxZoom = 10;
            gMapControl1.Zoom = 5;

            gMapControl1.DragButton = MouseButtons.Left;


        }

        private void loadingAllData()
        {
            List<Machinery> listMachinery = data.SelectAll();
            GMapOverlay markers = fillMapMarkers(listMachinery);
            gMapControl1.Overlays.Clear();
            gMapControl1.Overlays.Add(markers);
            fillTableGrid(listMachinery);
        }


        private void addEntry_button_Click(object sender, EventArgs e)
        {
            using (FormAddMarker formAddMarker = new FormAddMarker())
            {
                formAddMarker.ShowDialog();
                string newData = formAddMarker.name.ToString() + ";" + formAddMarker.lat.ToString() + ";" + formAddMarker.lng.ToString();
                if (formAddMarker.name.ToString() == "" || formAddMarker.lat.ToString() == "" || formAddMarker.lng.ToString() == "")
                {
                    MessageBox.Show("Fill all entries");
                }
                else
                {
                    data.insertNewData(newData);
                    loadingAllData();
                }



            }




        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            int id = (int)dataGridView1.Rows[rowIndex].Cells[0].Value;
            dataGridView1.Rows.RemoveAt(rowIndex);
            data.deleteEntry(id);
            loadingAllData();
        }


        private void gMapControl1_OnMarkerEnter(GMapMarker item)
        {

            if (currentMarker == null)
            {
                currentMarker = Convert.ToInt32(item.Tag);
                isMarkerEnter = true;
            }

        }

        private void gMapControl1_OnMarkerLeave(GMapMarker item)
        {
            if (!mouseIsDown)
            {
                isMarkerEnter = false;
                currentMarker = null;
            }
        }



        private GMapMarker getMarkerById(int? id)
        {
            return gMapControl1
                    .Overlays
                    .FirstOrDefault(x => x.Id == "markers")
                    .Markers
                    .FirstOrDefault(m => Convert.ToInt32(m.Tag) == id);
        }



        private void gMapControl1_MouseUp_1(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
            if (currentMarker != null)
            {
                GMapMarker marker = getMarkerById(currentMarker);
                data.ChangePosition(Convert.ToInt32(marker.Tag), marker.Position.Lat, marker.Position.Lng);
                fillTableGrid(data.SelectAll());
            }
        }

        private void gMapControl1_MouseDown_1(object sender, MouseEventArgs e)
        {

            mouseIsDown = true;
            mouseDownPoint = new Point(e.Location.X, e.Location.Y);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            data.closeConnection();
        }
    }
}
