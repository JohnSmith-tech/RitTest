using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RitProject
{
    public partial class FormAddMarker : Form
    {

        public string name { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }


        public FormAddMarker()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            lat = textBox2.Text;
            lng = textBox3.Text;
            Close();

        }


    }
}
